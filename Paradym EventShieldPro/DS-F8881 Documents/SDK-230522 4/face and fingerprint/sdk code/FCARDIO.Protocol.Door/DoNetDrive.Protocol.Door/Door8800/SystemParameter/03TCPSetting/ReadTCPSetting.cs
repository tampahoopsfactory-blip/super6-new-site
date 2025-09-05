using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.TCPSetting
{
    /// <summary>
    /// 获取TCP参数
    /// </summary>
    public class ReadTCPSetting : Door8800Command_ReadParameter
    {

        /// <summary>
        /// 命令是否使用广播
        /// </summary>
        protected bool UDPBroadcast = false;

        /// <summary>
        /// 获取TCP参数 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="bUDPBroadcast">命令是否使用广播</param>
        public ReadTCPSetting(INCommandDetail cd,bool bUDPBroadcast) : base(cd) { UDPBroadcast = bUDPBroadcast; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cd"></param>
        public ReadTCPSetting(INCommandDetail cd) : this(cd,false) {}


        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x01, 0x06);
            if (UDPBroadcast)
            {
                DoorPacket.SetUDPBroadcastPacket();
            }
        }

        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (CheckResponse(oPck, 0x89))
            {
                var buf = oPck.CmdData;
                ReadTCPSetting_Result rst = new ReadTCPSetting_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }
    }
}