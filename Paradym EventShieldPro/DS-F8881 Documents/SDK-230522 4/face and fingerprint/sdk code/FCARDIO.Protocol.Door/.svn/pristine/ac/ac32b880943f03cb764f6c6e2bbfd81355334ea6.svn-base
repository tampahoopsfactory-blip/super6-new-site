using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Password
{
    /// <summary>
    /// 从控制器中读取的密码数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReadAllPassword_Result_Base<T> : INCommandResult where T : PasswordDetail, new()
    {

        /// <summary>
        /// 读取到的卡片列表
        /// </summary>
        public List<T> PasswordList;

        /// <summary>
        /// 创建结构
        /// </summary>
        public ReadAllPassword_Result_Base(List<T> passwordList)
        {
            PasswordList = passwordList;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            PasswordList?.Clear();
            PasswordList = null;
        }

        

    }


}
