using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class ShootRequest :BaseRequest{

    public PlayManager playerMng;
    private bool isShoot = false;
    private RoleType rt;
    private Vector3 pos;
    private Vector3 rotation;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Shoot;
        base.Awake();
    }
    private void Update()
    {
        if (isShoot)
        {
            playerMng.RemoteShoot(rt,pos,rotation);
            isShoot = false;
        }
    }
    public void SendRequest(RoleType rt,Vector3 pos,Vector3 rotation)
    {
        string data = string.Format("{0}|{1},{2},{3}|{4},{5},{6}", (int)rt, pos.x, pos.y, pos.z, rotation.x, rotation.y, rotation.z);
        base.SendRequest(data);
    }
    public override void OnResponse(string data)
    {
        string[] strs = data.Split('|');
        rt = (RoleType)int.Parse(strs[0]);
        pos = UnityTools.PareVector3(strs[1]);
        rotation = UnityTools.PareVector3(strs[2]);
        isShoot = true;
    }
}
