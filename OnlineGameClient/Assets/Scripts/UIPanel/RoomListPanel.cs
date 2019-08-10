using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Common;
public class RoomListPanel : BasePanel {

    private RectTransform battleRes;
    private RectTransform roomList;

    //异步
    private List<UserData> udList = null;
    private UserData ud1 = null;
    private UserData ud2 = null;

    private ListRoomRequest listRoomRequest;
    private CreateRoomRequest createRoomRequest;
    private JoinRoomRequest joinRoomRequest;
    //列表的引用
    private VerticalLayoutGroup roomLayout;
    private GameObject roomItemPrefab;

    private void Awake()
    {
        roomList = transform.Find("RoomList").GetComponent<RectTransform>();
        roomLayout = roomList.transform.Find("ScrollRect/Layout").GetComponent<VerticalLayoutGroup>();
        roomItemPrefab = Resources.Load("UIPanel/RoomItem") as GameObject;

        battleRes = transform.Find("BattleRes").GetComponent<RectTransform>();

        roomList.Find("CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseClick);
        roomList.Find("CreateRoomButton").GetComponent<Button>().onClick.AddListener(OnCreateRoomClick);
        roomList.Find("ReFreshButton").GetComponent<Button>().onClick.AddListener(OnRefClick);

        createRoomRequest = GetComponent<CreateRoomRequest>();
        listRoomRequest = GetComponent<ListRoomRequest>();
        joinRoomRequest = GetComponent<JoinRoomRequest>();
    }
    private void OnCreateRoomClick()//创建房间
    {
        BasePanel panel = uiMng.PushPanel(UIPanelType.Room);
        createRoomRequest.SetRoomPanel(panel);
        createRoomRequest.SendRequest();//发起创建房间的请求
    }
    public void OnRefClick()//刷新房间
    {
        listRoomRequest.SendRequest();
    }
    public override void OnEnter()
    {
        base.OnEnter();
        EnterAnim();
        SetBattleRes();
    }
    public override void OnPause()
    {
        base.OnPause();
        HideAnim();
    }
    public override void OnResume()
    {
        base.OnResume();
        EnterAnim();
    }
    public override void OnExit()
    {
        base.OnExit();
        HideAnim();
    }
    private void OnCloseClick()//关闭按钮
    {
        PlayClickSound();
        uiMng.PopPanel();
    }
    //进入动画
    private void EnterAnim()
    {
        gameObject.SetActive(true);
        battleRes.localPosition = new Vector3(-1000,0);
        battleRes.DOLocalMoveX(-357, 0.5f);

        roomList.localPosition = new Vector3(1000,0);
        roomList.DOLocalMoveX(148,0.5f);
    }
    //隐藏动画
    private void HideAnim()
    {
        battleRes.DOLocalMoveX(-1000, 0.5f);
        roomList.DOLocalMoveX(1000, 0.5f).OnComplete(()=>gameObject.SetActive(false));
    }

    private void SetBattleRes()//设置自己的信息
    {
        UserData ud = facede.GetUserData();
        battleRes.Find("Username").GetComponent<Text>().text = ud.Username;
        battleRes.Find("TotalCount").GetComponent<Text>().text = "总场数:" + ud.TotalCount.ToString();
        battleRes.Find("WinCount").GetComponent<Text>().text = "胜场:" + ud.WinCount.ToString();
    }
    public void OnUpdateResultResponse(int totalCount,int winCount)
    {
        facede.UpdateResult(totalCount,winCount);
        SetBattleRes();
    }
    public void LoadRoomItemSync(List<UserData> udList)
    {
        this.udList = udList;
    }
    private void LoadRoomItem(List<UserData> udList)//加载房间
    {
        RoomItem[] riArray = roomLayout.GetComponentsInChildren<RoomItem>();
        foreach (RoomItem ri in riArray)
            ri.DestroySelf();


        int count = udList.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject roomItem = GameObject.Instantiate(roomItemPrefab);
            roomItem.transform.SetParent(roomLayout.transform);
            UserData ud = udList[i];
            
            roomItem.GetComponent<RoomItem>().SetRoomInfo(ud.Id,ud.Username,ud.TotalCount,ud.WinCount,this);
        }
        //计算多少个子
        /*
        int roomCount = roomLayout.GetComponentsInChildren<RoomItem>().Length;
        Vector2 size = roomLayout.GetComponent<RectTransform>().sizeDelta;
        roomLayout.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x,
            (roomItemPrefab.GetComponent<RectTransform>().sizeDelta.y * roomCount)
            + (roomCount * roomLayout.spacing) - 420.4f
            );*/
        int roomCount = roomLayout.GetComponentsInChildren<RoomItem>().Length;
        Vector2 size = roomLayout.GetComponent<RectTransform>().sizeDelta;
        roomLayout.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x,
            roomCount * (roomItemPrefab.GetComponent<RectTransform>().sizeDelta.y + roomLayout.spacing) - 420.4f);
    }
    public void OnJoinClick(int id)
    {
        joinRoomRequest.SendRequest(id);
    }
    public void OnJoinResponse(ReturnCode returnCode,UserData ud1,UserData ud2)
    {
        switch(returnCode)
        {
            case ReturnCode.NotFound:
                uiMng.ShowMessageSync("房间被销毁无法加入");
                break;
            case ReturnCode.Fail:
                uiMng.ShowMessageSync("房间已满,无法加入");
                break;
            case ReturnCode.Success:
                this.ud1 = ud1;
                this.ud2 = ud2;
                break;
        }
    }
    private void Update()
    {
        if (udList != null)
        {
            LoadRoomItem(udList);
            udList = null;
        }

        if (ud1 != null && ud2 != null)
        {
            BasePanel panel = uiMng.PushPanel(UIPanelType.Room);
            (panel as RoomPanel).SetAllPlayerResSync(ud1, ud2);
            ud1 = null;
            ud2 = null;
        }
    }
}
