using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using GameService.Controller;
using Common;
namespace GameService.Servers
{
    class Server
    {
        private IPEndPoint ipEndPoint;
        private Socket serverSocket;
        private ControllerManager controllerManager;
        //所有的客户端
        private List<Client> clientList = new List<Client>();
        //所有房间
        private List<Room> roomList = new List<Room>();
        public Server()
        {

        }
        public Server(string ipStr, int port)
        {
            controllerManager = new ControllerManager(this);
            SetIpAndPort(ipStr,port);
        }
        public void SetIpAndPort(string ipStr, int port)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
        }
        public void Start()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipEndPoint);
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallBack, null);
        }
        private void AcceptCallBack(IAsyncResult ar)
        {
            Socket clientSocket = serverSocket.EndAccept(ar);
            Client client = new Client(clientSocket,this);
            client.Start();
            clientList.Add(client);
            serverSocket.BeginAccept(AcceptCallBack, null);
        }
        //移除客户
        public void RemoveClient(Client client)
        {
            //加锁
            lock (clientList)
            {
                clientList.Remove(client);
            } 
        }
        public void SendResponse(Client client, ActionCode actionCode, string data)
        {
            //给客户端响应
            Console.WriteLine("发送了：" + data);
            client.Send(actionCode, data);
        }
        //中介
        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            controllerManager.HandleRequest(requestCode, actionCode, data, client);
        }
        public void CreateRoom(Client client)//创建房间
        {
            Room room = new Room(this);
            room.AddClient(client);
            roomList.Add(room);
        }
        public void RemoveRoom(Room room)
        {
            if(roomList != null && room != null)
            {
                roomList.Remove(room);
            }
        }
        public List<Room> GetRoomList()//获取房间
        {
            return roomList;
        }
        public Room GetRoomById(int id)
        {
            foreach (Room room in roomList)
            {
                if (room.GetId() == id)
                    return room;
            }
            return null;
        }
    }
}
