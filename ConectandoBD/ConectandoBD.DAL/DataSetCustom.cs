using System.Data;
using System.Data.SqlClient;

namespace ConectandoBD.DAL
{
    public class DataSetCustom
    {
        private readonly SqlDataAdapter adapter;

        public DataSetCustom(SqlConnection connection)
        {
            DataAdapterCustom adapter = new DataAdapterCustom();
            this.adapter = adapter.GetAdapter(connection);
        }

        public DataSet GetEmpleados()
        {
            DataSet empleadosDS = new DataSet();

            adapter.Fill(empleadosDS);
            return empleadosDS;
        }
    }
}
