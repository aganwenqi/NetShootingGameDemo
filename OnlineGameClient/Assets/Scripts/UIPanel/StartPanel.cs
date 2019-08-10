using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class StartPanel : BasePanel {

    private Button loginButton;
    private Animator btnAnimator;
    private void Awake()
    {
        loginButton = transform.Find("LoginButton").GetComponent<Button>();
        btnAnimator = loginButton.GetComponent<Animator>();
        loginButton.onClick.AddListener(OnLoginClick);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        
        //OnResume();
    }
    private void OnLoginClick()
    {
        PlayClickSound();
        uiMng.PushPanel(UIPanelType.Login);
        //btnAnimator.enabled = false;
        //loginButton.transform.DOScale(0, 0.4f).OnComplete(() => loginButton.enabled = false);
    }
    public override void OnPause()
    {
        base.OnPause();
        btnAnimator.enabled = false;
        loginButton.transform.DOScale(0, 0.4f).OnComplete(() => loginButton.enabled = false);
    }
    public override void OnResume()
    {
        base.OnPause();
        loginButton.enabled = true;
        loginButton.transform.DOScale(1, 0.4f).OnComplete(()=> btnAnimator.enabled = true);
    }
}
