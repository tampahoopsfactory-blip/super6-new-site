using DoNetDrive.Protocol.Door.Door8800.Holiday;
using System;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.Elevator.FC8864.Holiday
{
    /// <summary>
    /// 删除节假日参数
    /// </summary>
    public class DeleteHoliday_Parameter : Protocol.Door.Door8800.Holiday.DeleteHoliday_Parameter
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="list">删除节假日集合</param>
        public DeleteHoliday_Parameter(List<HolidayDetail> list) :base(list)
        {
            ListHoliday = list;
            if (!checkedParameter())
            {
                throw new ArgumentException("ListHoliday Error");
            }
        }
       
    }
}
