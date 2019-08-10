using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class MoveRequest : BaseRequest {
    private Transform localPlayerTransform;
    private PlayerMove localPlayerMove;
    private int syncRate = 60;

    private Transform remotePlayerTransform;
    private Animator remotePlayerAnim;

    private bool isSyncRemotePlayer = false;
    private Vector3 pos;
    private Vector3 rotation;
    private float forward;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Move;
        base.Awake();
    }
    private void Start()
    {
        //打开携程
        InvokeRepeating("SyncLocalPlayer",1,1f / syncRate);//马上开始，1秒调用20次
    }
    private void FixedUpdate()
    {
        if (isSyncRemotePlayer)
        {
            SyncRemotePlayer();
            isSyncRemotePlayer = false;
        }
    }
    public MoveRequest SetLocalPlayer(Transform localPlayerTransform, PlayerMove localPlayerMove)
    {
        this.localPlayerTransform = localPlayerTransform;
        this.localPlayerMove = localPlayerMove;
        return this;
    }
    public MoveRequest SetRemotePlayer(Transform remotePlayerTransform)
    {
        this.remotePlayerTransform = remotePlayerTransform;
        this.remotePlayerAnim = remotePlayerTransform.GetComponent<Animator>();
        return this;
    }
    private void SyncLocalPlayer()//1秒调用20次
    {
        SendRequest(localPlayerTransform.position.x, localPlayerTransform.position.y, localPlayerTransform.position.z,
            localPlayerTransform.eulerAngles.x, localPlayerTransform.eulerAngles.y, localPlayerTransform.eulerAngles.z,
            localPlayerMove.forward
            );
    }
    private void SyncRemotePlayer()
    {
        remotePlayerTransform.position = pos;
        remotePlayerTransform.eulerAngles = rotation;
        remotePlayerAnim.SetFloat("Forward",forward);
    }
    private void SendRequest(float x,float y,float z,float rotationX,float rotationY,float rotationZ,float forward)
    {
        string data = string.Format("{0},{1},{2}|{3},{4},{5}|{6}", x, y, z, rotationX, rotationY, rotationZ, forward);
        base.SendRequest(data);
    }
    public override void OnResponse(string data)
    {
        string[] strs = data.Split('|');
        pos =UnityTools.PareVector3(strs[0]);
        rotation = UnityTools.PareVector3(strs[1]);
        forward = float.Parse(strs[2]);
        isSyncRemotePlayer = true;
    }
    
}
