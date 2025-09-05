using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Door.Door8800.Card
{
    /// <summary>
    /// Door88/Door58 读取单个卡片在控制器中的信息
    ///  成功返回结果参考 {@link ReadCardDetail_Result}
    /// </summary>
    public class ReadCardDetail
        : Door8800Command_ReadParameter
    {

        /// <summary>
        /// Door88/Door58 读取单个卡片在控制器中的信息
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public ReadCardDetail(INCommandDetail cd, ReadCardDetail_Parameter parameter) : base(cd, parameter) { }

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

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x07, 0x03, 0x01, 0x05, GetCmdData());
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
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (CheckResponse(oPck, 0x21))
            {
                var buf = oPck.CmdData;
                bool IsReady = false;
                IsReady = (buf.GetByte(0) != 0xff);

                Door8800.Data.CardDetail cardDetail = null;
                if (IsReady)
                {
                    cardDetail = new Door8800.Data.CardDetail();
                    cardDetail.SetBytes(buf);

                }


                ReadCardDetail_Result rst = new ReadCardDetail_Result(IsReady, cardDetail);
                _Result = rst;
                CommandCompleted();
            }
        }
    }
}
