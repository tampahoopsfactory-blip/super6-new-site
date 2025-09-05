using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Door.Door8800.Door.PushButtonSetting
{
    /// <summary>
    /// 写入出门按钮功能
    /// 可设定出门按钮的按下5秒后常开，还可以设定出门按钮的使用时段
    /// 成功返回结果参考  ReadPushButtonSetting_Result
    /// </summary>
    public class WritePushButtonSetting
        : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="value">包括门号和出门按钮功能</param>
        public WritePushButtonSetting(INCommandDetail cd, WritePushButtonSetting_Parameter value) : base(cd, value) { }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WritePushButtonSetting_Parameter model = value as WritePushButtonSetting_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            WritePushButtonSetting_Parameter model = _Parameter as WritePushButtonSetting_Parameter;
            Packet(0x03, 0x0F, 0x01, 0xE3, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }


    }
}
