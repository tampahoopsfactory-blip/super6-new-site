using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using DoNetDrive.Common.Cryptography;

namespace DoNetDrive.Protocol.Fingerprint.AdditionalData 
{
    /// <summary>
    /// TCP Client 模式下读文件
    /// </summary>
    public class ReadFileByTCP  : Door8800Command_ReadParameter
    {
        /// <summary>
        /// 创建TCP Client 模式下读文件的命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public ReadFileByTCP(INCommandDetail cd,
            ReadFileByTCP_Parameter par) : base(cd, par) { }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            var par = _Parameter as ReadFileByTCP_Parameter;

            var buf = GetNewCmdDataBuf(6);
           
            buf.WriteByte(par.Type);
            buf.WriteByte(par.SerialNumber);
            buf.WriteInt(par.UserCode);
            Packet(0x0b, 0x15, 0x04, 6, buf);
        }

        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            if (oPck.CmdType == 0x3B && oPck.CmdIndex == 0x15 && oPck.CmdPar == 4)
            {
                var buf = oPck.CmdData;
                var rst = new ReadFileByTCP_Result();
                _Result = rst;
                var par = _Parameter as ReadFileByTCP_Parameter;
                rst.UserCode = par.UserCode;
                rst.FileType = par.Type;
                rst.FileSerialNumber = par.SerialNumber;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }
    }
}
