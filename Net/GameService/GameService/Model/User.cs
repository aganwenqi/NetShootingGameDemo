using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameService.Model
{
    class User
    {
        public User(int id,string username,string password)
        {
            this.ID = id;
            this.Username = username;
            this.Password = password;
        }
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
