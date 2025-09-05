using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.Controller
{
    /// <summary>
    /// 初始化控制器（控制器初始化后的数据状态：清空所有授权卡，清空所有节假日，清空所有开门时段，清空所有密码，清空所有记录，复位键盘密码，开锁保持时间为3秒）
    /// </summary>
    public class FormatController : Door8800Command_ReadParameter
    {
        /// <summary>
        /// 初始化控制器 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public FormatController(INCommandDetail cd) : base(cd) { }


        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x01, 0x0F, 0x00);
        }

        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            return;
        }
    }
}