using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door89H.Door.MultiCard
{
    /// <summary>
    /// 写多卡组合参数
    /// </summary>
    public class WriteMultiCard : DoNetDrive.Protocol.Door.Door8800.Door.MultiCard.WriteMultiCard
    {
        /// <summary>
        /// 下一包需要写入的卡号起始号
        /// </summary>
        private int mCardListIndex = 0;
        /// <summary>
        /// 指示组详情是否已发送
        /// </summary>
        private bool mGroupDetailIsSend = false;

        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="value"></param>
        public WriteMultiCard(INCommandDetail cd, WriteMultiCard_Parameter value) : base(cd, value) { }



        /// <summary>
        /// 写多卡AB组合成功的回调函数
        /// </summary>
        protected override void WriteMultiCard_GroupABCallBlack()
        {
            //检查当前组是否已发送完毕
            int iCount = 0;
            List<decimal> group = FindGroupAB(ref iCount);
            if (iCount > mCardListIndex)
            {
                //当前组还未写完，继续写;
                WriteMultiCard_GroupAB();
                return;
            }

            //当前组已发送完毕，切换下一个组
            mGroupDetailIsSend = false;

            mGroupNum++;
            if (mGroupType == GroupTypeA && mGroupNum > 5)
            {
                mGroupType = GroupTypeB;
                mGroupNum = 1;
            }

            if (mGroupType == GroupTypeB && mGroupNum > 20)
            {
                CommandCompleted();
                return;
            }

            WriteMultiCard_GroupAB();
        }

        /// <summary>
        /// 查询当前操作的AB组
        /// </summary>
        /// <param name="iGroupCardCount">返回 组中的卡数量</param>
        /// <returns></returns>
        private List<decimal> FindGroupAB(ref int iGroupCardCount)
        {
            int iMax = 50;
            List<decimal> group = null;
            if (mGroupType == GroupTypeA)
            {
                iMax = 50;
                group = mMultiCardPar.GroupA[mGroupNum - 1];

            }

            if (mGroupType == GroupTypeB)
            {
                iMax = 100;
                group = mMultiCardPar.GroupB[mGroupNum - 1];
            }

            iGroupCardCount = group.Count;
            if (iGroupCardCount > iMax) iGroupCardCount = iMax;

            return group;
        }

        /// <summary>
        /// 二十七 多卡开门A组设置
        /// </summary>
        protected override void WriteMultiCard_GroupAB()
        {
            //先找到对应的组

            int iCount = 0;
            List<decimal> group = FindGroupAB(ref iCount);
            var buf = GetCmdBuf();

            //检查是否已发送详情
            if (!mGroupDetailIsSend)
            {
                //检查到未发送组详情，开始发送组详情
                buf.WriteByte(mGroupType);
                buf.WriteByte(mGroupNum);
                buf.WriteByte(iCount);


                Packet(0x18, 0x02, 3);

                _ProcessStep++;
                mGroupDetailIsSend = true;
                mCardListIndex = 0;
                CommandReady();//设定命令当前状态为准备就绪，等待发送
                return;
            }


            //开始发送卡列表
            buf.WriteByte(mCardListIndex + 1);

            Core.Util.BigInt bCard = new Core.Util.BigInt();

            int iAddCount = 0;
            for (int i = mCardListIndex; i < iCount; i++)
            {
                bCard.BigValue = group[i];
                bCard.toBytes(buf, 9);
                iAddCount++;
                if (iAddCount == 20) break;
            }
            mCardListIndex += iAddCount;

            RewritePacket(0x18, 0x52, buf.ReadableBytes);

            CommandReady();

        }


        /// <summary>
        /// 写入固定多卡组
        /// </summary>
        protected override void WriteMultiCard_GroupFix()
        {
            //先找到对应的组
            Door8800.Door.MultiCard.MultiCard_GroupFix Fix;
            List<decimal> group = null;
            int iMax = 8;
            int iCount = 0;

            Fix = mMultiCardPar.GroupFix[mGroupNum - 1];
            group = Fix.CardList;

            var buf = GetCmdBuf();
            iCount = group.Count;
            if (iCount > iMax) iCount = iMax;

            buf.WriteByte(mMultiCardPar.DoorNum);//端口号
            buf.WriteByte(mGroupNum);//组号
            buf.WriteByte(iCount);//卡数
            buf.WriteByte(Fix.GroupType);//模式


            Core.Util.BigInt bCard = new Core.Util.BigInt();
            for (int i = 0; i < iCount; i++)
            {
                bCard.BigValue = group[i];
                bCard.toBytes(buf, 9);
            }
            //不足8个，补0
            if (iCount != 8)
            {
                iCount = 8 - iCount;
                for (int i = 0; i < iCount; i++)
                {
                    buf.WriteByte(0);
                    buf.WriteLong(0);
                }
            }


            RewritePacket(0x12, 0x02, buf.ReadableBytes);

            _ProcessStep++;
            CommandReady();

        }


    }
}
