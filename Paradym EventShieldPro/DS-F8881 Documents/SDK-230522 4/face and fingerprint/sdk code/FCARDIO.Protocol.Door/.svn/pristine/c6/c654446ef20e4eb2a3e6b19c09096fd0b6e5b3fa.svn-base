using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Door.Door8800.Door.AnyCardSetting
{
    /// <summary>
    /// 设置全卡开门功能
    /// 所有的卡都能开门，不需要权限首选注册，只要读卡器能识别就能开门。
    /// 成功返回结果参考 {@link ReadAnyCardSetting_Result}
    /// </summary>
    public class WriteAnyCardSetting
         : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="value">包括门号和出门按钮功能</param>
        public WriteAnyCardSetting(INCommandDetail cd, WriteAnyCardSetting_Parameter value) : base(cd, value) { }
        
        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteAnyCardSetting_Parameter model = value as WriteAnyCardSetting_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            WriteAnyCardSetting_Parameter model = _Parameter as WriteAnyCardSetting_Parameter;
            Packet(0x03, 0x11, 0x00, 4, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

    }
}
