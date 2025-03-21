using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work1
{
    public static class DBConfig
    {
        // Connection string ที่ใช้สำหรับเชื่อมต่อฐานข้อมูล
        public static string connectionString { get; } = "Data Source=KROM\\SQLEXPRESS;Initial Catalog=ExcelDataDB;Integrated Security=True;";
    }
}
