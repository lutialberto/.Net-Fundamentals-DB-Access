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
        private readonly SqlConnection connection;

        public EmpleadoBusiness()
        {
            connection = new SqlConnection
            {
                ConnectionString = Configuration.GetConnectionString()
            };
        }

        public DataSet EjecutarDataAdapter()
        {
            try
            {
                var connection = AbrirConexion();
                DataSetCustom ds = new DataSetCustom(connection);
                DataSet dsa = ds.GetEmpleados();

                ds.Update();

                return ds.GetEmpleados();
            }
            catch (Exception e)
            {
                ExceptionPrinter.Print(e);
                return null;
            }
        }

        public void EjecutarEnTransacciones()
        {
            var connection = AbrirConexion();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                DataAccessLayer dal = new DataAccessLayer();
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

        public List<Empleado> EjecutarQueryConPaginado(EmpleadoFilter filter)
        {
            try
            {
                var connection = AbrirConexion();
                DataAccessLayer dal = new DataAccessLayer();

                DataSet ds = dal.EjecutarQueryConPaginado(connection,filter);

                return MapDataSetToEmpleados(ds);
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
                    list.Add(new Empleado
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Nombre = Convert.ToString(row["Nombre"]),
                        Apellido = Convert.ToString(row["Apellido"]),
                        FechaNacimiento = Convert.ToDateTime(row["FechaNacimiento"]),
                        Sexo = Convert.IsDBNull(row["Sexo"]) ? null : Convert.ToString(row["Sexo"]),
                        Cargo = Convert.ToString(row["Cargo"]),
                        Salario = Convert.ToDecimal(row["Salario"]),
                        EmpresaId = Convert.IsDBNull(row["EmpresaId"]) ? null : (int?)Convert.ToInt32(row["EmpresaId"])
                    });
                }
                return list;
            }
            else
            {
                return null;
            }
        }

        public SqlDataReader GetUsuarioByFilter(EmpleadoFilter filter)
        {
            try
            {
                var connection = AbrirConexion();
                DataAccessLayer dal = new DataAccessLayer();
                return dal.GetUsuarioByFilter(connection, filter);
            }
            catch (Exception e)
            {
                ExceptionPrinter.Print(e);
                return null;
            }
        }

        public object EjecutarExecuteScalar(string query)
        {
            try
            {
                var connection = AbrirConexion();
                DataAccessLayer dal = new DataAccessLayer();
                return dal.EjecutarExecuteScalar(connection, query);
            }
            catch (Exception e)
            {
                ExceptionPrinter.Print(e);
                return null;
            }
        }

        public SqlConnection AbrirConexion()
        {
            try
            {
                connection.Open();
                Console.WriteLine("Se creo la conexión exitosamente");
                return connection;
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
                var connection = AbrirConexion();
                DataAccessLayer dal = new DataAccessLayer();
                return dal.EjecutarExecuteNonQuery(connection,nonNonQwerySentence);
            }
            catch (Exception e)
            {
                ExceptionPrinter.Print(e);
                return null;
            }
        }

        public void Dispose()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
                connection.Dispose();
                Console.WriteLine("Se está cerrando conexión");
            }
        }
    }
}
