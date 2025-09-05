using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Protocol;
using DoNetDrive.Protocol.POS.Data;
using DotNetty.Buffers;
using System.Collections.Generic;
using DoNetDrive.Protocol.POS.TemplateMethod;

namespace DoNetDrive.Protocol.POS.Subsidy
{
    /// <summary>
    /// 读取所有补贴
    /// </summary>
    public class ReadAllSubsidy : TemplateReadData_Base<SubsidyDetail>
    {
        public ReadAllSubsidy(DESDriveCommandDetail cd) : base(cd)
        {
        }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x07, 3);
            mReadBuffers = new List<IByteBuffer>();
            _ProcessMax = 1;
        }

        /// <summary>
        /// 检测下一包指令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <returns></returns>
        protected override bool CheckResponseNext(DESPacket oPck)
        {
            var subPck = oPck.CommandPacket;
            return (subPck.CmdType == 0x37 &&
                subPck.CmdIndex == 3 &&
                subPck.CmdPar == 0);
        }

        /// <summary>
        /// 检测结束指令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <returns></returns>
        protected override bool CheckResponseCompleted(DESPacket oPck)
        {
            var subPck = oPck.CommandPacket;
            return (subPck.CmdType == 0x37 &&
                subPck.CmdIndex == 3 &&
                subPck.CmdPar == 0xff && subPck.DataLen == 2);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        protected override TemplateResult_Base CreateResult(List<SubsidyDetail> dataList)
        {
            ReadAllSubsidy_Result result = new ReadAllSubsidy_Result(dataList);
            return result;
        }
    }
}
