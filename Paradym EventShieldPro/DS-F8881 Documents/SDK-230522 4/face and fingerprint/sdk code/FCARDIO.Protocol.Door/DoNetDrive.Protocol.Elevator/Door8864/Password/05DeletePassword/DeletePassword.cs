using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.Password;
using DoNetDrive.Protocol.OnlineAccess;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.Elevator.FC8864.Password
{
    /// <summary>
    /// 将密码列表从控制器删除
    /// </summary>
    public class DeletePassword : WritePasswordBase<PasswordDetail,Password_Parameter>
    {
        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>  
        protected override void CreateCommandPacket0()
        {
            var buf = GetNewCmdDataBuf(MaxBufSize);
            WritePasswordToBuf(buf);
            Packet(0x45, 0x5, 0x00, (uint)buf.ReadableBytes, buf);
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public DeletePassword(INCommandDetail cd, Password_Parameter par) : base(cd, par)
        {
            MaxBufSize = (mBatchCount * mDeleteDataLen) + 4;
            mBatchCount = 1;
        }

        /// <summary>
        /// 检测结束指令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <returns></returns>
        protected override bool CheckResponseCompleted(OnlineAccessPacket oPck)
        {
            return (oPck.CmdType == 0x55 &&
                oPck.CmdIndex == 5 &&
                oPck.CmdPar == 0xff);
        }

        /// <summary>
        /// 将数据部分写入到缓冲区
        /// </summary>
        /// <param name="databuf"></param>
        /// <param name="password">要写入到缓冲区的密码</param>
        protected override void WritePasswordBodyToBuf(IByteBuffer databuf, PasswordDetail password)
        {
            password.GetDeleteBytes(databuf);
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
        /// 没有实现
        /// </summary>
        /// <param name="passwordList"></param>
        /// <returns></returns>
        protected override ReadAllPassword_Result_Base<PasswordDetail> CreateResult(List<PasswordDetail> passwordList)
        {
            return new ReadAllPassword_Result_Base<PasswordDetail>(passwordList);
        }
    }
}
