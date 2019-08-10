using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TcpServices
{
    class Message
    {
        private byte[] data = new byte[1024];
        private int statIndex = 0;
        public void AddCount(int count)
        {
            statIndex += count;
        }
        public byte[] Data
        {
            get
            {
                return data;
            }
        }
        public int StartIndex
        {
            get { return StartIndex; }
        }
        public int ReaminSize
        {
            get { return data.Length - StartIndex; }
        }
        //解析数据
        public void ReadMessage()
        {
            while(true)
            {
                if (StartIndex <= 4) return;
                int count = BitConverter.ToInt32(data, 0);
                if ((StartIndex - 4) >= count)
                {
                    string s = Encoding.UTF8.GetString(data, 4, count);
                    Console.WriteLine("解析出来一条数据：" + s);
                    Array.Copy(data, count + 4, data, 0, StartIndex - 4 - count);
                    statIndex -= (count + 4);
                }
                else
                {
                    break;
                }
            }
        }
    }
}
