using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFacade : MonoBehaviour {

    private static GameFacade _instance;
    public static GameFacade Instance { get { return _instance; } }
    private UIManager uiMng;
    private AUdioManager audioMng;
    private PlayManager playerMng;
    private CameraManager cameraMng;
    private RequestManager requestMng;
    private ClientManager client;

    private bool isEnterPlaying = false;
    private void Awake()
    {
        if (_instance != null)
            Destroy(this.gameObject);
        else
            _instance = this;
    }
    void Start () {
        InitManager();
    }
    private void Update()
    {
        UpdateManager();
        if (isEnterPlaying)
        {
            EnterPlaying();
            isEnterPlaying = false;
        }
    }
    private void OnDestroy()
    {
        DestroyManager();
    }
    private void InitManager()
    {
        uiMng = new UIManager(this);
        audioMng = new AUdioManager(this);
        playerMng = new PlayManager(this);
        cameraMng = new CameraManager(this);
        requestMng = new RequestManager(this);
        client = new ClientManager(this);
        uiMng.OnInit();
        audioMng.OnInit();
        playerMng.OnInit();
        cameraMng.OnInit();
        requestMng.OnInit();
        client.OnInit();

    }
    private void UpdateManager()
    {
        uiMng.Update();
        audioMng.Update();
        playerMng.Update();
        cameraMng.Update();
        requestMng.Update();
        client.Update();
    }
    private void DestroyManager()
    {
        uiMng.OnDestory();
        audioMng.OnDestory();
        playerMng.OnDestory();
        cameraMng.OnDestory();
        requestMng.OnDestory();
        client.OnDestory();
    }
    
    public void AddRequest(ActionCode actionsCode, BaseRequest request)
    {
        requestMng.AddRequset(actionsCode, request);
    }
    public void RemoveRequest(ActionCode actionsCode)
    {
        requestMng.RemoveRequest(actionsCode);
    }
    public void HandleReponse(ActionCode actionsCode, string data)
    {
        requestMng.HandleReponse(actionsCode, data);
    }
    public void ShowMessage(string msg)
    {
        uiMng.ShowMessageSync(msg);
    }
    public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data)
    {
        client.SendRequest(requestCode,actionCode,data);
    }

    //播放音乐
    public void PlayBgSound(string soundName)
    {
        audioMng.PlayBgSound(soundName);
    }
    public void PlayNormalSound(string soundName)
    {
        audioMng.PlayNormalSound(soundName);
    }

    //数据
    public void SetUserData(UserData ud)
    {
        playerMng.UserData = ud;
    }
    public UserData GetUserData()
    {
        return playerMng.UserData;
    }

    public void SetCurrentRoleType(RoleType rt)
    {
        playerMng.SetCurrentRoleType(rt);
    }
    public GameObject GetCurrentRoleGameObject()
    {
        return playerMng.GetCurrentRoleGameObject();
    }

    //进入游玩界面
    public void EnterPlayingSync()
    {
        isEnterPlaying = true;
    }
    private void EnterPlaying()
    {
        playerMng.SpawnRoles();//生成角色
        cameraMng.FollowRole();//角色跟随
    }
    public void StartPlaying()
    {
        playerMng.AddControlScript();
        playerMng.CreateSyncRequest();
    }
    public void SendAttack(int damage)
    {
        playerMng.SendAttack(damage);
    }
    public void GameOver()
    {
        cameraMng.WalkthroughScene();
        playerMng.GameOver();
    }
    public void UpdateResult(int totalCount, int winCount)
    {
        playerMng.UpdateResult(totalCount, winCount);
    }
}
