using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Protocol;
using DoNetDrive.Protocol.POS.Data;

namespace DoNetDrive.Protocol.POS.Card
{
    /// <summary>
    /// 读取单个名单命令
    /// </summary>
    public class ReadCardDetail : Read_Command
    {
        public ReadCardDetail(Protocol.DESDriveCommandDetail cd, ReadCardDetail_Parameter parameter) : base(cd, parameter) { }

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            ReadCardDetail_Parameter model = value as ReadCardDetail_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        protected override void CreatePacket0()
        {
            Packet(0x05, 0x03, 0x01, 0x04, GetCmdData());
        }

        /// <summary>
        /// 获取参数结构的字节编码
        /// </summary>
        /// <returns></returns>
        private IByteBuffer GetCmdData()
        {
            ReadCardDetail_Parameter model = _Parameter as ReadCardDetail_Parameter;
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
                var buf = oPck.CmdData;
                bool IsReady = false;
                IsReady = (buf.GetByte(0) != 0xff);

                CardDetail cardDetail = null;
                if (IsReady)
                {
                    cardDetail = new CardDetail();
                    cardDetail.SetBytes(buf);

                }

                ReadCardDetail_Result rst = new ReadCardDetail_Result(IsReady, cardDetail);
                _Result = rst;
                CommandCompleted();
            }
        }
    }
}
