using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraManager : BaseManager {
    public CameraManager(GameFacade facade) : base(facade) { }
    private GameObject cameraGo;
    private Animator camerAnim;
    private FollowTarget followtarget;
    //初始位置
    private Vector3 originalPosition;
    private Vector3 originalRotation;
    public override void OnInit()
    {
        base.OnInit();
        cameraGo = Camera.main.gameObject;
        camerAnim = cameraGo.GetComponent<Animator>();
        followtarget = cameraGo.GetComponent<FollowTarget>();
        
    }
    //public override void Update()
    //{
    //    //base.Update();
    //    if (Input.GetMouseButtonDown(0))
    //        FollowTarget(null);
    //    else if (Input.GetMouseButtonDown(1))
    //        WalkthroughScene();
    //}
    public void FollowRole()
    {
        followtarget.target = facade.GetCurrentRoleGameObject().transform;
        camerAnim.enabled = false;
        originalPosition = cameraGo.transform.position;
        originalRotation = cameraGo.transform.eulerAngles;
        Quaternion targetQuaternion = Quaternion.LookRotation(followtarget.target.position - cameraGo.transform.position);
        cameraGo.transform.DORotateQuaternion(targetQuaternion,0.5f).OnComplete(delegate()
        { 
            followtarget.enabled = true;
        });
        
        
    }
    public void WalkthroughScene()
    {
        followtarget.enabled = false;
        cameraGo.transform.DOMove(originalPosition,1f);
        cameraGo.transform.DORotate(originalRotation, 1f).OnComplete(delegate () 
        {
            camerAnim.enabled = true;
        });
    }
}
