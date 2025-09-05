using DoNetDrive.Protocol.POS.TemplateMethod;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.POS.Data
{
    /// <summary>
    /// 卡号名单
    /// </summary>
    public class CardDetail : TemplateData_Base
    {
        /// <summary>
        /// 卡号
        /// </summary>
        public int CardData { get; set; }

        /// <summary>
        /// 卡类型
        /// 0--正常卡
        /// 1--黑名单
        /// 2--挂失卡
        /// </summary>
        public byte CardType { get; set; }

        public string ShowCardType
        {
            get
            {
                string[] mCardType = { "正常卡", "黑名单", "挂失卡" };
                return mCardType[CardType];
            }
        }

        /// <summary>
        /// 占位
        /// </summary>
        public int Standby { get; set; }

        public override void SetBytes(IByteBuffer databuf)
        {
            CardData = databuf.ReadInt();
            CardType = databuf.ReadByte();
            Standby = databuf.ReadMedium();
        }


        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            databuf.WriteInt(CardData);
            databuf.WriteByte(CardType);
            databuf.WriteMedium(Standby);
            return databuf;
        }

        /// <summary>
        /// 获取每个添加卡类长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 8;
        }

        public override IByteBuffer GetDeleteBytes(IByteBuffer databuf)
        {
            databuf.WriteInt(CardData);
            return databuf;
        }

        public override int GetDeleteDataLen()
        {
            return 4;
        }

        public override void SetFailBytes(IByteBuffer databuf)
        {
            CardData = databuf.ReadInt();
        }
    }
}
