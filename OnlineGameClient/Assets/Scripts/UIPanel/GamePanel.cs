using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Common;
public class GamePanel : BasePanel {

    private Text timer;
    private int time = -1;
    private Button successBtn;
    private Button failBtn;
    private Button exitBtn;
    private QuitBattleRequest quitBattleRequest;
    private void Start()
    {
        timer = transform.Find("Timer").GetComponent<Text>();
        successBtn = transform.Find("SuccessButton").GetComponent<Button>();
        successBtn.onClick.AddListener(OnResultClick);
        failBtn = transform.Find("FailButton").GetComponent<Button>();
        failBtn.onClick.AddListener(OnResultClick);
        successBtn.gameObject.SetActive(false);
        failBtn.gameObject.SetActive(false);
        exitBtn = transform.Find("ExitButton").GetComponent<Button>();
        exitBtn.onClick.AddListener(OnExitClick);
        quitBattleRequest = GetComponent<QuitBattleRequest>();
    }
    private void Update()
    {
        if (time > -1)
        {
            ShowTimer(time);
            time = -1;
        }
    }
    public override void OnEnter()
    {
        if(successBtn)
            successBtn.gameObject.SetActive(false);
        if(failBtn)
            failBtn.gameObject.SetActive(false);
        gameObject.transform.localScale = Vector3.one;
        gameObject.SetActive(true);
    }
    public override void OnExit()
    {
        //base.OnExit();
        //successBtn.gameObject.SetActive(false);
        //failBtn.gameObject.SetActive(false);
        gameObject.transform.DOScale(0,0.4f).OnComplete(()=>gameObject.SetActive(false));
        exitBtn.gameObject.SetActive(false);
    }
    private void OnResultClick()
    {
        uiMng.PopPanel();
        uiMng.PopPanel();
        facede.GameOver();
    }
    public void OnExitClick()
    {
        quitBattleRequest.SendRequest();
    }
    public void  OnExitResponse()
    {
        OnResultClick();
    }
    public void ShowTimerSync(int time)
    {
        this.time = time;
    }
    public void ShowTimer(int time)
    {
        if (time == 1)
            exitBtn.gameObject.SetActive(true);
        timer.gameObject.SetActive(true);
        timer.text = time.ToString();
        timer.transform.localScale = Vector3.one;
        Color tempColor = timer.color;
        tempColor.a = 1;
        timer.color = tempColor;
        timer.transform.DOScale(2,0.3f).SetDelay(0.3f);
        timer.DOFade(0, 0.3f).SetDelay(0.3f).OnComplete(()=>timer.gameObject.SetActive(false));
        facede.PlayNormalSound(AUdioManager.Sound_Alert);
    }
    public void OnGameOverResponse(ReturnCode returnCode)
    {
        Button tempBtn = null;
        if (returnCode == ReturnCode.Success)
        {
            tempBtn = successBtn;
        }
        else
        {
            tempBtn = failBtn;

        }
        tempBtn.gameObject.SetActive(true);
        tempBtn.transform.localScale = Vector3.zero;
        tempBtn.transform.DOScale(1, 0.4f);
    }
}
