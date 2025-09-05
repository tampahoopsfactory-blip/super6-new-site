using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.SystemParameter.FunctionParameter
{
    /// <summary>
    /// 设置卡片到期提示
    /// </summary>
    public class WriteCardPeriodSpeak : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 设置卡片到期提示 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含卡片到期提示参数</param>
        public WriteCardPeriodSpeak(INCommandDetail cd, WriteCardPeriodSpeak_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteCardPeriodSpeak_Parameter model = value as WriteCardPeriodSpeak_Parameter;
            if (model == null)
            {
                return false;
            }

            return model.checkedParameter();
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteCardPeriodSpeak_Parameter model = _Parameter as WriteCardPeriodSpeak_Parameter;
            Packet(0x01, 0x0A, 0x10, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }
    }
}