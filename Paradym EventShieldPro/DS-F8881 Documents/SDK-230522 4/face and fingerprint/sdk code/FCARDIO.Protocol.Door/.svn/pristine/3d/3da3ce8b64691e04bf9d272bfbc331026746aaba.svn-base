using DotNetty.Buffers;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DoNetDrive.Protocol.Fingerprint.Person
{
    /// <summary>
    /// 从设备中读取所有已注册的人员信息
    /// </summary>
    public class ReadPersonDataBase : Door8800Command_ReadParameter
    {
        /// <summary>
        /// 读取到的人员数据缓冲
        /// </summary>
        private Queue<IByteBuffer> mBufs;

        /// <summary>
        /// 指示当前命令进行的步骤
        /// </summary>
        private int mStep;

        /// <summary>
        /// 读索引
        /// </summary>
        private int mReadIndex;


        /// <summary>
        /// 每次读取数量
        /// </summary>
        private int mReadCount;

        /// <summary>
        /// 用户总数
        /// </summary>
        private int mUserTotal;

        /// <summary>
        /// 初始化命令结构
        /// </summary>
        /// <param name="detail"></param>
        public ReadPersonDataBase(INCommandDetail detail) : base(detail, null) { }

        /// <summary>
        /// 创建一个通讯指令
        /// </summary>
        protected override void CreatePacket0()
        {
            Packet(0x07, 0x01, 0x00);
            mStep = 1;
        }

        /// <summary>
        /// 处理返回值
        /// </summary>
        /// <param name="oPck"></param>
        protected override void CommandNext1(OnlineAccessPacket oPck)
        {
            switch (mStep)
            {
                case 1://读取人员数据库详情回调
                    if (CheckResponse(oPck))
                    {
                        ReadDetailCallBlack(oPck);
                    }

                    break;
                case 2://读取人员数据库内容
                    if (CheckResponse(oPck))//检查返回的是否为卡数据
                    {
                        try
                        {
                            ReadPersonDatabaseCallBlack(oPck.CmdData);
                        }
                        catch (Exception ex)
                        {

                            Trace.WriteLine("读取人员时发生错误！" + ex.ToString());
                        }

                        
                    }

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 读取人员数据库内容完毕
        /// </summary>
        /// <param name="cmdData"></param>
        private void ReadPersonDatabaseOverCallBlack()
        {
            int iCount = 0;//获取本次总传输的人员数量
            IByteBuffer buf;
            //开始解析卡数据
            List<Data.Person> personList = new List<Data.Person>();
            while (mBufs.Count > 0)
            {
                buf = mBufs.Dequeue();
                iCount = buf.ReadByte();
                for (int i = 0; i < iCount; i++)
                {
                    Data.Person person = new Data.Person();
                    person.SetBytes(buf);
                    personList.Add(person);
                }
                buf.Release();
            }
            ReadPersonDataBase_Result result = new ReadPersonDataBase_Result(personList);
            _Result = result;

            CommandCompleted();
        }

        /// <summary>
        /// 读取人员数据库内容
        /// </summary>
        /// <param name="cmdData"></param>
        private void ReadPersonDatabaseCallBlack(IByteBuffer buf)
        {
            int iCount = buf.GetByte(0);//获取本次传输的人员数量
            _ProcessStep += iCount;
            buf.Retain();
            mBufs.Enqueue(buf);
            fireCommandProcessEvent();

            mReadIndex += mReadCount;
            if (mReadIndex == mUserTotal)
            {
                try
                {
                    ReadPersonDatabaseOverCallBlack();
                    return;
                }
                catch (Exception ex)
                {

                    Trace.WriteLine("采集完毕，保存数据时发生错误：" + ex.ToString());
                }

            }

            try
            {
                //发送下一条
                buf = GetCmdBuf();
                ReadDetailNext(buf);
            }
            catch (Exception ex)
            {

                Trace.WriteLine("发送继续采集命令时发生错误：" + ex.ToString());
            }
           
        }

        /// <summary>
        /// 读取人员数据库信息回调
        /// </summary>
        /// <param name="cmdData"></param>
        private void ReadDetailCallBlack(OnlineAccessPacket oPck)
        {
            mUserTotal = oPck.CmdData.GetInt(4);
            if (mUserTotal == 0)
            {
                fireCommandProcessEvent();

                //没有用户
                List<Data.Person> personList = new List<Data.Person>();

                ReadPersonDataBase_Result result = new ReadPersonDataBase_Result(personList);
                _Result = result;

                CommandCompleted();
                return;
            }
            _ProcessMax = mUserTotal;
            mBufs = new Queue<IByteBuffer>();
            mStep = 2;
            mReadCount = 5;
            mReadIndex = 0;
            var buf = GetNewCmdDataBuf(5);
            ReadDetailNext(buf);
        }
        /// <summary>
        /// 继续读取人事档案
        /// </summary>
        /// <param name="buf"></param>
        private void ReadDetailNext(IByteBuffer buf)
        {

            int iNewCount = mUserTotal - mReadIndex;
            if (mReadCount > iNewCount) mReadCount = iNewCount;

            buf.WriteInt(mReadIndex);
            buf.WriteByte(mReadCount);
            Packet(0x07, 0x13, 0x00, 5, buf);
            CommandReady();
        }

        /// <summary>
        /// 命令重发时需要处理的函数
        /// </summary>
        protected override void CommandReSend()
        {
            return;
        }

        /// <summary>
        /// 命令释放时需要处理的函数
        /// </summary>
        protected override void Release1()
        {
            return;
        }
    }
}
