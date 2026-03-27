using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Data.Common;
using IniParser;
using IniParser.Model;
using Npgsql;

namespace Work1
{
    public class DatabaseConfig
    {
        public string Type { get; set; } // "MSSMS", "PostgreSQL", "MySQL", "MariaDB"
        public string Provider { get; set; } // e.g., "System.Data.SqlClient", "Npgsql", "MySql.Data.MySqlClient"
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
    }

    public class DBConfig
    {
        public DatabaseConfig Config { get; }

        public DBConfig()
            : this(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database_config.ini"))
        { }

        public DBConfig(string iniFilePath)
        {
            if (!File.Exists(iniFilePath))
                throw new FileNotFoundException("ไม่พบไฟล์ INI ที่ระบุ", iniFilePath);

            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(iniFilePath);

            var section = data["Database"];
            if (section == null)
                throw new InvalidOperationException("ไม่พบ section [Database] ในไฟล์ .ini");

            Config = new DatabaseConfig
            {
                Type = section["type"],
                Provider = section["provider"] ?? DefaultProvider(section["type"]),
                Host = section["host"] ?? "localhost",
                Port = int.TryParse(section["port"], out var p) ? p : DefaultPort(section["type"]),
                User = section["user"],
                Password = section["password"],
                Database = section["database"]
            };

            if (string.IsNullOrEmpty(Config.Type) || string.IsNullOrEmpty(Config.Provider))
                throw new InvalidOperationException("Database Type และ Provider ต้องไม่เป็นค่าว่าง");
        }

        private string DefaultProvider(string type)
        {
            switch (type?.Trim().ToLowerInvariant())
            {
                case "postgresql":
                    return "Npgsql";
                case "mssql":
                case "mssms":
                    return "System.Data.SqlClient";
                case "mysql":
                case "mariadb":
                    return "MySql.Data.MySqlClient";
                default:
                    throw new NotSupportedException($"DB type '{type}' not supported");
            }
        }

        private int DefaultPort(string type)
        {
            switch (type?.Trim().ToLowerInvariant())
            {
                case "postgresql":
                    return 5432;
                case "mssql":
                case "mssms":
                    return 1433;
                case "mysql":
                case "mariadb":
                    return 3306;
                default:
                    throw new NotSupportedException($"DB type '{type}' not supported");
            }
        }

        public DbProviderFactory Factory
        {
            get
            {
                if (Config.Type.Trim().ToLowerInvariant() == "postgresql")
                    return NpgsqlFactory.Instance;
                if (Config.Type.Trim().ToLowerInvariant() == "mysql")
                    return MySql.Data.MySqlClient.MySqlClientFactory.Instance;

                return DbProviderFactories.GetFactory(Config.Provider);
            }
        }

        public string ConnectionString
        {
            get
            {
                switch (Config.Type.Trim().ToLowerInvariant())
                {
                    case "mssql":
                    case "mssms":
                        return $"Server={Config.Host},{Config.Port};Database={Config.Database};User Id={Config.User};Password={Config.Password};TrustServerCertificate=True;";
                    case "postgresql":
                        return $"Host={Config.Host};Port={Config.Port};Database={Config.Database};Username={Config.User};Password={Config.Password};SSLMode=Prefer;Trust Server Certificate=True;";
                    case "mysql":
                    case "mariadb":
                        return $"Server={Config.Host};Port={Config.Port};Database={Config.Database};Uid={Config.User};Pwd={Config.Password};";
                    default:
                        throw new NotSupportedException($"DB type '{Config.Type}' not supported");
                }
            }
        }

        public DbConnection CreateConnection()
        {
            var connection = Factory.CreateConnection();
            if (connection == null)
                throw new InvalidOperationException("ไม่สามารถสร้างการเชื่อมต่อได้");

            connection.ConnectionString = ConnectionString;
            return connection;
        }
    }
}
