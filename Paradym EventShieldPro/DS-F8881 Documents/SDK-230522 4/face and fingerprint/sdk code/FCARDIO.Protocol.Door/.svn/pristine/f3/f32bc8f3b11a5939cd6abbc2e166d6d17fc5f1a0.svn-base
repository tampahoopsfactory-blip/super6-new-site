using DoNetDrive.Core.Command;

namespace DoNetDrive.Protocol.Fingerprint.Person
{
    /// <summary>
    /// 读取单个人员在控制器中的信息，命令成功后的返回值
    /// </summary>
    public class ReadPersonDetail_Result : INCommandResult
    {
        /// <summary>
        /// 卡片是否存在
        /// </summary>
        public bool IsReady;

        /// <summary>
        /// 卡片的详情
        /// </summary>
        public Data.Person Person;

        /// <summary>
        /// 创建结构
        /// </summary>
        /// <param name="isReady">卡片是否存在</param>
        /// <param name="card">CardDetail 保存卡详情的实体</param>
        public ReadPersonDetail_Result(bool isReady, Data.Person person)
        {
            IsReady = isReady;
            Person = person;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Person = null;
        }
    }
}
