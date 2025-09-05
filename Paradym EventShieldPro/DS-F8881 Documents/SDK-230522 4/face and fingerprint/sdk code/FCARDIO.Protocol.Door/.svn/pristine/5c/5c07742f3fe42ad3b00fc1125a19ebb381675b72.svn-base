using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.USBDrive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.PatrolEmpl.PatrolEmplDetail
{
    /// <summary>
    /// 读取单个巡更人员资料
    /// </summary>
    public class ReadPatrolEmplDetail : Read_Command
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        public ReadPatrolEmplDetail(INCommandDetail cd, ReadPatrolEmplDetail_Parameter par ) : base(cd, par)
        {
        }


        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            ReadPatrolEmplDetail_Parameter model = _Parameter as ReadPatrolEmplDetail_Parameter;
            Packet(3, 4, 4, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }


        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(USBDrivePacket oPck)
        {
            if (CheckResponse(oPck, 0x0F))
            {
                var buf = oPck.CmdData;
                ReadPatrolEmplDetail_Result rst = new ReadPatrolEmplDetail_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }
    }
}
