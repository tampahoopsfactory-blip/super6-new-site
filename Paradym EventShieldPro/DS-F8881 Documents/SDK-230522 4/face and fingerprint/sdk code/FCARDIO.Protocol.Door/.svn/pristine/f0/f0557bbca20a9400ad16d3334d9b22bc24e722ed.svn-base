using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.Card
{
    /// <summary>
    /// 写卡列表的泛型抽象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class WriteCardList_Parameter_Base<T> : AbstractParameter
        where T : Data.CardDetailBase
    {
        /// <summary>
        /// 需要写入的卡列表
        /// </summary>
        public List<T> CardList;


        /// <summary>
        /// 创建 将卡片列表写入到控制器非排序区 指令的参数
        /// </summary>
        /// <param name="cardList">需要写入的卡列表</param>
        public WriteCardList_Parameter_Base(List<T> cardList)
        {
            CardList = cardList;
            if (!checkedParameter())
            {
                throw new ArgumentException("cardList Error");
            }
        }

        /// <summary>
        /// 检查卡片列表参数，任何情况下都不能为空，元素数不能为0,列表元素不能为空
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (CardList == null) return false;


            if (CardList.Count == 0) return false;


            foreach (var c in CardList)
            {
                if (c == null) return false;
                if (c.CardData == 0) return false;

            }

            return true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            CardList = null;
        }

        /// <summary>
        /// 不实现此功能
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf;
        }

        /// <summary>
        /// 不实现此功能
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0;
        }

        /// <summary>
        /// 不实现此功能
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            return;
        }
    }
}
