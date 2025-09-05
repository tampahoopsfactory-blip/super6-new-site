using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Protocol;
using DoNetDrive.Protocol.POS.Data;

namespace DoNetDrive.Protocol.POS.Subsidy
{
    /// <summary>
    /// 读取单个补贴命令
    /// </summary>
    public class ReadSubsidyDetail : Read_Command
    {
        public ReadSubsidyDetail(Protocol.DESDriveCommandDetail cd, ReadSubsidyDetail_Parameter parameter) : base(cd, parameter) { }

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            ReadSubsidyDetail_Parameter model = value as ReadSubsidyDetail_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        protected override void CreatePacket0()
        {
            Packet(0x07, 0x03, 0x01, 0x04, GetCmdData());
        }

        /// <summary>
        /// 获取参数结构的字节编码
        /// </summary>
        /// <returns></returns>
        private IByteBuffer GetCmdData()
        {
            ReadSubsidyDetail_Parameter model = _Parameter as ReadSubsidyDetail_Parameter;
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
            if (CheckResponse(oPck, 0x10))
            {
                var buf = oPck.CommandPacket.CmdData;
                bool IsReady = false;
                IsReady = (buf.GetByte(0) != 0xff);

                SubsidyDetail subsidyDetail = null;
                if (IsReady)
                {
                    subsidyDetail = new SubsidyDetail();
                    subsidyDetail.SetBytes(buf);

                }


                ReadSubsidyDetail_Result rst = new ReadSubsidyDetail_Result(IsReady, subsidyDetail);
                _Result = rst;
                CommandCompleted();
            }
        }
    }
}
