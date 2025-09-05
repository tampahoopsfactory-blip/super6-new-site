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
    /// 读取出门按钮功能
    /// 可设定出门按钮的按下5秒后常开，还可以设定出门按钮的使用时段
    /// 成功返回结果参考  ReadPushButtonSetting_Result
    /// </summary>
    public class ReadPushButtonSetting
        : Door8800Command_Read_DoorParameter
    {
        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="value">需要读取的门号结构</param>
        public ReadPushButtonSetting(INCommandDetail cd, DoorPort_Parameter value) : base(cd, value) { }

       

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            DoorPort_Parameter model = _Parameter as DoorPort_Parameter;
            Packet(0x03, 0x0F, 0x00, 0x01, model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        
        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (CheckResponse(oPck, 0xE3))
            {
                var buf = oPck.CmdData;
                PushButtonSetting_Result rst = new PushButtonSetting_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }

       
    }
}
