using System;

namespace ConectandoBD.Entity
{
    public class Empleado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public string Cargo { get; set; }
        public decimal Salario { get; set; }
        public int? EmpresaId { get; set; }
    }
}
