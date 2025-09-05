using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.FrameCommand;
using DoNetDrive.Protocol.POS.Protocol;

namespace DoNetDrive.Protocol.POS
{
    /// <summary>
    /// 针对命令中的写参数命令进行抽象封装
    /// </summary>
    public abstract class CommandEx : DESCommand
    {

        protected DESCommandPacket PosPacket;

        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含命令所需要的其他参数</param>
        public CommandEx(Protocol.DESDriveCommandDetail cd, INCommandParameter par) : base(cd, par)
        {

        }

        /// <summary>
        /// 命令重发时，对命令中一些缓冲做清空或参数重置<br/>
        /// 此命令一般情况下不需要实现！
        /// </summary>
        protected override void CommandReSend()
        {
            return;
        }

        /// <summary>
        /// 释放命令占用的内存<br/>
        /// 此命令一般情况下不需要实现！
        /// </summary>
        protected override void Release1()
        {
            return;
        }



        /// <summary>
        /// 检查指令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <param name="CmdType">命令类型</param>
        /// <param name="CmdIndex">命令索引</param>
        /// <param name="CmdPar">命令参数</param>
        /// <returns></returns>
        protected virtual bool CheckResponse(DESCommandPacket oPck, byte CmdType, byte CmdIndex, byte CmdPar)
        {
            return (oPck.CmdType == CmdType &&
                oPck.CmdIndex == CmdIndex &&
                oPck.CmdPar == CmdPar);

        }

        /// <summary>
        /// 检查指令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <param name="CmdType">命令类型</param>
        /// <param name="CmdIndex">命令索引</param>
        /// <param name="CmdPar">命令参数</param>
        /// <param name="dl">参数长度</param>
        /// <returns></returns>
        protected virtual bool CheckResponse(DESCommandPacket oPck, byte CmdType, byte CmdIndex, byte CmdPar, int dl)
        {
            return (oPck.CmdType == CmdType &&
                oPck.CmdIndex == CmdIndex &&
                oPck.CmdPar == CmdPar &&
                oPck.DataLen == dl);

        }

        /// <summary>
        /// 检查指令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <param name="CmdType">命令类型</param>
        /// <param name="CmdIndex">命令索引</param>
        /// <param name="CmdPar">命令参数</param>
        /// <param name="dl">参数长度</param>
        /// <returns></returns>
        protected virtual bool CheckResponse(DESCommandPacket oPck, int dl)
        {
            return (oPck.DataLen == dl);

        }



        /// <summary>
        /// 重置命令内容
        /// </summary>
        /// <param name="ct">命令类型</param>
        /// <param name="ci">命令索引</param>
        /// <param name="cp">命令参数</param>
        /// <param name="dl">数据长度</param>
        protected virtual void RewritePacket(byte ct, byte ci, byte cp, int dl)
        {
            PosPacket.CmdType = ct;
            PosPacket.CmdIndex = ci;
            PosPacket.CmdPar = cp;
            PosPacket.DataLen = dl;

        }


        /// <summary>
        /// 重置命令内容
        /// </summary>
        /// <param name="ci">命令索引</param>
        /// <param name="cp">命令参数</param>
        /// <param name="dl">数据长度</param>
        protected virtual void RewritePacket(byte ci, byte cp, int dl)
        {
            PosPacket.CmdIndex = ci;
            PosPacket.CmdPar = cp;
            PosPacket.DataLen = dl;

        }

    }
}
