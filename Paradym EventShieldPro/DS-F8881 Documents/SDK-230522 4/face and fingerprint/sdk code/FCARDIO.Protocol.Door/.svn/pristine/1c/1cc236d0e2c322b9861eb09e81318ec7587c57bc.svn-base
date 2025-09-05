using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using DoNetDrive.Common.Cryptography;

namespace DoNetDrive.Protocol.Fingerprint.AdditionalData
{
    /// <summary>
    /// TCP Client 模式下写文件
    /// </summary>
    public class WriteFileByTCP : Door8800Command_ReadParameter
    {
        /// <summary>
        /// 创建TCP Client 模式下写文件的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public WriteFileByTCP(INCommandDetail cd,
            WriteFileByTCP_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            var par = _Parameter as WriteFileByTCP_Parameter;
            var imgBuf = par.FileDatas;
            int iFileLen = imgBuf.Length;
            int iLen = 14 + iFileLen;
            var buf = GetNewCmdDataBuf(iLen);
            buf.WriteInt(par.UserCode);
            buf.WriteByte(par.FileType);
            buf.WriteByte(par.FileNum);
            uint iCRC32 = CRC32_C.CalculateDigest(imgBuf, 0, (uint)iFileLen);

            buf.WriteInt((int)iCRC32);
            buf.WriteInt(iFileLen);
            buf.WriteBytes(imgBuf, 0, iFileLen);
            Packet(0x0b, 0x07, 0x00, (uint)iLen, buf);
        }

        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (CheckResponse(oPck, 5))
            {
                var buf = oPck.CmdData;
                var rst = new WriteFileByTCP_Result();
                _Result = rst;

                rst.Result = buf.ReadByte();
                rst.RepeatUser = buf.ReadUnsignedInt();
                CommandCompleted();
            }
        }

    }
}
