using System;

namespace ConectandoBD.Entity
{
    public class EmpleadoFilter
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public decimal? Salario { get; set; }
        public string Cargo { get; set; }
        public int? EmpresaId { get; set; }
        public PaginateProperties PaginateProperties { get; set; }
    }
}
