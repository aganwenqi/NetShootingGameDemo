using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class StartPlayRequest : BaseRequest {
    bool isStartPlaying = false;
    public override void Awake()
    {
        actionCode = ActionCode.StartPlay;
        base.Awake();
    }
    private void Update()
    {
        if (isStartPlaying)
        {
            facade.StartPlaying();
            isStartPlaying = false;
        }
    }
    public override void OnResponse(string data)
    {
        isStartPlaying = true;
    }
}
