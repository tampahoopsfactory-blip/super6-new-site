using DoNetDrive.Protocol.USBDrive;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.Transaction.ReadTransactionDatabase
{
    /// <summary>
    /// 读取记录数据库空间信息的返回值
    /// </summary>
    public class ReadDataBaseDetailCallBack : IReadTransactionDatabase_CallBack
    {
        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="ReadTransactionDatabaseCommand"></param>
        /// <param name="bPck"></param>
        public void CommandCallBack(ReadTransactionDatabase_Base ReadTransactionDatabaseCommand, USBDrivePacket bPck)
        {
            ReadTransactionDatabaseCommand.ReadDataBaseDetailCallBack(bPck);
            ReadTransactionDatabaseCommand.readDataBaseDetailCallBack = new ReadTransactionDatabaseByIndexCallBack();
        }
    }
}
