using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameService.Servers;
using GameService.DAO;
using GameService.Model;
namespace GameService.Controller
{
    class UserController:BaseController
    {
        private UserDAO userDAO = new UserDAO();
        private ResultDAO resultDAO = new ResultDAO();
        public UserController()
        {
            requestCode = RequestCode.User;
        }
        public string Login(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            User user = userDAO.VerifyUser(client.MysqlConn, strs[0], strs[1]);
            if (user == null)
            {
                //验证失败
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                Result res = resultDAO.GetResultByUserid(client.MysqlConn,user.ID);
                client.User = user;
                client.Result = res;
                //状态，用户名，用户战绩，用户胜场
                return string.Format("{0},{1},{2},{3}", ((int)ReturnCode.Success).ToString(),user.Username,res.TotalCount,res.WinCount);
            }
        }
        public string Register(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            string username = strs[0];string password = strs[1];
            bool res = userDAO.GetUserByUsername(client.MysqlConn, username);
            if (res)
            {
                //验证失败
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                userDAO.AddUser(client.MysqlConn, username, password);
                return ((int)ReturnCode.Success).ToString();
            }
        }
    }
}
