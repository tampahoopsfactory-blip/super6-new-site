using DotNetty.Buffers;
using DoNetDrive.Protocol.Util;
using System;


namespace DoNetDrive.Protocol.Elevator.FC8864.Data
{
    /// <summary>
    /// 授权卡信息 抽象类
    /// </summary>
    public abstract class CardDetailBase : DoNetDrive.Protocol.Door.Door8800.Data.CardDetailBase
    {
        /// <summary>
        /// 开门时段；1-64门的开门时段；时段取值范围：1-64；
        /// <para>TimeGroup[0] -- 1门的时段；</para>
        /// <para>TimeGroup[1] -- 2门的时段；</para>
        /// <para>TimeGroup[2] -- 3门的时段；</para>
        /// <para>TimeGroup[3] -- 4门的时段；</para>
        /// <para>.........................；</para>
        /// <para>TimeGroup[64] -- 64门的时段；</para>
        /// </summary>
        public new byte[] TimeGroup;


        /// <summary>
        /// 开门权限
        /// <para>1-65门的开门权限；0--无权，1--有权开门</para>
        /// <para>bit0 -- 1门的权限</para>
        /// <para>bit1 -- 2门的权限</para>
        /// <para>bit2 -- 3门的权限</para>
        /// <para>bit3 -- 4门的权限</para>
        /// <para>................；</para>
        /// <para>bit65 -- 65门的权限</para>
        /// </summary>
        public byte[] DoorNumList;

        /// <summary>
        /// 特权<para/>
        /// 0 -- 普通卡      <para/>
        /// 1 -- 首卡        <para/>
        /// 2 -- 常开        <para/>
        /// 3 -- 巡更        <para/>
        /// 4 -- 防盗设置卡  <para/>
        /// 5 -- 管理卡  <para/>
        /// </summary>
        public new int Privilege;

     


        /// <summary>
        /// 从一个卡详情实例，复制一个副本
        /// </summary>
        /// <param name="sur"></param>
        public CardDetailBase(CardDetailBase sur)
        {
            CopyFrom(sur);
        }

        /// <summary>
        /// 从一个卡详情实例，复制一个副本
        /// </summary>
        /// <param name="sur"></param>
        public void CopyFrom(CardDetailBase sur)
        {
            CardData = sur.CardData;
            Password = sur.Password;
            Expiry = sur.Expiry;
            TimeGroup = (byte[])sur.TimeGroup.Clone();
            DoorNumList = (byte[])sur.DoorNumList.Clone();
            OpenTimes = sur.OpenTimes;
            Privilege = sur.Privilege;
            CardStatus = sur.CardStatus;
            Holiday = (byte[])sur.Holiday.Clone();
            HolidayUse = sur.HolidayUse;
            
        }

        /// <summary>
        /// 初始化卡详情实例中的数值
        /// </summary>
        public CardDetailBase()
        {
            CardData = 0;
            OpenTimes = 65535;
            Password = string.Empty;
            Expiry = DateTime.Now.AddYears(5);
            TimeGroup = new byte[64];
            Door = 15;
            Privilege = 0;
            CardStatus = 0;
            Holiday = new byte[] { (byte)255, (byte)255, (byte)255, (byte)255 };
            RecordTime = DateTime.Now;
            EnterStatus = 0;
            HolidayUse = true;
        }


