using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System;
using System.Text;
using System.Linq;

public class Message
{


    private byte[] data = new byte[1024];
    private int statIndex = 0;
    /*
    public void AddCount(int count)
    {
        statIndex += count;
    }*/
    public byte[] Data
    {
        get
        {
            return data;
        }
    }
    public int StartIndex
    {
        get { return statIndex; }
    }
    public int ReaminSize
    {
        get { return data.Length - statIndex; }
    }
    //解析数据
    public void ReadMessage(int newDataAmount, Action<ActionCode, string> action)
    {
        statIndex += newDataAmount;
        while (true)
        {
            if (statIndex <= 4) return;

            int count = BitConverter.ToInt32(data, 0);
            if ((statIndex - 4) >= count)
            {
                //解析数据
                ActionCode actionCode = (ActionCode)BitConverter.ToInt32(data, 4);
                string s = Encoding.UTF8.GetString(data, 8, count - 4);
                if (action != null)
                    action.Invoke(actionCode, s);

                Array.Copy(data, count + 4, data, 0, statIndex - 4 - count);
                statIndex -= (count + 4);
            }
            else
            {
                break;
            }
        }
    }
    //数据包装
    public static byte[] PackData(RequestCode requestCode,ActionCode actionCode, string data)
    {
        byte[] reuquestCodeBytes = BitConverter.GetBytes((int)requestCode);//转换出来还是4个字节
        byte[] actionCodeBytes = BitConverter.GetBytes((int)actionCode);
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);

        int dataAmount = reuquestCodeBytes.Length + actionCodeBytes.Length + dataBytes.Length;
        byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);

        return dataAmountBytes.Concat(reuquestCodeBytes).Concat(actionCodeBytes).Concat(dataBytes).ToArray<byte>();
    }
}
