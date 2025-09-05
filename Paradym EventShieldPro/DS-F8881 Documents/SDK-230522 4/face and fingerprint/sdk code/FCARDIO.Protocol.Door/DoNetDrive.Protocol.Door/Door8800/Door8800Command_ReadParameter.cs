using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800
{
    /// <summary>
    /// 针对命令中的读参数命令进行抽象封装
    /// </summary>
    public abstract class Door8800Command_ReadParameter: Door8800CommandEx
    {       

        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        public Door8800Command_ReadParameter(INCommandDetail cd):base(cd,null)
        {

        }

        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="cd">包含命令所需的远程主机详情 （IP、端口、SN、密码、重发次数等）</param>
        /// <param name="par">包含命令所需要的其他参数</param>
        public Door8800Command_ReadParameter(INCommandDetail cd, INCommandParameter par) : base(cd, par)
        {

        }

        /// <summary>
        /// 进行命令参数的检查<br/>
        /// 只有在有参数时才需要实现
        /// </summary>
        /// <param name="value">命令包含的参数</param>
        /// <returns>true 表示检查通过，false 表示检查不通过</returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            return true;
        }

        
    }
}
