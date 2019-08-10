using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Common;
public class LoginPanel : BasePanel
{
    private Button closeButton;
    private InputField usernameIF;
    private InputField passwordIF;
    private LoginRequest loginRequest;
    //private Button loginButton;
    //private Button registerButton;
    private void Awake()
    {
        loginRequest = GetComponent<LoginRequest>();
        usernameIF = transform.Find("UserNameLabel/UserNameInput").GetComponent<InputField>();
        passwordIF = transform.Find("PasswordLabel/PasswordInput").GetComponent<InputField>();

        closeButton = transform.Find("CloseButton").GetComponent<Button>();
        closeButton.onClick.AddListener(OnCloseClick);
        transform.Find("LoginButton").GetComponent<Button>().onClick.AddListener(OnLoginClick);
        transform.Find("RegisterButton").GetComponent<Button>().onClick.AddListener(OnRegisterClick);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        EnterAnim();   
    }
    //暂停
    public override void OnPause()
    {
        base.OnPause();
        transform.DOScale(0, 0.4f);//播放渐变
        Tween tweener = transform.DOLocalMove(new Vector3(0, -500, 0), 0.4f);
        tweener.OnComplete(() => gameObject.SetActive(false));
    }
    //继续
    public override void OnResume()
    {
        base.OnResume();
        EnterAnim();
    }
    //退出栈
    public override void OnExit()
    {
        base.OnExit();
        HideAnim();
        //gameObject.SetActive(false);
    }
    //按钮按下
    private void OnCloseClick()
    {
        PlayClickSound();
        uiMng.PopPanel();
        //HideAnim();
    }
    //登录
    private void OnLoginClick()
    {
        PlayClickSound();
        string msg = "";
        if (string.IsNullOrEmpty(usernameIF.text))
        {
            msg += "用户名不能为空 ";
        }
        if (string.IsNullOrEmpty(passwordIF.text))
        {
            msg += "密码不能为空 ";
        }
        if (msg != "")
        {
            uiMng.ShowMessage(msg);return;
        }
        //TODO发送到服务端进行验证
        loginRequest.SendRequest(usernameIF.text, passwordIF.text);
    }
    public void OnLoginResponse(ReturnCode returnCode)
    {
        //Debug.Log("登录："+ returnCode);
        if (returnCode == ReturnCode.Success)
        {
            //登陆成果 进入房间列表
            uiMng.PushPanelSync(UIPanelType.RoomList);
        }
        else
        {
            //登陆失败
            uiMng.ShowMessageSync("用户名或密码错误，无法登陆，请重新输入！！");
        }
    }
    //按下注册按钮
    private void OnRegisterClick()
    {
        PlayClickSound();
        uiMng.PushPanel(UIPanelType.Register);
    }
   
    //进入动画
    private void EnterAnim()
    {
        gameObject.SetActive(true);

        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.4f);//播放渐变
        transform.localPosition = new Vector3(0, -500, 0);
        transform.DOLocalMove(Vector3.zero, 0.4f);
    }
    //隐藏动画
    private void HideAnim()
    {
        transform.DOScale(0, 0.4f);//播放渐变
        Tween tweener = transform.DOLocalMove(new Vector3(0, -500, 0), 0.4f);
        //tweener.OnComplete(() => uiMng.PopPanel());
    }
}
