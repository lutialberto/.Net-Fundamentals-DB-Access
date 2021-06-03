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
                SelectCommand = ConfigSelectCommand(connection)
            };
            return adapter;
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
