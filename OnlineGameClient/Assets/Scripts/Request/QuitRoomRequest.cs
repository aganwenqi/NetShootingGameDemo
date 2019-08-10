using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class QuitRoomRequest : BaseRequest {
    private RoomPanel roomPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        roomPanel = GetComponent<RoomPanel>();
        actionCode = ActionCode.QuitRoom;
        base.Awake();
    }
    public override void SendRequest()
    {
        base.SendRequest("r");
    }
    public override void OnResponse(string data)
    {
        ReturnCode returnCode = (ReturnCode)int.Parse(data);
        if(returnCode == ReturnCode.Success)
        {
            roomPanel.OnExitResponse();
        }
    }
}
