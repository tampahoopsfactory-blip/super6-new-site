using DotNetty.Buffers;
using System;
using DoNetDrive.Common.Extensions;
using DoNetDrive.Protocol.Util;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.KeyboardCardIssuingManage
{
    /// <summary>
    /// 设置 键盘发卡管理功能
    /// </summary>
    public class WriteKeyboardCardIssuingManage_Parameter : AbstractParameter
    {

        /// <summary>
        /// 是否启用功能
        /// </summary>
        public bool Use;

        /// <summary>
        /// 密码，最大长度8个字符，由数字组成。
        /// </summary>
        public string Password;


        /// <summary>
        /// 提供给 AlarmPassword_Result 使用
        /// </summary>
        public WriteKeyboardCardIssuingManage_Parameter()
        {
        }

        /// <summary>
        /// 创建结构，并传入门号和是否启动胁迫报警功能、胁迫报警密码、报警选项
        /// </summary>
        /// <param name="use">是否启动胁迫报警功能</param>
        /// <param name="pwd">胁迫报警密码</param>
        public WriteKeyboardCardIssuingManage_Parameter(bool use, string pwd)
        {
            Use = use;
            SetPassword(pwd);
        }

        /// <summary>
        /// 胁迫报警密码，最大长度8个字符，由数字组成。
        /// </summary>
        /// <param name="pwd">胁迫报警密码</param>
        public void SetPassword(String pwd)
        {
            if (string.IsNullOrEmpty(pwd))
            {
                this.Password = null;
            }
            else
            {
                if (!pwd.IsHex() || pwd.Length > 8 || pwd.Length < 4)
                {
                    throw new ArgumentException("pwd is error!");
                }

                this.Password = pwd;
            }
        }


        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (!Password.IsHex() || Password.Length > 8 || Password.Length < 4)
                throw new ArgumentException("pwd is error!");
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
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {

            databuf.WriteBoolean(Use);

            string shex = Password;
            if (shex.Length < 8) shex = shex + new string('f', 8 - shex.Length);
            databuf.WriteBytes(shex.HexToByte());

            return databuf;
        }

        /// <summary>
        /// 指示此类结构编码为字节缓冲后的长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 5;
        }

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            Use = databuf.ReadBoolean();
            Password = StringUtil.ByteBufToHex(databuf, 4).TrimEnd('F');
        }

    }
}
