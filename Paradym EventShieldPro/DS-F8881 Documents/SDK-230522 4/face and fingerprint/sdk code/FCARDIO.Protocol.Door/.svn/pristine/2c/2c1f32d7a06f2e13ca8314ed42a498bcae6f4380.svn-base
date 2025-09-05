using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Fingerprint.Alarm.AlarmPassword
{
    /// <summary>
    /// 读取 胁迫报警密码
    /// </summary>
    public class ReadAlarmPassword : Door8800Command_ReadParameter
    {

        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="value">需要读取的门号结构</param>
        public ReadAlarmPassword(INCommandDetail cd) : base(cd, null)
        {
        }


        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x04, 0x05, 0x01);
        }


        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (CheckResponse(oPck, 0x06))
            {
                var buf = oPck.CmdData;
                ReadAlarmPassword_Result rst = new ReadAlarmPassword_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }

    }
}
