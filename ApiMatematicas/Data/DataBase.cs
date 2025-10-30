using Npgsql;
using Microsoft.Extensions.Configuration;

namespace ApiMatematicas.Data
{
    public sealed class Database
    {
        private static Database? _instance;
        private readonly string _connectionString;

        private Database(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Inicialización desde DI
        public static Database Initialize(string connectionString)
        {
            if (_instance == null)
            {
                _instance = new Database(connectionString);
            }
            return _instance;
        }

        public static Database Instance
        {
            get
            {
                if (_instance == null)
                    throw new Exception("Database no inicializada. Llama a Initialize primero.");
                return _instance;
            }
        }

        public NpgsqlConnection GetConnection()
        {
            var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}
