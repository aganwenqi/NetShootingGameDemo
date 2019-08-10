using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameService.Model;
using MySql.Data.MySqlClient;
namespace GameService.DAO
{
    class ResultDAO
    {
        public Result GetResultByUserid(MySqlConnection conn,int userId)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from result where userid = @userid", conn);
                cmd.Parameters.AddWithValue("userid", userId);
                reader = cmd.ExecuteReader();

                Result res = null;
                if (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    int totalcount = reader.GetInt32("totalcount");
                    int wincount = reader.GetInt32("wincount");

                    res = new Result(id, userId, totalcount, wincount);
                    
                }
                else
                {
                    res = new Result(-1, userId, 0, 0);
                }
                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine("在GetUserByUsername的时候出现异常：" + e);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return null;
        }
        public void UpdateOrAddResult(MySqlConnection conn, Result res)
        {
            try
            {
                MySqlCommand cmd = null;
                
                
                if (res.Id <= -1)
                {
                    cmd = new MySqlCommand("insert into result set totalcount =@totalcount,wincount=@wincount,userid=@userid", conn);
                    
                }
                else
                {
                    cmd = new MySqlCommand("update result set totalcount =@totalcount,wincount=@wincount where userid=@userid", conn);

                }
                cmd.Parameters.AddWithValue("totalcount", res.TotalCount);
                cmd.Parameters.AddWithValue("wincount", res.WinCount);
                cmd.Parameters.AddWithValue("userid", res.UserId);
                cmd.ExecuteNonQuery();
                if (res.Id <= -1)
                {
                    Result tempRes = GetResultByUserid(conn,res.UserId);
                    res.Id = tempRes.Id;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在UpdateOrAddResult的时候出现异常：" + e);
            }
        }
    }
}
