using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DoNetDrive.Common.Extensions;
using DoNetDrive.Protocol.Util;

namespace DoNetDrive.Protocol.Door.Door8800.Door.AlarmPassword
{
    /// <summary>
    /// 写入胁迫报警功能
    /// 功能开启后，在密码键盘读卡器上输入特定密码后就会报警；
    /// </summary>
    public class WriteAlarmPassword_parameter
        : AbstractParameter
    {
        /// <summary>
        /// 门号<br/>
        ///门端口在控制板中的索引号，取值：1-4
        /// </summary>
        public int DoorNum;

        /// <summary>
        /// 是否启用胁迫报警功能
        /// </summary>
        public bool Use;

        /// <summary>
        /// 胁迫报警密码，最大长度8个字符，由数字组成。
        /// </summary>
        public String Password;

        /// <summary>
        /// 报警选项
        /// 1   不开门，报警输出
        /// 2   开门，报警输出
        /// 3   锁定门，报警，只能软件解锁
        /// </summary>
        public int AlarmOption;

        /// <summary>
        /// 提供给 AlarmPassword_Result 使用
        /// </summary>
        public WriteAlarmPassword_parameter()
        {
        }

        /// <summary>
        /// 创建结构，并传入门号和是否启动胁迫报警功能、胁迫报警密码、报警选项
        /// </summary>
        /// <param name="door">门号</param>
        /// <param name="use">是否启动胁迫报警功能</param>
        /// <param name="pwd">胁迫报警密码</param>
        /// <param name="alarmOption">报警选项</param>
        public WriteAlarmPassword_parameter(byte door, bool use, String pwd, int alarmOption)
        {
            DoorNum = door;
            Use = use;
            AlarmOption = alarmOption;
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
                if (!pwd.IsNum() || pwd.Length > 8 || pwd.Length < 4)
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
            if (DoorNum < 1 || DoorNum > 4)
                throw new ArgumentException("door Is error!");

            if (!Password.IsNum() || Password.Length > 8 || Password.Length < 4)
                throw new ArgumentException("pwd is error!");
            if (AlarmOption < 1 || AlarmOption > 3)
                throw new ArgumentException("AlarmOption is error!");
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
            if (databuf.WritableBytes != 7)
            {
                throw new ArgumentException("databuf Error!");
            }
            databuf.WriteByte(DoorNum);
            databuf.WriteBoolean(Use);

            string shex = Password;
            if (shex.Length < 8) shex = shex + new string('f', 8 - shex.Length);
            databuf.WriteBytes(shex.HexToByte());

            databuf.WriteByte(AlarmOption);
            return databuf;
        }

        /// <summary>
        /// 指示此类结构编码为字节缓冲后的长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 7;
        }

        /// <summary>
        /// 将字节缓冲解码为类结构
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            DoorNum = databuf.ReadByte();
            Use = databuf.ReadBoolean();
            Password = StringUtil.ByteBufToHex(databuf, 4).TrimEnd('F');
            //Password = databuf.ReadByte().ToString();
            AlarmOption = databuf.ReadByte();
        }

    }
}
