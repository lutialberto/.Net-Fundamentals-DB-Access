using ConectandoBD.BLL;
using ConectandoBD.Entity;
using ConectandoBD.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ConectandoBD
{
    class Program
    {
        static void Main()
        {
            try
            {
                //Correr cada metodo por separado

                ConectarseABaseDeDatos();
                //EjecutarExecuteNonQuery();
                //EjecutarExecuteScalar();
                //EjecutarExecuteReader();
                //EjecutarDataAdapter();
                //EjecutarQueryConPaginado();
                //EjecutarEnTransacciones();
            }
            catch (Exception e)
            {
                ExceptionPrinter.Print(e);
            }
        }

        private static void EjecutarEnTransacciones()
        {
            using (EmpleadoBusiness business = new EmpleadoBusiness())
            {
                business.EjecutarEnTransacciones();
            }
        }

        private static void EjecutarQueryConPaginado()
        {
            using (EmpleadoBusiness business = new EmpleadoBusiness())
            {
                var filter = new EmpleadoFilter
                {
                    PaginateProperties = new PaginateProperties
                    {
                        Order = 1,
                        PageIndex = 0,
                        PageSize = 2,
                        SortBy = "Nombre"
                    }
                };

                List<Empleado> empleados = business.EjecutarQueryConPaginado(filter);
            }
        }

        private static void EjecutarDataAdapter()
        {
            using (EmpleadoBusiness business = new EmpleadoBusiness())
            {
                DataSet ds = business.EjecutarDataAdapter();
            }
        }

        private static void EjecutarExecuteReader()
        {
            using (EmpleadoBusiness business = new EmpleadoBusiness())
            {
                var filter = new EmpleadoFilter
                {
                    Apellido = "Rubio Cuestas",
                    Nombre = "Elena"
                };

                EjecutamosReaderYGeneramosException(business, filter);
                EjecutamosReader(business, filter);
            }
        }

        private static void EjecutamosReader(EmpleadoBusiness business, EmpleadoFilter filter)
        {
            business.GetUsuarioByFilterLayendoConreader(filter);
        }

        private static void EjecutamosReaderYGeneramosException(EmpleadoBusiness business, EmpleadoFilter filter)
        {
            SqlDataReader results = business.GetUsuarioByFilter(filter);

            //lo siguiente debería fallar
            if (results != null)
            {
                while (results.Read())
                {
                    Console.WriteLine($"| { results.GetString(0) } | { results.GetString(1) } | { results.GetDateTime(2) } |");
                }
            }
        }

        private static void EjecutarExecuteScalar()
        {
            using (EmpleadoBusiness business = new EmpleadoBusiness())
            {
                string query = "SELECT SALARIO,nombre FROM Empleado";

                var queryResult = business.EjecutarExecuteScalar(query);

                var message = queryResult == null ? "No le pegamos a nada" : $"Obtuvimos esto -> { queryResult }";

                Console.WriteLine(message);
            }
        }

        private static void EjecutarExecuteNonQuery()
        {
            using (EmpleadoBusiness business = new EmpleadoBusiness())
            {
                string nonNonQwerySentence = "UPDATE Empleado SET Apellido = Apellido + ' OtroApellidoMas' WHERE Id = 1";

                var registrosAfectados = business.EjecutarExecuteNonQuery(nonNonQwerySentence);

                Console.WriteLine($"Registros afectados : {registrosAfectados.GetValueOrDefault(-1)}");
            }
        }

        private static void ConectarseABaseDeDatos()
        {
            using (EmpleadoBusiness business = new EmpleadoBusiness())
            {
                business.AbrirConexion();
            }
        }

    }
}
