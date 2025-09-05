using DotNetty.Buffers;
using DoNetDrive.Core.Data;
using DoNetDrive.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.TCPSetting
{
    /// <summary>
    /// TCP/IP信息
    /// </summary>
    public class TCPDetail : AbstractData
    {
        /// <summary>
        /// MAC地址
        /// </summary>
        public string mMAC { get; set; }

        /// <summary>
        /// IP地址 
        /// </summary>
        public string mIP { get; set; }

        /// <summary>
        /// IP掩码
        /// </summary>
        public string mIPMask { get; set; }

        /// <summary>
        /// 网关
        /// </summary>
        public string mIPGateway { get; set; }

        /// <summary>
        /// DNS
        /// </summary>
        public string mDNS { get; set; }

        /// <summary>
        /// 备用DNS
        /// </summary>
        public string mDNSBackup { get; set; }

        /// <summary>
        /// 控制器网络协议类型：1--TCP  client （控制器就是 Client）;2--TCP Server（控制器就是 Server）
        /// </summary>
        public ushort mProtocolType { get; set; }

        /// <summary>
        /// 控制器使用的TCP端口
        /// </summary>
        public int mTCPPort { get; set; }

        /// <summary>
        /// 控制器使用的UDP端口
        /// </summary>
        public int mUDPPort { get; set; }

        /// <summary>
        /// 控制器作为客户端时，目标服务器的端口
        /// </summary>
        public int mServerPort { get; set; }

        /// <summary>
        /// 控制器作为客户端时，目标服务器的IP
        /// </summary>
        public string mServerIP { get; set; }

        /// <summary>
        /// 自动获得IP
        /// </summary>
        public bool mAutoIP { get; set; }

        /// <summary>
        /// 控制器作为客户端时，目标服务器的域名
        /// </summary>
        public string mServerAddr { get; set; }

        /// <summary>
        /// 对IP信息参数进行编码
        /// </summary>
        /// <param name="databuf">需要填充参数结构的字节缓冲区</param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            SaveMACToByteBuf(mMAC, databuf);
            SaveIPToByteBuf(mIP, databuf);
            SaveIPToByteBuf(mIPMask, databuf);
            SaveIPToByteBuf(mIPGateway, databuf);
            SaveIPToByteBuf(mDNS, databuf);
            SaveIPToByteBuf(mDNSBackup, databuf);
            databuf.WriteByte(mProtocolType);
            databuf.WriteShort(mTCPPort);
            databuf.WriteShort(mUDPPort);
            databuf.WriteShort(mServerPort);
            SaveIPToByteBuf(mServerIP, databuf);
            databuf.WriteByte(mAutoIP ? 1 : 0);

            if (string.IsNullOrEmpty(mServerAddr))
            {
                Save0toByteBuf(databuf, 99);
            }
            else
            {
                byte[] tmp = mServerAddr.GetBytes();
                databuf.WriteBytes(tmp);
                int iCount = 99 - tmp.Length;
                if (iCount > 0)
                {
                    Save0toByteBuf(databuf, iCount);
                }
            }
            return databuf;
        }

        /// <summary>
        /// MAC地址从结构转换成数组字节
        /// </summary>
        /// <param name="mac">MAC地址</param>
        /// <param name="buf"></param>
        private void SaveMACToByteBuf(string mac, IByteBuffer buf)
        {
            if (!CheckMAC(mac))
            {
                for (int i = 0; i < 6; i++)
                {
                    buf.WriteByte(0);
                }
                return;
            }
            string[] macList = mac.Split('-');
            int iLen = macList.Length;
            for (int i = 0; i < iLen; i++)
            {
                buf.WriteByte(Convert.ToInt32(macList[i], 16));
            }
        }

        /// <summary>
        /// 检查MAC地址
        /// </summary>
        /// <param name="mac">检查MAC地址</param>
        /// <returns></returns>
        public static bool CheckMAC(string mac)
        {
            if (string.IsNullOrEmpty(mac))
            {
                return false;
            }
            string[] macList = mac.Split('-');
            int iLen = macList.Length;
            if (iLen != 6)
            {
                return false;
            }
            try
            {
                int iValue;
                for (int i = 0; i < iLen; i++)
                {
                    iValue = Convert.ToInt32(macList[i], 16);
                    if (iValue < 0 || iValue > 255)
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// IP地址及与之相同格式的结构转换成数组字节
        /// </summary>
        /// <param name="IP">IP地址</param>
        /// <param name="buf"></param>
        public static void SaveIPToByteBuf(string IP, IByteBuffer buf)
        {
            if (!CheckIP(IP))
            {
                for (int i = 0; i < 4; i++)
                {
                    buf.WriteByte(0);
                }
                return;
            }
            String[] ipList = IP.Split('.');
            int iLen = ipList.Length;
            for (int i = 0; i < iLen; i++)
            {
                buf.WriteByte(Convert.ToInt32(ipList[i]));
            }
        }

        /// <summary>
        /// 检查IP地址及与之相同格式数据
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool CheckIP(string ip)
        {
            if (string.IsNullOrEmpty(ip))
            {
                return false;
            }
            string[] ipList = ip.Split('.');
            int iLen = ipList.Length;
            if (iLen != 4)
            {
                return false;
            }
            try
            {
                int iValue;
                for (int i = 0; i < iLen; i++)
                {
                    iValue = Convert.ToInt32(ipList[i]);
                    if (iValue < 0 || iValue > 255)
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 写入字节
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="iCount"></param>
        public static void Save0toByteBuf(IByteBuffer buf, int iCount)
        {
            for (int i = 0; i < iCount; i++)
            {
                buf.WriteByte(0);
            }
        }

        /// <summary>
        /// 字节长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x89;
        }

        /// <summary>
        /// 对IP信息参数进行解码
        /// </summary>
        /// <param name="databuf">包含参数结构的缓冲区</param>
        public override void SetBytes(IByteBuffer databuf)
        {
            StringBuilder strbuilder = new StringBuilder(30);
            //data.markReaderIndex();
            //MAC地址
            for (int i = 0; i < 6; i++)
            {
                ushort iMacValue = databuf.ReadByte();
                string hex = iMacValue.ToString("X2");
                strbuilder.Append(hex);
                if (i < 5)
                {
                    strbuilder.Append("-");
                }
            }

            mMAC = strbuilder.ToString();

            //IP地址
            strbuilder.Clear();
            ReadIPByByteBuf(databuf, strbuilder);
            mIP = strbuilder.ToString();

            //IP掩码
            strbuilder.Clear();
            ReadIPByByteBuf(databuf, strbuilder);
            mIPMask = strbuilder.ToString();

            //网关
            strbuilder.Clear();
            ReadIPByByteBuf(databuf, strbuilder);
            mIPGateway = strbuilder.ToString();

            //DNS
            strbuilder.Clear();
            ReadIPByByteBuf(databuf, strbuilder);
            mDNS = strbuilder.ToString();

            //DNS
            strbuilder.Clear();
            ReadIPByByteBuf(databuf, strbuilder);
            mDNSBackup = strbuilder.ToString();

            //控制器网络协议类型
            mProtocolType = databuf.ReadByte();

            //控制器使用的TCP端口
            mTCPPort = databuf.ReadUnsignedShort();
            //控制器使用的UDP端口
            mUDPPort = databuf.ReadUnsignedShort();
            //控制器作为客户端时，目标服务器的端口
            mServerPort = databuf.ReadUnsignedShort();
            //控制器作为客户端时，目标服务器的IP 
            strbuilder.Clear();
            ReadIPByByteBuf(databuf, strbuilder);
            mServerIP = strbuilder.ToString();
            //自动获得IP
            mAutoIP = (databuf.ReadByte() == 1);
            //控制器作为客户端时，目标服务器的域名
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
                mServerAddr = string.Empty;
            }
            else
            {
                byte[] tmp = new byte[iCharCount];
                mServerAddr=databuf.ReadString( iCharCount, Encoding.ASCII);
            }
            strbuilder = null;
            return ;
        }

        /// <summary>
        /// 字节转换成字符串
        /// </summary>
        /// <param name="data">包含参数结构的缓冲区</param>
        /// <param name="strbuilder">字符串</param>
        public static void ReadIPByByteBuf(IByteBuffer data, StringBuilder strbuilder)
        {
            for (int i = 0; i < 4; i++)
            {
                strbuilder.Append(data.ReadByte());
                if (i < 3)
                {
                    strbuilder.Append(".");
                }
            }

        }
    }
}