using DoNetDrive.Protocol.POS.Data;
using DoNetDrive.Protocol.POS.Protocol;
using DoNetDrive.Protocol.POS.TemplateMethod;
using DotNetty.Buffers;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.POS.CardType
{
    /// <summary>
    /// 添加卡类
    /// </summary>
    public class AddCardType : TemplateWriteData_Base<WriteCardTypeDetail_Parameter, CardTypeDetail>
    {

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public AddCardType(Protocol.DESDriveCommandDetail cd, WriteCardTypeDetail_Parameter par) : base(cd, par)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DataList"></param>
        /// <returns></returns>
        protected override TemplateResult_Base CreateResult(List<CardTypeDetail> DataList)
        {
            ReadDataBase_Result result = new ReadDataBase_Result(DataList);
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
            return (subPck.CmdType == 0x38 &&
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
            CardTypeDetail cardTypeDetail = data as CardTypeDetail;
            cardTypeDetail.GetBytes(databuf);
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
            Packet(0x08, 0x4, 0x00, (uint)buf.ReadableBytes, buf);
        }

        protected override void CreateCommandNextPacket(IByteBuffer buf)
        {
            Packet(0x08, 0x4, 0x00, (uint)buf.ReadableBytes, buf);
        }
    }
}
