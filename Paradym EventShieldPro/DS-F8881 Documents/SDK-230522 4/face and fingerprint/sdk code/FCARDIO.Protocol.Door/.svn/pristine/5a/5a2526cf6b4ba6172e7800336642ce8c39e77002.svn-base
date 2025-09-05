using DoNetDrive.Protocol.Door.Door8800.Door.MultiCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door89H.Door.MultiCard
{
    /// <summary>
    /// 适用于Door89H协议的多卡组合参数
    /// </summary>
    public class WriteMultiCard_Parameter : DoNetDrive.Protocol.Door.Door8800.Door.MultiCard.WriteMultiCard_Parameter
    {
        /// <summary>
        /// 初始化多卡参数，支持多卡验证参数和AB组，等待模式和固定组多卡
        /// </summary>
        /// <param name="door">门号</param>
        /// <param name="readerWaitMode">刷卡时组合卡错误等待模式</param>
        /// <param name="antiPassback">防潜回功能检测模式</param>
        /// <param name="verifytype">开门验证方式0--禁用多卡验证。1--A组和B组组合验证（A组任意数量，B组任意数量）。          2--固定组合验证（原Door8800验证方式）          3--数量验证（此方式不需要特定组，只要是合法卡刷卡一次数量即可）
        /// 当验证模式为3时，【A组刷卡数量】字段规定的就是合法卡刷卡数量</param>
        /// <param name="agroupcount">A组刷卡数量/自由组合刷卡数量    取值范围：0-20，当多卡验证方式为自由组合时，此值指示自由组合刷卡数量</param>
        /// <param name="bgroupcount">B组刷卡数量 取值范围：0-100</param>
        /// <param name="group_a">多卡A组组合</param>
        /// <param name="group_b">多卡B组组合</param>
        /// <param name="group_fix">多卡固定组合</param>
        public WriteMultiCard_Parameter(byte door,
            byte readerWaitMode, byte antiPassback,
            byte verifytype, byte agroupcount, byte bgroupcount,
            List<List<decimal>> group_a, List<List<decimal>> group_b,
            List<MultiCard_GroupFix> group_fix)
            : base(door,
                 readerWaitMode, antiPassback,
                 verifytype, agroupcount, bgroupcount,
                 group_a, group_b, group_fix)
        {

        }

        /// <summary>
        /// 获取卡号最大值
        /// </summary>
        public override decimal GetMaxCardValue()
        {
            Core.Util.BigInt big = new Core.Util.BigInt();
            big.ByteValue_4 = 255;
            big.UInt64Value = UInt64.MaxValue;
            return big.BigValue;
        }

    }
}
