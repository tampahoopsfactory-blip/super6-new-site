using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.TCPSetting
{
    /// <summary>
    /// 设置TCP参数_参数
    /// </summary>
    public class WriteTCPSetting_Parameter : AbstractParameter
    {
        /// <summary>
        /// TCP参数信息
        /// </summary>
        public TCPDetail TCP;

        /// <summary>
        /// 命令是否用UDP广播发送？
        /// </summary>
        public bool UDPBroadcast;

        /// <summary>
        /// 提供给 ReadTCPSetting_Result 使用
        /// </summary>
        public WriteTCPSetting_Parameter()
        {

        }
        /// <summary>
        /// 使用TCP参数信息初始化实例
        /// </summary>
        /// <param name="_TCP">TCP参数信息</param>
        public WriteTCPSetting_Parameter(TCPDetail _TCP)
        {
            TCP = _TCP;

            if (!checkedParameter())
            {
                throw new ArgumentException("TCP Error");
            }
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (TCP == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(TCP.mMAC) || 
                string.IsNullOrEmpty(TCP.mIP) ||
                string.IsNullOrEmpty(TCP.mIPMask) ||
                string.IsNullOrEmpty(TCP.mIPGateway) ||
                string.IsNullOrEmpty(TCP.mDNS) ||
                string.IsNullOrEmpty(TCP.mDNSBackup) ||
                string.IsNullOrEmpty(TCP.mServerIP)  )
            {
                return false;
            }
            if (TCP.mTCPPort <= 0 || TCP.mTCPPort > 65535 ||
                TCP.mUDPPort <= 0 || TCP.mUDPPort > 65535 ||
                TCP.mServerPort <= 0 || TCP.mServerPort > 65535)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            if (TCP != null)
            {
                TCP = null;
            }

            return;
        }

        /// <summary>
        /// 编码参数
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return TCP.GetBytes(databuf);
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return TCP.GetDataLen();
        }

        /// <summary>
        /// 解码参数
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (TCP == null)
            {
                TCP = new TCPDetail();
            }
            TCP.SetBytes(databuf);
        }
    }
}