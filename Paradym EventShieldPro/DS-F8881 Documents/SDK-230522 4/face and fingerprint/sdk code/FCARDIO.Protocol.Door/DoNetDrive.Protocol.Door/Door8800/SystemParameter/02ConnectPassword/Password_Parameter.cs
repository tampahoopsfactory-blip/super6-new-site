using DotNetty.Buffers;
using DoNetDrive.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.ConnectPassword
{
    /// <summary>
    /// 通讯密码
    /// </summary>
    public class Password_Parameter : AbstractParameter
    {
        /// <summary>
        /// 保存通讯密码的数组
        /// </summary>
        public string Password;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public Password_Parameter() { }

        /// <summary>
        /// 使用通讯密码初始化实例
        /// </summary>
        /// <param name="_PWD">通讯密码：十六进制字符串</param>
        public Password_Parameter(string _PWD)
        {
            Password = _PWD;

            if (!checkedParameter())
            {
                throw new ArgumentException("PWD Error");
            }
        }
        /// <summary>
        /// 使用字节数组初始化实例
        /// </summary>
        /// <param name="_PWD">表示通讯密码的字节数组</param>
        public Password_Parameter(byte[] _PWD) : this(_PWD.ToHex()) { }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (string.IsNullOrEmpty(Password))
            {
                return false;
            }
            if (!Password.IsHex())
            {
                return false;
            }
            if (Password.Length < 8)
            {
                Password = Password + new string('F', 8 - Password.Length);
            }
            if(Password.Length >8)
            {
                Password = Password.Substring(0, 8);
            }
            return true;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 4;
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
            Util.StringUtil.HextoByteBuf(Password, databuf);
            return databuf;
        }

        

        /// <summary>
        /// 解码参数
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (databuf.ReadableBytes != 4)
            {
                throw new ArgumentException("databuf Error");
            }
            Password = ByteBufferUtil.HexDump(databuf);
        }
    }
}