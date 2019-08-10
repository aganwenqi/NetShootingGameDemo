using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
namespace TcpServices
{
    class Program
    {
        static void Main(string[] args)
        {
            StartServerAsysc();
            Console.ReadKey();
        }
        static void StartServerAsysc()
        {
            //ip4,流，协议
            Socket socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 8888);
            socketServer.Bind(ipEndPoint);//绑定端口和ip
            socketServer.Listen(50);//开始监听，监听队列最多只有50个人，设置0表示无限制

            // Socket clientSocket = socketServer.Accept();//接受一个客户端
            socketServer.BeginAccept(AcceptCallBack,socketServer);
            
        }
        static Message msg = new Message();
        static void AcceptCallBack(IAsyncResult ar)
        {
            Socket socketServer = ar.AsyncState as Socket;
            Socket clientSocket = socketServer.EndAccept(ar);

            string msgStr = "hello_德玛西亚";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(msgStr);//把字符串转换为byte数组
            clientSocket.Send(data);//发送消息

            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.ReaminSize, SocketFlags.None, ReceiveCallBack, clientSocket);//异步
            socketServer.BeginAccept(AcceptCallBack, socketServer);
        }
        static byte[] dataBuffer = new byte[1024];//存放数据
        static void ReceiveCallBack(IAsyncResult ar)
        {
            Socket clientSocket = null;
            try
            {
                clientSocket = ar.AsyncState as Socket;
                int count = clientSocket.EndReceive(ar);//解析消息
                if (count == 0)
                {
                    clientSocket.Close();
                    return;
                }
                msg.AddCount(count);//添加字符
                msg.ReadMessage();
                //string msga = Encoding.UTF8.GetString(dataBuffer, 0, count);
                //Console.WriteLine("接受到的数据：" + msga);
                //clientSocket.BeginReceive(dataBuffer, 0, 1024, SocketFlags.None, ReceiveCallBack, clientSocket);
                clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.ReaminSize, SocketFlags.None, ReceiveCallBack, clientSocket);//异步

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (clientSocket != null)
                    clientSocket.Close();
            } 
        }
        static void StartServerSync()
        {
            //ip4,流，协议
            Socket socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 8888);
            socketServer.Bind(ipEndPoint);//绑定端口和ip
            socketServer.Listen(50);//开始监听，监听队列最多只有50个人，设置0表示无限制

            Socket clientSocket = socketServer.Accept();//接受一个客户端
            string msg = "hello_德玛西亚";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);//把字符串转换为byte数组
            clientSocket.Send(data);//发送消息

            //接受客服端消息
            byte[] dataBuffer = new byte[1024];
            int count = clientSocket.Receive(dataBuffer);
            string msgReceive = System.Text.Encoding.UTF8.GetString(dataBuffer, 0, count);//接受范围
            Console.WriteLine(msgReceive);

            clientSocket.Close();
            socketServer.Close();
        }
    }
}
