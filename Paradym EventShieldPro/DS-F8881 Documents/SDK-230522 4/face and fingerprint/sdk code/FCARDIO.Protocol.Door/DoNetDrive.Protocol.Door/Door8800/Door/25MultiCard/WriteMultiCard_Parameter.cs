using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace DoNetDrive.Protocol.Door.Door8800.Door.MultiCard
{
    /// <summary>
    /// 固定多卡组，单组结构
    /// </summary>
    public class MultiCard_GroupFix
    {
        /// <summary>
        /// 1--入门多卡组，2--出门多卡组，3--出入门通用组。
        /// </summary>
        public byte GroupType;

        /// <summary>
        /// 固定多卡组中的卡列表。
        /// </summary>
        public List<decimal> CardList;


    }

    /// <summary>
    /// 多卡组合参数
    /// </summary>
    public class WriteMultiCard_Parameter : AbstractParameter
    {
        /// <summary>
        /// 门号
        /// 门端口在控制板中的索引号，取值：1-4
        /// </summary>
        public int DoorNum;

        /// <summary>
        /// 刷卡模式 0--表示多卡时当遇到非组合内的刷卡时继续等待下一张正确的卡(默认参数)。          1--表示当遇到非组合内刷卡时直接退出。
        /// </summary>
        public byte ReaderWaitMode;

        /// <summary>
        /// 防潜回检测 0--多卡时当开启防潜回功能时要进行防潜回检测。            1--多卡时当开启防潜回功能时不进行防潜回检测。
        /// </summary>
        public byte AntiPassback;

        /// <summary>
        /// 开门验证方式0--禁用多卡验证。1--A组和B组组合验证（A组任意数量，B组任意数量）。          2--固定组合验证（原Door8800验证方式）          3--数量验证（此方式不需要特定组，只要是合法卡刷卡一次数量即可）
        /// 当验证模式为3时，【A组刷卡数量】字段规定的就是合法卡刷卡数量
        /// </summary>
        public byte VerifyType;

        /// <summary>
        /// A组刷卡数量/自由组合刷卡数量    取值范围：0-50，当多卡验证方式为自由组合时，此值指示自由组合刷卡数量
        /// </summary>
        public byte AGroupCount;

        /// <summary>
        /// B组刷卡数量 取值范围：0-100
        /// </summary>
        public byte BGroupCount;

        /// <summary>
        /// 多卡组A组
        /// </summary>
        public List<List<decimal>> GroupA;

        /// <summary>
        /// 多卡组B组
        /// </summary>
        public List<List<decimal>> GroupB;


        /// <summary>
        /// 多卡固定组
        /// </summary>
        public List<MultiCard_GroupFix> GroupFix;

        /// <summary>
        /// 指示仅支持固定组多卡，不支持AB组多卡
        /// </summary>
        public readonly bool IsOnlyGroupFix;


        /// <summary>
        /// 用于对 MultiCard_Result 支持，不对参数初始化
        /// </summary>
        public WriteMultiCard_Parameter(bool bOnlyGroupFix) {
            IsOnlyGroupFix = bOnlyGroupFix;
        }

        /// <summary>
        /// 初始化多卡参数，不支持多卡验证参数和AB组，仅支持 等待模式和固定组多卡
        /// </summary>
        /// <param name="door">门号</param>
        /// <param name="readerWaitMode">刷卡时组合卡错误等待模式</param>
        /// <param name="antiPassback">防潜回功能检测模式</param>
        /// <param name="group_fix">多卡固定组合</param>
        public WriteMultiCard_Parameter(byte door,
            byte readerWaitMode, byte antiPassback,
            List<MultiCard_GroupFix> group_fix)
        {

            DoorNum = door;
            ReaderWaitMode = readerWaitMode;
            AntiPassback = antiPassback;

            VerifyType = 0;
            AGroupCount = 0;
            BGroupCount = 0;

            GroupA = null;
            GroupB = null;
            GroupFix = group_fix;

            IsOnlyGroupFix = true;

            checkedParameter();

        }

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
        {

            DoorNum = door;
            ReaderWaitMode = readerWaitMode;
            AntiPassback = antiPassback;

            VerifyType = verifytype;
            AGroupCount = agroupcount;
            BGroupCount = bgroupcount;

            GroupA = group_a;
            GroupB = group_b;

            GroupFix = group_fix;


            IsOnlyGroupFix = false;


            checkedParameter();

        }

        /// <summary>
        /// 检查多卡参数是否正确
        /// </summary>
        /// <returns></returns>
        public override bool checkedParameter()
        {
            if (DoorNum < 1 || DoorNum > 4)
                throw new ArgumentException("Door Error!");

            if (ReaderWaitMode < 0 || ReaderWaitMode > 1)
                throw new ArgumentException("Mode Error!");
            if (AntiPassback < 0 || AntiPassback > 1)
                throw new ArgumentException("AntiPassback Error!");

            if (!IsOnlyGroupFix)
            {
                if (VerifyType < 0 || VerifyType > 3)
                    throw new ArgumentException("VerifyType Error!");

                if (AGroupCount < 0 || AGroupCount > 50)
                    throw new ArgumentException("AGroupCount Error!");

                if (BGroupCount < 0 || BGroupCount > 100)
                    throw new ArgumentException("BGroupCount Error!");

            }


            if (!IsOnlyGroupFix)
            {
                switch (VerifyType)
                {
                    case 1:
                        if (GroupA == null || GroupB == null)
                            throw new ArgumentException("GroupA or GroupB Error!");

                        if (GroupA.Count < 5 || GroupB.Count < 20)
                            throw new ArgumentException("GroupA or GroupB Error!");

                        CheckGroup(GroupA);
                        CheckGroup(GroupB);

                        break;
                    case 2:
                        if (GroupFix == null)

                            if (GroupFix.Count < 10)
                                throw new ArgumentException("GroupA or GroupB Error!");

                        foreach (var fix in GroupFix)
                        {
                            CheckGroup(fix.CardList);
                        }

                        break;
                }
            }
            else
            {
                if (GroupFix == null)

                    if (GroupFix.Count < 10)
                        throw new ArgumentException("GroupA or GroupB Error!");

                foreach (var fix in GroupFix)
                {
                    CheckGroup(fix.CardList);
                }
            }

            return true;
        }

        /// <summary>
        /// 检查卡组
        /// </summary>
        /// <param name="checkGroup"></param>

        private void CheckGroup(List<List<decimal>> checkGroup)
        {
            foreach (var group in checkGroup)
            {
                CheckGroup(group);
            }
        }


        /// <summary>
        /// 获取卡号最大值
        /// </summary>
        public virtual decimal GetMaxCardValue() => UInt32.MaxValue;

        /// <summary>
        /// 检查卡组数据
        /// </summary>
        /// <param name="group">卡组集合</param>
        private void CheckGroup(List<decimal> group)
        {
            if (group == null)
            {
                throw new ArgumentException("Card Group Error!");
            }
            decimal iMax = GetMaxCardValue();
            foreach (var c in group)
            {
                if (c > iMax)
                {
                    throw new ArgumentException("Card Error!");
                }

                if (c == 0)
                {
                    throw new ArgumentException("Card Error!");
                }
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            return;

        }

        /// <summary>
        /// 将 多卡开门检测模式参数 编码到字节流
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        internal IByteBuffer CheckMode_GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes < 3)
            {
                throw new ArgumentException("buf Error!");
            }
            databuf.WriteByte(DoorNum);
            databuf.WriteByte(ReaderWaitMode);
            databuf.WriteByte(AntiPassback);
            return databuf;
        }

        /// <summary>
        /// 将 多卡开门验证方式 编码到字节流
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        internal IByteBuffer VerifyType_GetBytes(IByteBuffer databuf)
        {
            if (databuf.WritableBytes < 4)
            {
                throw new ArgumentException("buf Error!");
            }
            databuf.WriteByte(DoorNum);
            databuf.WriteByte(VerifyType);
            databuf.WriteByte(AGroupCount);
            databuf.WriteByte(BGroupCount);
            return databuf;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetDataLen()
        {

            return 0;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databuf"></param>
        public override void SetBytes(IByteBuffer databuf)
        {
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databuf"></param>
        /// <returns></returns>
        public override IByteBuffer GetBytes(IByteBuffer databuf)
        {
            return databuf;
        }
    }
}
