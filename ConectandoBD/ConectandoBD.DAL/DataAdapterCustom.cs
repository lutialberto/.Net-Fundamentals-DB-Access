using System;
using System.Data;
using System.Data.SqlClient;

namespace ConectandoBD.DAL
{
    public class DataAdapterCustom
    {
        public SqlDataAdapter GetAdapter(SqlConnection connection)
        {
            SqlDataAdapter adapter = new SqlDataAdapter
            {
                SelectCommand = ConfigSelectCommand(connection),
                UpdateCommand = ConfigUpdateCommand(connection),
                InsertCommand = ConfigInsertCommand(connection)
            };
            return adapter;
        }
        private SqlCommand ConfigUpdateCommand(SqlConnection connection)
        {
            SqlCommand cmdUpdateEmpresa = new SqlCommand
            {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "Empleado_UpdateEmpresa"
            };
            cmdUpdateEmpresa.Parameters.AddRange(new SqlParameter[]
            {
                new SqlParameter() { ParameterName = "@Id", Value = 1, SqlDbType = SqlDbType.Int },
                new SqlParameter() { ParameterName = "@EmpresaNombre", Value = "Google", SqlDbType = SqlDbType.VarChar }
            });
            return cmdUpdateEmpresa;
        }

        private SqlCommand ConfigInsertCommand(SqlConnection connection)
        {
            SqlCommand cmdInsert = new SqlCommand
            {
                Connection = connection,
                CommandType = CommandType.StoredProcedure,
                CommandText = "Empleado_Insert"
            };
            cmdInsert.Parameters.AddRange(new SqlParameter[]
            {
                new SqlParameter() { ParameterName = "@Nombre",             Value = "Luis",                     SqlDbType = SqlDbType.Int },
                new SqlParameter() { ParameterName = "@Apellido",           Value = "Gomez",                    SqlDbType = SqlDbType.VarChar },
                new SqlParameter() { ParameterName = "@FechaNacimiento",    Value = new DateTime(1993,10,30),   SqlDbType = SqlDbType.DateTime },
                new SqlParameter() { ParameterName = "@Sexo",               Value = "M",                        SqlDbType = SqlDbType.Char },
                new SqlParameter() { ParameterName = "@Cargo",              Value = "Programador",              SqlDbType = SqlDbType.VarChar },
                new SqlParameter() { ParameterName = "@Salario",            Value = 2000,                       SqlDbType = SqlDbType.Decimal },
                new SqlParameter() { ParameterName = "@EmpresaId",          Value = null,                       SqlDbType = SqlDbType.Int }
            });
            return cmdInsert;
        }

        private SqlCommand ConfigSelectCommand(SqlConnection connection)
        {
            return new SqlCommand
            {
                Connection = connection,
                CommandType = CommandType.Text,
                CommandText = "SELECT * FROM Empleado"
            };
        }

    }
}
