using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.Password
{
    /// <summary>
    /// 写入密码 参数
    /// </summary>
    public class Password_Parameter : Password_Parameter_Base<PasswordDetail>
    {
      
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="list">要写入的密码集合</param>
        public Password_Parameter(List<PasswordDetail> list):base (list){ }

        
        /// <summary>
        /// 检查每个密码
        /// </summary>
        /// <param name="password">密码信息</param>
        /// <returns></returns>
        protected override bool checkedParameterItem(PasswordDetail password)
        {
            if (password.Password.Length > 8 || password.Password.Length < 4)
            {
                return false;
            }
            return true;
        }

        
    }
}
