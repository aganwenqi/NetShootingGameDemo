using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace GameService.Tools
{
    class ConnHelper
    {
        public const string CONNECTIONSTRING = "Database=game01;Data Source=127.0.0.1;port=3306;User Id=root; Password=ganwenqi12";

        public static MySqlConnection Connect()
        {
            MySqlConnection conn = new MySqlConnection(CONNECTIONSTRING);
            try
            {
                conn.Open();
                return conn;
            }
            catch (Exception e)
            {
                Console.WriteLine("链接数据库的时候实现异常：" + e);
                return null;
            }

        }
        public static void CloseConnection(MySqlConnection conn)
        {
            if (conn != null)
                conn.Close();
            else
            {
                Console.WriteLine("MySqlConnection不能为空");
            }
        }
    }
}
