using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager {
    protected GameFacade facade;
    public BaseManager(GameFacade facade)
    {
        this.facade = facade;
    }
    public virtual void OnInit() { }//开始
    public virtual void Update() { }
    public virtual void OnDestory() { }//结束
}
