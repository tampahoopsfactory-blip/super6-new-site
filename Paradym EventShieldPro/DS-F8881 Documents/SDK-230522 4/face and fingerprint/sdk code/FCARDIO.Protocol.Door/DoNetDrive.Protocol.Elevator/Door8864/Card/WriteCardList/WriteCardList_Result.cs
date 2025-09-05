using FCARDIO.Protocol.Elevator.FC8864.Card.CardListBySequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using FCARDIO.Core.Command;

namespace FCARDIO.Protocol.Elevator.FC8864.Card
{
    /// <summary>
    /// 将卡片列表写入到控制器的返回值
    /// </summary>
    public class WriteCardList_Result : INCommandResult
    {
        /// <summary>
        /// 无法写入的卡数量
        /// </summary>
        public readonly int FailTotal;

        /// <summary>
        /// 无法写入的卡列表
        /// </summary>
        public  List<UInt64> CardList;

        /// <summary>
        /// 创建结构 
        /// </summary>
        /// <param name="failtotal">卡数量</param>
        /// <param name="cardList">卡列表</param>
        public WriteCardList_Result(int failtotal, List<UInt64> cardList)
        {
            FailTotal = failtotal;
            CardList = cardList;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            CardList?.Clear();
            CardList = null;
        }

        internal void SetBytes(IByteBuffer buf)
        {
            throw new NotImplementedException();
        }
    }
}
