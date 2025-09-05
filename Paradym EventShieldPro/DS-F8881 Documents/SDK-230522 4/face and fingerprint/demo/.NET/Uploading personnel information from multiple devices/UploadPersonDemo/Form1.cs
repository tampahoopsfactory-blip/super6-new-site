using DoNetDrive.Core;
using DoNetDrive.Core.Command;
using DoNetDrive.Core.Connector.TCPClient;
using DoNetDrive.Core.Connector.UDP;
using DoNetDrive.Protocol;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.SearchControltor;
using DoNetDrive.Protocol.Fingerprint.AdditionalData;
using DoNetDrive.Protocol.Fingerprint.Data;
using DoNetDrive.Protocol.Fingerprint.Person;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UploadPersonDemo
{
    public partial class Form1 : Form
    {
        ConnectorAllocator mAllocator;
        /// <summary>
        /// 随机数产生器
        /// </summary>
        public static Random CodeRandom = new Random();
        public static int GetRandomNum() => CodeRandom.Next(1, 65530);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mAllocator = ConnectorAllocator.GetAllocator();
            IniLoadLocalIP();
        }

        /// <summary>
        /// 初始化本机IP
        /// </summary>
        private void IniLoadLocalIP()
        {
            Cmb_IP.Items.Clear();

            IPHostEntry localentry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress oItem in localentry.AddressList)
            {
                IPAddress ip = oItem;

                if (ip.IsIPv4MappedToIPv6)
                {
                    ip = ip.MapToIPv4();
                }
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    Cmb_IP.Items.Add(ip.ToString());
                }
            }
            if (Cmb_IP.Items.Count > 0)
            {
                Cmb_IP.SelectedIndex = Cmb_IP.Items.Count - 1;
            }
        }

        private int mSearchID;
        private void Btn_search_Click(object sender, EventArgs e)
        {
            mSearchID = GetRandomNum();
            mSNList = new List<string>();
            DeviceInfoList = new List<DeviceInfo>();
            BeginSearchDrive();
        }
        private int mDrivePort = 8101;
        List<string> mSNList;
        List<DeviceInfo> DeviceInfoList;
        private void BeginSearchDrive()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action)(() => BeginSearchDrive()));
                return;
            }
            try
            {
                mDrivePort = (int)Num_UdpPort.Value;
            }
            catch (Exception)
            {

                mDrivePort = 8101;
                //txtUDPLocalPort.Text = "8101";
            }
            if (mDrivePort < 0 || mDrivePort > 65535)
            {
                mDrivePort = 8101;
                //txtUDPLocalPort.Text = "8101";
            }

            var cmdDtl = SearchCommandDetail();
            cmdDtl.Timeout = 2000;
            cmdDtl.RestartCount = 0;
            cmdDtl.UserData = mSearchID;
            var searchPar = new SearchControltor_Parameter((ushort)mSearchID);
            searchPar.UDPBroadcast = true;

            var searchCmd = new SearchControltor(cmdDtl, searchPar);
            mAllocator.AddCommand(searchCmd);
            cmdDtl.CommandCompleteEvent += search_CommandCompleteEvent;
            cmdDtl.CommandTimeout += search_CommandTimeout;
            cmdDtl.CommandErrorEvent += search_CommandErrorEvent;
        }

        private void search_CommandErrorEvent(object sender, CommandEventArgs e)
        {
            this.Invoke((Action)(() => Txt_log.AppendText($" search error" + System.Environment.NewLine)));

        }

        private void search_CommandTimeout(object sender, CommandEventArgs e)
        {
            this.Invoke((Action)(() => Txt_log.AppendText($"search Complete" + System.Environment.NewLine)));
        }

        private void search_CommandCompleteEvent(object sender, CommandEventArgs e)
        {
            ushort iSearchID = (ushort)(int)e.CommandDetail.UserData;
            if (iSearchID != mSearchID) return;
            SearchControltor_Result rst = e.Result as SearchControltor_Result;

            Task.Run(() =>
            {
                Invoke(AddSearchDrive, rst);
            });
        }
        public void Invoke<T>(Action<T> act, T par)
        {
            try
            {
                this.Invoke((Delegate)act, par);
            }
            catch (Exception)
            {

                return;
            }

        }
        private void AddSearchDrive(SearchControltor_Result oDriveResult)
        {
            if (oDriveResult == null)
                return;
            if (this.InvokeRequired)
            {
                Invoke(AddSearchDrive, oDriveResult);
                return;
            }
            if (oDriveResult == null)
                return;
            var oTCP = oDriveResult.TCP;
            string sSearchSN = oDriveResult.SN;
            if (sSearchSN.Length != 16) return;

            string sSNFormat = sSearchSN.Substring(0, 8);


            #region 将网络代码写入到设备
            var cmdDtl = SearchCommandDetail();
            var connDTL = cmdDtl.Connector as UDPClientDetail;
            cmdDtl.SN = sSearchSN;
            connDTL.Port = oTCP.mUDPPort;
            var searchPar = new SearchControltor_Parameter((ushort)mSearchID);
            searchPar.UDPBroadcast = true;

            var searchCmd = new WriteControltorNetCode(cmdDtl, searchPar);
            mAllocator.AddCommand(searchCmd);
            #endregion

            if (!mSNList.Contains(sSearchSN))
            {
                mSNList.Add(sSearchSN);
                DeviceInfo deviceInfo = new DeviceInfo(sSearchSN, oTCP.mIP, "");
                DeviceInfoList.Add(deviceInfo);
                this.Invoke((Action)(() => Txt_log.AppendText($"search   SN:{sSearchSN} ,IP:{ oTCP.mIP}, PORT:{oTCP.mUDPPort}" + System.Environment.NewLine)));
            }

        }
        private OnlineAccessCommandDetail SearchCommandDetail()
        {
            string sDestIP = "255.255.255.255";
            string sSearchSN = "0000000000000000", sPassword = "FFFFFFFF";
            int iDrivePort = mDrivePort;

            string sLocalIP = Cmb_IP.SelectedItem?.ToString();
            var oUDPDtl = new UDPClientDetail(sDestIP, iDrivePort,
                sLocalIP, 9999);

            var dtl = new OnlineAccessCommandDetail(oUDPDtl, sSearchSN, sPassword);
            dtl.Timeout = 2000;
            dtl.RestartCount = 3;
            dtl.UserData = null;
            return dtl;
        }
        private bool mUDPIsBind = false;
        private void Btn_Bind_Click(object sender, EventArgs e)
        {
            string sLocalIP = Cmb_IP.SelectedItem?.ToString();
            UDPServerDetail detail = new UDPServerDetail(sLocalIP, (int)Num_UpdServerPort.Value);
            if (mUDPIsBind)
            {
                //关闭UDP服务器
                mAllocator.CloseConnector(detail);
                mUDPIsBind = false;

            }
            else
            {
                Btn_Bind.Enabled = false;
                mUDPIsBind = true;
                Num_UpdServerPort.Enabled = false;
                Cmb_IP.Enabled = false;
                Btn_search.Enabled = true;
                //打开UDP服务器
                mAllocator.OpenConnector(detail);
                //等待后续事件，事件触发 mAllocator_ConnectorConnectedEvent 表示绑定成功
                //事件触发 mAllocator_ConnectorClosedEvent 表示绑定失败
            }
        }

        private void Btn_UploadPerson_Click(object sender, EventArgs e)
        {
            if (DeviceInfoList == null || !DeviceInfoList.Any())
            {
                MessageBox.Show("No equipment");
                return;
            }
            var peoples = GetPeoples();
            foreach (var item in DeviceInfoList)
            {
                var cmdDtl = GetCommandDetail(item);
                cmdDtl.CommandCompleteEvent += (s, o) =>
                {
                    OnlineAccessCommandDetail fcDtl = o.CommandDetail as OnlineAccessCommandDetail;
                    if (o.Result is WritePerson_Result)
                    {
                        var result = o.Result as WritePerson_Result;

                        if (result.FailTotal > 0)
                        {
                            WritePersonCallBlack(fcDtl, result);
                        }
                        else
                        {
                            this.Invoke((Action)(() => Txt_log.AppendText($"SN:{fcDtl.SN}, Uploaded successfully ,Count=" + peoples.Count + System.Environment.NewLine)));
                        }
                        fcDtl.UserData = new List<Person>(peoples.ToArray());
                        WritePersonImage(o.CommandDetail);
                    }
                    else
                    {
                        WritePersonImage(o.CommandDetail);
                    }
                };
                var par = new AddPerson_Parameter(peoples);
                var cmd = new AddPerson(cmdDtl, par);
                mAllocator.AddCommand(cmd);
            }
        }


        private void WritePersonImage(INCommandDetail detail)
        {
            OnlineAccessCommandDetail fcDtl = detail as OnlineAccessCommandDetail;
            var persons = detail.UserData as List<Person>;
            if (persons.Count > 0)
            {
                var person = persons.FirstOrDefault();
                persons.RemoveAt(0);
                byte[] datas = System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "/" + person.UserCode + ".jpg");

                WriteFeatureCode_Parameter par = new WriteFeatureCode_Parameter((int)person.UserCode, 1, 1, datas);
                par.WaitRepeatMessage = true;
                WriteFeatureCode cmd = new WriteFeatureCode(detail, par);
                detail.Timeout = 5000;
                mAllocator.AddCommand(cmd);
                this.Invoke((Action)(() => Txt_log.AppendText($"SN:{fcDtl.SN}, Uploaded Person Image successfully ,UserCode=" + person.UserCode + System.Environment.NewLine)));

            }
            else
            {
                this.Invoke((Action)(() => Txt_log.AppendText($"SN:{fcDtl.SN}, Uploaded Person Image Complete " + System.Environment.NewLine)));
            }

        }
        private void WritePersonCallBlack(OnlineAccessCommandDetail fcDtl, WritePerson_Result result)
        {
            if (result != null)
            {
                if (result.FailTotal > 0)
                {
                    //   var upd=  cmde.CommandDetail as UDPClientDetail;

                    StringBuilder strBuf = new StringBuilder();
                    strBuf.AppendLine("SN:" + fcDtl.SN + ",error:");
                    foreach (var item in result.PersonList)
                    {
                        strBuf.Append(item.ToString("00000000000000000000")).Append("(0x").Append(item.ToString("X18")).Append(")");
                    }
                    //txtDebug.Text = strBuf.ToString();
                    this.Invoke((Action)(() => Txt_log.AppendText(strBuf.ToString() + System.Environment.NewLine)));

                }
            }
        }
        private INCommandDetail GetCommandDetail(DeviceInfo item)
        {
            var connectType = CommandDetailFactory.ConnectType.UDPClient;
            var protocolType = CommandDetailFactory.ControllerType.Door89H;
            var cmdDtl = CommandDetailFactory.CreateDetail(connectType, item.IP, (int)Num_UdpPort.Value,
               protocolType, item.SN, "ffffffff");
            var dtl = cmdDtl.Connector as TCPClientDetail;
            dtl.LocalAddr = Cmb_IP.SelectedItem.ToString();
            dtl.LocalPort = (int)Num_UpdServerPort.Value;
            cmdDtl.Timeout = 600;
            cmdDtl.RestartCount = 3;
            return cmdDtl;
        }

        private List<Person> GetPeoples()
        {
            List<Person> peoples = new List<Person>();
            for (int i = 0; i < 4; i++)
            {
                var code = i + 1;
                peoples.Add(new Person()
                {
                    PName = "user" + i,
                    UserCode = (uint)code,
                    PCode = code.ToString()
                });
            }
            return peoples;
        }
    }
}
