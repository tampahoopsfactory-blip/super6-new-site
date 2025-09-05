using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800;
using System;

namespace DoNetDrive.Protocol.Fingerprint.Door.Remote
{
    /// <summary>
    /// 远程开门_带验证码
    /// </summary>
    public class Remote_CheckNum_Parameter : AbstractParameter
    {

        /// <summary>
        /// 验证码
        /// </summary>
        private byte CheckNum;


        /// <summary>
        /// 远程开关门参数初始化实例
        /// </summary>
        /// <param name="_CheckNum">验证码</param>
        public Remote_CheckNum_Parameter(byte _CheckNum)
        {
            CheckNum = _CheckNum;
            checkedParameter();
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
           
            if (CheckNum < 1 || CheckNum > 254)
            {
                throw new ArgumentException("CheckNum must between 1 and 254!");
            }
            return true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
        }

        /// <summary>
        /// 对远程开关门参数进行编码
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteByte(CheckNum);
           
            return databuf;
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
        /// 对远程开关门参数进行解码
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            CheckNum = databuf.ReadByte();
          
        }
    }
}
