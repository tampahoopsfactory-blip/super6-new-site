using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.Password;
using DoNetDrive.Protocol.OnlineAccess;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.Elevator.FC8864.Password
{
    /// <summary>
    /// 将密码列表写入到控制器
    /// </summary>
    public class AddPassword : WritePasswordBase<PasswordDetail,Password_Parameter>
    {
        
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public AddPassword(INCommandDetail cd, Password_Parameter par) : base(cd, par)
        {
            MaxBufSize = (mBatchCount * mParDataLen) + 4;
        }

        /// <summary>
        /// 将数据部分写入到缓冲区
        /// </summary>
        /// <param name="password">要写入的密码</param>
        /// <param name="databuf"></param>
        protected override void WritePasswordBodyToBuf(IByteBuffer databuf, PasswordDetail password)
        {
            password.GetBytes(databuf);
        }

       
        /// <summary>
        /// 检测结束指令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <returns></returns>
        protected override bool CheckResponseCompleted(OnlineAccessPacket oPck)
        {
            return (oPck.CmdType == 0x55 &&
                oPck.CmdIndex == 4 &&
                oPck.CmdPar == 0xff);
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreateCommandPacket0()
        {
            var buf = GetNewCmdDataBuf(MaxBufSize);
            WritePasswordToBuf(buf);
            Packet(0x45, 0x4, 0x00, (uint)buf.ReadableBytes, buf);
        }

        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            base.CommandNext1(oPck);
        }
        /// <summary>
        /// 创建返回值
        /// </summary>
        /// <param name="passwordList">无法写入的密码列表</param>
        /// <returns></returns>
        protected override ReadAllPassword_Result_Base<PasswordDetail> CreateResult(List<PasswordDetail> passwordList)
        {
            ReadAllPassword_Result result = new ReadAllPassword_Result(passwordList);
            return result;
        }
    }
}
