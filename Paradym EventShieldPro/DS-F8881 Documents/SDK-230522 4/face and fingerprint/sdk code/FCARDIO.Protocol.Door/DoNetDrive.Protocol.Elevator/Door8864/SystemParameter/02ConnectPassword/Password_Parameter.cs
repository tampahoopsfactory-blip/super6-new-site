using DotNetty.Buffers;
using DoNetDrive.Common.Extensions;
using System;

namespace DoNetDrive.Protocol.Elevator.FC8864.SystemParameter.ConnectPassword
{
    /// <summary>
    /// 通讯密码
    /// </summary>
    public class Password_Parameter : Protocol.Door.Door8800.SystemParameter.ConnectPassword.Password_Parameter
    {
        /// <summary>
        /// 构建一个空的实例
        /// </summary>
        public Password_Parameter() { }

        /// <summary>
        /// 使用通讯密码初始化实例
        /// </summary>
        /// <param name="_PWD">通讯密码：十六进制字符串</param>
        public Password_Parameter(string _PWD)
        {
            Password = _PWD;

        }
    }
}