        /// <summary>
        /// 对卡号进行比较，以便进行排序
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public int CompareTo(CardDetailBase o)
        {
            if (o.CardData == CardData)
            {
                return 0;
            }
            else if (CardData < o.CardData)
            {
                return -1;
            }
            else if (CardData > o.CardData)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取一个卡详情实例，序列化到buf中的字节占比
        /// </summary>
        /// <returns></returns>
        public override abstract int GetDataLen();

        /// <summary>
        /// 将卡号序列化并写入buf中
        /// </summary>
        /// <param name="data"></param>
        public override abstract void WriteCardData(IByteBuffer data);

        /// <summary>
        /// 将卡详情实例写入到buf中
        /// </summary>
        /// <param name="data"></param>
        public override void GetBytes(IByteBuffer data)
        {
            WriteCardData(data);

            Password = StringUtil.FillHexString(Password, 8, "F", true);
            StringUtil.HextoByteBuf(Password, data);

            TimeUtil.DateToBCD_yyMMddhhmm(data, Expiry);

            data.WriteBytes(TimeGroup, 0, 64);

            data.WriteShort(OpenTimes);

            for (int i = 0; i < 8; i++)
            {
                byte[] list = new byte[8];
                for (int j = 0; j < 8; j++)
                {
                    list[j] = DoorNumList[i * 8 + j];
                }

                byte type = DoNetDrive.Common.NumUtil.BitToByte(list);
                data.WriteByte(type);
            }
            data.WriteByte(DoorNumList[64]);

            data.WriteByte(Privilege);

            data.WriteByte(CardStatus);

            data.WriteBytes(Holiday, 0, 4);

            //不写入最近读卡时间
            for (int i = 0; i < 6; i++)
            {
                data.WriteByte(0);
            }
        }

        /// <summary>
        /// 从buf中读取卡号
        /// </summary>
        /// <param name="data"></param>
        public override abstract void ReadCardData(IByteBuffer data);

        /// <summary>
        /// 从buf中读取卡详情数据
        /// </summary>
        /// <param name="data"></param>
        public override void SetBytes(IByteBuffer data)
        {
            ReadCardData(data);

            Password = StringUtil.ByteBufToHex(data, 4);

            Expiry = TimeUtil.BCDTimeToDate_yyMMddhhmm(data);

            //开门时段
            data.ReadBytes(TimeGroup, 0, 64);

            OpenTimes = data.ReadUnsignedShort();
            //权限
            DoorNumList = new byte[65];
            for (int i = 0; i < 8; i++)
            {
                byte type = data.ReadByte();
                var bytelist = DoNetDrive.Common.NumUtil.ByteToBit(type);
                for (int j = 0; j < 8; j++)
                {
                    DoorNumList[i * 8 + j] = bytelist[j];
                }
            }
            DoorNumList[64] = data.ReadByte();

            int bData = data.ReadByte();
            Privilege = bData & 7;
            HolidayUse = (bData & 8) == 8;
            CardStatus = data.ReadByte();

            data.ReadBytes(Holiday, 0, 4);


            RecordTime = TimeUtil.BCDTimeToDate_yyMMddhhmmss(data);
        }


        /// <summary>
        ///获取指定门的开门时段号
        /// </summary>
        /// <param name="iDoor">取值范围1-4</param>
        /// <returns>开门时段号</returns>
        public new int GetTimeGroup(int iDoor)
        {
            if (iDoor < 0 || iDoor > 64)
            {

                throw new ArgumentException("Door 1-4");
            }
            return TimeGroup[iDoor - 1];
        }
        /// <summary>
        /// 设置指定门的开门时段号
        /// </summary>
        /// <param name="iDoor">门号，取值范围：1-4</param>
        /// <param name="iNum">开门时段号，取值范围：1-64</param>
        public new void SetTimeGroup(int iDoor, int iNum)
        {
            if (iDoor < 0 || iDoor > 64)
            {

                throw new ArgumentException("Door 1-64");
            }

            if (iNum < 0 || iNum > 64)
            {

                throw new ArgumentException("Num 1-64");
            }
            TimeGroup[iDoor - 1] = (byte)iNum;
        }


        /// <summary>
        /// 获取指定门是否有权限
        /// </summary>
        /// <param name="iDoor">门号，取值范围：1-65</param>
        /// <returns>true 有权限，false 无权限。</returns>
        public new bool GetDoor(int iDoor)
        {
            if (iDoor < 0 || iDoor > 65)
            {

                throw new ArgumentException("Door 1-65");
            }
            iDoor -= 1;
            return DoorNumList[iDoor] == 1;
            
            //int iMaskValue = (int)Math.Pow(2, iDoor);
            //return ((Door & iMaskValue) > 0);
        }

        /// <summary>
        /// 设置指定门是否有权限
        /// </summary>
        /// <param name="iDoor">门号，取值范围：1-65</param>
        /// <param name="bUse">true 有权限，false 无权限。</param>
        public new void SetDoor(int iDoor, bool bUse)
        {
            if (iDoor < 0 || iDoor > 65)
            {

                throw new ArgumentException("Door 1-65");
            }
            iDoor -= 1;
            if (bUse)
                DoorNumList[iDoor] = 1;
            else
                DoorNumList[iDoor] = 0;
            //int iMaskValue = (int)Math.Pow(2, iDoor);
            //bool bOldValue = (Door & iMaskValue) > 0;
            //if (bUse == bOldValue)
            //{
            //    return;
            //}
            //if (bUse)
            //{
            //    Door = Door | iMaskValue;
            //}
            //else
            //{
            //    Door = Door ^ iMaskValue;
            //}
        }

        /// <summary>
        ///防盗设置卡
        /// </summary>
        public bool IsManagementSetting()
        {
            return Privilege == 5;
        }

        /// <summary>
        ///防盗设置卡
        /// </summary>
        public void SetManagementSetting()
        {
            Privilege = 5;
        }

    }
}
