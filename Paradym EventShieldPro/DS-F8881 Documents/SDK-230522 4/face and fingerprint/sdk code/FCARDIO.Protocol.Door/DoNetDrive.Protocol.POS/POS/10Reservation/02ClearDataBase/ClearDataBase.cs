using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.POS.Reservation
{
    /// <summary>
    /// 清空所有订餐信息
    /// </summary>
    public class ClearDataBase : Read_Command
    {
        /// <summary>
        /// 初始化命令结构 
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public ClearDataBase(Protocol.DESDriveCommandDetail cd) : base(cd, null) { }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x0A, 0x02);
        }

        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(DESPacket oPck)
        {
            return;
        }

        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext0(DESPacket oPck)
        {
            if (CheckResponse_OK(oPck))
            {
                CommandCompleted();
            }

        }
    }
}
