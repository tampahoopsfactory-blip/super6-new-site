using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.SearchControltor
{
    /// <summary>
    /// 获取搜索控制器_结果
    /// </summary>
    public class SearchControltor_Result : AbstractData, INCommandResult
    {
        /// <summary>
        /// TCP参数
        /// </summary>
        private TCPSetting.TCPDetail _TCP;


        /// <summary>
        /// SN参数
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// SN的字节数组
        /// </summary>
        public byte[] SNByte { get; set; }

        /// <summary>
        /// TCP
        /// </summary>
        public TCPSetting.TCPDetail TCP => _TCP;

        /// <summary>
        /// 实例化TCP参数
        /// </summary>
        public SearchControltor_Result()
        {
            _TCP = new TCPSetting.TCPDetail();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _TCP = null;
            return;
        }

        /// <summary>
        /// 参数编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x89;
        }

        /// <summary>
        /// SN 是否为可见字符
        /// </summary>
        /// <returns></returns>
        public bool SNIsAsciiString()
        {
            if (SNByte == null) return false;
            bool SNIsAscII = true;
            foreach (var b in SNByte)
            {

                if (!(
                    (b >= 0x41 && b <= 0x5A) || //A-Z
                    (b >= 0x30 && b <= 0x39) || //0-9
                    b == 0x2D//-
                    )
                    )
                {
                    SNIsAscII = false;
                    break;
                }
            }
            return SNIsAscII;

        }

        /// <summary>
        /// 对TCP参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            _TCP.SetBytes(databuf);
        }
    }
}
