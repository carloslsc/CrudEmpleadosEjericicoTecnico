using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudEmpleadosEjercicioTecnico.Models.ViewModels
{
    //Clase para poder mandar el tipo de cambio que se planea utilizar junto con la lista de empleados
    public class EmpleadoVM
    {
        public List<Empleado> darrEmpleado { get; set; }

        public int intTipoCambioMXN { get; set; }
    }
}
