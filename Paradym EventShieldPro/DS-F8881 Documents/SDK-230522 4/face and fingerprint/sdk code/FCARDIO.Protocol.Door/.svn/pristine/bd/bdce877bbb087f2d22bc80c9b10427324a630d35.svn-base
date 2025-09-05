using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door89H.Data
{
    /// <summary>
    /// 刷卡记录<br/>
    /// TransactionCode 事件代码含义表：<br/>
    /// 1   合法开门                                                    
    /// 2   密码开门------------卡号为密码                              
    /// 3   卡加密码                                                    
    /// 4   手动输入卡加密码                                            
    /// 5   首卡开门                                                    
    /// 6   门常开 --- 常开工作方式中，刷卡进入常开状态                 
    /// 7   多卡开门 -- 多卡验证组合完毕后触发                          
    /// 8   重复读卡                                                    
    /// 9   有效期过期                                                  
    /// 10  开门时段过期                                               
    /// 11  节假日无效                                                 
    /// 12  未注册卡                                                   
    /// 13  巡更卡 -- 不开门                                           
    /// 14  探测锁定                                                   
    /// 15  无有效次数                                                 
    /// 16  防潜回                                                     
    /// 17  密码错误------------卡号为错误密码                         
    /// 18  密码加卡模式密码错误----卡号为卡号。                       
    /// 19  锁定时(读卡)或(读卡加密码)开门                             
    /// 20  锁定时(密码开门)                                           
    /// 21  首卡未开门                                                 
    /// 22  挂失卡                                                     
    /// 23  黑名单卡                                                   
    /// 24  门内上限已满，禁止入门。                                   
    /// 25  开启防盗布防状态(设置卡)                                   
    /// 26  撤销防盗布防状态(设置卡)                                   
    /// 27  开启防盗布防状态(密码)                                     
    /// 28  撤销防盗布防状态(密码)                                     
    /// 29  互锁时(读卡)或(读卡加密码)开门                             
    /// 30  互锁时(密码开门)                                           
    /// 31  全卡开门                                                   
    /// 32  多卡开门--等待下张卡                                       
    /// 33  多卡开门--组合错误                                         
    /// 34  非首卡时段刷卡无效                                         
    /// 35  非首卡时段密码无效                                         
    /// 36  禁止刷卡开门 -- 【开门认证方式】验证模式中禁用了刷卡开门时 
    /// 37  禁止密码开门 -- 【开门认证方式】验证模式中禁用了密码开门时 
    /// 38  门内已刷卡，等待门外刷卡。（门内外刷卡验证）               
    /// 39  门外已刷卡，等待门内刷卡。（门内外刷卡验证）               
    /// 40  请刷管理卡(在开启管理卡功能后提示)(电梯板)                 
    /// 41  请刷普通卡(在开启管理卡功能后提示)(电梯板)                 
    /// 42  首卡未读卡时禁止密码开门。                                 
    /// 43  控制器已过期_刷卡                                          
    /// 44  控制器已过期_密码                                          
    /// 45  合法卡开门—有效期即将过期                                 
    /// 46  拒绝开门--区域反潜回失去主机连接。                         
    /// 47  拒绝开门--区域互锁，失去主机连接                           
    /// 48  区域防潜回--拒绝开门    
    /// 49  区域互锁--有门未关好，拒绝开门
    /// </summary>

    public class CardTransaction : Door8800.Data.CardTransaction
    {

        /// <summary>
        /// 9字节卡号
        /// </summary>
        public Core.Util.BigInt BigCard;

        /// <summary>
        /// 4字节卡号
        /// </summary>
        public override uint CardData { get => BigCard.UInt32Value; set => BigCard.BigValue = value; }

        /// <summary>
        /// 获取读卡记录格式长度
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {
            return 17;
        }

        /// <summary>
        /// 从buf中读取卡号数据
        /// </summary>
        /// <param name="data"></param>
        protected override void ReadCardData(IByteBuffer data)
        {
            BigCard.SetBytes(data, 9);
        }


    }
}
