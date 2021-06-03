using ConectandoBD.Entity;
using ConectandoBD.Utils;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ConectandoBD.DAL
{
    public class DataAccessLayer : IDisposable
    {
        public SqlConnection connection;

        public DataAccessLayer()
        {
            connection = new SqlConnection
            {
                ConnectionString = Configuration.GetConnectionString()
            };
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

        public int EjecutarExecuteNonQuery(SqlConnection connection, string nonNonQwerySentence)
        {
            SqlCommand cmd = new SqlCommand
            {
                Connection = connection,
                CommandType = CommandType.Text,
                CommandText = nonNonQwerySentence
            };
            int registrosAfectados = cmd.ExecuteNonQuery();

            return registrosAfectados;
        }

        public int EjecutarExecuteNonQueryConTransaccion(SqlTransaction transaction, SqlConnection connection, string nonNonQwerySentence)
        {
            SqlCommand cmd = new SqlCommand
            {
                Connection = transaction != null ? transaction.Connection : connection,
                Transaction = transaction,
                CommandType = CommandType.Text,
                CommandText = nonNonQwerySentence
            };
            int registrosAfectados = cmd.ExecuteNonQuery();

            return registrosAfectados;
        }

        public object EjecutarExecuteScalar(SqlConnection connection, string query)
        {
            SqlCommand cmd = new SqlCommand
            {
                Connection = connection,
                CommandType = CommandType.Text,
                CommandText = query
            };
            object queryResult = cmd.ExecuteScalar();

            return queryResult;
        }

        public SqlDataReader GetUsuarioByFilter(SqlConnection connection, EmpleadoFilter filter)
        {
            SqlCommand cmd = new SqlCommand
            {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "Empleado_GetByFilter"
            };

            cmd.Parameters.AddRange(new SqlParameter[]
            {
                new SqlParameter() { ParameterName = "@Nombre",     Value = filter.Nombre,      SqlDbType = SqlDbType.VarChar },
                new SqlParameter() { ParameterName = "@Apellido",   Value = filter.Apellido,    SqlDbType = SqlDbType.VarChar }
            });

            SqlDataReader result = cmd.ExecuteReader();

            return result;
        }

        public DataSet EjecutarQueryConPaginado(SqlConnection connection, EmpleadoFilter filter)
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                adapter.SelectCommand = ConfigSelectCommand(connection, filter);

                DataSet ds = new DataSet();
                adapter.Fill(ds);

                if (DataSetHelper.HasRecords(ds) && filter.PaginateProperties != null)
                {
                    filter.PaginateProperties.RecordsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["total_records"]);
                }
                return ds;
            }
        }

        private SqlCommand ConfigSelectCommand(SqlConnection connection, EmpleadoFilter filter)
        {
            SqlCommand cmd = new SqlCommand
            {
                CommandText = "Empleado_GetByFilterPaginado",
                CommandType = CommandType.StoredProcedure,
                Connection = connection
            };
            cmd.Parameters.AddRange(new SqlParameter[]
            {
                new SqlParameter() { ParameterName = "@Nombre", Value = filter.Nombre, SqlDbType = SqlDbType.VarChar },
                new SqlParameter() { ParameterName = "@Apellido", Value = filter.Apellido, SqlDbType = SqlDbType.VarChar },
                new SqlParameter() { ParameterName = "@FechaNacimiento", Value = filter.FechaNacimiento, SqlDbType = SqlDbType.DateTime },
                new SqlParameter() { ParameterName = "@Sexo", Value = filter.Sexo, SqlDbType = SqlDbType.Char },
                new SqlParameter() { ParameterName = "@Salario", Value = filter.Salario, SqlDbType = SqlDbType.Decimal },
                new SqlParameter() { ParameterName = "@Cargo", Value = filter.Cargo, SqlDbType = SqlDbType.VarChar },
                new SqlParameter() { ParameterName = "@EmpresaId", Value = filter.EmpresaId, SqlDbType = SqlDbType.Int },
                
                //Parametros del Paginado
                new SqlParameter(){ ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Value = filter.PaginateProperties?.PageSize },
                new SqlParameter(){ ParameterName = "@PageIndex", SqlDbType = SqlDbType.Int, Value = filter.PaginateProperties?.PageIndex },
                new SqlParameter(){ ParameterName = "@SortBy", SqlDbType = SqlDbType.VarChar, Value = filter.PaginateProperties?.SortBy },
                new SqlParameter(){ ParameterName = "@Order", SqlDbType = SqlDbType.Int, Value = filter.PaginateProperties?.Order }
            });
            return cmd;
        }

        public void Dispose()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
                connection.Dispose();
            }
        }
    }
}
