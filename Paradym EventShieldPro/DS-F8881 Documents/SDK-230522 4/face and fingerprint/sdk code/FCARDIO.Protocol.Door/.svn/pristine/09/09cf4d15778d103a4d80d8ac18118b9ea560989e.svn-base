using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.POS.Card
{
    public class ReadCardDetail_Result : INCommandResult
    {
        /// <summary>
        /// 名单是否存在
        /// </summary>
        public bool IsReady;

        /// <summary>
        /// 菜单的详情
        /// </summary>
        public Data.CardDetail CardDetail;


        /// <summary>
        /// 创建结构
        /// </summary>
        /// <param name="isReady">名单是否存在</param>
        /// <param name="CardDetail">CardDetail 保存菜单详情的实体</param>
        public ReadCardDetail_Result(bool isReady, Data.CardDetail CardDetail)
        {
            IsReady = isReady;
            this.CardDetail = CardDetail;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            CardDetail = null;
        }
    }
}
