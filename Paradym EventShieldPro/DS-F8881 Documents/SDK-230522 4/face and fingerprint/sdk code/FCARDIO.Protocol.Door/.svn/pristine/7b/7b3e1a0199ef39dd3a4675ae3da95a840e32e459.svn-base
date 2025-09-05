using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.USBDrive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.SystemParameter.RecordStorageMode
{
    /// <summary>
    /// 记录存储方式
    /// </summary>
    public class ReadRecordStorageMode : Read_Command
    {
        /// <summary>
        /// 读取记录存储方式 初始化命令
        /// </summary>
        /// <param name="cd"></param>
        public ReadRecordStorageMode(INCommandDetail cd) : base(cd, null)
        {
        }


        /// <summary>
        /// 命令返回值的判断
        /// </summary>
        /// <param name="oPck">包含返回指令的Packet</param>
        protected override void CommandNext1(USBDrivePacket oPck)
        {
            if (CheckResponse(oPck, 1))
            {
                var buf = oPck.CmdData;
                ReadRecordStorageMode_Result rst = new ReadRecordStorageMode_Result();
                _Result = rst;
                rst.SetBytes(buf);
                CommandCompleted();
            }
        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(1, 7);
        }
    }
}
