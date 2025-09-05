using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Fingerprint.Person
{
    /// <summary>
    /// 读取单个人员在控制器中的信息
    /// </summary>
    public class ReadPersonDetail : Door8800Command_ReadParameter
    {

        /// <summary>
        /// 读取单个人员在控制器中的信息
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public ReadPersonDetail(INCommandDetail cd, ReadPersonDetail_Parameter parameter) : base(cd, parameter) { }

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            ReadPersonDetail_Parameter model = value as ReadPersonDetail_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
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
            ReadPersonDetail_Parameter model = _Parameter as ReadPersonDetail_Parameter;
            var acl = _Connector.GetByteBufAllocator();
            var buf = acl.Buffer(model.GetDataLen());
            model.GetBytes(buf);
            return buf;
        }

        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (CheckResponse(oPck, 0xA1))
            {
                var buf = oPck.CmdData;
                bool IsReady = false;
                IsReady = (buf.GetByte(0) != 0xff);

                Data.Person person = null;
                if (IsReady)
                {
                    person = new Data.Person();
                    person.SetBytes(buf);

                }


                ReadPersonDetail_Result rst = new ReadPersonDetail_Result(IsReady, person);
                _Result = rst;
                CommandCompleted();
            }
        }
    }
}
