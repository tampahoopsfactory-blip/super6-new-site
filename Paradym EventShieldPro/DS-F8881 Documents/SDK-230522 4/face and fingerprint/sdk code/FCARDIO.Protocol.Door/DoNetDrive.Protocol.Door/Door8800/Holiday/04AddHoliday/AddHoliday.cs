using DoNetDrive.Core.Command;
using System;

namespace DoNetDrive.Protocol.Door.Door8800.Holiday
{
    /// <summary>
    /// 添加节假日到控制版
    /// </summary>
    public class AddHoliday : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="par"></param>
        public AddHoliday(INCommandDetail cd, AddHoliday_Parameter par) : base(cd, par){

        }

        /// <summary>
        /// 检查命令参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            AddHoliday_Parameter model = value as AddHoliday_Parameter;
            if (model == null)
            {
                return false;
            }

            return model.checkedParameter();
        }
        /// <summary>
        /// 将命令打包成一个Packet，准备发送
        /// </summary>
        protected override void CreatePacket0()
        {
            AddHoliday_Parameter model = _Parameter as AddHoliday_Parameter;
            var acl = _Connector.GetByteBufAllocator();
            var buf = acl.Buffer(model.GetDataLen());
            Packet(0x04, 0x04, 0x00, Convert.ToUInt32(model.GetDataLen()), model.GetBytes(buf));
        }

       

    }
}
