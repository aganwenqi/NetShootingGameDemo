using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Common;
using MySql.Data.MySqlClient;
using GameService.Tools;
using GameService.Model;
using GameService.DAO;
namespace GameService.Servers
{
    class Client
    {
        private Socket clientSocket;
        private Server server;
        private Message msg = new Message();
        private MySqlConnection mysqlConn;

        private Room room;
        private User user;
        private Result result;
        private ResultDAO resultDAO = new ResultDAO();
        public MySqlConnection MysqlConn { get => mysqlConn; }
        internal User User { get => user; set => user = value; }
        internal Result Result { get => result; set => result = value; }
        internal Room Room { get => room; set => room = value; }
        public int Hp { get; set; }

        public bool TakeDamage(int damage)
        {
            Hp -= damage;
            Hp = Math.Max(Hp, 0);
            if (Hp <= 0) return true;
            return false;
        }
        public bool isDie()
        {
            return Hp <= 0;
        }
        public string GetUserData()
        {
            return user.ID+","+ user.Username + "," + result.TotalCount + "," + result.WinCount;
        }
        public Client()
        {

        }
        public Client(Socket clientSocket, Server server)
        {
            this.clientSocket = clientSocket;
            this.server = server;
            //建立数据库链接
            mysqlConn = ConnHelper.Connect();
        }
        //开启监听
        public void Start()
        {
            if (clientSocket == null || clientSocket.Connected == false) return;
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.ReaminSize, SocketFlags.None, ReceiveCallBack, null);
        }
        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                if (clientSocket == null || clientSocket.Connected == false) return;
                int count = clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    Close();
                }
                //开始接收数据
                msg.ReadMessage(count, OnProcessMessage);
                Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("接收异常："+e);
                Close();
            }
            
        }
        private void OnProcessMessage(RequestCode requestCode, ActionCode actionCode, string data)
        {
            server.HandleRequest(requestCode, actionCode, data,this);
        }
        private void Close()
        {
            //关闭
            ConnHelper.CloseConnection(MysqlConn);
            if (clientSocket != null)
            {
                clientSocket.Close();
                
            }
            if (room != null)
            {
                room.QuitRoom(this);
            }
            server.RemoveClient(this);
            
        }
        public void Send(ActionCode actionCode, string data)
        {
            try
            {
                byte[] bytes = Message.PackData(actionCode, data);
                clientSocket.Send(bytes);
            }
            catch (Exception e)
            {
                Console.WriteLine("无法发送消息:" + e);
            }
        }
        public bool IsHouseOwner()
        {
            return room.IsHouserOwner(this);
        }
        public void UpdateResult(bool isVictory)
        {
            UpdateResultToDB(isVictory);
            UpdateResultToClient();
        }
        private void UpdateResultToDB(bool isVictory)
        {
            result.TotalCount++;
            if (isVictory)
            {
                result.WinCount++;
            }
            resultDAO.UpdateOrAddResult(mysqlConn, result);
        }
        private void UpdateResultToClient()
        {
            Send(ActionCode.UpdateResult, string.Format("{0},{1}", result.TotalCount, result.WinCount));
        }
    }
}
