using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.TCPSetting;
using DoNetDrive.Protocol.Util;
using DotNetty.Buffers;
using System;
using System.Text;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter
{
    /// <summary>
    /// 命令设备立刻重新连接服务器 返回值
    /// </summary>
    public class RequireConnectServer_Result : INCommandResult
    {
        /// <summary>
        /// 保活包状态返回值 
        /// 1--已重新连接（UDP时表示已发送保活包）
        /// 2--Server 参数未设置
        /// 3--Server 参数错误
        /// 4--Server 连接失败 （TCP）
        /// 5--服务器无应答
        /// 6--网络参数设置错误
        /// 7--网线未连接
        /// 8--Wifi 未连接
        /// </summary>
        public int ResultStatus;


        /// <summary>
        /// 构建一个获取设备客户端连接状返回值的实例
        /// </summary>
        public RequireConnectServer_Result() { ResultStatus = 0; }

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
            ResultStatus = databuf.ReadByte();

        }
    }
}
