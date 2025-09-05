using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.POS.Protocol;
using DoNetDrive.Protocol.POS.Data;

namespace DoNetDrive.Protocol.POS.Menu
{
    /// <summary>
    /// 读取单个菜单命令
    /// </summary>
    public class ReadMenuDetail : Read_Command
    {
        public ReadMenuDetail(Protocol.DESDriveCommandDetail cd, ReadMenuDetail_Parameter parameter) : base(cd, parameter) { }

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            ReadMenuDetail_Parameter model = value as ReadMenuDetail_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        protected override void CreatePacket0()
        {
            Packet(0x06, 0x03, 0x01, 0x04, GetCmdData());
        }

        /// <summary>
        /// 获取参数结构的字节编码
        /// </summary>
        /// <returns></returns>
        private IByteBuffer GetCmdData()
        {
            ReadMenuDetail_Parameter model = _Parameter as ReadMenuDetail_Parameter;
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
            if (CheckResponse(oPck, 0x40))
            {
                var buf = oPck.CommandPacket.CmdData;
                bool IsReady = false;
                IsReady = (buf.GetByte(0) != 0xff);

                MenuDetail menuDetail = null;
                if (IsReady)
                {
                    menuDetail = new MenuDetail();
                    menuDetail.SetBytes(buf);

                }

                ReadMenuDetail_Result rst = new ReadMenuDetail_Result(IsReady, menuDetail);
                _Result = rst;
                CommandCompleted();
            }
        }
    }
}
