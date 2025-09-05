using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Fingerprint.AdditionalData 
{
    /// <summary>
    /// TCP Client 模式下读文件 参数
    /// </summary>
    public class ReadFileByTCP_Parameter : ReadFeatureCode_Parameter
    {
        /// <summary>
        /// 创建 TCP Client 模式下读文件 参数
        /// </summary>
        /// <param name="userCode">用户号</param>
        /// <param name="type">文件类型</param>
        /// <param name="serialNumber">序号</param>
        public ReadFileByTCP_Parameter(int userCode, int type, int serialNumber) 
            : base(userCode, type, serialNumber)
        {
        }
    }
}
