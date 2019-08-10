using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class RequestManager : BaseManager
{
    public RequestManager(GameFacade facade) : base(facade) { }
    private Dictionary<ActionCode, BaseRequest> requestDict = new Dictionary<ActionCode, BaseRequest>();

    public void AddRequset(ActionCode actionsCode,BaseRequest request)
    {
        requestDict.Add(actionsCode, request);
    }
    public void RemoveRequest(ActionCode actionsCode)
    {
        requestDict.Remove(actionsCode);
    }
    public void HandleReponse(ActionCode actionsCode, string data)
    {
        
        BaseRequest request = requestDict.TryGet<ActionCode, BaseRequest>(actionsCode);//从字典里获取数据

        if (request == null)
        {
            Debug.LogWarning("无法得到actionsCode[" + actionsCode + "]对应的Request类"); return;
        }
        request.OnResponse(data);
    }
}
