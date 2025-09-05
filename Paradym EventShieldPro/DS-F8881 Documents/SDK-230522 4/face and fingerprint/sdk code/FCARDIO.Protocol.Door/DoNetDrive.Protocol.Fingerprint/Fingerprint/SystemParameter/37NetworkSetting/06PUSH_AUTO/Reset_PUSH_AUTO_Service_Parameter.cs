using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 重置推送服务推送断点
    /// </summary>
    public class Reset_PUSH_AUTO_Service_Parameter : AbstractParameter
    {
        /// <summary>
        /// 认证记录 推送断点
        /// </summary>
        public int UserTransaction;
        /// <summary>
        /// 门磁记录 推送断点
        /// </summary>
        public int DoorSensorTransaction;
        /// <summary>
        /// 系统记录 推送断点
        /// </summary>
        public int SystemTransaction;
        /// <summary>
        /// 体温记录 推送断点
        /// </summary>
        public int BodyTempTransaction;
        /// <summary>
        /// 记录照片 推送断点
        /// </summary>
        public int TransactionImage;

        /// <summary>
        /// 创建重置推送服务推送断点参数
        /// </summary>
        public Reset_PUSH_AUTO_Service_Parameter() { }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (UserTransaction < 0) return false;
            if (DoorSensorTransaction < 0) return false;
            if (SystemTransaction < 0) return false;
            if (BodyTempTransaction < 0) return false;
            if (TransactionImage < 0) return false;
            return true;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 20;
        }


        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            return;
        }

        /// <summary>
        /// 编码参数
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteInt(UserTransaction);
            databuf.WriteInt(DoorSensorTransaction);
            databuf.WriteInt(SystemTransaction);
            databuf.WriteInt(BodyTempTransaction);
            databuf.WriteInt(TransactionImage); 
            return databuf;
        }

        /// <summary>
        /// 废弃
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            return;
        }
    }
}
