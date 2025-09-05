using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Core.Connector;
using DoNetDrive.Core.Packet;
using DoNetDrive.Protocol.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.POS.Protocol
{
    public class DESRequestHandle : AbstractRequestHandle
    {
        public DESRequestHandle(IByteBufferAllocator allocator, Func< String, byte, byte, AbstractTransaction> factory):base(new DESPacketDecompile(allocator))
        {

        }

        public override void DisposeResponse(INConnector connector, IByteBuffer msg)
        {
            throw new NotImplementedException();
        }

        protected override void fireRequestEvent(INConnector connector, INPacket p)
        {
            throw new NotImplementedException();
        }
    }
}
