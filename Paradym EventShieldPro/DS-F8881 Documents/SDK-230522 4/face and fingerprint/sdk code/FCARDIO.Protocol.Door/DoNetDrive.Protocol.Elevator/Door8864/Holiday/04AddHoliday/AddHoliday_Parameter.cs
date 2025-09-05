using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DoNetDrive.Protocol.Door.Door8800.Holiday;

namespace DoNetDrive.Protocol.Elevator.FC8864.Holiday
{
    /// <summary>
    /// 添加节假日参数
    /// </summary>
    public class AddHoliday_Parameter : Protocol.Door.Door8800.Holiday.AddHoliday_Parameter
    {

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="list">节假日集合</param>
        public AddHoliday_Parameter(List<HolidayDetail> list) : base(list) {
            ListHoliday = list;
            if (!checkedParameter())
            {
                throw new ArgumentException("ListHoliday Error");
            }
        }
      
    }
}
