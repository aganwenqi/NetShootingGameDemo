using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace MySqlController
{
    class Program
    {
        static void Main(string[] args)
        {
            string dataStr = "Database=test007;Data Source=127.0.0.1;port=3306;User Id=root; Password=ganwenqi12";
            //链接mysql数据库
            MySqlConnection conn = new MySqlConnection(dataStr);
            conn.Open();

            #region 查询
            /*
            //Sql指令，和查询的数据库
            MySqlCommand cmd = new MySqlCommand("select * from user",conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string username = reader.GetString("username");
                string password = reader.GetString("password");
                Console.WriteLine(username+"  "+password);
            }
            
            reader.Close();
            */
            #endregion
            #region 插入
            /*
            string username = "m004";string password = "ganwenqi";
            //MySqlCommand cmd = new MySqlCommand("insert into user set username ='" + username+"'"+",password = '"+password+"'",conn);
            MySqlCommand cmd = new MySqlCommand("insert into user set username=@un , password = @pwd",conn);
            cmd.Parameters.AddWithValue("un",username);
            cmd.Parameters.AddWithValue("pwd", password);
            //插入
            cmd.ExecuteNonQuery();*/
            #endregion
            #region 删除
            /*
            MySqlCommand cmd = new MySqlCommand("delete from user where id = @id",conn);
            cmd.Parameters.AddWithValue("id",1);
            cmd.ExecuteNonQuery();*/
            #endregion
            #region 更新
            /*
            MySqlCommand cmd = new MySqlCommand("update user set password = @pwd where id = @id",conn);
            cmd.Parameters.AddWithValue("id",02);
            cmd.Parameters.AddWithValue("pwd", "sdfsdf");
            cmd.ExecuteNonQuery();*/
            #endregion
            conn.Close();
        }
    }
}
