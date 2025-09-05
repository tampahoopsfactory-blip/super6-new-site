using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Core.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoNetDrive.Protocol.FrameCommand;

namespace DoNetDrive.Protocol.POS.Protocol
{
    /// <summary>
    /// 使用DES加密的命令包
    /// 适用于：FC5000 系列消费机
    /// </summary>
    public abstract class DESCommand : AbstractFrameCommand<DESPacket>
    {
        /// <summary>
        /// 创建一个DES命令类，包含命令目标和连接通道信息，还有命令参数
        /// </summary>
        /// <param name="cd">命令目标和连接通道信息</param>
        /// <param name="par">命令参数</param>
        public DESCommand(DESDriveCommandDetail cd, INCommandParameter par) : base(cd, par)
        {
            _IsWaitResponse = true;
        }

        /// <summary>
        /// 创建命令的解码器
        /// </summary>
        protected override INPacketDecompile CreateDecompile(IByteBufferAllocator Allocator)
        {
            return new DESPacketDecompile(_Connector.GetByteBufAllocator());
        }

        /// <summary>
        /// 获取当前命令所使用的缓冲区
        /// </summary>
        /// <returns></returns>
        protected virtual IByteBuffer GetCmdBuf()
        {
            if (FPacket == null) return null;
            if (FPacket.CommandPacket == null) return null;

            var buf = FPacket.CommandPacket.CmdData;
            buf.Clear();
            return buf;
        }

        protected override void CommandNext(INPacket readPacket)
        {
            DESPacket psk = readPacket as DESPacket;
            
            psk.Password = FPacket.Password;
            //检查是否需要解密
            var acl = _Connector.GetByteBufAllocator();
            if (acl == null) return;
            bool bRet = psk.DecodeSubPacket(acl);
            if (!bRet) return;//解密失败
            
            var pkt = psk.CommandPacket;
            Console.WriteLine($"收到 网络标识：{pkt.Code:X}  命令：{pkt.CmdType:X},分类：{pkt.CmdIndex:X}，参数：{pkt.CmdPar:X}，数据长度：{pkt.DataLen}");

            if (psk.Code != FPacket.Code) return;
            base.CommandNext(readPacket);
        }

        /// <summary>
        /// 命令返回值成功
        /// </summary>
        /// <param name="pck">需要检查的指令--一般值接收到的数据包</param>
        /// <returns></returns>
        protected override bool CheckResponse_OK(DESPacket pck)
        {
            var subPck = pck.CommandPacket;
            if (subPck == null) return false;
            return (subPck.CmdType == 0x21 && subPck.CmdIndex == 1);
        }

        /// <summary>
        /// 通讯密码错误
        /// </summary>
        /// <param name="pck">需要检查的指令--一般值接收到的数据包</param>
        /// <returns></returns>
        protected override bool CheckResponse_PasswordErr(DESPacket pck)
        {
            var subPck = pck.CommandPacket;

            return (subPck.CmdType == 0x21 && subPck.CmdIndex == 2);
        }

        /// <summary>
        /// 检验和错误
        /// </summary>
        /// <param name="pck">需要检查的指令--一般值接收到的数据包</param>
        /// <returns></returns>
        protected override bool CheckResponse_CheckSumErr(DESPacket pck)
        {
            var subPck = pck.CommandPacket;

            return (subPck.CmdType == 0x21 && subPck.CmdIndex == 3);
        }

        /// <summary>
        /// 指定命令的返回值中的CmdType需要增加的量
        /// </summary>
        protected int ResultCmdTypeAddValkue = 0x30;

        /// <summary>
        /// 检查返回指令是否正确
        /// </summary>
        /// <param name="pck">需要检查的指令--一般值接收到的数据包</param>
        /// <returns></returns>
        protected virtual bool CheckResponse(DESPacket pck)
        {
            return CheckResponse(pck, pck.DataLen);
        }

        /// <summary>
        /// 检查返回指令是否正确
        /// </summary>
        /// <param name="pck">需要检查的指令--一般值接收到的数据包</param>
        /// <param name="dl">返回值的长度要求</param>
        /// <returns></returns>
        protected virtual bool CheckResponse(DESPacket pck, int DataLen)
        {
            var subPck = FPacket.CommandPacket;
            return CheckResponse(pck, subPck.CmdType, subPck.CmdIndex, subPck.CmdPar, DataLen);
        }

        /// <summary>
        /// 检查返回指令是否正确
        /// </summary>
        /// <param name="pck">需要检查的指令--一般值接收到的数据包</param>
        /// <param name="ct"></param>
        /// <param name="ci"></param>
        /// <param name="cp"></param>
        /// <param name="dl">返回值的长度要求</param>
        /// <returns></returns>
        protected virtual bool CheckResponse(DESPacket pck,
            byte CmdType, byte CmdIndex, byte CmdPar,
                       int DataLen)
        {
            var subPck = pck.CommandPacket;
            return (subPck.CmdType == (ResultCmdTypeAddValkue + CmdType) &&
                subPck.CmdIndex == CmdIndex &&
                subPck.CmdPar == CmdPar &&
                subPck.DataLen == DataLen);
        }

        /// <summary>
        /// 检查返回指令是否正确
        /// </summary>
        /// <param name="pck">需要检查的指令--一般值接收到的数据包</param>
        /// <param name="CmdType">类型</param>
        /// <param name="CmdIndex">索引</param>
        /// <param name="CmdPar">参数</param>
        /// <returns></returns>
        protected virtual bool CheckResponse(DESPacket pck,
            byte CmdType, byte CmdIndex, byte CmdPar)
        {
            var subPck = pck.CommandPacket;
            return (subPck.CmdType == (ResultCmdTypeAddValkue + CmdType) &&
                subPck.CmdIndex == CmdIndex &&
                subPck.CmdPar == CmdPar );
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
    }
}
