using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.TCPSetting;
using DoNetDrive.Protocol.Util;
using DotNetty.Buffers;
using System;
using System.Text;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 获取自动推送服务器详情返回值
    /// </summary>
    public class Read_PUSH_AUTO_Service_Result : INCommandResult
    {

        /// <summary>
        /// 启动标志
        /// 1--启用；0--禁用
        /// </summary>
        public bool IsUse;

        /// <summary>
        /// 认证记录
        /// </summary>
        public Transaction_PUSH_Detail UserTransaction;
        /// <summary>
        /// 门磁记录
        /// </summary>
        public Transaction_PUSH_Detail DoorSensorTransaction;
        /// <summary>
        /// 系统记录
        /// </summary>
        public Transaction_PUSH_Detail SystemTransaction;
        /// <summary>
        /// 体温记录
        /// </summary>
        public Transaction_PUSH_Detail BodyTempTransaction;
        /// <summary>
        /// 记录照片
        /// </summary>
        public Transaction_PUSH_Detail TransactionImage;

        /// <summary>
        /// 人员待推送数量
        /// </summary>
        public int User_Readable;

        /// <summary>
        /// 构建一个获取自动推送服务器详情返回值的实例
        /// </summary>
        public Read_PUSH_AUTO_Service_Result()
        {
            IsUse = false;
            UserTransaction = new Transaction_PUSH_Detail();
            DoorSensorTransaction = new Transaction_PUSH_Detail();
            SystemTransaction = new Transaction_PUSH_Detail();
            BodyTempTransaction = new Transaction_PUSH_Detail();
            TransactionImage = new Transaction_PUSH_Detail();
            User_Readable = 0;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            return;
        }


        /// <summary>
        /// 解码参数
        /// </summary>
        /// <param name="databuf"></param>
        public void SetBytes(IByteBuffer databuf)
        {
            IsUse = databuf.ReadBoolean();
            UserTransaction.SetBytes(databuf);
            DoorSensorTransaction.SetBytes(databuf);
            SystemTransaction.SetBytes(databuf);
            BodyTempTransaction.SetBytes(databuf);
            TransactionImage.SetBytes(databuf);
            User_Readable = databuf.ReadInt();
        }
    }
}
