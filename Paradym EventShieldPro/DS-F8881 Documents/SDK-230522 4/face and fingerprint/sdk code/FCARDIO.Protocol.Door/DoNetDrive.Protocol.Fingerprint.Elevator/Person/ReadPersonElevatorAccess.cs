using System;
using System.Collections.Generic;
using System.Text;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Fingerprint.Elevator
{
    /// <summary>
    /// 读取人员电梯扩展权限
    /// </summary>
    public class ReadPersonElevatorAccess : Door8800Command_ReadParameter
    {
        /// <summary>
        /// 创建读取人员电梯扩展权限的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">人员编号</param>
        public ReadPersonElevatorAccess(INCommandDetail cd, ReadPersonElevatorAccess_Parameter par) : base(cd,par) { }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            var model = value as ReadPersonElevatorAccess_Parameter;
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
            var model = _Parameter as ReadPersonElevatorAccess_Parameter;

            Packet(7, 6, 1, (uint)model.GetDataLen(), model.GetBytes(GetNewCmdDataBuf(model.GetDataLen())));
        }

        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (CheckResponse(oPck, 69))
            {
                var buf = oPck.CmdData;
                ReadPersonElevatorAccess_Result rst = new ReadPersonElevatorAccess_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }
    }
}
