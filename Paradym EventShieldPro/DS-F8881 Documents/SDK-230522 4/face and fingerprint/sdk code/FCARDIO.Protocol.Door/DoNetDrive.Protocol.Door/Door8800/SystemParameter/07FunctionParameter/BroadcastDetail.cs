using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 语音段开关_模型
    /// </summary>
    public class BroadcastDetail
    {
        /// <summary>
        /// 语音段开关的字节数组
        /// </summary>
        public byte[] Broadcast { get; set; }

        /// <summary>
        /// 定义语音段开关的字节数组的长度
        /// </summary>
        public BroadcastDetail()
        {
            Broadcast = new byte[10];
        }

        /// <summary>
        /// 语音段开关的字节数组信息
        /// </summary>
        /// <param name="data"></param>
        public BroadcastDetail(byte[] data)
        {
            if (data.Length != 10)
            {
                throw new ArgumentException("data Length Error");
            }

            Broadcast = data;
        }
    }
}