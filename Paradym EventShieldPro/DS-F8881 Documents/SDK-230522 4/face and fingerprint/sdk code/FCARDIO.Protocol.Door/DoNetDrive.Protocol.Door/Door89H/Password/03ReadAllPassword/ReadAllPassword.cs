using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.Password;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door89H.Password
{
    /// <summary>
    /// 从控制器读取所有密码
    /// </summary>
    public class ReadAllPassword : ReadAllPassword_Base<PasswordDetail>
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="cd"></param>
        public ReadAllPassword(INCommandDetail cd) : base(cd)
        {

        }

        /// <summary>
        /// 创建返回值
        /// </summary>
        /// <param name="passwordList"></param>
        /// <returns></returns>
        protected override ReadAllPassword_Result_Base<PasswordDetail> CreateResult(List<PasswordDetail> passwordList)
        {
            ReadAllPassword_Result result = new ReadAllPassword_Result(passwordList);
            return result;
        }
    }
}
