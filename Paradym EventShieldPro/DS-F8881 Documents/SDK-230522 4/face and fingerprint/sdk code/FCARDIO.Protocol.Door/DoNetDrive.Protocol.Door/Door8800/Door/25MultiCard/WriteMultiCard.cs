using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Core.Packet;
using DoNetDrive.Protocol.OnlineAccess;

namespace DoNetDrive.Protocol.Door.Door8800.Door.MultiCard
{
    /// <summary>
    /// 写多卡组合参数
    /// </summary>
    public class WriteMultiCard : Door8800Command_WriteParameter
    {
        /// <summary>
        /// 多卡参数
        /// </summary>
        protected WriteMultiCard_Parameter mMultiCardPar;

        /// <summary>
        /// 当前命令步骤
        /// </summary>
        protected int Step;


        /// <summary>
        /// 多卡A组类型
        /// </summary>
        public const int GroupTypeA = 0;
        /// <summary>
        /// 多卡B组类型
        /// </summary>
        public const int GroupTypeB = 1;

        /// <summary>
        /// 当前正在写入的组合类型
        /// </summary>
        protected int mGroupType;

        /// <summary>
        /// 当前正在写入的组号
        /// </summary>
        protected int mGroupNum;


        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="value"></param>
        public WriteMultiCard(INCommandDetail cd, WriteMultiCard_Parameter value) : base(cd, value)
        {

            mMultiCardPar = value;
        }

        /// <summary>
        /// 检查多卡组合参数合法性
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            WriteMultiCard_Parameter model = value as WriteMultiCard_Parameter;
            if (model == null) return false;
            return model.checkedParameter();
        }

        /// <summary>
        /// 创建第一个数据包
        /// </summary>
        protected override void CreatePacket0()
        {
            //发送 设置多卡开门检测模式参数
            var buf = GetNewCmdDataBuf(404);
            Packet(0x03, 0x17, 0x00, 3, mMultiCardPar.CheckMode_GetBytes(buf));
            Step = 1;
            IniPacketProcess();
        }

        /// <summary>
        /// 初始化指令的步骤数
        /// </summary>
        protected virtual void IniPacketProcess()
        {
            if (mMultiCardPar.IsOnlyGroupFix)
            {
                _ProcessMax = 11;
            }
            else
            {
                switch (mMultiCardPar.VerifyType)
                {
                    case 1:
                        _ProcessMax = 27;
                        break;
                    case 2:
                        _ProcessMax = 12;
                        break;
                    default:
                        _ProcessMax = 2;
                        break;
                }

            }
        }



        /// <summary>
        /// 接收到响应，开始处理下一步命令
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext0(OnlineAccessPacket oPck)
        {

            switch (Step)
            {
                case 1://二十六、设置 多卡开门验证方式
                    if (CheckResponse_OK(oPck))
                    {
                        WriteCheckModeCallBlack();
                    }
                    break;
                case 2:
                    if (CheckResponse_OK(oPck))
                    {
                        WriteVerifyTypeCallBlack();
                    }
                    break;
                case 3://继续写AB组
                    if (CheckResponse_OK(oPck))
                    {
                        WriteMultiCard_GroupABCallBlack();
                    }
                    break;
                case 4://继续写固定组
                    if (CheckResponse_OK(oPck))
                    {
                        WriteMultiCard_GroupFixCallBlack();
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 写多卡开门检测模式的回调
        /// </summary>
        protected virtual void WriteCheckModeCallBlack()
        {
            _ProcessStep = 2;

            if (mMultiCardPar.IsOnlyGroupFix)
            {
                //写固定多卡组合
                mGroupNum = 1;
                WriteMultiCard_GroupFix();
                Step = 4;
            }
            else
            {
                var buf = GetCmdBuf();
                mMultiCardPar.VerifyType_GetBytes(buf);
                //写多卡开门验证方式
                Packet(0x18, 0x00, 4);
                Step = 2;
            }
            CommandReady();//设定命令当前状态为准备就绪，等待发送

        }

        /// <summary>
        /// 写多卡开门验证方式的回调
        /// </summary>
        protected virtual void WriteVerifyTypeCallBlack()
        {
            _ProcessStep = 3;
            //检查需要发送的内容
            switch (mMultiCardPar.VerifyType)
            {
                case 1://多卡AB组
                       //开始写A组
                    mGroupType = GroupTypeA;
                    mGroupNum = 1;
                    WriteMultiCard_GroupAB();

                    Step = 3;
                    break;
                case 2://固定组合
                       //开始写第一个固定组合
                    mGroupNum = 1;
                    WriteMultiCard_GroupFix();
                    Step = 4;
                    break;
                default://其他方式
                    CommandCompleted();
                    break;
            }
        }

        /// <summary>
        /// 写多卡AB组合成功的回调函数
        /// </summary>
        protected virtual void WriteMultiCard_GroupABCallBlack()
        {
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
        /// 二十七 多卡开门A组设置
        /// </summary>
        protected virtual void WriteMultiCard_GroupAB()
        {
            //先找到对应的组
            List<decimal> group = null;
            int iMax = 50;
            int iCount = 0;

            if (mGroupType == GroupTypeA) group = mMultiCardPar.GroupA[mGroupNum - 1];
            if (mGroupType == GroupTypeB)
            {
                iMax = 100;
                group = mMultiCardPar.GroupB[mGroupNum - 1];
            }

            var buf = GetCmdBuf();
            iCount = group.Count;
            if (iCount > iMax) iCount = iMax;

            buf.WriteByte(mGroupType);
            buf.WriteByte(mGroupNum);
            buf.WriteByte(iCount);

            for (int i = 0; i < iCount; i++)
            {
                buf.WriteInt((int)group[i]);
            }


            RewritePacket(0x18, 0x02,buf.ReadableBytes);


            _ProcessStep++;
            CommandReady();

        }


        /// <summary>
        /// 固定组多卡写成功后的回调
        /// </summary>
        protected virtual void WriteMultiCard_GroupFixCallBlack()
        {
            mGroupNum++;
            if (mGroupNum > 10)
            {
                CommandCompleted();
                return;
            }
            WriteMultiCard_GroupFix();
        }

        /// <summary>
        /// 写入固定多卡组
        /// </summary>
        protected virtual void WriteMultiCard_GroupFix()
        {
            //先找到对应的组
            MultiCard_GroupFix Fix;
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

            for (int i = 0; i < iCount; i++)
            {
                buf.WriteInt((int)group[i]);
            }

            //不足8个，补0
            if (iCount != 8)
            {
                iCount = 8 - iCount;
                for (int i = 0; i < iCount; i++)
                {
                    buf.WriteInt(0);
                }
            }




            RewritePacket(0x12, 0x02, buf.ReadableBytes);

            _ProcessStep++;
            CommandReady();

        }





        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {

        }



    }
}
