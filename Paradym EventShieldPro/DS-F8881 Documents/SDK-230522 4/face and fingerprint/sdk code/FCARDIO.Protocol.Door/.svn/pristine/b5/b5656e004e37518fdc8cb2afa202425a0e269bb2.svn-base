using DoNetDrive.Core.Command;
using System.Collections.Generic;

namespace DoNetDrive.Protocol.Door.Door8800.TemplateMethod
{
    /// <summary>
    /// 
    /// </summary>
    public class TemplateResult_Base : INCommandResult
    {
        /// <summary>
        /// 
        /// </summary>
        public List<TemplateData_Base> DataList;

        /// <summary>
        /// 创建结构
        /// </summary>
        public TemplateResult_Base(List<TemplateData_Base> DataList)
        {
            this.DataList = DataList;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            DataList?.Clear();
            DataList = null;
        }

    }
}
