using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.Password
{
    /// <summary>
    /// 写密码列表的泛型抽象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Password_Parameter_Base<T> : AbstractParameter where T : PasswordDetail,new ()
    {
        /// <summary>
        /// 要添加的密码集合
        /// </summary>
        public List<T> PasswordList { get; set; }

        /// <summary>
        /// 创建 将密码列表写入到控制器或从控制器删除 指令的参数
        /// </summary>
        public Password_Parameter_Base(List<T> passwordList)
        {
            PasswordList = passwordList;
        }

        /// <summary>
        /// 提供给继承类使用
        /// </summary>
        public Password_Parameter_Base()
        {

        }

        /// <summary>
        /// 检查参数
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (PasswordList == null || PasswordList.Count == 0)
            {
                return false;
            }
            foreach (var item in PasswordList)
            {
                if (!checkedParameterItem(item))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 检查每个密码
        /// </summary>
        /// <param name="password">密码信息</param>
        /// <returns></returns>
        protected virtual bool checkedParameterItem(T password)
        {
            return true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            PasswordList = null;
        }

        /// <summary>
        /// 不实现此功能
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            return;
        }

        /// <summary>
        /// 不实现此功能
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 0;
        }

        /// <summary>
        /// 不实现此功能
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf;
        }
    }
}
