using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.Person
{
    /// <summary>
    /// 删除人员
    /// </summary>
    public class DeletePerson : WritePersonBase
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public DeletePerson(INCommandDetail cd, WritePerson_ParameterBase parameter) : base(cd, parameter)
        {
            MaxBufSize = 20 * 4 + 4;
            mPacketMax = 20;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void CreatePacket0()
        {
            var buf = GetNewCmdDataBuf(MaxBufSize);
            WritePersonToBuf(buf);
            Packet(0x07, 0x05, 0x00, (uint)buf.ReadableBytes, buf);
        }

        /// <summary>
        /// 将数据部分写入到缓冲区
        /// </summary>
        /// <param name="person"></param>
        /// <param name="buf"></param>
        protected override void WritePersonToBuf(Data.Person person, IByteBuffer buf)
        {
            buf.WriteInt((int)person.UserCode);
        }
    }
}
