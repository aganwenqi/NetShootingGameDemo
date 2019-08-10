using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class BaseRequest : MonoBehaviour {

    protected RequestCode requestCode = RequestCode.None;
    protected ActionCode actionCode = ActionCode.None;
    protected GameFacade facade;
	// Use this for initialization
	public virtual void Awake () {
        GameFacade.Instance.AddRequest(actionCode, this);
        facade = GameFacade.Instance;
	}
    protected void SendRequest(string data)
    {
        facade.SendRequest(requestCode, actionCode, data);
    }
    public virtual void SendRequest()
    {

    }
    public virtual void OnResponse(string data)
    {

    }
    public virtual void OnDestroy()
    {
        if(facade != null)
            facade.RemoveRequest(actionCode);
    }
}
