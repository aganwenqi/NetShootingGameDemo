using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameService;
using System.Reflection;
using GameService.Servers;
namespace GameService.Controller
{
    class ControllerManager
    {
        private Dictionary<RequestCode, BaseController> controllerDict = new Dictionary<RequestCode, BaseController>();
        private Server server;

        public ControllerManager(Server server)
        {
            this.server = server;
            Init();
        }
        void Init()
        {
            DefultController defaultController = new DefultController();
            controllerDict.Add(defaultController.RequesCode, defaultController);
            controllerDict.Add(RequestCode.User,new UserController());
            controllerDict.Add(RequestCode.Room, new RoomController());
            controllerDict.Add(RequestCode.Game, new GameController());
        }
        public void HandleRequest(RequestCode requestCode,ActionCode actionCode,string data,Client client)
        {
            BaseController controller;
            //获取数据
           bool isGet = controllerDict.TryGetValue(requestCode,out controller);
            if (isGet == false)
            {
                Console.WriteLine("无法得到"+requestCode +"所对应Controller,无法处理请求");
                return;
            }
            //反射

            string methodName = Enum.GetName(typeof(ActionCode),actionCode);
            MethodInfo mi = controller.GetType().GetMethod(methodName);
            if (mi == null)
            {
                Console.WriteLine("[警告]在Controller["+controller.GetType()+"]没有对应的处理方法：["+methodName+"]");return;
            }
            object[] paremeters = new object[] { data ,client,server};
            object o = mi.Invoke(controller, paremeters);
            if (o == null || string.IsNullOrEmpty(o as string))
            {
                return;
            }
            server.SendResponse(client, actionCode, o as string);
        }
    }
}
