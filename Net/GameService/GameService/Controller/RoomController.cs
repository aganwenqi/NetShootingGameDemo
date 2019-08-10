using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameService.Servers;

namespace GameService.Controller
{
    class RoomController:BaseController
    {
        public RoomController()
        {
            requestCode = RequestCode.Room;
        }
        public string CreateRoom(string data, Client client, Server server)
        {
            server.CreateRoom(client);
            return ((int)ReturnCode.Success).ToString()+","+ ((int)RoleType.Blue).ToString();
        }
        public string ListRoom(string data, Client client, Server server)
        {
            StringBuilder sb = new StringBuilder();
            foreach(Room room in server.GetRoomList())
            {
                if (room.IsWaitngJoin())
                {
                    sb.Append(room.GetHouserOwnerData()+"|");
                }
            }
            if(sb.Length == 0)
            {
                sb.Append("0");
            }
            else
            {
                sb.Remove(sb.Length-1,1);
            }
            return sb.ToString();
        }
        public string JoinRoom(string data, Client client, Server server)
        {
            int id = int.Parse(data);
            Room room = server.GetRoomById(id);
            if (room == null)
            {
                //没找到房间
                return ((int)ReturnCode.NotFound).ToString();
            }
            else if (room.IsWaitngJoin() == false)
            {
                //不可加入
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                //可以加入
                room.AddClient(client);
                string roomData = room.GetRoomData();
                room.BroadcastMessage(client,ActionCode.UpdateRoom,roomData);//username,tc,wc|id,username,tc,wc
                return ((int)ReturnCode.Success).ToString()+","+((int)RoleType.Red).ToString()+"-" +roomData;//"return code,roletype-id,username,tc,wc|id,username,tc,wc"
            }
        }
        public string QuitRoom(string data, Client client, Server server)
        {
            bool isHouseOwner = client.IsHouseOwner();
            Room room = client.Room;
            if (isHouseOwner)//我是房主，我要退出
            {
                room.BroadcastMessage(client,ActionCode.QuitRoom, ((int)ReturnCode.Success).ToString());
                room.Close();
            }
            else
            {
                
                client.Room.RemoveClient(client);
                room.BroadcastMessage(client, ActionCode.UpdateRoom, room.GetRoomData());
            }
            return ((int)ReturnCode.Success).ToString();
        }
    }
}
