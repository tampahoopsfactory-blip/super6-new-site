using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoNetDrive.Core.Command;
using DoNetDrive.Protocol.Fingerprint.Elevator;

namespace DoNetDrive.Protocol.Fingerprint.Test
{
    public partial class frmElevator : frmNodeForm
    {
        #region 单例模式
        private static object lockobj = new object();
        private static frmElevator onlyObj;
        public static frmElevator GetForm(INMain main)
        {
            if (onlyObj == null)
            {
                lock (lockobj)
                {
                    if (onlyObj == null)
                    {
                        onlyObj = new frmElevator(main);
                        frmMain.AddNodeForms(onlyObj);
                    }
                }
            }
            return onlyObj;
        }

        private frmElevator(INMain main) : base(main)
        {
            InitializeComponent();

        }


        #endregion

        #region 定义函数名称
        /// <summary>
        /// 在这里定义命令名称
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> IniCommandClassNameList()
        {
            var dicCommandClasss = new Dictionary<string, string>();

            dicCommandClasss.Add(GetClassName(typeof(ReadPersonElevatorAccess)), "读取人员电梯扩展权限");
            dicCommandClasss.Add(GetClassName(typeof(WritePersonElevatorAccess)), "写入人员电梯扩展权限");

            dicCommandClasss.Add(GetClassName(typeof(ReadWorkType)), "读取电梯工作模式");
            dicCommandClasss.Add(GetClassName(typeof(WriteWorkType)), "设置电梯工作模式");

            dicCommandClasss.Add(GetClassName(typeof(ReadRelayType)), "读取电梯继电器输出类型");
            dicCommandClasss.Add(GetClassName(typeof(WriteRelayType)), "写入电梯继电器输出类型");


            dicCommandClasss.Add(GetClassName(typeof(ReadReleaseTime)), "读取电梯开锁输出时长");
            dicCommandClasss.Add(GetClassName(typeof(WriteReleaseTime)), "写入电梯开锁输出时长");

            dicCommandClasss.Add(GetClassName(typeof(ReadTimingOpen)), "读取电梯定时常开");
            dicCommandClasss.Add(GetClassName(typeof(WriteTimingOpen)), "写入电梯定时常开");


            dicCommandClasss.Add(GetClassName(typeof(OpenRelay)), "远程开门");
            dicCommandClasss.Add(GetClassName(typeof(CloseRelay)), "远程关门");
            dicCommandClasss.Add(GetClassName(typeof(HoldRelay)), "远程常开");
            dicCommandClasss.Add(GetClassName(typeof(LockRelay)), "远程锁定");
            dicCommandClasss.Add(GetClassName(typeof(UnlockRelay)), "远程解锁");
            return dicCommandClasss;
        }
        private static string GetClassName(System.Type tType)
        {
            string sKey = tType.FullName;
            //sKey = sKey.Replace('.', '_');
            return sKey;
        }
        #endregion


        #region 人员电梯扩展权限
        private void btnReadPersonElevatorAccess_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            ReadPersonElevatorAccess_Parameter par = new ReadPersonElevatorAccess_Parameter(6);

            ReadPersonElevatorAccess cmd = new ReadPersonElevatorAccess(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadPersonElevatorAccess_Result result = cmde.Command.getResult() as ReadPersonElevatorAccess_Result;

                sb.Clear();
                sb.Append("用户号：").Append(result.UserCode)
                .Append(",状态：").Append(result.Status)
                .Append(",继电器列表：").AppendLine()
                .Append(string.Join(",", result.RelayAccesss.ToArray()));

                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        private void btnWritePersonElevatorAccess_Click(object sender, EventArgs e)
        {
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WritePersonElevatorAccess_Parameter par = new WritePersonElevatorAccess_Parameter()
            {
                UserCode = 5
            };
            for (int i = 0; i < 64; i++)
            {
                par.RelayAccesss.Add(0);

            }
            par.RelayAccesss[0] = 0;
            par.RelayAccesss[1] = 1;
            par.RelayAccesss[2] = 0;
            par.RelayAccesss[3] = 1;
            par.RelayAccesss[4] = 1;
            par.RelayAccesss[5] = 0;
            par.RelayAccesss[6] = 0;
            par.RelayAccesss[7] = 1;
            WritePersonElevatorAccess cmd = new WritePersonElevatorAccess(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                StringBuilder sb = new StringBuilder();
                WritePersonElevatorAccess_Result result = cmde.Command.getResult() as WritePersonElevatorAccess_Result;

                sb.Clear();
                sb.Append("用户号：").Append(result.UserCode)
                .Append(",状态：").Append(result.Status);
                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }
        #endregion


        #region 电梯设备工作模式（开启/禁用电梯模式）
        /// <summary>
        /// 读取电梯设备工作模式（开启/禁用电梯模式）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadWorkType_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            ReadWorkType cmd = new ReadWorkType(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadWorkType_Result result = cmde.Command.getResult() as ReadWorkType_Result;

                sb.Append("工作模式：").Append(result.WorkType);

                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }
        /// <summary>
        /// 设置电梯设备工作模式（开启/禁用电梯模式）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWriteWorkType_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteWorkType_Parameter par = new WriteWorkType_Parameter() { WorkType = 1 };
            WriteWorkType cmd = new WriteWorkType(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
            };
        }
        #endregion


