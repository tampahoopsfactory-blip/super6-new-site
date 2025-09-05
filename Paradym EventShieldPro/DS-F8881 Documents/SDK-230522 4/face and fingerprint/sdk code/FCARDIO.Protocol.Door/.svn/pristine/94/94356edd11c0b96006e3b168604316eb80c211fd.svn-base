using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.Fingerprint.Data;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.Fingerprint.Person
{
    /// <summary>
    /// 将人员列表写入到控制器
    /// </summary>
    public class AddPerson : WritePersonBase
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="parameter"></param>
        public AddPerson(INCommandDetail cd, WritePerson_ParameterBase parameter) : base(cd, parameter)
        {
            _ProcessMax = parameter.PersonList.Count;
            mPacketMax = 1;
            MaxBufSize = mPacketMax * 0xA1 + 1;
            
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void CreatePacket0()
        {
            var buf = GetNewCmdDataBuf(MaxBufSize);
            WritePersonToBuf(buf);
            Packet(0x07, 0x04, 0x00, (uint)buf.ReadableBytes, buf);
        }

        /// <summary>
        /// 将数据部分写入到缓冲区
        /// </summary>
        /// <param name="person"></param>
        /// <param name="buf"></param>
        protected override void WritePersonToBuf(Data.Person person, IByteBuffer buf)
        {
            person.GetBytes(buf);
        }
    }
}
