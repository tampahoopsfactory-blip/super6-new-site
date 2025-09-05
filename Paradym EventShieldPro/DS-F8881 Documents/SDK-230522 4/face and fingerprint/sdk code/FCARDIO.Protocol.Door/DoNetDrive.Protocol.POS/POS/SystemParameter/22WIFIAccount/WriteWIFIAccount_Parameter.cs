using DotNetty.Buffers;
using System;
using System.Text;

namespace DoNetDrive.Protocol.POS.SystemParameter.WIFIAccount
{
    /// <summary>
    /// 设置WIFI账号及密码命令参数
    /// </summary>
    public class WriteWIFIAccount_Parameter : AbstractParameter
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password;

        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public WriteWIFIAccount_Parameter() { }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="Account">账号</param>
        /// <param name="Password">密码</param>
        public WriteWIFIAccount_Parameter(string Account, string Password)
        {
            this.Account = Account;
            this.Password = Password;
            if (!checkedParameter())
            {
                throw new ArgumentException("Parameter Error");
            }
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (Account == null || Account.Length > 0x20)
            {
                return false;
            }
            if (Password == null || Password.Length > 0x20 )
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
        /// 对有效期参数进行编码
        /// </summary>
        /// <param name="databuf">需要填充参数结构的字节缓冲区</param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes != GetDataLen())
            {
                throw new ArgumentException("databuf len error");
            }
            Util.StringUtil.WriteString(databuf, Account, 0x20, Encoding.GetEncoding("GB2312"));
            Util.StringUtil.WriteString(databuf, Password, 0x20, Encoding.GetEncoding("GB2312"));
            return databuf;
        }

        /// <summary>
        /// 获取数据长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x40;
        }

        /// <summary>
        /// 对有效期参数进行解码
        /// </summary>
        /// <param name="databuf">包含参数结构的缓冲区</param>
        public override void SetBytes(IByteBuffer databuf)
        {
            if (databuf.ReadableBytes != GetDataLen())
            {
                throw new ArgumentException("databuf Error");
            }
            Account = Util.StringUtil.GetString(databuf, 0x20, Encoding.GetEncoding("GB2312"));
            Password = Util.StringUtil.GetString(databuf, 0x20, Encoding.GetEncoding("GB2312"));
        }
    }
}
