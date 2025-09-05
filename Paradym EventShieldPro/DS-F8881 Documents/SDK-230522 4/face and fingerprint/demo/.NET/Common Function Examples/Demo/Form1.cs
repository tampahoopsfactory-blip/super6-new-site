using DoNetDrive.Core;
using DoNetDrive.Core.Command;
using DoNetDrive.Core.Connector;
using DoNetDrive.Core.Connector.TCPClient;
using DoNetDrive.Core.Connector.UDP;
using DoNetDrive.Core.Data;
using DoNetDrive.Protocol;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.SearchControltor;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.Fingerprint.Data;
using DoNetDrive.Protocol.Fingerprint.Person;
using DoNetDrive.Protocol.OnlineAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    /// <summary>
    /// 
    /// 
    /// 
    /// 
    /// 
    /// </summary>
    public partial class Form1 : Form
    {
        ConnectorAllocator mAllocator;
        public static Random CodeRandom = new Random();
        public static int GetRandomNum() => CodeRandom.Next(1, 65530);
        public Form1()
        {
            Init();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string hostName = Dns.GetHostName();   //获取本机名
            IPHostEntry localhost = Dns.GetHostEntry(hostName);    //方法已过期，可以获取IPv4的地址
            for (int i = 0; i < localhost.AddressList.Length; i++)
            {
                if (localhost.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    cmbLocalNetAddr.Items.Add(localhost.AddressList[i]);
            }                                                   //IPHostEntry localhost = Dns.GetHostEntry(hostName);   //获取IPv6地址
            cmbLocalNetAddr.SelectedIndex = 0;

            Btn_UDPBind_Click(null, null);
        }

        private void Init()
        {
            mAllocator = ConnectorAllocator.GetAllocator();
            mAllocator.CommandCompleteEvent += mAllocator_CommandCompleteEvent;
            mAllocator.CommandErrorEvent += mAllocator_CommandErrorEvent;
            mAllocator.CommandProcessEvent += mAllocator_CommandProcessEvent;
            mAllocator.CommandTimeout += mAllocator_CommandTimeout;
            mAllocator.AuthenticationErrorEvent += MAllocator_AuthenticationErrorEvent;

            mAllocator.TransactionMessage += MAllocator_TransactionMessage;

            mAllocator.ConnectorConnectedEvent += mAllocator_ConnectorConnectedEvent;
            mAllocator.ConnectorClosedEvent += mAllocator_ConnectorClosedEvent;
            mAllocator.ConnectorErrorEvent += mAllocator_ConnectorErrorEvent;

            mAllocator.ClientOnline += MAllocator_ClientOnline;
            mAllocator.ClientOffline += MAllocator_ClientOffline;
        }

        private void mAllocator_CommandCompleteEvent(object sender, CommandEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                txtLog.AppendText("命令成功" + Environment.NewLine);
            }));
        }

        private void mAllocator_CommandErrorEvent(object sender, CommandEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                txtLog.AppendText("命令错误" + Environment.NewLine);
            }));
        }

        private void mAllocator_CommandProcessEvent(object sender, CommandEventArgs e)
        {

        }

        private void mAllocator_CommandTimeout(object sender, CommandEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                txtLog.AppendText("命令超时" + Environment.NewLine);
            }));
        }

        private void MAllocator_AuthenticationErrorEvent(object sender, CommandEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                txtLog.AppendText("设备信息错误" + Environment.NewLine);
            }));
        }

        private void MAllocator_TransactionMessage(INConnectorDetail connector, INData EventData)
        {
        }

        private void mAllocator_ConnectorConnectedEvent(object sender, INConnectorDetail connector)
        {
            if (!connector.IsFaulted && connector is UDPServerDetail)
            {
                Btn_UploadPerson_Click(null, null);
            }
        }

        private void mAllocator_ConnectorClosedEvent(object sender, INConnectorDetail connector)
        {
        }

        private void mAllocator_ConnectorErrorEvent(object sender, INConnectorDetail connector)
        {
        }

        private void MAllocator_ClientOnline(object sender, ServerEventArgs e)
        {
        }

        private void MAllocator_ClientOffline(object sender, ServerEventArgs e)
        {

        }
        /// <summary>
        /// 自动搜索的随机数
        /// </summary>
        private int mSearchID;
        private int mDrivePort;
        private void btnSearchEqut_Click(object sender, EventArgs e)
        {
            mSearchID = GetRandomNum();
            mDrivePort = int.Parse(Num_UdpPort.Value.ToString());
            var cmdDtl = SearchCommandDetail();
            cmdDtl.Timeout = 2000;
            cmdDtl.RestartCount = 0;
            cmdDtl.UserData = mSearchID;
            var searchPar = new SearchControltor_Parameter((ushort)mSearchID);
            searchPar.UDPBroadcast = true;

            var searchCmd = new SearchControltor(cmdDtl, searchPar);
            mAllocator.AddCommand(searchCmd);
            cmdDtl.CommandCompleteEvent += search_CommandCompleteEvent;
        }

        private void search_CommandCompleteEvent(object sender, CommandEventArgs e)
        {
            SearchControltor_Result rst = e.Result as SearchControltor_Result;
            this.Invoke(new Action(() =>
            {
                txtLog.AppendText("SN:" + rst.SN + Environment.NewLine);
                txtLog.AppendText("IP:" + rst.TCP.mIP + Environment.NewLine);
                txtLog.AppendText("MAC:" + rst.TCP.mMAC + Environment.NewLine);
                txtLog.AppendText("自动获得IP:" + rst.TCP.mAutoIP + Environment.NewLine);
                txtLog.AppendText("子网掩码:" + rst.TCP.mIPMask + Environment.NewLine);
                txtLog.AppendText("网关IP:" + rst.TCP.mIPGateway + Environment.NewLine);
                txtLog.AppendText("DNS:" + rst.TCP.mDNS + Environment.NewLine);
                txtLog.AppendText("备用DNS:" + rst.TCP.mDNSBackup + Environment.NewLine);
                txtLog.AppendText("UPD端口:" + rst.TCP.mUDPPort + Environment.NewLine);
                txtLog.AppendText("服务器IP:" + rst.TCP.mServerIP + Environment.NewLine);
                txtLog.AppendText("服务器端口:" + rst.TCP.mServerPort + Environment.NewLine);
                txtLog.AppendText("服务器域名:" + rst.TCP.mServerAddr + Environment.NewLine);
                txtLog.AppendText(Environment.NewLine);
                txtIP.Text = rst.TCP.mIP;
                txtSN.Text = rst.SN;
            }));
        }

        private OnlineAccessCommandDetail SearchCommandDetail()
        {
            string sDestIP = "255.255.255.255";
            string sSearchSN = "0000000000000000", sPassword = "FFFFFFFF";
            int iDrivePort = mDrivePort;

            string sLocalIP = cmbLocalNetAddr.SelectedItem?.ToString();
            var oUDPDtl = new UDPClientDetail(sDestIP, iDrivePort,
                sLocalIP, int.Parse(Num_UDPLocalPort.Value.ToString()));

            var dtl = new OnlineAccessCommandDetail(oUDPDtl, sSearchSN, sPassword);
            dtl.Timeout = 2000;
            dtl.RestartCount = 3;
            dtl.UserData = null;
            return dtl;
        }
        private bool mUDPIsBind = false;
        private string mServerIP;
        private int mServerPort;
        private void Btn_UDPBind_Click(object sender, EventArgs e)
        {
            int port = int.Parse(Num_UDPLocalPort.Value.ToString());
            string sLocalIP = cmbLocalNetAddr.Text;
            UDPServerDetail detail = new UDPServerDetail(sLocalIP, port);
            if (mUDPIsBind)
            {
                //关闭UDP服务器
                mAllocator.CloseConnector(detail);
                mUDPIsBind = false;
                cmbLocalNetAddr.Enabled = true;
                Num_UDPLocalPort.Enabled = true;
                Btn_UDPBind.Text = "绑定端口";
            }
            else
            {

                mUDPIsBind = true;
                cmbLocalNetAddr.Enabled = false;
                Num_UDPLocalPort.Enabled = false;
                //打开UDP服务器
                mAllocator.OpenConnector(detail);
                mServerIP = sLocalIP;
                mServerPort = port;
                //等待后续事件，事件触发 mAllocator_ConnectorConnectedEvent 表示绑定成功
                //事件触发 mAllocator_ConnectorClosedEvent 表示绑定失败
                Btn_UDPBind.Text = "解除绑定";

            }
        }

        private void readCardSaveInfo_Click(object sender, EventArgs e)
        {
            var cmdDtl = GetCommandDetail();
            cmdDtl.Timeout = 3000;
            var cmd = new ReadPersonDatabaseDetail(cmdDtl);
            mAllocator.AddCommand(cmd);
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadPersonDatabaseDetail_Result result = cmde.Command.getResult() as ReadPersonDatabaseDetail_Result;
                this.Invoke(new Action(() =>
                {
                    txtLog.AppendText("用户存储信息" + Environment.NewLine);
                    txtLog.AppendText("人员档案最大容量:" + result.SortDataBaseSize + Environment.NewLine);
                    txtLog.AppendText("已存储人员数量:" + result.SortPersonSize + Environment.NewLine);
                    txtLog.AppendText("指纹特征码最大容量:" + result.SortFingerprintDataBaseSize + Environment.NewLine);
                    txtLog.AppendText("已存储指纹数量:" + result.SortFingerprintSize + Environment.NewLine);
                    txtLog.AppendText("人脸特征码最大容量:" + result.SortFaceDataBaseSize + Environment.NewLine);
                    txtLog.AppendText("已存储人脸数量:" + result.SortFaceSize + Environment.NewLine);
                    txtLog.AppendText(Environment.NewLine);
                }));
            };
        }

        public INCommandDetail GetCommandDetail()
        {
            string sn, password;
            sn = txtSN.Text;
            if (string.IsNullOrEmpty(sn))
            {
                sn = "0000000000000000";
            }
            password = txtPassword.Text;
            if (password.Length != 8)
            {
                password = Door8800Command.NULLPassword;
            }
            CommandDetailFactory.ConnectType connectType = CommandDetailFactory.ConnectType.UDPClient;
            string addr = txtIP.Text;
            int port = (int)Num_UdpPort.Value;
            if (port > 65535) port = 8000;
            return GetCommandDetail(connectType, sn, password, addr, port);

        }
        public INCommandDetail GetCommandDetail(CommandDetailFactory.ConnectType connectType,
       string sSN, string sPassword, string sRemoteIP, int iRemotePort)
        {
            var protocolType = CommandDetailFactory.ControllerType.Door89H;
            var cmdDtl = CommandDetailFactory.CreateDetail(connectType, sRemoteIP, iRemotePort,
                protocolType, sSN, sPassword);

            if (connectType == CommandDetailFactory.ConnectType.UDPClient)
            {
                var dtl = cmdDtl.Connector as TCPClientDetail;
                dtl.LocalAddr = mServerIP;
                dtl.LocalPort = mServerPort;
            }
            cmdDtl.Timeout = 600;
            cmdDtl.RestartCount = 3;
            return cmdDtl;
        }

        private void btnClearMessage_Click(object sender, EventArgs e)
        {
            txtLog.Text = "";
        }

        private void btnReadCardListInfo_Click(object sender, EventArgs e)
        {
            var cmdDtl = GetCommandDetail();
            var cmd = new ReadPersonDataBase(cmdDtl);
            cmdDtl.Timeout = 15000;
            mAllocator.AddCommand(cmd);
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {
                ReadPersonDataBase_Result result = cmde.Command.getResult() as ReadPersonDataBase_Result;
                StringBuilder sLogs = new StringBuilder();
                StringBuilder sLogs2 = new StringBuilder();
                Invoke(new Action(() =>
                {
                    if (result.DataBaseSize > 0)
                    {
                        result.PersonList = result.PersonList.OrderBy(o => o.IsFaceFeatureCode).ToList();
                        foreach (Person person in result.PersonList)
                        {
                            txtLog.AppendText("用户号" + person.UserCode + Environment.NewLine);
                            txtLog.AppendText("姓名:" + person.PName + Environment.NewLine);
                            txtLog.AppendText("卡号:" + person.CardData + Environment.NewLine);
                            txtLog.AppendText("有效期:" + person.Expiry.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine);
                            txtLog.AppendText("开门时段:" + person.TimeGroup + Environment.NewLine);
                            txtLog.AppendText("状态:" + person.CardStatus + Environment.NewLine);
                            txtLog.AppendText("节假日:" + person.Holiday + Environment.NewLine);
                            txtLog.AppendText("出入标志:" + person.EnterStatus + Environment.NewLine);
                            txtLog.AppendText("最近读卡时间:" + person.RecordTime + Environment.NewLine);
                            txtLog.AppendText("是否有人脸:" + person.IsFaceFeatureCode + Environment.NewLine);
                            txtLog.AppendText("指纹数:" + person.FingerprintFeatureCodeCout + Environment.NewLine);
                            txtLog.AppendText(Environment.NewLine);
                        }
                    }

                }));
            };
        }
        uint UserCode = 0;
        private void Btn_UploadPerson_Click(object sender, EventArgs e)
        {
            UserCode = (uint)GetRandomNum();
            Person person = new Person(UserCode, "测试_" + UserCode);
            var cmdDtl = GetCommandDetail();
            byte[] datas = System.IO.File.ReadAllBytes("person.jpg");
            datas = ImageTool.ConvertImage(datas, 480, 640, 122880);
            IdentificationData id = new IdentificationData(1, datas);

            var par = new AddPersonAndImage_Parameter(person, id);
            par.WaitRepeatMessage = true;//固件版本v4.28以上才能用
            var cmd = new AddPeosonAndImage(cmdDtl, par);
            mAllocator.AddCommand(cmd);
            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>
            {

                var result = cmde.Result as AddPersonAndImage_Result;

                // 识别信息上传状态 1--上传完毕；2--特征码无法识别；3--人员照片不可识别；4--人员照片或特征码重复
                if (result != null)
                {
                    var ids = result.IdDataUploadStatus;
                    Invoke(new Action(() =>
                    {
                        txtLog.AppendText($"命令成功,人员添加状态:{result.UserUploadStatus},照片上传状态:{ids[0]}" + Environment.NewLine);
                        if (ids[0] == 4)
                        {
                            txtLog.AppendText($"照片重复，重复的人员编号:{result.IdDataRepeatUser[0]}" + Environment.NewLine);
                        }
                    }));

                }
            };
        }
        private void Btn_DeletePerson_Click(object sender, EventArgs e)
        {
            var cmdDtl = GetCommandDetail();
            cmdDtl.Timeout = 15000;
            cmdDtl.RestartCount = 0;
            INCommand cmd = null;
            if (UserCode == 0) return;//需要先上传人员才能删除
            List<Person> persons = new List<Person>() { new Person() { UserCode = UserCode } };
            var par = new DeletePerson_Parameter(persons);
            cmd = new DeletePerson(cmdDtl, par);
            mAllocator.AddCommand(cmd);
        }

        private void btnClearPerson_Click(object sender, EventArgs e)
        {
            var cmdDtl = GetCommandDetail();
            var cmd = new ClearPersonDataBase(cmdDtl);
            mAllocator.AddCommand(cmd);
        }
    }
}
