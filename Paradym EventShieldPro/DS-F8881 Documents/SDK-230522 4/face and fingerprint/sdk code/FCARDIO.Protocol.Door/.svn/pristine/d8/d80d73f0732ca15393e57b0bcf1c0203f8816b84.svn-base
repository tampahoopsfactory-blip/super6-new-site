using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Door.ManageKeyboardSetting
{
    /// <summary>
    /// 键盘管理功能
    /// </summary>
    public class WriteManageKeyboardSetting : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 参数对象
        /// </summary>
        protected WriteManageKeyboardSetting_Parameter mManageKeyboardPar;
        /// <summary>
        /// 当前命令步骤
        /// </summary>
        protected int Step;

        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="value"></param>
        public WriteManageKeyboardSetting(INCommandDetail cd, WriteManageKeyboardSetting_Parameter value) : base(cd, value) { mManageKeyboardPar = value; }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteManageKeyboardSetting_Parameter model = value as WriteManageKeyboardSetting_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            var buf = GetNewCmdDataBuf(2);
            Packet(0x03, 0x15, 0x00, 2, mManageKeyboardPar.Setting_GetBytes(buf));
            Step = 1;
            IniPacketProcess();
        }

        /// <summary>
        /// 初始化指令的步骤数
        /// </summary>
        private void IniPacketProcess()
        {
            _ProcessMax = 2;


        }


        /// <summary>
        /// 接收到响应，开始处理下一步命令
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext0(OnlineAccessPacket oPck)
        {

            switch (Step)
            {
                case 1:
                    if (CheckResponse_OK(oPck))
                    {
                        WritePassword();
                    }
                    break;
                case 2:

                    CommandCompleted();
                    break;
                default:
                    break;
            }
            return;
        }

        /// <summary>
        /// 写密码
        /// </summary>
        private void WritePassword()
        {
            _ProcessStep = 2;
            var buf = GetNewCmdDataBuf(5);
            Packet(0x03, 0x15, 0x02, 5, mManageKeyboardPar.Password_GetBytes(buf));
            Step = 2;
            CommandReady();
        }


    }
}
