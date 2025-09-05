using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.Holiday
{
    /// <summary>
    /// 添加节假日参数
    /// </summary>
    public class AddHoliday_Parameter : AbstractParameter
    {
        /// <summary>
        /// 节假日集合
        /// </summary>
        protected List<HolidayDetail> ListHoliday;

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="list">节假日集合</param>
        public AddHoliday_Parameter(List<HolidayDetail> list) {
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
            if (ListHoliday == null)
            {
                return false;
            }
            foreach (HolidayDetail item in ListHoliday)
            {
                if (item.HolidayType < 1 || item.HolidayType > 3)
                {
                    throw new ArgumentException("HolidayType Error!");
                }
                if (item.Holiday.Year > 2099 || item.Holiday.Year < 2000)
                {
                    throw new ArgumentException("Year Error!");
                }

            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            ListHoliday = null;
        }

        /// <summary>
        /// 将结构编码为字节缓冲
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            int iLen = 5 * ListHoliday.Count +4;
            if (databuf.WritableBytes != iLen)
            {
                //throw new ArgumentException("Crad Error");
            }
            databuf.WriteInt(ListHoliday.Count);
            foreach (var holiday in ListHoliday)
            {
                databuf = holiday.GetBytes(databuf);
            }

            return databuf;
        }

        /// <summary>
        /// 获取写入参数长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            int iLen = (5 * ListHoliday.Count) + 4;
            return iLen;
        }

        /// <summary>
        /// 没有实现
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {

        }
    }
}
