using DoNetDrive.Protocol.USBDrive;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.Transaction.ReadTransactionDatabase
{
    /// <summary>
    /// 重复读取遗漏的记录的返回值
    /// </summary>
    public class ReReadDatabaseCallBack : IReadTransactionDatabase_CallBack
    {
        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="command"></param>
        /// <param name="bPck"></param>
        public void CommandCallBack(ReadTransactionDatabase_Base command, USBDrivePacket bPck)
        {
            command.ReReadDatabaseCallBack(bPck);
        }
    }
}
