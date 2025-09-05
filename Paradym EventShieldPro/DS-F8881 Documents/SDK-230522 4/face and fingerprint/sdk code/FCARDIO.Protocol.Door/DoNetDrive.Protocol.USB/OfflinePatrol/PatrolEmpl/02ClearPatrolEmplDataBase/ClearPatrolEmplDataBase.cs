using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.USBDrive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.PatrolEmpl.ClearPatrolEmplDataBase
{
    /// <summary>
    /// 删除所有巡更人员
    /// </summary>
    public class ClearPatrolEmplDataBase : Read_Command
    {
        /// <summary>
        ///  初始化命令
        /// </summary>
        /// <param name="cd"></param>
        public ClearPatrolEmplDataBase(INCommandDetail cd) : base(cd, null)
        {
        }


        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(3, 2);
        }


        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(USBDrivePacket oPck)
        {
            
        }
    }
}
