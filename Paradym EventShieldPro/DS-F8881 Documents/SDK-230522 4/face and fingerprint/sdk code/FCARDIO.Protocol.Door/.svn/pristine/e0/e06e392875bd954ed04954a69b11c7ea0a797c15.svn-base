using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.TemplateMethod;
using DoNetDrive.Protocol.POS.Protocol;
using DotNetty.Buffers;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.POS.Card
{
    /// <summary>
    /// 删除名单命令
    /// </summary>
    public class DeleteCard : TemplateWriteData_Base<WriteCard_Parameter, Data.CardDetail>
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
        public DeleteCard(Protocol.DESDriveCommandDetail cd, WriteCard_Parameter par) : base(cd, par)
        {
            par.mIsDeleteCommand = true;
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
        /// 
        /// </summary>
        /// <param name="databuf"></param>
        /// <param name="data"></param>
        protected override void WriteDataBodyToBuf(IByteBuffer databuf, TemplateData_Base data)
        {
            Data.CardDetail cardDetail = data as Data.CardDetail;
            cardDetail.GetDeleteBytes(databuf);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void CreateCommandPacket0()
        {
            var buf = GetNewCmdDataBuf(MaxBufSize);
            WriteDataToBuf(buf);
            Packet(0x05, 0x05, 0x00, (uint)buf.ReadableBytes, buf);
        }

        protected override bool CheckResponseCompleted(DESPacket oPck)
        {
            return false;
        }

        protected override void CreateCommandNextPacket(IByteBuffer buf)
        {
            Packet(0x05, 0x05, 0x00, (uint)buf.ReadableBytes, buf);
        }
    }
}
