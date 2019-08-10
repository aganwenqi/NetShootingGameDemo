using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Common;
public class RegisterPanel : BasePanel
{
    private InputField usernameIF;
    private InputField passwordIF;
    private InputField rePasswordIF;
    private RegisterRequest registerRequest;
    private void Start()
    {
        registerRequest = GetComponent<RegisterRequest>();
        usernameIF = transform.Find("UserNameLabel/UserNameInput").GetComponent<InputField>();
        passwordIF = transform.Find("PasswordLabel/PasswordInput").GetComponent<InputField>();
        rePasswordIF = transform.Find("RePasswordLabel/RePasswordInput").GetComponent<InputField>();

        transform.Find("RegisterButton").GetComponent<Button>().onClick.AddListener(OnRegisterClick);
        transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseClick);
    }
    public override void OnEnter()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.4f);//播放渐变
        transform.localPosition = new Vector3(0, -500, 0);
        transform.DOLocalMove(Vector3.zero, 0.4f);

    }
    private void OnRegisterClick()//注册按钮按下了
    {
        PlayClickSound();
        string msg = "";
        if (string.IsNullOrEmpty(usernameIF.text))
        {
            msg += "用户名不能为空";
        }
        if (string.IsNullOrEmpty(passwordIF.text))
        {
            msg += "\n密码不能为空";
        }
        if(passwordIF.text != rePasswordIF.text)
        {
            msg += "\n密码不一致";
        }
        if (msg != "")
        {
            uiMng.ShowMessage(msg);return;
        }
        //发送到服务器端进行注册
        registerRequest.SendRequest(usernameIF.text,passwordIF.text);
    }

    public void OnRegisterResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            uiMng.ShowMessageSync("注册成功");
        }
        else
        {
            uiMng.ShowMessageSync("用户名重复");
        }
    }
    private void OnCloseClick()//Close
    {
        PlayClickSound();
        //uiMng.PopPanel();
        transform.DOScale(0, 0.4f);//播放渐变
        Tween tweener = transform.DOLocalMove(new Vector3(0, -500, 0), 0.4f);
        uiMng.PopPanel();
        //tweener.OnComplete(() => uiMng.PopPanel());
    }
    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
    }
}
