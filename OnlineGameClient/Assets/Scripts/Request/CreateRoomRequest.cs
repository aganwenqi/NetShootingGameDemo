using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class CreateRoomRequest : BaseRequest{

    private RoomPanel roomPanel;
    public override void Awake()
    {
        //roomPanel = GetComponent<RoomPanel>();
        requestCode = RequestCode.Room;
        actionCode = ActionCode.CreateRoom;
        base.Awake();
    }
    private void OnEnable()
    {
        //SendRequest();
    }
    public void SetRoomPanel(BasePanel panel)
    {
            roomPanel = panel as RoomPanel;
    }
    public override void SendRequest()
    {
        Debug.Log("创建房间");
        base.SendRequest("r");
    }
    public override void OnResponse(string data)
    {
        base.OnResponse(data);
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        RoleType roleType = (RoleType)int.Parse(strs[1]);
        facade.SetCurrentRoleType(roleType);
        if (returnCode == ReturnCode.Success)
        {
            roomPanel.SetLocalPlayerResSync();
            //roomPanel.CrearEnemyPlayerRes();
        }
    }
}
