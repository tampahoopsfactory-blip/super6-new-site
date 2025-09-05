using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door8800.Door.MultiCard
{
    /// <summary>
    /// 读多卡参数
    /// </summary>
    public class ReadMultiCard : Door8800Command_ReadParameter
    {
        /// <summary>
        /// 参数
        /// </summary>
        private ReadMultiCard_Parameter _Par;

        /// <summary>
        /// 返回值
        /// </summary>
        protected MultiCard_Result mResult;

        /// <summary>
        /// 此命令对应的门端口号
        /// </summary>
        protected int mPort;

        /// <summary>
        /// 当前命令步骤
        /// </summary>
        protected int Step;

        /// <summary>
        /// 当前正在读取的组合类型
        /// </summary>
        protected int mGroupType;

        /// <summary>
        /// 当前正在读取的组号
        /// </summary>
        protected int mGroupNum;


        /// <summary>
        /// 读单个门的多卡参数
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="value"></param>
        public ReadMultiCard(INCommandDetail cd, ReadMultiCard_Parameter value) : base(cd, value) { _Par = value; }

        /// <summary>
        /// 检查端口是否正确
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool CheckCommandParameter(INCommandParameter value)
        {
            DoorPort_Parameter model = value as DoorPort_Parameter;
            if (model == null) return false;

            return model.checkedParameter();
        }

        /// <summary>
        /// 创建命令
        /// </summary>
        protected override void CreatePacket0()
        {

            mResult = new MultiCard_Result(_Par.IsOnlyGroupFix);
            _Result = mResult;

            DoorPort_Parameter model = _Parameter as DoorPort_Parameter;
            mPort = model.Door;
            mResult.DoorNum = mPort;
            
            //1、第一步，读取多卡开门检测模式参数
            Packet(0x03, 0x17, 0x01, 0x01, model.GetBytes(_Connector.GetByteBufAllocator().Buffer(1)));
            if (!_Par.IsOnlyGroupFix)
            {
                _ProcessMax = 27;
            }
            else
            {
                _ProcessMax = 2;
            }
            _ProcessStep = 1;
            Step = 1;
        }


        /// <summary>
        /// 命令回应处理
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext0(OnlineAccessPacket oPck)
        {

            switch (Step)
            {
                case 1://读取 多卡开门检测模式参数
                    if (CheckResponse(oPck, 3))
                    {
                        ReadCheckModeCallblack(oPck.CmdData);
                    }
                    break;
                case 2: //读取 多卡开门验证方式 的返回
                    if (CheckResponse(oPck, 4))
                    {
                        ReadVerifyType(oPck.CmdData);
                    }

                    break;
                case 3://读取AB组的数据
                    if (CheckResponse(oPck))
                    {
                        ReadGroupAB(oPck.CmdData);
                    }


                    break;
                case 4://固定组数据
                    if (CheckResponse(oPck))
                    {
                        ReadGroupFix(oPck.CmdData);
                    }
                    break;
                default:
                    break;
            }

        }
        /// <summary>
        /// 多卡开门检测模式参数
        /// </summary>
        /// <param name="tmpBuf"></param>
        protected void ReadCheckModeCallblack(IByteBuffer tmpBuf)
        {
            int iDoor = tmpBuf.ReadByte();
            if (iDoor != mPort)
            {
                return;
            }

            mResult.CheckMode_SetBytes(tmpBuf);

            _ProcessStep++;

            if (_Par.IsOnlyGroupFix)
            {
                //读固定组合
                CreateReadGroupFixPacket();
                return;
            }
            //读 多卡开门验证方式
            DoorPacket.CmdIndex = 0x18;//修改命令为读取 多卡开门验证方式
            CommandReady();//设定命令当前状态为准备就绪，等待发送
            Step = 2;//使命令进入下一个阶段
        }

        /// <summary>
        /// 创建读取多卡固定组的命令
        /// </summary>
        protected virtual void CreateReadGroupFixPacket()
        {
            _ProcessMax = 3;
            _ProcessStep = 3;

            DoorPacket.CmdIndex = 0x12;
            DoorPacket.CmdPar = 0x3;
            CommandReady();//设定命令当前状态为准备就绪，等待发送
            Step = 4;//使命令进入下一个阶段
        }

        /// <summary>
        /// 读多卡验证方式
        /// </summary>
        /// <param name="tmpBuf"></param>
        protected void ReadVerifyType(IByteBuffer tmpBuf)
        {
            int iDoor = tmpBuf.ReadByte();
            if (iDoor != mPort)
            {
                return;
            }

            mResult.VerifyType_SetBytes(tmpBuf);

            _ProcessStep++;
            
            switch (mResult.VerifyType)
            {
                case 1://读取多卡AB组
                    //初始化AB组容器
                    mResult.GroupA = new List<List<decimal>>();
                    for (int i = 0; i < 5; i++)
                    {
                        mResult.GroupA.Add(new List<decimal>());
                    }
                    mResult.GroupB = new List<List<decimal>>();
                    for (int i = 0; i < 20; i++)
                    {
                        mResult.GroupB.Add(new List<decimal>());
                    }

                    //开启读取多卡开门AB组内容
                    tmpBuf = _Connector.GetByteBufAllocator().Buffer(2);
                    tmpBuf.WriteByte(0).WriteByte(1);
                    mGroupType = WriteMultiCard.GroupTypeA;
                    mGroupNum = 1;
                    Packet(0x03, 0x18, 0x03, 2, tmpBuf);
                    CommandReady();//设定命令当前状态为准备就绪，等待发送
                    Step = 3;//使命令进入下一个阶段
                    break;
                case 2://读取多卡固定组
                    CreateReadGroupFixPacket();
                    break;
                default:
                    _ProcessStep = _ProcessMax;
                    //命令完成，什么都不读了
                    CommandCompleted();
                    break;

            }
        }

        /// <summary>
        /// 读取AB组的数据
        /// </summary>
        /// <param name="tmpBuf"></param>
        private void ReadGroupAB(IByteBuffer tmpBuf)
        {
            var iGroupType = tmpBuf.ReadByte();//组类别：0--A组；
            var iGroupNum = tmpBuf.ReadByte(); //组号：取值范围 1 - 5；
            var iCardCount = tmpBuf.ReadByte();
            if (iGroupType != mGroupType)
            {
                return;
            }
            if (iGroupNum != mGroupNum)
            {
                return;
            }

            List<decimal> group = FindGroupAB();
            

            if (iCardCount > 0)
            {
                ReadGroupABCard(group, tmpBuf);
            }

            ReadGroupABNext();

        }

        /// <summary>
        /// 查询当前AB组的容器
        /// </summary>
        /// <returns></returns>
        protected List<decimal> FindGroupAB()
        {
            List<decimal> group = null;
            if (mGroupType == WriteMultiCard.GroupTypeA)
            {

                group = mResult.GroupA[mGroupNum - 1];
            }
            if (mGroupType == WriteMultiCard.GroupTypeB)
            {
                group = mResult.GroupB[mGroupNum - 1];
            }
            return group;
        }


        /// <summary>
        /// 读下一个AB组
        /// </summary>
        protected void ReadGroupABNext()
        {
            //读下一个组
            var cmdBuf = DoorPacket.CmdData;
            mGroupNum++;
            if (mGroupType == WriteMultiCard.GroupTypeA && mGroupNum > 5)
            {
                mGroupType = WriteMultiCard.GroupTypeB;
                cmdBuf.SetByte(0, 1);//改变下一包要读的组类型
                mGroupNum = 1;
            }

            if (mGroupType == WriteMultiCard.GroupTypeB && mGroupNum > 20)
            {
                cmdBuf = null;
                CommandCompleted();
                return;
            }

            cmdBuf.SetByte(1, mGroupNum);//改变下一包要读的组号
            _ProcessStep++;
            CommandReady();
        }


        /// <summary>
        /// 读取AB组的卡数据
        /// </summary>
        /// <param name="group"></param>
        /// <param name="tmpBuf"></param>
        protected virtual void ReadGroupABCard(List<decimal> group, IByteBuffer tmpBuf)
        {
            while (tmpBuf.ReadableBytes >= 4)
            {
                UInt64 card = tmpBuf.ReadUnsignedInt();
                group.Add(card);
            }
        }

        /// <summary>
        /// 解析读取到的固定组内容
        /// </summary>
        /// <param name="tmpBuf"></param>
        private void ReadGroupFix(IByteBuffer tmpBuf)
        {
            var iDoorPort = tmpBuf.ReadByte();//端口号
            if (mPort != iDoorPort)
            {
                return;
            }
            List<MultiCard_GroupFix> FixGroups = new List<MultiCard_GroupFix>();
            mResult.GroupFix = FixGroups;
            int iCount;
            int iIndex = 1;
            for (int i = 1; i <= 10; i++)
            {
                MultiCard_GroupFix group = new MultiCard_GroupFix();
                iCount = tmpBuf.ReadByte();
                group.GroupType = tmpBuf.ReadByte();
                var cardList = new List<decimal>();
                group.CardList = cardList;
                if (iCount > 0)
                {

                    for (int j = 0; j < iCount; j++)
                    {
                        cardList.Add(tmpBuf.ReadUnsignedInt());
                    }
                }


                FixGroups.Add(group);

                iIndex += 34;
                if (tmpBuf.Capacity < iIndex)
                {
                    tmpBuf.SetReaderIndex(iIndex);
                }

            }

            CommandCompleted();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {

        }


        /// <summary>
        /// 释放命令占用的内存<br/>
        /// 此命令一般情况下不需要实现！
        /// </summary>
        protected override void Release1()
        {
            _Par = null;
        }
    }
}
