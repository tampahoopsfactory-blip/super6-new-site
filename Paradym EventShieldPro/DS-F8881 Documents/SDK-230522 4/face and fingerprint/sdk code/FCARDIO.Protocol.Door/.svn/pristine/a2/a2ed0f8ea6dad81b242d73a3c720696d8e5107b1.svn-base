using DoNetDrive.Core.Packet;
using DoNetDrive.Protocol.OnlineAccess;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint
{
    public abstract class BaseSubCommand:ISubCommand
    {
        /// <summary>
        /// 和子命令想关联的主命令
        /// </summary>
        protected ICombinedCommand mCommand;

        /// <summary>
        /// 进度
        /// </summary>
        public int ProcessStep { get; set; }

        /// <summary>
        /// 进度最大值
        /// </summary>
        public int ProcessMax { get; set; }

        /// <summary>
        /// 标识命令是否已结束
        /// </summary>
        protected bool _IsOver;

        /// <summary>
        /// 判断命令是否已结束
        /// </summary>
        /// <returns></returns>
        public bool IsCommandOver() => _IsOver;

        /// <summary>
        /// 创建一个子命令
        /// </summary>
        /// <param name="mainCmd"></param>
        public BaseSubCommand(ICombinedCommand mainCmd)
        {
            mCommand = mainCmd;
            _IsOver = false;
        }

        /// <summary>
        /// 检查命令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <param name="CmdType"></param>
        /// <param name="CmdIndex"></param>
        /// <param name="CmdPar"></param>
        /// <param name="DataLen"></param>
        /// <returns></returns>
        protected virtual bool CheckResponse(OnlineAccessPacket oPck, int CmdType, int CmdIndex, int CmdPar, int DataLen)
        {
            return (oPck.CmdType == (0x30 + CmdType) && oPck.CmdIndex == CmdIndex && 
                oPck.CmdPar == CmdPar && oPck.DataLen == DataLen);
        }

        /// <summary>
        /// 检查命令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <param name="CmdType"></param>
        /// <param name="CmdIndex"></param>
        /// <param name="CmdPar"></param>
        /// <returns></returns>
        protected virtual bool CheckResponse(OnlineAccessPacket oPck, int CmdType, int CmdIndex, int CmdPar)
        {
            return (oPck.CmdType == (0x30 + CmdType) && oPck.CmdIndex == CmdIndex && oPck.CmdPar == CmdPar);
        }


        /// <summary>
        /// 检查命令返回值
        /// </summary>
        /// <param name="oPck"></param>
        /// <param name="DataLen"></param>
        /// <returns></returns>
        protected virtual bool CheckResponse(OnlineAccessPacket oPck, int DataLen)
        {
            var pk = mCommand.GetPacket() as OnlineAccessPacket;
            return (oPck.CmdType == (0x30 + pk.CmdType) && oPck.CmdIndex == pk.CmdIndex && 
                oPck.CmdPar == pk.CmdPar && oPck.DataLen == DataLen);
        }

        /// <summary>
        /// 检查命令返回值 OK
        /// </summary>
        /// <param name="oPck"></param>
        /// <returns></returns>
        protected virtual bool CheckResponseOK(OnlineAccessPacket oPck)
        {
            var pk = mCommand.GetPacket() as OnlineAccessPacket;
            return (oPck.CmdType == (0x21) && oPck.CmdIndex == 1 &&
                oPck.CmdPar == 0 && oPck.DataLen == 0);
        }

        /// <summary>
        /// 释放占用的资源
        /// </summary>
        public virtual void Release()
        {
            mCommand = null;
        }

        /// <summary>
        /// 推进命令进行的函数
        /// </summary>
        /// <param name="oPck"></param>
        public abstract void CommandNext(INPacket oPck);

        /// <summary>
        /// 命令执行完毕
        /// </summary>
        protected void CommandOver()
        {
            _IsOver = true;
            mCommand.SubCommandOver(this);
        }

        /// <summary>
        /// 命令准备就绪
        /// </summary>
        protected void CommandReady()
        {
            mCommand.SubCommandReady(this);
        }


        /// <summary>
        /// 修改当前命令
        /// </summary>
        /// <param name="CmdType"></param>
        /// <param name="CmdIndex"></param>
        /// <param name="CmdPar"></param>
        /// <param name="DataLen"></param>
        /// <param name="CmdDataBuf"></param>
        protected void Packet(byte CmdType, byte CmdIndex, byte CmdPar, uint DataLen, IByteBuffer CmdDataBuf)
        {
            mCommand.ChangePacket(CmdType, CmdIndex, CmdPar, DataLen, CmdDataBuf);
        }

        /// <summary>
        /// 获取当前执行的命令
        /// </summary>
        /// <returns></returns>
        public OnlineAccessPacket GetPacket()
        {
            return mCommand.GetPacket() as OnlineAccessPacket;
        }

        /// <summary>
        /// 获取一个新的缓冲区
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public IByteBuffer GetNewCmdDataBuf(int v)
        {
            return mCommand.GetNewSubCmdDataBuf(v);
        }

        /// <summary>
        /// 获取当前的命令数据缓冲区
        /// </summary>
        /// <returns></returns>
        public IByteBuffer GetCmdBuf()
        {
            return mCommand.GetSubCmdBuf();
        }
    }
}
