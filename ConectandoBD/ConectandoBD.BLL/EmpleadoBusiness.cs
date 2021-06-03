using ConectandoBD.DAL;
using ConectandoBD.Entity;
using ConectandoBD.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ConectandoBD.BLL
{
    public class EmpleadoBusiness : IDisposable
    {
        public DataSet EjecutarDataAdapter()
        {
            try
            {
                using (DataAccessLayer dal = new DataAccessLayer())
                {
                    var connection = dal.AbrirConexion();
                    DataSetCustom ds = new DataSetCustom(connection);
                    DataSet dsa = ds.GetEmpleados();

                    return dsa;
                }
            }
            catch (Exception e)
            {
                ExceptionPrinter.Print(e);
                return null;
            }
        }

        public void EjecutarEnTransacciones()
        {
            using (DataAccessLayer dal = new DataAccessLayer())
            {
                var connection = dal.AbrirConexion();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    dal.EjecutarExecuteNonQueryConTransaccion(transaction, connection, "UPDATE Empleado SET Nombre = 'EsperandoElCommit' WHERE Id = 1");
                    dal.EjecutarExecuteNonQueryConTransaccion(transaction, connection, "UPDATE TablaQueNoExiste SET Nombre = 'YaNoGeneraRollback' WHERE Id = 2");

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    ExceptionPrinter.Print(e);
                }
                finally
                {
                    transaction.Dispose();
                }
            }
        }

        public List<Empleado> EjecutarQueryConPaginado(EmpleadoFilter filter)
        {
            try
            {
                using (DataAccessLayer dal = new DataAccessLayer())
                {
                    var connection = dal.AbrirConexion();
                    DataSet ds = dal.EjecutarQueryConPaginado(connection, filter);
                    return MapDataSetToEmpleados(ds);
                }
            }
            catch (Exception e)
            {
                ExceptionPrinter.Print(e);
                return null;
            }
        }

        private List<Empleado> MapDataSetToEmpleados(DataSet ds)
        {
            List<Empleado> list = new List<Empleado>();
            
            if (DataSetHelper.HasRecords(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(MapDataRowToEmpleado(row));
                }
                return list;
            }
            else
            {
                return null;
            }
        }

        private static Empleado MapDataRowToEmpleado(DataRow row)
        {
            return new Empleado
            {
                Id = Convert.ToInt32(row["Id"]),
                Nombre = Convert.ToString(row["Nombre"]),
                Apellido = Convert.ToString(row["Apellido"]),
                FechaNacimiento = Convert.ToDateTime(row["FechaNacimiento"]),
                Sexo = Convert.IsDBNull(row["Sexo"]) ? null : Convert.ToString(row["Sexo"]),
                Cargo = Convert.ToString(row["Cargo"]),
                Salario = Convert.ToDecimal(row["Salario"]),
                EmpresaId = Convert.IsDBNull(row["EmpresaId"]) ? null : (int?)Convert.ToInt32(row["EmpresaId"])
            };
        }

        public void GetUsuarioByFilterLayendoConreader(EmpleadoFilter filter)
        {
            try
            {
                using (DataAccessLayer dal = new DataAccessLayer())
                {
                    var connection = dal.AbrirConexion();
                    var reader = dal.GetUsuarioByFilter(connection, filter);

                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"| { reader.GetString(0) } | { reader.GetString(1) } | { reader.GetDateTime(2) } |");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionPrinter.Print(e);
            }
        }

        public SqlDataReader GetUsuarioByFilter(EmpleadoFilter filter)
        {
            try
            {
                using (DataAccessLayer dal = new DataAccessLayer())
                {
                    var connection = dal.AbrirConexion();
                    return dal.GetUsuarioByFilter(connection, filter);
                }
            }
            catch (Exception e)
            {
                ExceptionPrinter.Print(e);
                return null;
            }
        }

        public void AbrirConexion()
        {
            try
            {
                using (DataAccessLayer dal = new DataAccessLayer())
                {
                    var connection = dal.AbrirConexion();
                    Console.WriteLine("Esto se debería leer entre que abro y cierro la conexión a BD");
                }
            }
            catch (Exception e)
            {
                ExceptionPrinter.Print(e);
            }
        }

        public object EjecutarExecuteScalar(string query)
        {
            try
            {
                using (DataAccessLayer dal = new DataAccessLayer())
                {
                    var connection = dal.AbrirConexion();
                    return dal.EjecutarExecuteScalar(connection, query);
                }
            }
            catch (Exception e)
            {
                ExceptionPrinter.Print(e);
                return null;
            }
        }

        public int? EjecutarExecuteNonQuery(string nonNonQwerySentence)
        {
            try
            {
                using (DataAccessLayer dal = new DataAccessLayer())
                {
                    var connection = dal.AbrirConexion();
                    return dal.EjecutarExecuteNonQuery(connection, nonNonQwerySentence);
                }
            }
            catch (Exception e)
            {
                ExceptionPrinter.Print(e);
                return null;
            }
        }

        public void Dispose()
        {
        }
    }
}
