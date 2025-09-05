
using DoNetDrive.Protocol.POS.TemplateMethod;
using DoNetDrive.Protocol.Util;
using DoNetDrive.Common.Extensions;
using DotNetty.Buffers;
using System;

namespace DoNetDrive.Protocol.POS.Data
{
    /// <summary>
    /// 补贴
    /// </summary>
    public class SubsidyDetail : TemplateData_Base
    {
        /// <summary>
        /// 卡号
        /// </summary>
        public int CardData { get; set; }

        /// <summary>
        /// 补贴状态
        /// </summary>
        public byte SubsidyState { get; set; }

        public string ShowSubsidyState
        {
            get
            {
                return SubsidyState == 0 ? "未发" : "已发";
            }
        }

        /// <summary>
        /// 补贴金额
        /// </summary>
        public decimal SubsidyMoney { get; set; }

        /// <summary>
        /// 补贴实际发放金
        /// </summary>
        public decimal ActualSubsidyMoney { get; set; }

        /// <summary>
        /// 补贴截止时间
        /// </summary>
        public DateTime SubsidyDate { get; set; }

        /// <summary>
        /// 补贴类型
        /// </summary>
        public byte SubsidyType { get; set; }

        public string ShowSubsidyType
        {
            get
            {
                string[] mSubsidyType = { "", "充值到补贴账户，永不过期", "充值到子账户，会过期", "充值到补贴账户，会过期", "充值到现金账户" };
                return mSubsidyType[SubsidyType];
            }
        }

        /// <summary>
        /// 自定义编号
        /// </summary>
        public byte CustomNumber { get; set; }

        public override void SetBytes(IByteBuffer data)
        {
            CardData = data.ReadInt();

            //byte[] bData3 = new byte[3];
            //data.ReadBytes(bData3, 0, 3);
            SubsidyState = data.ReadByte();
            SubsidyMoney = (decimal)data.ReadShort() / (decimal)100.0;

            byte[] bData = new byte[4];
            data.ReadBytes(bData, 0, 4);
            SubsidyDate = BufToDateTime(bData);

            //SubsidyDate = TimeUtil.BCDTimeToDate_yyMMdd(data);
            ActualSubsidyMoney = (decimal)data.ReadUnsignedMedium() / (decimal)100.0;

            SubsidyType = data.ReadByte();
            CustomNumber = data.ReadByte();
        }


        public override IByteBuffer GetBytes(IByteBuffer data)
        {
            data.WriteInt(CardData);
            data.WriteByte(SubsidyState);
            data.WriteShort((int)(SubsidyMoney * 100));
            byte[] bData = DateTimeToBuf(SubsidyDate);
            data.WriteBytes(bData);

            data.WriteMedium((int)(ActualSubsidyMoney * 100));
            data.WriteByte(SubsidyType);
            data.WriteByte(CustomNumber);
            return data;
        }

        /// <summary>
        /// 获取每个添加卡类长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 16;
        }

        public override IByteBuffer GetDeleteBytes(IByteBuffer data)
        {
            throw new NotImplementedException();
        }

        public override int GetDeleteDataLen()
        {
            throw new NotImplementedException();
        }

        public override void SetFailBytes(IByteBuffer buf)
        {
            throw new NotImplementedException();
        }

        DateTime BufToDateTime(byte[] bData)
        {
            try
            {
                Array.Reverse(bData);
                var time = bData.ToInt32(); //0x80ca3050;
                var s = time >> 26 & 0x3f;
                var m = time >> 20 & 0x3f;
                var h = time >> 15 & 0x1f;
                var d = time >> 10 & 0x1f;
                var mm = time >> 6 & 0xf;
                var y = time & 0x3f;

                return new DateTime(2000 + (int)y, (int)mm, (int)d, (int)h, (int)m, (int)s);
            }
            catch (Exception ex)
            {
                return DateTime.Now;
            }

        }

        byte[] DateTimeToBuf(DateTime bDate)
        {
            uint bitTime = 0;

            bitTime = (uint)(bDate.Year - 2000);
            bitTime += (uint)(bDate.Month << 6);
            bitTime += (uint)(bDate.Day << 10);
            bitTime += (uint)(bDate.Hour << 15);
            bitTime += (uint)(bDate.Minute << 20);
            uint uSecond = (uint)bDate.Second;
            bitTime += (uSecond << 26);
            byte[] bData = bitTime.To4Bytes();
            
            Array.Reverse(bData);
            return bData;// new DateTime(2000 + (int)y, (int)mm, (int)d, (int)h, (int)m, (int)s);
        }


    }
}
