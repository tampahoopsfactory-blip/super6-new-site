using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Protocol;
using DoNetDrive.Protocol.POS.TemplateMethod;
using DotNetty.Buffers;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.POS.Card
{
    /// <summary>
    /// 添加名单
    /// </summary>
    public class AddCard : TemplateWriteData_Base<WriteCard_Parameter,Data.CardDetail>
    {
        /// <summary>
        /// 当前命令进度
        /// </summary>
        protected int mStep = 0;

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public AddCard(Protocol.DESDriveCommandDetail cd, WriteCard_Parameter par) : base(cd, par)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataList"></param>
        /// <returns></returns>
        protected override TemplateResult_Base CreateResult(List<Data.CardDetail> DataList)
        {
            ReadAllCard_Result result = new ReadAllCard_Result(DataList);
            return result;
        }

        /// <summary>
        /// 检测结束指令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <returns></returns>
        protected override bool CheckResponseCompleted(DESPacket oPck)
        {
            var subPck = oPck.CommandPacket;
            return (subPck.CmdType == 0x35 &&
                subPck.CmdIndex == 4 &&
                subPck.CmdPar == 0xff);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databuf"></param>
        /// <param name="data"></param>
        protected override void WriteDataBodyToBuf(IByteBuffer databuf, TemplateData_Base data)
        {
            Data.CardDetail menuDetail = data as Data.CardDetail;
            menuDetail.GetBytes(databuf);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        public override int GetBatchCount()
        {
            return 20;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void CreateCommandPacket0()
        {
            var buf = GetNewCmdDataBuf(MaxBufSize);
            WriteDataToBuf(buf);
            Packet(0x05, 0x4, 0x00, (uint)buf.ReadableBytes, buf);
        }

        protected override void CreateCommandNextPacket(IByteBuffer buf)
        {
            Packet(0x05, 0x4, 0x00, (uint)buf.ReadableBytes, buf);
        }
    }
}
