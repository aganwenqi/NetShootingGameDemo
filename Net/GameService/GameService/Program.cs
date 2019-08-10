using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameService.Servers;
namespace GameService
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server("127.0.0.1", 8888);
            server.Start();
            Console.ReadKey();
        }
    }
}
