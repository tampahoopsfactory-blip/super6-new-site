using DotNetty.Buffers;
using DoNetDrive.Protocol.Transaction;
using DoNetDrive.Protocol.USBDrive;
using System;

namespace DoNetDrive.Protocol.USB.OfflinePatrol
{
    /// <summary>
    /// 
    /// </summary>
    public class USBCommandObserverHandler : USBDriveRequestHandle
    {
        //public event DisposeRequestEventEventHandler DisposeRequestEvent;
        //public event DisposeResponseEventEventHandler DisposeResponseEvent;

        //public delegate void DisposeRequestEventEventHandler(INConnector connector, string msg);
        //public delegate void DisposeResponseEventEventHandler(INConnector connector, string msg);

        public USBCommandObserverHandler(IByteBufferAllocator allocator, Func<byte, byte, AbstractTransaction> factory):base(allocator, factory)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Pck"></param>
        /// <returns></returns>
        protected override bool CheckPacketIsTransaction(USBDrivePacket Pck)
        {
            return Pck.CmdType == 0x31 && Pck.CmdIndex == 0x0E;
        }
    }
}
