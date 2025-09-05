using System;
using System.Collections.Generic;
using System.Text;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.OnlineAccess;


namespace DoNetDrive.Protocol.Fingerprint.Elevator
{
    /// <summary>
    /// 读取定时常开参数
    /// </summary>
    public class ReadTimingOpen : Door8800Command_ReadParameter
    {
        /// <summary>
        /// 创建读取定时常开参数的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">端口号</param>
        public ReadTimingOpen(INCommandDetail cd, ReadTimingOpen_Parameter par) : base(cd, par) { }


        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            var model = value as ReadTimingOpen_Parameter;
            if (model == null)
            {
                return false;
            }
            return model.checkedParameter();
        }


        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            var model = _Parameter as ReadTimingOpen_Parameter;

            Packet(0x03, 0x26, 0x00, 0x01, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (CheckResponse(oPck, 0xE3))
            {
                var buf = oPck.CmdData;
                ReadTimingOpen_Result rst = new ReadTimingOpen_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }
    }
}
