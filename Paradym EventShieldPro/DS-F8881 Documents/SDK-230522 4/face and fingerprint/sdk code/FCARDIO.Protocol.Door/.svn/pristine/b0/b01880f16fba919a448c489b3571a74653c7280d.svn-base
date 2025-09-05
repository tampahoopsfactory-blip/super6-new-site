using DoNetDrive.Protocol.USBDrive;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.Transaction.ReadTransactionDatabase
{
    /// <summary>
    /// 读记录数据库的返回值
    /// </summary>
    public class ReadTransactionDatabaseByIndexCallBack : IReadTransactionDatabase_CallBack
    {
        
        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="bPck"></param>
        public void CommandCallBack(ReadTransactionDatabase_Base ReadTransactionDatabaseCommand, USBDrivePacket bPck)
        {
            ReadTransactionDatabaseCommand.ReadTransactionDatabaseByIndexCallBack(bPck);
            if (ReadTransactionDatabaseCommand.mStep == 3)
            {
                ReadTransactionDatabaseCommand.readDataBaseDetailCallBack = new ReReadDatabaseCallBack();
            }
            
        }
    }
}
