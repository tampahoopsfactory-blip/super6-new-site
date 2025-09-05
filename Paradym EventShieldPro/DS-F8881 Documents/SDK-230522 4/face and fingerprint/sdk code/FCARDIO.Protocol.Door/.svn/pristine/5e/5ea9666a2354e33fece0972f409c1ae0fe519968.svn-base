using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.POS.Protocol
{
    /// <summary>
    /// DES的数据包的命令详情部分
    /// </summary>
    public class DESCommandPacket : Packet.BasePacket
    {
        /// <summary>
        /// 信息代码
        /// </summary>
        public UInt32 Code;

        /// <summary>
        /// 命令类型
        /// </summary>
        public byte CmdType;
        /// <summary>
        /// 命令索引
        /// </summary>
        public byte CmdIndex;
        /// <summary>
        /// 命令参数
        /// </summary>
        public byte CmdPar;

        /// <summary>
        /// 创建一个包含信息代码，命令类型，命令索引，命令参数，数据长度，数据内容 的数据包
        /// </summary>
        /// <param name="cc">信息代码</param>
        /// <param name="ct">命令类型</param>
        /// <param name="ci">命令索引</param>
        /// <param name="cp">命令参数</param>
        /// <param name="dl">数据长度</param>
        /// <param name="cd">数据内容</param>
        public DESCommandPacket(UInt32 cc,
               byte ct, byte ci, byte cp,
               int dl, IByteBuffer cd)
        {
            Code = cc;
            SetPacket(ct, ci, cp, dl, cd);
        }

        /// <summary>
        /// 根据一个缓冲区创建子命令包
        /// </summary>
        /// <param name="buf"></param>
        public DESCommandPacket(IByteBufferAllocator Allocator, IByteBuffer buf)
        {
            Decomplie(Allocator, buf);
        }

        /// <summary>
        /// 反编译命令包
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public bool Decomplie(IByteBufferAllocator Allocator, IByteBuffer buf)
        {
            buf.MarkReaderIndex();
            Code = buf.ReadUnsignedInt();
            CmdType = buf.ReadByte();
            CmdIndex = buf.ReadByte();
            CmdPar = buf.ReadByte();

            Release();
            DataLen = buf.ReadUnsignedShort();

            if (DataLen > 0)
            {
                if(DataLen>4096)
                {
                    DataLen = 0;
                    Code = 0;
                    buf.ResetReaderIndex();
                    return false;
                }
                CmdData = Allocator.Buffer(DataLen);
                buf.ReadBytes(CmdData, DataLen);
            }
            buf.ResetReaderIndex();

            //计算校验和
            int iCount = DataLen + 9;
            for (int i = 0; i < iCount; i++)
            {
                Check += buf.ReadByte();
            }
            byte bCheck = buf.ReadByte();
            buf.ResetReaderIndex();
            Check &= 255;
            if(Check != bCheck)
            {
                Release();
                return false;
            }
            else
            {
                return true;
            }


        }

        #region 改变数据包结构

        /// <summary>
        /// 修改命令包内容
        /// </summary>
        /// <param name="ct">命令类型</param>
        /// <param name="ci">命令索引</param>
        /// <param name="cp">命令参数</param>
        public void SetPacket(byte ct, byte ci, byte cp)
        {
            SetPacket(ct, ci, cp, 0, null);
        }

        /// <summary>
        /// 修改命令包内容
        /// </summary>
        /// <param name="ct">命令类型</param>
        /// <param name="ci">命令索引</param>
        /// <param name="cp">命令参数</param>
        /// <param name="dl">数据长度</param>
        /// <param name="cd">数据内容</param>
        public void SetPacket(byte ct, byte ci, byte cp,
                       int dl, IByteBuffer cd)
        {
            CmdType = ct;
            CmdIndex = ci;
            CmdPar = cp;

            SetPacketCmdData(dl, cd);
        }
        #endregion

        /// <summary>
        /// 获取数据包的打包后的ByteBuf，用于加密
        /// </summary>
        /// <param name="Allocator">用于分配ByteBuf的分配器</param>
        /// <returns>组装后的命令包</returns>
        public override IByteBuffer GetPacketData(IByteBufferAllocator Allocator)
        {
            //信息代码    分类   命令   参数    长度       数据     检验值
            //4byte       1Byte  1Byte  1Byte   2Byte    可变长度   1Byte
            var buf = Allocator.Buffer(10 + DataLen);
            Check = 0;

            buf.WriteInt((int)Code);// 信息代码
            buf.WriteByte(CmdType);// 分类
            buf.WriteByte(CmdIndex);// 命令
            buf.WriteByte(CmdPar);// 参数

            if (DataLen > 0)
            {
                try
                {
                    if (CmdData.ReadableBytes >= DataLen)
                    {
                        buf.WriteShort(DataLen);// 长度
                        buf.WriteBytes(CmdData, CmdData.ReaderIndex, DataLen);// 数据
                    }
                    else
                    {
                        buf.WriteShort(0);//长度
                    }
                }
                catch (Exception)
                {

                    buf.SetShort(7, 0);
                }
            }
            else
                buf.WriteShort(0);//长度

            //计算校验和
            buf.MarkReaderIndex();
            int icount = buf.ReadableBytes;

            for (int i = 0; i < icount; i++)
            {
                Check = Check + buf.ReadByte();
            }

            buf.ResetReaderIndex();

            Check = Check % 256;// 和校验取低字节
            buf.WriteByte(Check);

            return buf;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Release()
        {
            if (CmdData != null)
            {
                if (CmdData.ReferenceCount > 0)
                    CmdData.Release();

            }
            DataLen = 0;
            CmdData = null;
            Check = 0;
        }
    }
}
