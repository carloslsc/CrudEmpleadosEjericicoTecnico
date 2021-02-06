using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CrudEmpleadosEjercicioTecnico.Models
{
    public class Empleado
    {
        [Key]
        public Guid IdEmpleado { get; set; }

        [Required(ErrorMessage = "El nombre del empleado es obligatorio.")]
        [MinLength(3, ErrorMessage = "Escriba al menos 4 caracteres.")]
        [MaxLength(50, ErrorMessage = "Escriba un maximo de 50 caracteres.")]
        [Display(Name = "Nombre Empleado")]
        public string NombreEmpleado { get; set; }

        [Required(ErrorMessage = "Los apellidos del empleado es obligatorio.")]
        [MinLength(3, ErrorMessage = "Escriba al menos 4 caracteres.")]
        [MaxLength(50, ErrorMessage = "Escriba un maximo de 50 caracteres.")]
        [Display(Name = "Apellidos Empleado")]
        public string ApellidosEmpleado { get; set; }

        [Required(ErrorMessage = "El E-mail del empleado es obligatorio.")]
        [EmailAddress(ErrorMessage = "Escriba un E-mail válido.")]
        [Display(Name = "E-mail")]
        public string EmailEmpleado { get; set; }

        [Required(ErrorMessage = "El telefono del empleado es obligatorio.")]
        [Display(Name = "Teléfono")]
        public string TelefonoEmpleado { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento del empleado es obligatorio.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Nacimiento")]
        public DateTime FecNacEmpleado { get; set; }

        [Required(ErrorMessage = "El salario del empleado es obligatorio")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [Display(Name = "Salario")]
        public decimal SalarioEmpleado { get; set; }

        [Required(ErrorMessage = "La empresa del empleado es obligatorio.")]
        [Display(Name = "Empresa")]
        public string EmpresaEmpleado { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Foto Perfil")] 
        [DefaultValue("avatar_default.png")]
        public string FotoEmpleado { get; set; }
    }
}
