using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800.Door;
using DoNetDrive.Protocol.Door.Door8800.Door.MultiCard;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Protocol.Door.Door89H.Door.MultiCard
{
    /// <summary>
    /// 读多卡参数
    /// </summary>
    public class ReadMultiCard : DoNetDrive.Protocol.Door.Door8800.Door.MultiCard.ReadMultiCard
    {

        /// <summary>
        /// 读单个门的多卡参数
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="value"></param>
        public ReadMultiCard(INCommandDetail cd, DoorPort_Parameter value) : base(cd, new ReadMultiCard_Parameter(value.Door, false)) { }


        /// <summary>
        /// 命令回应处理
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext0(OnlineAccessPacket oPck)
        {

            switch (Step)
            {
                case 1://读取 多卡开门检测模式参数
                    base.CommandNext0(oPck);
                    return;
                case 2: //读取 多卡开门验证方式 的返回
                    base.CommandNext0(oPck);
                    break;
                case 3://读取AB组的数据
                    if (CheckResponse(oPck))
                    {
                        ReadGroupAB_Step1(oPck.CmdData);
                    }
                    if (CheckResponse(oPck, 0x03, 0x18, 0x53))
                    {
                        ReadGroupAB_Step2(oPck.CmdData);
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
        /// 读取AB组的数据
        /// </summary>
        /// <param name="tmpBuf"></param>
        private void ReadGroupAB_Step1(IByteBuffer tmpBuf)
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
            //查询此分组的容器
            List<decimal> group = FindGroupAB();

            if (iCardCount > 0)
            {   //初始化容器
                group.Clear();
                for (int i = 0; i < iCardCount; i++)
                {
                    group.Add(0);
                }
            }


            if (iCardCount == 0)
            {
                ReadGroupABNext();
            }

        }

        /// <summary>
        /// 接受AB组中组合的卡列表
        /// </summary>
        /// <param name="tmpBuf"></param>
        private void ReadGroupAB_Step2(IByteBuffer tmpBuf)
        {
            int iBeginNum = tmpBuf.ReadByte();
            if (iBeginNum > 0) iBeginNum--;
            //查询此分组的容器
            List<decimal> group = FindGroupAB();
            int iCount = tmpBuf.ReadableBytes / 9; //计算缓冲中的卡数

            Core.Util.BigInt bCard = new Core.Util.BigInt();

            for (int i = 0; i < iCount; i++)
            {
                bCard.SetBytes(tmpBuf, 9);
                group[i + iBeginNum] = bCard.BigValue;
            }

            if (group.Count == (iBeginNum + iCount))
            {
                ReadGroupABNext();
            }
            else
            {
                CommandWaitResponse();
            }
        }

        /// <summary>
        /// 创建读取多卡固定组的命令
        /// </summary>
        protected override void CreateReadGroupFixPacket()
        {
            base.CreateReadGroupFixPacket();
            _ProcessMax = 13;

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
            if (mResult.GroupFix == null)
            {
                List<MultiCard_GroupFix> FixGroups = new List<MultiCard_GroupFix>();
                mResult.GroupFix = FixGroups;

                for (int i = 1; i <= 10; i++)
                {
                    FixGroups.Add(new MultiCard_GroupFix());
                }

            }

            int groupNum = tmpBuf.ReadByte();
            if (groupNum > 10) return;

            _ProcessStep = 3 + groupNum;

            fireCommandProcessEvent();

            MultiCard_GroupFix group = mResult.GroupFix[groupNum - 1];

            int iCount = tmpBuf.ReadByte();//卡数
            group.GroupType = tmpBuf.ReadByte();//模式

            if (group.CardList == null)
                group.CardList = new List<decimal>();
            else
                group.CardList.Clear();
            Core.Util.BigInt bCard = new Core.Util.BigInt();
            for (int i = 0; i < iCount; i++)
            {
                bCard.SetBytes(tmpBuf, 9);
                group.CardList.Add(bCard.BigValue);
            }

            if (groupNum == 10)
            {
                CommandCompleted();
            }
            else
            {
                CommandWaitResponse();
            }
        }

    }
}
