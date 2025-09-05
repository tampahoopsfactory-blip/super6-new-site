using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door89H
{
    /// <summary>
    /// 上传固件的参数
    /// </summary>
    public class UpdateSoftware_Parameter : AbstractParameter
    {
        /// <summary>
        /// 固件文件
        /// </summary>
        public byte[] Datas;

        /// <summary>
        /// 固件解密后的CRC32
        /// </summary>
        public uint SoftwareCRC32;

        /// <summary>
        /// 等待校验的时间，单位毫秒
        /// </summary>
        public int WaitVerifyTime;

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="datas">固件文件</param>
        public UpdateSoftware_Parameter( byte[] datas,uint crc)
        {

            WaitVerifyTime = 8000;
            Datas = datas;
            SoftwareCRC32 = crc;
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Datas == null || Datas.Length == 0 )
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
            return Datas.Length;
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
            return databuf;
        }


        /// <summary>
        /// 解码参数
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {

        }
    }
}
