using DoNetDrive.Core.Command;
using System;

namespace DoNetDrive.Protocol.Elevator.FC8864.Holiday
{
    /// <summary>
    /// 添加节假日到控制版
    /// </summary>
    public class AddHoliday : Protocol.Door.Door8800.Holiday.AddHoliday
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public AddHoliday(INCommandDetail cd, AddHoliday_Parameter par) : base(cd, par){

        }

        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            AddHoliday_Parameter model = _Parameter as AddHoliday_Parameter;
            var acl = _Connector.GetByteBufAllocator();
            var buf = acl.Buffer(model.GetDataLen());
            Packet(0x44, 0x4, 0x00, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(buf));
        }
    }
}
