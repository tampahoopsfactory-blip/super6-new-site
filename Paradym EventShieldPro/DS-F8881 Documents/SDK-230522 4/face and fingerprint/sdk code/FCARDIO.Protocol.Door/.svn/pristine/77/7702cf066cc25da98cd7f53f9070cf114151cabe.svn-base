using DotNetty.Buffers;
using DoNetDrive.Core.Data;
using System.Text;
using System;
using DoNetDrive.Protocol.Util;

namespace DoNetDrive.Protocol.POS.Data
{
    /// <summary>
    /// 定额扣费规则
    /// </summary>
    public class FixedFeeRuleDetail : AbstractData
    {
        /// <summary>
        /// 序号
        /// </summary>
        public byte SerialNumber { get; set; }

        public string ShowBeginTime
        {
            get
            {
                return BeginTime.ToString("HH:mm");
            }
        }

        public string ShowEndTime
        {
            get
            {
                return EndTime.ToString("HH:mm");
            }
        }

        public bool ShowIsReservation
        {
            get
            {
                return IsReservation == 1;
            }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 定额值
        /// </summary>
        public decimal FixedFee { get; set; }

        /// <summary>
        /// 消费限额
        /// </summary>
        public decimal ConsumptionLimits { get; set; }

        /// <summary>
        /// 限次
        /// </summary>
        public byte Limite { get; set; }


        /// <summary>
        /// 计次卡扣次
        /// </summary>
        public byte CountingCardsDeductionCount { get; set; }

        /// <summary>
        /// 计次卡限次
        /// </summary>
        public byte CountingCardsLimitsCount { get; set; }

        /// <summary>
        /// 是否订餐
        /// </summary>
        public byte IsReservation { get; set; }

        /// <summary>
        /// 餐段名称
        /// </summary>
        public string MealTimeName { get; set; }

        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteByte(SerialNumber);

            //databuf.WriteUnsignedShort(BeginTime);
            //databuf.WriteUnsignedShort(EndTime);
            databuf.WriteByte(ByteUtil.ByteToBCD((byte)BeginTime.Hour));
            databuf.WriteByte(ByteUtil.ByteToBCD((byte)BeginTime.Minute));
            databuf.WriteByte(ByteUtil.ByteToBCD((byte)EndTime.Hour));
            databuf.WriteByte(ByteUtil.ByteToBCD((byte)EndTime.Minute));
            databuf.WriteMedium(Convert.ToInt32(FixedFee * 100));
            databuf.WriteMedium(Convert.ToInt32(ConsumptionLimits * 100));
            databuf.WriteByte(Limite);
            databuf.WriteByte(CountingCardsDeductionCount);
            databuf.WriteByte(CountingCardsLimitsCount);
            databuf.WriteByte(IsReservation);
            Util.StringUtil.WriteString(databuf, MealTimeName, 10, Encoding.GetEncoding("GB2312"));
            return databuf;
        }

        public override int GetDataLen()
        {
            return 25;
        }

        public override void SetBytes(IByteBuffer databuf)
        {
            SerialNumber = databuf.ReadByte();
            //BeginTime = databuf.ReadUnsignedShort();
            //EndTime = databuf.ReadUnsignedShort();
            byte b1 = ByteUtil.BCDToByte(databuf.ReadByte());
            byte b2 = ByteUtil.BCDToByte(databuf.ReadByte());
            byte b3 = ByteUtil.BCDToByte(databuf.ReadByte());
            byte b4 = ByteUtil.BCDToByte(databuf.ReadByte());
            BeginTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,b1,b2,0);
            EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,b3,b4,0);
            FixedFee = (decimal)databuf.ReadUnsignedMedium() / (decimal)100;
            ConsumptionLimits = (decimal)databuf.ReadUnsignedMedium() / (decimal)100;
            Limite = databuf.ReadByte();
            CountingCardsDeductionCount = databuf.ReadByte();
            CountingCardsLimitsCount = databuf.ReadByte();
            IsReservation = databuf.ReadByte();
            MealTimeName = Util.StringUtil.GetString(databuf, 10, Encoding.GetEncoding("GB2312"));
        }
    }
}
