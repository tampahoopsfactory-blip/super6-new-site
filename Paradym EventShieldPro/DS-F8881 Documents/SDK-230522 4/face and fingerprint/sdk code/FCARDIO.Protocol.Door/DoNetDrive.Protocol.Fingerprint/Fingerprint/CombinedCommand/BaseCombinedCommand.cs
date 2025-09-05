using DoNetDrive.Core.Command;
using DoNetDrive.Core.Connector;
using DoNetDrive.Core.Packet;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using DotNetty.Buffers;
using System;


namespace DoNetDrive.Protocol.Fingerprint
{
    /// <summary>
    /// 抽象的复合命令
    /// </summary>
    public abstract class BaseCombinedCommand: Door8800Command_WriteParameter, ICombinedCommand
    {
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par"></param>
        public BaseCombinedCommand(INCommandDetail cd, INCommandParameter par) : base(cd, par) { }


        /// <summary>
        /// 修改当前命令
        /// </summary>
        /// <param name="CmdType"></param>
        /// <param name="CmdIndex"></param>
        /// <param name="CmdPar"></param>
        /// <param name="DataLen"></param>
        /// <param name="CmdDataBuf"></param>
        public void ChangePacket(byte CmdType, byte CmdIndex, byte CmdPar, uint DataLen, IByteBuffer CmdDataBuf)
        {
            Packet(CmdType, CmdIndex, CmdPar, DataLen, CmdDataBuf);
        }

        /// <summary>
        /// 命令执行完毕
        /// </summary>
        /// <param name="subCmd"></param>
        public abstract void SubCommandOver(ISubCommand subCmd);

        /// <summary>
        /// 命令准备就绪
        /// </summary>
        /// <param name="subCmd"></param>
        public virtual void SubCommandReady(ISubCommand subCmd)
        {
            _ProcessStep = subCmd.ProcessStep;
            _ProcessMax = subCmd.ProcessMax;
            fireCommandProcessEvent();
            CommandReady();
        }

        /// <summary>
        /// 获取当前执行的命令
        /// </summary>
        /// <returns></returns>
        public INPacket GetPacket()
        {
            return _Packet;
        }

        /// <summary>
        /// 获取一个新的缓冲区
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public IByteBuffer GetNewSubCmdDataBuf(int v)
        {
            return GetNewCmdDataBuf(v);
        }

        /// <summary>
        /// 获取当前的命令数据缓冲区
        /// </summary>
        /// <returns></returns>
        public IByteBuffer GetSubCmdBuf()
        {
            return GetCmdBuf();
        }

        /// <summary>
        /// 获取命令相关的命令详情
        /// </summary>
        /// <returns></returns>
        public INCommandDetail GetCommandDetail()
        {
            return CommandDetail;
        }

        /// <summary>
        /// 获取连接通道
        /// </summary>
        /// <returns></returns>
        public INConnector GetConnector()
        {
            return _Connector;
        }

    }
}
