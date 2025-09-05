using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Data
{
    /// <summary>
    /// 软件操作记录
    /// TransactionCode 事件代码含义表：
    /// 1  软件开门                           
    /// 2  软件关门                           
    /// 3  软件常开                           
    /// 4  控制器自动进入常开                 
    /// 5  控制器自动关闭门                   
    /// 6  长按出门按钮常开                   
    /// 7  长按出门按钮常闭                   
    /// 8  软件锁定                           
    /// 9  软件解除锁定                       
    /// 10 控制器定时锁定--到时间自动锁定    
    /// 11 控制器定时锁定--到时间自动解除锁定
    /// 12 报警--锁定                        
    /// 13 报警--解除锁定                    
    /// 14 互锁时远程开门     
    /// </summary>
    public class SoftwareTransaction : AbstractDoorTransaction
    {
        /// <summary>
        /// 创建一个软件操作记录
        /// </summary>
        public SoftwareTransaction():base(4)//4	软件操作记录
        {

        }
    }
}
