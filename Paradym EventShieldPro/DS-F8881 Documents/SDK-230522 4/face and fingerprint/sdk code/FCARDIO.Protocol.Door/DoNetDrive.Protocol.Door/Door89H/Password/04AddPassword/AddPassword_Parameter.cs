using DoNetDrive.Protocol.Door.Door89H.Password;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door89H.Password
{
    /// <summary>
    /// 写入密码到控制器参数
    /// </summary>
    public class AddPassword_Parameter : Door8800.Password.Password_Parameter_Base<PasswordDetail>
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="list">要添加的密码集合</param>
        public AddPassword_Parameter(List<PasswordDetail> list) : base(list) { }

        /// <summary>
        /// 检查每个密码
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        protected override bool checkedParameterItem(PasswordDetail password)
        {
            if (password.OpenTimes < 0 || password.OpenTimes > 65535)
            {
                throw new ArgumentException("Password.OpenTimes must between 0 and 65535!");
            }
            if (password.Expiry < new DateTime(2000,1,1) || password.Expiry > new DateTime(2099,12,31))
            {
                throw new ArgumentException("Password.Expiry must between 2000-1-1 and 2099-12-31!");
            }

            return true;
        }
    }
}
