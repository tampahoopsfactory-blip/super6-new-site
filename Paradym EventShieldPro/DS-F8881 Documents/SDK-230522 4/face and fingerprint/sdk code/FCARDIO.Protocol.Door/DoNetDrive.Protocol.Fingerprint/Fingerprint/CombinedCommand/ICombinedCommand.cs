using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoNetDrive.Core.Command;
using DoNetDrive.Core.Connector;
using DoNetDrive.Core.Packet;
using DoNetDrive.Protocol.Fingerprint.AdditionalData;
using DoNetDrive.Protocol.OnlineAccess;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Fingerprint
{
    /// <summary>
    /// 组合命令接口
    /// </summary>
    public interface ICombinedCommand
    {
        /// <summary>
        /// 修改当前命令
        /// </summary>
        /// <param name="CmdType"></param>
        /// <param name="CmdIndex"></param>
        /// <param name="CmdPar"></param>
        /// <param name="DataLen"></param>
        /// <param name="CmdDataBuf"></param>
        void ChangePacket(byte CmdType , byte CmdIndex , byte CmdPar ,uint  DataLen , IByteBuffer CmdDataBuf);

        /// <summary>
        /// 命令执行完毕
        /// </summary>
        /// <param name="subCmd"></param>
        void SubCommandOver(ISubCommand subCmd);

        /// <summary>
        /// 命令准备就绪
        /// </summary>
        /// <param name="subCmd"></param>
        void SubCommandReady(ISubCommand subCmd);

        /// <summary>
        /// 获取当前执行的命令
        /// </summary>
        /// <returns></returns>
        INPacket GetPacket();


        /// <summary>
        /// 获取当前的命令数据缓冲区
        /// </summary>
        /// <returns></returns>
        IByteBuffer GetSubCmdBuf();

        /// <summary>
        /// 获取一个新的缓冲区
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        IByteBuffer GetNewSubCmdDataBuf(int v);


        /// <summary>
        /// 获取命令相关的命令详情
        /// </summary>
        /// <returns></returns>
        INCommandDetail GetCommandDetail();

        /// <summary>
        /// 获取连接通道
        /// </summary>
        /// <returns></returns>
        INConnector GetConnector();
    }
}
