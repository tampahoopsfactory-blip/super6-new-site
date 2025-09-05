using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.Holiday
{
    /// <summary>
    /// 删除节假日参数
    /// </summary>
    public class DeleteHoliday_Parameter : AbstractParameter
    {
        /// <summary>
        /// 删除节假日集合
        /// </summary>
        public List<HolidayDetail> ListHoliday;


        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="list">删除节假日集合</param>
        public DeleteHoliday_Parameter(List<HolidayDetail> list)
        {
            ListHoliday = list;
            if (!checkedParameter())
            {
                throw new ArgumentException("ListHoliday Error");
            }
        }
        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (ListHoliday == null || ListHoliday.Count == 0)
            {
                throw new ArgumentException("ListHoliday is Null");
            }
            foreach (HolidayDetail item in ListHoliday)
            {
                if (item.Index  == 0)
                {
                    throw new ArgumentException("Holiday.Index Error");
                }
            }
            return true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            ListHoliday = null;
        }

        /// <summary>
        /// 将 节假日参数 编码到字节流
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteInt(ListHoliday.Count);
            foreach (var holiday in ListHoliday)
            {
                databuf.WriteByte(holiday.Index);
            }
            return databuf;
        }

        /// <summary>
        /// 获取写入参数的长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            int iLen = (1 * ListHoliday.Count) + 4;
            return iLen;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {

        }
    }
}
