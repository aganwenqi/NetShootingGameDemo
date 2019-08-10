using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using Common;
//socket链接
public class ClientManager:BaseManager
{
    private const string IP = "127.0.0.1";
    private const int PORT = 8888;
    private Socket clientSocket;

    private Message msg = new Message();
    public ClientManager(GameFacade facade) : base(facade) { }
    public override void OnInit()
    {
        base.OnInit();
        clientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        try
        {
            clientSocket.Connect(IP, PORT);
            Start();
        }
        catch (Exception e)
        {
            Debug.LogWarning("无法链接到服务器端，请检查您的网络" +e);
        }
        
    }
    //开始监听
    private void Start()
    {
        if (clientSocket == null || clientSocket.Connected == false) return;//判断是否空或断开
        clientSocket.BeginReceive(msg.Data,msg.StartIndex,msg.ReaminSize,SocketFlags.None, ReceiveCallBack,null);
    }
    private void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            if (clientSocket == null || clientSocket.Connected == false) return;//判断是否空或断开
            int count = clientSocket.EndReceive(ar);//接收了多少数据
            msg.ReadMessage(count, OnProcessDataCallBack);
            Start();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    //接收解析的数据
    public void OnProcessDataCallBack(ActionCode actionCode, string data)
    {
        facade.HandleReponse(actionCode, data);
    }
    public void SendRequest(RequestCode requestCode,ActionCode actionCode,string data)
    {
        //包装
        byte[] bytes = Message.PackData(requestCode,actionCode,data);
        clientSocket.Send(bytes);
    }
    public override void OnDestory()
    {
        base.OnDestory();
        try
        {
            clientSocket.Close();
        }
        catch (Exception e)
        {
            Debug.LogWarning("无法关闭跟服务器的连接" + e);
        }
    }
}
