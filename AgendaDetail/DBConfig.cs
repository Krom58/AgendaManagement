using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaDetail
{
    public static class DBConfig
    {
        // Connection string ที่ใช้สำหรับเชื่อมต่อฐานข้อมูล
        public static string connectionString { get; } = "Server=10.10.0.42\\SQLSET;Database=ExcelDataDB;User Id=sa;Password=Wutt@1976;Trusted_Connection=False;";
    }
}
