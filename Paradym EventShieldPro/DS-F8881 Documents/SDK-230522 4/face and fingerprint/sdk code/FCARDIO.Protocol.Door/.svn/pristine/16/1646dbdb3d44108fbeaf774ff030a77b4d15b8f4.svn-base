using DoNetDrive.Protocol.Door.Door8800;
using DotNetty.Buffers;
using System;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.TCPSetting;
using System.Net;
using DoNetDrive.Common.Extensions;
using System.Text;

namespace DoNetDrive.Protocol.Fingerprint.SystemParameter 
{
    /// <summary>
    /// 写入客户端模式通讯方式 参数
    /// </summary>
    public class WriteClientWorkMode_Parameter : AbstractParameter
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
        /// 构建一个空的实例
        /// </summary>
        public WriteClientWorkMode_Parameter() { ClientModel = 0; }

        /// <summary>
        /// 创建网络服务器参数
        /// </summary>
        /// <param name="iMode">客户端模式通讯方式；0--禁用;1--UDP;2--TCP Client;</param>
        public WriteClientWorkMode_Parameter(int iMode)
        {
            ClientModel = iMode;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (ClientModel < 0 || ClientModel > 5)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 1;
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
            databuf.WriteByte(ClientModel);
            
            return databuf;
        }



        /// <summary>
        /// 解码参数
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            ClientModel = databuf.ReadByte();

        }
    }
}
