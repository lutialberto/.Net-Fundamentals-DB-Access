namespace ConectandoBD.Utils
{
    public static class Configuration
    {
        private const string Server = "localhost\\SQLEXPRESS";
        private const string DBName = "CursoPuntoNet2021";
        public static string GetConnectionString()
        {
            return string.Concat(
                "Data Source=",
                Server,
                ";",
                "Initial Catalog=",
                DBName,
                ";Integrated Security=true;"
            );
        }
    }
}
