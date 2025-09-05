using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Elevator.FC8864.Data
{
    /// <summary>
    /// 卡片权限详情
    /// </summary>
    public class CardDetail : CardDetailBase
    {
        public override uint CardData { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public CardDetail() { }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="sur"></param>
        public CardDetail(CardDetailBase sur) : base(sur) { }

        /// <summary>
        /// 获取一个卡详情实例，序列化到buf中的字节占比
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0x65;//33字节
        }

        /// <summary>
        /// 将卡号序列化并写入buf中
        /// </summary>
        /// <param name="data"></param>
        public override void WriteCardData(IByteBuffer data)
        {
            data.WriteByte(0);
            data.WriteInt((int)CardData);
        }

        /// <summary>
        /// 从buf中读取卡号
        /// </summary>
        /// <param name="data"></param>
        public override void ReadCardData(IByteBuffer data)
        {
            data.ReadByte();

            CardData = data.ReadUnsignedInt();
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public bool CheckData()
        {
            if (CardData == 0) return false;
            if (TimeGroup == null || TimeGroup.Length != 64)
                return false;
            if (Expiry.Year < 2000 || Expiry.Year > 2099)
                return false;

            if (Privilege > 5 || Privilege < 0)
                return false;
            foreach (byte item in TimeGroup)
            {
                if (item < 0 || item > 64)
                    return false;
            }

            if (CardStatus > 2) return false;

            if (DoorNumList == null || DoorNumList.Length != 65)
                return false;
            foreach (byte item in DoorNumList)
            {
                if (item > 1)
                    return false;
            }

            return true;
        }
    }
}
