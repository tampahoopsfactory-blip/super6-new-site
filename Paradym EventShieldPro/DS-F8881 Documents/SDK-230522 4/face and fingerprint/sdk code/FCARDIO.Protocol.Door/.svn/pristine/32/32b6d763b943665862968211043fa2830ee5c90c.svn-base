using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.POS.CardType
{
    public class ReadCardTypeDetail_Result : INCommandResult
    {
        /// <summary>
        /// 菜单是否存在
        /// </summary>
        public bool IsReady;

        /// <summary>
        /// 菜单的详情
        /// </summary>
        public Data.CardTypeDetail CardTypeDetail;


        /// <summary>
        /// 创建结构
        /// </summary>
        /// <param name="isReady">菜单是否存在</param>
        /// <param name="CardTypeDetail">CardTypeDetail 保存菜单详情的实体</param>
        public ReadCardTypeDetail_Result(bool isReady, Data.CardTypeDetail CardTypeDetail)
        {
            IsReady = isReady;
            this.CardTypeDetail = CardTypeDetail;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            CardTypeDetail = null;
        }
    }
}