        #region 继电器类型 COM _NO  COM _NC
        private void btnReadRelayType_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            ReadRelayType cmd = new ReadRelayType(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadRelayType_Result result = cmde.Command.getResult() as ReadRelayType_Result;

                sb.AppendLine("继电器输出类型：").Append(string.Join(",", result.RelayTypes.ToArray()));

                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        private void btnWriteRelayType_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteRelayType_Parameter par = new WriteRelayType_Parameter();
            for (int i = 0; i < 64; i++)
            {
                par.RelayTypes.Add(1);
            }
            WriteRelayType cmd = new WriteRelayType(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
            };
        }
        #endregion


        #region 电梯继电器板的继电器开锁输出时长
        private void btnReadReleaseTime_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            ReadReleaseTime cmd = new ReadReleaseTime(cmdDtl);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadReleaseTime_Result result = cmde.Command.getResult() as ReadReleaseTime_Result;

                sb.AppendLine("输出时长：").Append(string.Join(",", result.ReleaseTimes.ToArray()));

                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        private void btnWriteReleaseTime_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteReleaseTime_Parameter par = new WriteReleaseTime_Parameter();
            for (int i = 0; i < 64; i++)
            {
                par.ReleaseTimes.Add(4);
            }
            WriteReleaseTime cmd = new WriteReleaseTime(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
            };
        }
        #endregion


        #region 定时常开
        private void btnReadTimingOpen_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;

            ReadTimingOpen_Parameter par = new ReadTimingOpen_Parameter(1);

            ReadTimingOpen cmd = new ReadTimingOpen(cmdDtl, par);
            mMainForm.AddCommand(cmd);

            //处理返回值
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadTimingOpen_Result result = cmde.Command.getResult() as ReadTimingOpen_Result;

                sb.Append("端口号：").Append(result.Port);
                sb.Append("，启用状态：").Append(result.Use);
                sb.Append("，常开模式：").Append(result.WorkType).AppendLine();
                //sb.Append("时段：").Append(result.WeekTimeGroup.ToString());

                mMainForm.AddCmdLog(cmde, sb.ToString());
            };
        }

        private void btnWriteTimingOpen_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            WriteTimingOpen_Parameter par = new WriteTimingOpen_Parameter()
            {
                Port = 1,
                Use = 1,
                WorkType = 3

            };
            //设置周时段
            for (int i = 0; i < 7; i++)
            {
                var week = par.WeekTimeGroup.GetItem(i);
                var Segment = week.GetItem(0);
                Segment.SetBeginTime(10, 39);
                Segment.SetEndTime(10, 40);
            }
            WriteTimingOpen cmd = new WriteTimingOpen(cmdDtl, par);
            mMainForm.AddCommand(cmd);

        }
        #endregion

        #region 远程操作

        private RemoteRelay_Patameter GetRelayList()
        {
            RemoteRelay_Patameter par = new RemoteRelay_Patameter();
            var Relays = par.Relays;
            for (int i = 0; i < 64; i++)
            {
                Relays.Add(0);
            }

            Relays[1] = 1;
            Relays[3] = 1;
            Relays[5] = 1;
            Relays[50] = 1;
            return par;
        }


        private void RemoteCommand<T>(Func<INCommandDetail, RemoteRelay_Patameter,T> createObject) where T :  OpenRelay
        {

            var cmdDtl = mMainForm.GetCommandDetail();
            if (cmdDtl == null) return;
            var par = GetRelayList();

            T cmd = createObject(cmdDtl, par);
            mMainForm.AddCommand(cmd);
        }

        private void btnOpenRelay_Click(object sender, EventArgs e)
        {
            RemoteCommand((x, y) => new OpenRelay(x, y));
        }


        private void btnCloseRelay_Click(object sender, EventArgs e)
        {

            RemoteCommand((x, y) => new CloseRelay(x, y));
        }

        private void btnHoldRelay_Click(object sender, EventArgs e)
        {
            RemoteCommand((x, y) => new HoldRelay(x, y));
        }

        private void btnLockRelay_Click(object sender, EventArgs e)
        {
            RemoteCommand((x, y) => new LockRelay(x, y));
        }

        private void btnUnlockRelay_Click(object sender, EventArgs e)
        {
            RemoteCommand((x, y) => new UnlockRelay(x, y));
        }
        #endregion
    }
}