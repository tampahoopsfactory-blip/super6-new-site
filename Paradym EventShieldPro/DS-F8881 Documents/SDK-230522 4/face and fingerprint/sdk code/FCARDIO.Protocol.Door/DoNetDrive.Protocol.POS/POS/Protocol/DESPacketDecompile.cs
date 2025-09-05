using DotNetty.Buffers;
using DoNetDrive.Protocol.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.POS.Protocol
{
    /// <summary>
    /// 数据包 解析类；<para/>
    /// 适用于使用DES进行加密的消费机等设备通讯
    /// </summary>
    public class DESPacketDecompile : BaseDecompile<DESPacket>
    {
        public DESPacketDecompile(IByteBufferAllocator acr) :base(acr)
        {
            _Buf = acr.Buffer(16);
        }

        protected override BaseDecompileStep<DESPacket> GetFirstDecompileStep()
        {
            return (BaseDecompileStep<DESPacket>)DESPacketDecompileStep.Code;
        }
    }
}
