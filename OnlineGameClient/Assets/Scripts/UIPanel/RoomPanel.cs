using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Common;
public class RoomPanel : BasePanel {

    private Text localPlayerUsername;
    private Text localPlayerTotalCount;
    private Text localPlayerWinCount;

    //敌方
    private Text enemyPlayerUsername;
    private Text enemyPlayerTotalCount;
    private Text enemyPlayerWinCount;

    //面板
    private Transform bluePanel;
    private Transform redPanel;
    private Transform startButton;
    private Transform exitButton;

    private CreateRoomRequest roomRequest;

    private UserData userData = null;
    private UserData userData1 = null;
    private UserData userData2 = null;

    private QuitRoomRequest quitRoomRequest;
    private StartGameRequest startGameRequest;
    //退出房间的异步
    private bool isPopPanel = false;
    private void Awake()
    {
        bluePanel = transform.Find("BluePanel");
        redPanel = transform.Find("RedPanel");
        startButton = transform.Find("StartButton");
        exitButton = transform.Find("ExitButton");

        localPlayerUsername = transform.Find("BluePanel/username").GetComponent<Text>();
        localPlayerTotalCount = transform.Find("BluePanel/totalCount").GetComponent<Text>();
        localPlayerWinCount = transform.Find("BluePanel/winCount").GetComponent<Text>();

        enemyPlayerUsername = transform.Find("RedPanel/username").GetComponent<Text>();
        enemyPlayerTotalCount = transform.Find("RedPanel/totalCount").GetComponent<Text>();
        enemyPlayerWinCount = transform.Find("RedPanel/winCount").GetComponent<Text>();

        transform.Find("StartButton").GetComponent<Button>().onClick.AddListener(OnStartClick);
        transform.Find("ExitButton").GetComponent<Button>().onClick.AddListener(OnExitClick);

        roomRequest = GetComponent<CreateRoomRequest>();
        quitRoomRequest = GetComponent<QuitRoomRequest>();
        startGameRequest = GetComponent<StartGameRequest>();
    }
    public override void OnEnter()
    {
        base.OnEnter();
        EnterAnim();
    }
    public override void OnPause()
    {
        base.OnPause();
        ExitAnim();
    }
    public override void OnResume()
    {
        base.OnResume();
        EnterAnim();
    }
    public override void OnExit()
    {
        base.OnExit();
        ExitAnim();
    }
    private void Update()
    {
        if (userData != null)
        {
            SetLocalPlayerRes(userData.Username, userData.TotalCount.ToString(), userData.WinCount.ToString());
            CrearEnemyPlayerRes();
            userData = null;
        }

        if (userData1 != null)
        {
            SetLocalPlayerRes(userData1.Username, userData1.TotalCount.ToString(), userData1.WinCount.ToString());
            if (userData2 != null)
                SetEnemyPlayerRes(userData2.Username, userData2.TotalCount.ToString(), userData2.WinCount.ToString());
            else
                CrearEnemyPlayerRes();
            userData1 = null;
            userData2 = null;
        }

        //退出房间
        if (isPopPanel)
        {
            uiMng.PopPanel();
            isPopPanel = false;
        }
    }
    public void SetLocalPlayerResSync()
    {
        userData = facede.GetUserData();
    }
    public void SetAllPlayerResSync(UserData ud1,UserData ud2)
    {
        this.userData1 = ud1;
        this.userData2 = ud2;
        
    }
    public void SetLocalPlayerRes(string username, string totalCount, string winCount)
    {
        localPlayerUsername.text = username;
        localPlayerTotalCount.text = "总场数:" + totalCount;
        localPlayerWinCount.text = "胜场:" + winCount;
    }
    public void SetEnemyPlayerRes(string username, string totalCount, string winCount)
    {
        enemyPlayerUsername.text = username;
        enemyPlayerTotalCount.text = "总场数:" + totalCount;
        enemyPlayerWinCount.text = "胜场:" + winCount;
    }
    public void CrearEnemyPlayerRes()
    {
        enemyPlayerUsername.text = "等待玩家加入";
        enemyPlayerTotalCount.text = "...";
        enemyPlayerWinCount.text = "";
    }
    private void OnStartClick()//开始按钮
    {
        startGameRequest.SendRequest();//发起请求
    }
    public void OnStartResponse(ReturnCode returnCode)//服务器返回开始按钮激活成功
    {
        if (returnCode == ReturnCode.Fail)
        {
            uiMng.ShowMessageSync("您不是房主,无法开始游戏！！");
        }
        else
        {
            //切换游戏面板
            uiMng.PushPanelSync(UIPanelType.Game);
            facede.EnterPlayingSync();
        }
    }
    private void OnExitClick()//退出房间按钮
    {
        quitRoomRequest.SendRequest();
    }
    public void OnExitResponse()//服务器说你退出成功
    {
        isPopPanel = true;
        
    }
    private void EnterAnim()
    {
        gameObject.SetActive(true);
        bluePanel.localPosition = new Vector3(-1000,0,0);
        bluePanel.DOLocalMoveX(-224,0.4f);
        redPanel.localPosition = new Vector3(1000,0,0);
        redPanel.DOLocalMoveX(132,0.4f);
        startButton.localScale = Vector3.zero;
        startButton.DOScale(1,0.4f);
        exitButton.localScale = Vector3.zero;
        exitButton.DOScale(1, 0.4f);
    }
    private void ExitAnim()
    {
        bluePanel.DOLocalMoveX(-1000,0.4f);
        redPanel.DOLocalMoveX(1000, 0.4f);
        startButton.DOScale(0, 0.4f);
        exitButton.DOScale(0, 0.4f).OnComplete(()=>gameObject.SetActive(false));
    }
}
