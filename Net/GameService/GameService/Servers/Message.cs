using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
namespace GameService.Servers
{
    class Message
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
            get { return data.Length - StartIndex; }
        }
        //解析数据
        public void ReadMessage(int newDataAmount,Action<RequestCode,ActionCode,string> action)
        {
            statIndex += newDataAmount;
            while (true)
            {
                if (StartIndex <= 4) return;
                int count = BitConverter.ToInt32(data, 0);
                if ((StartIndex - 4) >= count)
                {
                    //解析数据
                    RequestCode requestCode =(RequestCode)BitConverter.ToInt32(data, 4);
                    ActionCode actionCode = (ActionCode)BitConverter.ToInt32(data, 8);
                    string s = Encoding.UTF8.GetString(data,12,count-8);

                    action(requestCode, actionCode, s);

                    Array.Copy(data, count + 4, data, 0, StartIndex - 4 - count);
                    statIndex -= (count + 4);
                }
                else
                {
                    break;
                }
            }
        }
        //数据包装
        public static byte[] PackData(ActionCode actionCode, string data)
        {
            byte[] reuquestCodeBytes = BitConverter.GetBytes((int)actionCode);//转换出来还是4个字节
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            int dataAmount = reuquestCodeBytes.Length + dataBytes.Length;
            byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);

           return dataAmountBytes.Concat(reuquestCodeBytes).Concat(dataBytes).ToArray<byte>();
        }
    }
}
