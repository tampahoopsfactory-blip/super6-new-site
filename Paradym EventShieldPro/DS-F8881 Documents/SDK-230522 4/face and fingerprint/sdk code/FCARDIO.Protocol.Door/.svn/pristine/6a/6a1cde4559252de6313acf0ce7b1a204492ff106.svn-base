using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Protocol;
using DoNetDrive.Protocol.POS.Data;

namespace DoNetDrive.Protocol.POS.CardType
{
    /// <summary>
    /// 读取单个卡类命令
    /// </summary>
    public class ReadCardTypeDetail : Read_Command
    {
        public ReadCardTypeDetail(Protocol.DESDriveCommandDetail cd, ReadCardTypeDetail_Parameter parameter) : base(cd, parameter) { }

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            ReadCardTypeDetail_Parameter model = value as ReadCardTypeDetail_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        protected override void CreatePacket0()
        {
            Packet(0x08, 0x03, 0x01, 0x01, GetCmdData());
        }

        /// <summary>
        /// 获取参数结构的字节编码
        /// </summary>
        /// <returns></returns>
        private IByteBuffer GetCmdData()
        {
            ReadCardTypeDetail_Parameter model = _Parameter as ReadCardTypeDetail_Parameter;
            var acl = _Connector.GetByteBufAllocator();
            var buf = acl.Buffer(model.GetDataLen());
            model.GetBytes(buf);
            return buf;
        }

        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(DESPacket oPck)
        {
            if (CheckResponse(oPck, 0x08))
            {
                var buf = oPck.CommandPacket.CmdData;
                bool IsReady = false;
                IsReady = (buf.GetByte(0) != 0xff);

                CardTypeDetail CardTypeDetail = null;
                if (IsReady)
                {
                    CardTypeDetail = new CardTypeDetail();
                    CardTypeDetail.SetBytes(buf);

                }

                ReadCardTypeDetail_Result rst = new ReadCardTypeDetail_Result(IsReady, CardTypeDetail);
                _Result = rst;
                CommandCompleted();
            }
        }
    }
}
