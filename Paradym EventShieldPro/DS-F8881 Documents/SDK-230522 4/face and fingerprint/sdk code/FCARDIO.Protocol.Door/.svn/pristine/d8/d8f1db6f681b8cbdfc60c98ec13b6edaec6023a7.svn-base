using DotNetty.Buffers;
using DoNetDrive.Protocol.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoNetDrive.Protocol.POS.TemplateMethod;

namespace DoNetDrive.Protocol.POS.Data
{
    public class ReservationDetail : TemplateData_Base
    {
        /// <summary>
        /// 卡号
        /// 4字节
        /// </summary>
        public int CardData { get; set; }

        /// <summary>
        /// 订餐日期
        /// 3字节
        /// </summary>
        public DateTime ReservationDate { get; set; }

        /// <summary>
        /// 餐段权限
        /// 8位表示8个餐段
        /// </summary>
        public int TimeGroup;

        private static StringBuilder mStrBuf = new StringBuilder(1024);

        /// <summary>
        /// 显示餐段权限
        /// </summary>
        public string ShowTimeGroup
        {
            get
            {
                mStrBuf.Clear();
                for (int i = 1; i <= 8; i++)
                {
                    mStrBuf.Append(GetTimeGroup(i) ? "时段" + i.ToString() + "，" : "");
                }
                //if (Person.HolidayUse)
                //{

                //}
                //else
                //{
                //    mStrBuf.Append("节假日不受限制！");
                //}


                return mStrBuf.ToString();
            }
        }

        /// <summary>
        /// 获取指定餐段是否有权限
        /// </summary>
        /// <param name="iDoor">餐段权限，取值范围：1-8</param>
        /// <returns>true 有权限，false 无权限</returns>
        public bool GetTimeGroup(int iTimeGroup)
        {
            if (iTimeGroup < 0 || iTimeGroup > 8)
            {

                throw new ArgumentException("TimeGroup 1-8");
            }
            iTimeGroup -= 1;

            int iBitIndex = iTimeGroup % 8;
            int iMaskValue = (int)Math.Pow(2, iBitIndex);
            int iByteValue = TimeGroup & iMaskValue;
            if (iBitIndex > 0)
            {
                iByteValue = iByteValue >> (iBitIndex);
            }
            return iByteValue == 1;
        }

        /// <summary>
        /// 设置指定门是否有权限
        /// </summary>
        /// <param name="iTimeGroup">餐段，取值范围：1-8</param>
        /// <param name="bUse">true 有权限，false 无权限。</param>
        public void SetTimeGroup(int iTimeGroup, bool bUse)
        {
            if (iTimeGroup < 1 || iTimeGroup > 8)
            {

                throw new ArgumentException("TimeGroup 1-8");
            }
            iTimeGroup -= 1;
            int iMaskValue = (int)Math.Pow(2, iTimeGroup);
            bool bOldValue = (TimeGroup & iMaskValue) > 0;
            if (bUse == bOldValue)
            {
                return;
            }
            if (bUse)
            {
                TimeGroup = TimeGroup | iMaskValue;
            }
            else
            {
                TimeGroup = TimeGroup ^ iMaskValue;
            }
        }


        public override void SetBytes(IByteBuffer data)
        {
            CardData = data.ReadInt();
            ReservationDate = TimeUtil.BCDTimeToDate_yyMMdd(data);
            TimeGroup = data.ReadByte();
        }

        public override IByteBuffer GetBytes(IByteBuffer data)
        {
            data.WriteInt(CardData);
            TimeUtil.DateToBCD_yyMMdd(data, ReservationDate);
            data.WriteByte(TimeGroup);
            return data;
        }

        /// <summary>
        /// 获取每个添加卡类长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 8;
        }

        public override IByteBuffer GetDeleteBytes(IByteBuffer data)
        {
            throw new NotImplementedException();
        }

        public override int GetDeleteDataLen()
        {
            throw new NotImplementedException();
        }

        public override void SetFailBytes(IByteBuffer databuf)
        {

        }
    }
}