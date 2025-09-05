using DotNetty.Buffers;
using DoNetDrive.Protocol.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.POS.Protocol
{
    /// <summary>
    /// DES数据包解析状态
    /// </summary>
    public class DESPacketDecompileStep
    {
        /// <summary>
        /// 解析信息代码
        /// </summary>
        public static INDecompileStep<DESPacket> Code = new DecompileStep_CommandCode();

        /// <summary>
        /// 解析信息SN
        /// </summary>
        public static INDecompileStep<DESPacket> SN=new DecompileStep_SN();

        /// <summary>
        /// 解析命令长度
        /// </summary>
        public static INDecompileStep<DESPacket> DataLen=new DecompileStep_DataLen();

        /// <summary>
        /// 解析数据部分
        /// </summary>
        public static INDecompileStep<DESPacket> CmdData=new DecompileStep_CmdData();

        /// <summary>
        /// 判断检验和
        /// </summary>
        public static INDecompileStep<DESPacket> CheckSum=new DecompileStep_CheckSum();

        /// <summary>
        /// 解析信息代码
        /// </summary>
        public class DecompileStep_CommandCode : BaseDecompileStep<DESPacket>
        {
            public override int ReadBufLen => 4;

            protected override void DecompileNext(BaseDecompile<DESPacket> decompile, IByteBuffer buf, DESPacket pck)
            {
                AddSumCheck(buf, pck);
                pck.Code = buf.ReadUnsignedInt();
                decompile.DecompileStep = SN; //进入到下一步骤
            }
        }

        /// <summary>
        /// 解析SN
        /// </summary>
        public class DecompileStep_SN : BaseDecompileStep<DESPacket>
        {
            public override int ReadBufLen => 16;

            protected override void DecompileNext(BaseDecompile<DESPacket> decompile, IByteBuffer buf, DESPacket pck)
            {
                AddSumCheck(buf, pck);
                if (pck.SN == null)
                {
                    pck.SN = new byte[16];
                }
                buf.ReadBytes(pck.SN);
                decompile.DecompileStep = DataLen; //进入到下一步骤
            }
        }

        /// <summary>
        /// 解析命令长度
        /// </summary>
        public class DecompileStep_DataLen : BaseDecompileStep<DESPacket>
        {
            public override int ReadBufLen => 2;

            protected override void DecompileNext(BaseDecompile<DESPacket> decompile, IByteBuffer buf, DESPacket pck)
            {
                AddSumCheck(buf, pck);
                var iLen = buf.ReadUnsignedShort();
                if (iLen > 2048 || iLen < 8)
                {
                    //数据长度异常
                    decompile.ClearBuf();
                    return;
                }
                pck.DataLen = iLen;
                if (pck.CmdData != null)
                {
                    if (pck.CmdData.ReferenceCount > 0)
                    {
                        pck.CmdData.Release();
                    }
                    pck.CmdData = null;
                }
                if (iLen > 0)
                {
                    pck.CmdData = decompile.GetNewByteBuffer(iLen);
                }
                decompile.DecompileStep = CmdData; //进入到下一步骤
            }
        }

        /// <summary>
        /// 解析命令数据
        /// </summary>
        public class DecompileStep_CmdData : INDecompileStep<DESPacket>
        {
            public bool DecompileStep(BaseDecompile<DESPacket> decompile, byte value )
            {
                var p = decompile.GetPacket();
                var buf = p.CmdData;
                var iLen = p.DataLen;
                p.Check += value;
                buf.WriteByte(value);
                if (buf.ReadableBytes == iLen)
                {
                    decompile.DecompileStep = CheckSum; //进入到下一步骤
                }
                return false;
            }
        }

        /// <summary>
        /// 解析命令校验和
        /// </summary>
        public class DecompileStep_CheckSum : INDecompileStep<DESPacket>
        {
            public bool DecompileStep(BaseDecompile<DESPacket> decompile, byte value)
            {
                var p = decompile.GetPacket();
                int chk = p.Check & 255;
                p.Check = chk;
               
                if (chk == (int)value)
                {
                    decompile.DecompileStep = Code; //进入到下一步骤
                    return true;
                
                }
                else
                {
                    //校验和检测失败
                    decompile.ClearBuf();
                    return false;
                }
                
            }
        }
    }


}
