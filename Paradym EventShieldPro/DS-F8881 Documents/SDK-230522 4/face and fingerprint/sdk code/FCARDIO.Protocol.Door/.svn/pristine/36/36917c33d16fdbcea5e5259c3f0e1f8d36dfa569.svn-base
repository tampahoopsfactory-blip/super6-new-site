using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.POS.Subsidy
{
    /// <summary>
    /// 读取单个补贴在控制器中的信息，命令成功后的返回值
    /// </summary>
    public class ReadSubsidyDetail_Result : INCommandResult
    {
        /// <summary>
        /// 卡片是否存在
        /// </summary>
        public bool IsReady;

        /// <summary>
        /// 卡片的详情
        /// </summary>
        public Data.SubsidyDetail SubsidyDetail;


        /// <summary>
        /// 创建结构
        /// </summary>
        /// <param name="isReady">卡片是否存在</param>
        /// <param name="card">CardDetail 保存卡详情的实体</param>
        public ReadSubsidyDetail_Result(bool isReady, Data.SubsidyDetail SubsidyDetail)
        {
            IsReady = isReady;
            this.SubsidyDetail = SubsidyDetail;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            SubsidyDetail = null;
        }
    }
}
