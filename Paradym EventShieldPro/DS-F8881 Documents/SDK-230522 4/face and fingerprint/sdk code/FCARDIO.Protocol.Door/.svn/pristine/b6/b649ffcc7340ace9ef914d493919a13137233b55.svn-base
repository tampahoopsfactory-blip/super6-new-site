using DoNetDrive.Protocol.Door.Door8800.Card;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.Elevator.FC8864.Card.CardListBySequence
{
    /// <summary>
    /// 将卡片列表写入到控制器非排序区
    /// </summary>
    public class WriteCardListBySequence_Parameter : WriteCardList_Parameter_Base<Data.CardDetail>
    {
        /// <summary>
        /// 创建 将卡片列表写入到控制器非排序区 指令的参数
        /// </summary>
        /// <param name="cardList">需要写入的卡列表</param>
        public WriteCardListBySequence_Parameter( List<Data.CardDetail> cardList):base(cardList)
        {
        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (CardList == null || CardList.Count == 0) return false;

            foreach (var c in CardList)
            {
                if (c == null) return false;
                if (!c.CheckData()) return false;
            }

            return true;
        }
    }
}
