using DoNetDrive.Protocol.POS.Data;
using DoNetDrive.Protocol.POS.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.POS.ConsumeParameter.ReservationRule
{
    /// <summary>
    /// 读取订餐规则
    /// </summary>
    public class ReadReservationRule : Read_Command
    {
        /// <summary>
        /// 获取设备有效期 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public ReadReservationRule(DESDriveCommandDetail cd) : base(cd) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x03, 0x0C);
        }

        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(DESPacket oPck)
        {
            if (CheckResponse(oPck, 392))
            {
                var buf = oPck.CommandPacket.CmdData;
                ReadReservationRule_Result rst = new ReadReservationRule_Result();
                WeekReservationRule wr = new WeekReservationRule();
                wr.SetBytes(buf);
                rst.WeekReservationRule = wr;
                _Result = rst;
                //rst.SetBytes(buf);
                CommandCompleted();
            }
        }
    }
}
