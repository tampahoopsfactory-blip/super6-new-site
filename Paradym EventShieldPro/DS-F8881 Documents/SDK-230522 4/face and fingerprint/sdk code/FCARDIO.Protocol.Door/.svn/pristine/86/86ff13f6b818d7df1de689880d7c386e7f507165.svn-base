using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.TCPSetting;
using DoNetDrive.Protocol.Util;
using DotNetty.Buffers;
using System;
using System.Text;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter 
{
    /// <summary>
    /// 获取设备客户端连接状态 返回值
    /// </summary>
    public class ReadClientStatus_Result : INCommandResult
    {
        /// <summary>
        /// 客户端模式通讯方式 
        ///0--禁用;
        ///1--UDP;
        ///2--TCP Client;
        ///3--TCP Client + TLS ;
        ///4--MQTT（TCP Client）;
        ///5--MQTT（TCP Client） + TLS ;
        /// </summary>
        public int ClientModel;
        /// <summary>
        /// 服务器IP
        /// </summary>
        public string ServerIP;

        /// <summary>
        /// 最近一次保活包发送时间
        /// </summary>
        public DateTime LastKeepaliveTime;

        /// <summary>
        /// 连接状态
        /// 0--TCP Client 未连接；
        /// 1--TCP Client 已连接；
        /// 2--UDP Client 无连接状态
        /// 255--已禁用
        /// </summary>
        public int ConnectStatus;

        /// <summary>
        /// 构建一个获取设备客户端连接状返回值的实例
        /// </summary>
        public ReadClientStatus_Result() { ClientModel = 0; }

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
            ClientModel = databuf.ReadByte();

            StringBuilder strbuilder = new StringBuilder(30);
            TCPDetail.ReadIPByByteBuf(databuf, strbuilder);
            ServerIP = strbuilder.ToString();

            LastKeepaliveTime = TimeUtil.BCDTimeToDate_yyyyMMddhhmmss(databuf);
            ConnectStatus = databuf.ReadByte();

        }
    }
}
