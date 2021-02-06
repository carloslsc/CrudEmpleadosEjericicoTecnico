using CrudEmpleadosEjercicioTecnico.Data;
using CrudEmpleadosEjercicioTecnico.Models;
using CrudEmpleadosEjercicioTecnico.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CrudEmpleadosEjercicioTecnico.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EmpleadoController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            this._context = context;
            this._hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {
            var darrEmpleados = await this._context.Empleado.ToListAsync();
            if (id == 1)
            {
                foreach (var empleado in darrEmpleados)
                {
                    empleado.SalarioEmpleado = empleado.SalarioEmpleado / 21.5m;
                }
            }

            EmpleadoVM emplvm = new EmpleadoVM()
            {
                darrEmpleado = darrEmpleados,
                intTipoCambioMXN = (id == 1) ? 0 : 1
            };

            return View(emplvm);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Empleado empleado, List<IFormFile>? fileToUpload)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = this._hostEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                if (archivos.Count() > 0)
                {
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes");
                    var extension = Path.GetExtension(archivos[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    empleado.FotoEmpleado = @"\imagenes\" + nombreArchivo + extension;
                }
                else
                {
                    empleado.FotoEmpleado = @"\imagenes\avatar_default.png";
                }

                this._context.Empleado.Add(empleado);
                await this._context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Editar(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await this._context.Empleado.AsNoTracking().FirstOrDefaultAsync(e => e.IdEmpleado == id);
            if (empleado == null)
            {
                return NotFound();
            }
            
            return View(empleado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                var emp = await this._context.Empleado.AsNoTracking().FirstOrDefaultAsync(e => e.IdEmpleado == empleado.IdEmpleado);

                if (archivos.Count() > 0)
                {
                    //Editamos Imagen
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    var nuevaExtension = Path.GetExtension(archivos[0].FileName);

                    if (emp.FotoEmpleado != null)
                    {
                        var rutaImagen = Path.Combine(rutaPrincipal, emp.FotoEmpleado.TrimStart('\\'));

                        if (System.IO.File.Exists(rutaImagen))
                        {
                            System.IO.File.Delete(rutaImagen);
                        }
                    }

                    //Subimos nuevamente el archivo
                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + nuevaExtension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    empleado.FotoEmpleado = @"\imagenes\" + nombreArchivo + nuevaExtension;

                    this._context.Update(empleado);
                    await this._context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else if (empleado.FotoEmpleado != null && !empleado.FotoEmpleado.StartsWith("\\"))
                {
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\");

                    string base64 = empleado.FotoEmpleado.Split(',')[1];
                    byte[] bytes = Convert.FromBase64String(base64);
                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(base64)))
                    {
                        using (Bitmap bm2 = new Bitmap(ms))
                        {
                            bm2.Save(subidas + nombreArchivo + ".png");
                        }
                    }

                    empleado.FotoEmpleado = @"\imagenes\" + nombreArchivo + ".png";
                }
                else
                {
                    //Aqui es cuando la imagen ya existe y no se reemplaza
                    //Debe conservar la que ya esta en base de datos
                    empleado.FotoEmpleado = emp.FotoEmpleado;
                }

                this._context.Update(empleado);
                await this._context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(empleado);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await this._context.Empleado.AsNoTracking().FirstOrDefaultAsync(e => e.IdEmpleado == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRegistro(Guid id)
        {
            var empleado = await this._context.Empleado.FindAsync(id);

            if(empleado == null)
            {
                return View();
            }

            if (empleado.FotoEmpleado != null)
            {
                string rutaPrincipal = _hostEnvironment.WebRootPath;
                var rutaImagen = Path.Combine(rutaPrincipal, empleado.FotoEmpleado.TrimStart('\\'));

                if (empleado.FotoEmpleado != "\\imagenes\\avatar_default.png")
                {
                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }
                }
            }

            this._context.Empleado.Remove(empleado);
            await this._context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
