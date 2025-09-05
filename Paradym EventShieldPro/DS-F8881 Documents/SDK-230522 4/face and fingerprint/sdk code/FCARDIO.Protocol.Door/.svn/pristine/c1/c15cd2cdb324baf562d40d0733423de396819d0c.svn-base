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
    /// 写入网络服务器参数
    /// </summary>
    public class WriteNetworkServerDetail_Parameter : AbstractParameter
    {
        /// <summary>
        /// 服务器端口,取值范围：0-65534
        /// </summary>
        public int ServerPort;

        /// <summary>
        /// 服务器IP
        /// </summary>
        public string ServerIP;


        /// <summary>
        /// 服务器域名
        /// </summary>
        public string ServerDomain;
        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteNetworkServerDetail_Parameter() { ServerPort = 0; ServerIP = string.Empty; ServerDomain = string.Empty; }

        /// <summary>
        /// 创建网络服务器参数
        /// </summary>
        /// <param name="iServerPort">服务器端口,取值范围：0-65534</param>
        /// <param name="sServerIP">服务器IP</param>
        public WriteNetworkServerDetail_Parameter(int iServerPort, string sServerIP)
        {
            ServerPort = iServerPort;
            ServerIP = sServerIP;
            ServerDomain = string.Empty;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (ServerPort < 0 || ServerPort > 65534)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(ServerDomain))
            {
                try
                {
                    var ips = Dns.GetHostAddresses(ServerDomain);
                    if (ips.Length == 0) return false;
                    var sIP = ips[0].ToString();
                    if (!string.IsNullOrEmpty(ServerIP))
                    {
                        if (ServerIP != "0.0.0.0")
                        {
                            if (!ServerIP.Equals(sIP))
                            {
                                ServerIP = sIP;
                                //return false;
                            }
                        }

                    }
                }
                catch (Exception)
                {

                    return false;
                }

            }


            if (!string.IsNullOrEmpty(ServerIP))
            {
                if (!TCPDetail.CheckIP(ServerIP))
                {
                    return false;
                }
            }


            return true;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 105;
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
            databuf.WriteUnsignedShort((ushort)ServerPort);
            TCPDetail.SaveIPToByteBuf(ServerIP, databuf);


            if (string.IsNullOrEmpty(ServerDomain))
            {
                TCPDetail.Save0toByteBuf(databuf, 99);
            }
            else
            {
                byte[] tmp = ServerDomain.GetBytes();
                databuf.WriteBytes(tmp);
                int iCount = 99 - tmp.Length;
                if (iCount > 0) TCPDetail.Save0toByteBuf(databuf, iCount);
            }
            return databuf;
        }



        /// <summary>
        /// 解码参数
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            StringBuilder strbuilder = new StringBuilder(30);
            ServerPort = databuf.ReadUnsignedShort();

            TCPDetail.ReadIPByByteBuf(databuf, strbuilder);
            ServerIP = strbuilder.ToString();

            //读取域名
            int iLen = databuf.ReadableBytes;
            int iReadIndex = databuf.ReaderIndex;
            int iCharCount = 0;
            byte bValue = 0;
            for (int i = 0; i < iLen; i++)
            {
                bValue = databuf.ReadByte();
                if (bValue == 0)
                {
                    break;
                }
                else
                {
                    iCharCount++;
                }
            }
            databuf.SetReaderIndex(iReadIndex);
            if (iCharCount == 0)
            {
                ServerDomain = string.Empty;
            }
            else
            {
                byte[] tmp = new byte[iCharCount];
                ServerDomain = databuf.ReadString(iCharCount, Encoding.ASCII);
            }
        }
    }
}
