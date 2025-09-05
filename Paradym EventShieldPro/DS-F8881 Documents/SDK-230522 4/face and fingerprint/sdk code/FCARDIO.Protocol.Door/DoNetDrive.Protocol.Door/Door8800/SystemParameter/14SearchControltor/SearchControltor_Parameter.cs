using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.SearchControltor
{
    /// <summary>
    /// 搜索控制器
    /// </summary>
    public class SearchControltor_Parameter : AbstractParameter
    {
        /// <summary>
        /// 网络标识 取值范围：1-65535
        /// </summary>
        public ushort NetNum;

        /// <summary>
        /// 命令是否用UDP广播发送？
        /// </summary>
        public bool UDPBroadcast;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public SearchControltor_Parameter() { }

        /// <summary>
        /// 使用网络标识参数初始化实例
        /// </summary>
        /// <param name="_NetNum">网络标识参数</param>
        public SearchControltor_Parameter(ushort _NetNum)
        {
            NetNum = _NetNum;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (NetNum == 0)
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
        /// 对网络标识参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf.WriteUnsignedShort(NetNum);
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x02;
        }

        /// <summary>
        /// 对网络标识参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            NetNum = databuf.ReadUnsignedShort();
        }
    }
}