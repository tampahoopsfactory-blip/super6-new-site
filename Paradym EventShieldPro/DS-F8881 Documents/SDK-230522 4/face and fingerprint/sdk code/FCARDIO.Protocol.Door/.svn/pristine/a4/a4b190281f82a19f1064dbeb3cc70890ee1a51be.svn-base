using DoNetDrive.Protocol.Door.Door8800.Card;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.Elevator.FC8864.Card.CardListBySort
{
    /// <summary>
    /// 将卡片列表写入到控制器排序区
    /// </summary>
    public class WriteCardListBySort_Parameter :  WriteCardList_Parameter_Base<Data.CardDetail>
    {

        /// <summary>
        /// 创建 将卡片列表写入到控制器排序区 指令的参数
        /// </summary>
        /// <param name="cardList">需要上传的卡片列表</param>
        public WriteCardListBySort_Parameter(List<FC8864.Data.CardDetail> cardList):base(cardList)
        {
            cardList?.Sort();
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
