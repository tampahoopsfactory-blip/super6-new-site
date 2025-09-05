using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Data
{

    /// <summary>
    /// 报警记录
    /// TransactionCode 事件代码含义表
    /// 1     门磁报警                     
    /// 2     匪警报警                     
    /// 3     消防报警                     
    /// 4     非法卡刷报警                 
    /// 5     胁迫报警                     
    /// 6     消防报警(命令通知)           
    /// 7     烟雾报警                     
    /// 8     防盗报警                     
    /// 9     黑名单报警                   
    /// 10    开门超时报警                
    /// 0x11  门磁报警撤销              
    /// 0x12  匪警报警撤销              
    /// 0x13  消防报警撤销              
    /// 0x14  非法卡刷报警撤销          
    /// 0x15  胁迫报警撤销              
    /// 0x17  撤销烟雾报警              
    /// 0x18  关闭防盗报警              
    /// 0x19  关闭黑名单报警            
    /// 0x1A  关闭开门超时报警          
    /// 0x21  门磁报警撤销(命令通知)    
    /// 0x22  匪警报警撤销(命令通知)    
    /// 0x23  消防报警撤销(命令通知)    
    /// 0x24  非法卡刷报警撤销(命令通知)
    /// 0x25  胁迫报警撤销(命令通知)    
    /// 0x27  撤销烟雾报警(命令通知)    
    /// 0x28  关闭防盗报警(软件关闭)    
    /// 0x29  关闭黑名单报警(软件关闭)  
    /// 0x2A  关闭开门超时报警          
    /// </summary>
    public class AlarmTransaction : AbstractDoorTransaction
    {
        /// <summary>
        /// 初始化参数
        /// </summary>
        public AlarmTransaction(): base(5)//5	报警记录
        {

        }
    }
}
