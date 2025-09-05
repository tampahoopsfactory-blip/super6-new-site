using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoNetDrive.Common;
using DotNetty.Buffers;


namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.TCPClient
{
    /// <summary>
    /// 强制断开TCP连接_参数
    /// </summary>
    public class TCPClient_Parameter : AbstractParameter
    {
        /// <summary>
        /// TCP客户端信息
        /// </summary>
        public TCPClientDetail tCPClientDetail;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public TCPClient_Parameter() { }

        /// <summary>
        /// 使用强制断开TCP连接参数初始化实例
        /// </summary>
        /// <param name="_TCPClientDetail">强制断开TCP连接参数</param>
        public TCPClient_Parameter(TCPClientDetail _TCPClientDetail)
        {
            tCPClientDetail = _TCPClientDetail;
            if (!checkedParameter())
            {
                throw new ArgumentException("tCPClientDetail Error");
            }
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (tCPClientDetail == null)
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
            return;
        }

        /// <summary>
        /// 对强制断开TCP连接参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            Utility.StringUtility.SaveIPToByteBuf(tCPClientDetail.IP[0], databuf);
            databuf.WriteShort(tCPClientDetail.TCPPort[0]);
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x06;
        }

        /// <summary>
        /// 对强制断开TCP连接参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (tCPClientDetail == null)
            {
                tCPClientDetail = new TCPClientDetail();
            }
            tCPClientDetail.TCPClientNum = databuf.ReadByte();
            tCPClientDetail.IP = new string[tCPClientDetail.TCPClientNum];
            tCPClientDetail.TCPPort = new ushort[tCPClientDetail.TCPClientNum];
            tCPClientDetail.ConnectTime = new DateTime[tCPClientDetail.TCPClientNum];
            StringBuilder strbuilder = new StringBuilder();
            for (int i = 0; i < tCPClientDetail.TCPClientNum; i++)
            {
                //IP地址
                strbuilder.Clear();
                Utility.StringUtility.ReadIPByByteBuf(databuf, strbuilder);
                tCPClientDetail.IP[i] = strbuilder.ToString();
                //TCP端口
                tCPClientDetail.TCPPort[i] = databuf.ReadUnsignedShort();
                //接入时间
                byte[] btData = new byte[6];
                databuf.ReadBytes(btData, 0, 6);
                tCPClientDetail.ConnectTime[i] = DataUtil.ByteToDateTime(btData);
            }
        }
    }
}
