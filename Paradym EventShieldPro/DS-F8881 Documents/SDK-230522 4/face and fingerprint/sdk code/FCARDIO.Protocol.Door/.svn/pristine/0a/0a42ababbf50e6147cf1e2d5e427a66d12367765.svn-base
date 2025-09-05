using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Elevator.FC8864.Password
{
    /// <summary>
    /// 删除密码 参数
    /// </summary>
    public class DeletePassword_Parameter : Password_Parameter
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="list">要写入的密码集合</param>
        public DeletePassword_Parameter(List<PasswordDetail> list) : base(list) { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (PasswordList == null || PasswordList.Count == 0)
            {
                return false;
            }
            int iOut = 0;
            foreach (var item in PasswordList)
            {
                if (item.Password.Length > 8 || item.Password.Length < 4)
                {
                    throw new ArgumentException("Password.Length Error!");
                }
                if (!int.TryParse(item.Password, out iOut) || iOut < 0)
                {
                    throw new ArgumentException("Password Error!");
                }
                
            }

            return true;
        }
    }
}
