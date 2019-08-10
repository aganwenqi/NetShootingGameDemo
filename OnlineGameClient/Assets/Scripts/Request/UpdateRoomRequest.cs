using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class UpdateRoomRequest : BaseRequest {
    private RoomPanel roomPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.UpdateRoom;
        roomPanel = GetComponent<RoomPanel>();
        base.Awake();
    }
    public override void OnResponse(string data)
    {
        UserData ud1 = null;
        UserData ud2 = null;
        string[] udsStrArray = data.Split('|');
        ud1 = new UserData(udsStrArray[0]);
        if(udsStrArray.Length > 1)
            ud2 = new UserData(udsStrArray[1]);
        roomPanel.SetAllPlayerResSync(ud1,ud2);
    }
}
