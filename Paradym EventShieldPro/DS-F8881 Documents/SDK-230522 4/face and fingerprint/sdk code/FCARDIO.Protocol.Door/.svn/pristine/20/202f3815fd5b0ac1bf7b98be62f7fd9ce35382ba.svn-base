using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.Card
{
    /// <summary>
    ///Door88\Door58 将卡片列表写入到控制器非排序区
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
        /// 检查卡片列表参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            foreach (var c in CardList)
            {
                if (c.Expiry.Year < 2000 || c.Expiry.Year > 2099)
                    return false;
                if (c.Privilege > 4 || c.Privilege < 0)
                    return false;
                if (c.TimeGroup == null || c.TimeGroup.Length != 4)
                    return false;
                foreach (var item in c.TimeGroup)
                {
                    if (item < 0 || item > 64)
                        return false;
                }
                if (c.CardStatus > 2) return false;
            }
            return true;
        }
    }
}
