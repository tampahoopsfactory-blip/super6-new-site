using DoNetDrive.Protocol.USBDrive;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.Transaction.ReadTransactionDatabase
{
    /// <summary>
    /// 读取新记录 每一个步骤的抽象状态 
    /// </summary>
    public interface IReadTransactionDatabase_CallBack
    {

        /// <summary>
        /// 处理返回值
        /// </summary>
        void CommandCallBack(ReadTransactionDatabase_Base command, USBDrivePacket bPck);
    }
}
