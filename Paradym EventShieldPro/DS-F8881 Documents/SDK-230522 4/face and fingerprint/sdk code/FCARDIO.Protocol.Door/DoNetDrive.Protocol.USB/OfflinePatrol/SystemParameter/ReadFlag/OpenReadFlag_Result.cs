using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.ReadFlag
{
    /// <summary>
    /// 读卡返回参数
    /// </summary>
    public class OpenReadFlag_Result : INCommandResult
    {
        public int CardData;
        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public void SetBytes(IByteBuffer databuf)
        {
            CardData = databuf.ReadUnsignedMedium();
        }
        public void Dispose()
        {
            
        }
    }
}
