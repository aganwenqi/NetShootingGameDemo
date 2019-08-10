using System;
using System.Collections.Generic;
using System.Text;
namespace Common
{
    public enum ActionCode
    {
        None,
        Login,
        Register,
        ListRoom,
        CreateRoom,
        JoinRoom,
        UpdateRoom,//有新玩家加入
        QuitRoom,
        StartGame,
        ShowTimer,
        StartPlay,
        Move,
        Shoot,
        Attack,//受到伤害
        GameOver,
        UpdateResult,
        QuitBattle
    }
}
