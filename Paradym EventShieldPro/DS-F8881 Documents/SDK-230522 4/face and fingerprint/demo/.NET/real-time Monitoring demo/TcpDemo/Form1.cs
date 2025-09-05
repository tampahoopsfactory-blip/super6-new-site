using DoNetDrive.Core;
using DoNetDrive.Core.Command;
using DoNetDrive.Core.Connector;
using DoNetDrive.Core.Connector.TCPServer;
using DoNetDrive.Core.Connector.TCPServer.Client;
using DoNetDrive.Core.Data;
using DoNetDrive.Protocol;
using DoNetDrive.Protocol.Door8800;
using DoNetDrive.Protocol.Fingerprint.Data.Transaction;
using DoNetDrive.Protocol.Transaction;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TcpDemo
{
    public partial class Form1 : Form
    {
        ConnectorAllocator mAllocator;
        ConnectorObserverHandler mObserver;
        private ConcurrentDictionary<string, MyTCPServerClientDetail> mTCPClients;
        //   string LocalIP = string.Empty;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            IniLoadLocalIP();
            IniForm();
        }

        private void IniLoadLocalIP()
        {
            cmbLocalIP.Items.Clear();

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
                    cmbLocalIP.Items.Add(ip.ToString());
                }
            }

            if (cmbLocalIP.Items.Count > 0)
            {
                cmbLocalIP.SelectedIndex = cmbLocalIP.Items.Count - 1;
            }
        }

        private void IniForm()
        {
            mAllocator = ConnectorAllocator.GetAllocator();
            mObserver = new ConnectorObserverHandler();
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

            mObserver.DisposeRequestEvent += MObserver_DisposeRequestEvent;
            mObserver.DisposeResponseEvent += MObserver_DisposeResponseEvent;
        }

        private void mAllocator_CommandErrorEvent(object sender, CommandEventArgs e)
        {

        }

        private void mAllocator_CommandCompleteEvent(object sender, CommandEventArgs e)
        {

        }

        private void mAllocator_CommandProcessEvent(object sender, CommandEventArgs e)
        {

        }

        private void mAllocator_CommandTimeout(object sender, CommandEventArgs e)
        {

        }

        private void MAllocator_AuthenticationErrorEvent(object sender, CommandEventArgs e)
        {

        }

        private void MAllocator_TransactionMessage(INConnectorDetail connector, INData EventData)
        {

            var conn = mAllocator.GetConnector(connector);
            if (conn != null)
            {
                if (!conn.IsForciblyConnect())
                {
                    conn.OpenForciblyConnect();
                }
            }

            Door8800Transaction fcTrn = EventData as Door8800Transaction;
            StringBuilder strbuf = new StringBuilder();
            var evn = fcTrn.EventData;
            var tcp = connector as TCPServerClientDetail;
            var sRemoteIP = tcp.Remote.Addr;
            var iRemotePort = tcp.Remote.Port;
            if (fcTrn.CmdIndex <= 4)
            {
                //"序号："
                strbuf.Append("Serial No.:").Append(evn.SerialNumber).Append("；");

                if (evn.TransactionType < 4)
                {
                    //  string[] codeNameList = frmRecord.mTransactionCodeNameList[evn.TransactionType];
                    //"事件："
                    strbuf.Append("Event:").Append(evn.TransactionCode).Append("(").Append(evn.TransactionCode).Append(")");
                    //"；时间："
                    strbuf.Append(";Time:").Append(evn.TransactionDate.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                strbuf.AppendLine();
                if (fcTrn.CmdIndex == 1)
                {
                    CardTransaction cardtrn = evn as CardTransaction;
                    //"用户号："
                    strbuf.Append("User:").Append(cardtrn.UserCode.ToString());
                    //；出/入：
                    strbuf.Append(";Exit/Entry:").Append(cardtrn.Accesstype.ToString());

                    string sPhotoUse, PhotoNULL;
                    sPhotoUse = "ON";//有
                    PhotoNULL = "OFF";//无
                    //"，照片："
                    strbuf.Append(",Photo:").Append(cardtrn.Photo == 1 ? sPhotoUse : PhotoNULL);
                }
                if (fcTrn.CmdIndex == 4)
                {
                    BodyTemperatureTransaction btr = evn as BodyTemperatureTransaction;
                    //体温：
                    strbuf.Append("BodyTmp:").Append((float)btr.BodyTemperature / (float)10);
                }
            }
            if (fcTrn.CmdIndex == 0x22)
            {
                //"保活包消息，远端信息：
                strbuf.Append(fcTrn.SN).Append("Keepalive message,Remote:").Append(tcp.Remote.ToString());
            }

            if (fcTrn.CmdIndex == 0xA0)
            {

                //连接测试消息
                INCommandDetail cmdDtl;
                cmdDtl = GetTCPCommandDetail(fcTrn.SN, "FFFFFFFF", connector.GetKey());
                var sndConntmsg = new DoNetDrive.Protocol.Fingerprint.SystemParameter.SendConnectTestResponse(cmdDtl);
                AddCommand(sndConntmsg);
                //"连接测试消息，远端信息："
                strbuf.Append(fcTrn.SN).Append("Connection test message,Remote:").Append(tcp.Remote.ToString());
            }
            Invoke(() =>
            {
                textBox1.AppendText(strbuf.ToString() + System.Environment.NewLine);
            });
           
        }

        private void DownBodyTemperature()
        {

        }
        private INCommandDetail GetTCPCommandDetail(string sSN, string sPassword, string sKey)
        {
            var connectType = CommandDetailFactory.ConnectType.TCPServerClient;
            return GetCommandDetail(connectType, sSN, sPassword, sKey, 0);
        }
        public INCommandDetail GetCommandDetail(CommandDetailFactory.ConnectType connectType,
           string sSN, string sPassword, string sRemoteIP, int iRemotePort)
        {

            var protocolType = CommandDetailFactory.ControllerType.Door89H;
            var cmdDtl = CommandDetailFactory.CreateDetail(connectType, sRemoteIP, iRemotePort,
                protocolType, sSN, sPassword);
            cmdDtl.Timeout = 600;
            cmdDtl.RestartCount = 3;
            return cmdDtl;

        }
        private void Invoke(Action p)
        {
            try
            {
                Invoke((Delegate)p);
            }
            catch (Exception)
            {

                return;
            }

        }
        public void AddCommand(INCommand cmd)
        {
            if (cmd.CommandDetail == null) return;
            try
            {
                mAllocator.AddCommand(cmd);
            }
            catch (Exception)
            {
            }

        }
        private void mAllocator_ConnectorConnectedEvent(object sender, INConnectorDetail connector)
        {

        }

        private void mAllocator_ConnectorClosedEvent(object sender, INConnectorDetail connector)
        {

        }

        private void mAllocator_ConnectorErrorEvent(object sender, INConnectorDetail connector)
        {

        }

        private void MAllocator_ClientOnline(object sender, ServerEventArgs e)
        {
            INConnector inc = sender as INConnector;
            inc.AddRequestHandle(mObserver);
            switch (inc.GetConnectorType())
            {
                case ConnectorType.TCPServerClient://tcp 客户端已连接
                                                   //inc.OpenForciblyConnect();
                    Door8800RequestHandle fC8800Request =
                        new Door8800RequestHandle(DotNetty.Buffers.UnpooledByteBufferAllocator.Default, RequestHandleFactory);
                    inc.RemoveRequestHandle(typeof(Door8800RequestHandle));//先删除，防止已存在就无法添加。
                    inc.AddRequestHandle(fC8800Request);
                    TCPServerClientDetail clientDetail = inc.GetConnectorDetail() as TCPServerClientDetail;
                    AddTCPClient(inc.GetKey(), clientDetail.Remote);
                    //AddUDPClient(inc.GetConnectorDetail());
                    break;
            }
        }
        private AbstractTransaction RequestHandleFactory(string sn, byte cmdIndex, byte cmdPar)
        {

            //在这里需要根据SN进行类型判定，也可以根据SN来进行查表
            if (cmdIndex >= 1 && cmdIndex <= 4)
            {
                return DoNetDrive.Protocol.Fingerprint.Transaction.ReadTransactionDatabaseByIndex.NewTransactionTable[cmdIndex]();
            }
            if (cmdIndex == 0x22)
            {
                return new DoNetDrive.Protocol.Door.Door8800.Data.Transaction.KeepaliveTransaction();
            }

            if (cmdIndex == 0xA0)
            {
                return new DoNetDrive.Protocol.Door.Door8800.Data.Transaction.ConnectMessageTransaction();
            }
            return null;
        }
        private void AddTCPClient(string sKey, IPDetail remoteIP)
        {
            MyTCPServerClientDetail myTCP = new MyTCPServerClientDetail(
                sKey, new IPEndPoint(IPAddress.Parse(remoteIP.Addr), remoteIP.Port));
            if (!mTCPClients.TryAdd(sKey, myTCP))
            {
                //重复添加客户端，Key:{sKey}
                //  AddLog(GetLanguage("AddTCPClient0", sKey));
                return;
            }
            else
            {
                Invoke(new Action(() =>
                {
                    cmbTCPClientList.BeginUpdate();
                    cmbTCPClientList.Items.Add(myTCP);
                    if (cmbTCPClientList.Items.Count == 1)
                    {
                        cmbTCPClientList.SelectedIndex = 0;
                    }
                    else
                    {
                        cmbTCPClientList.SelectedIndex = cmbTCPClientList.Items.Count - 1;
                    }
                    cmbTCPClientList.EndUpdate();
                }));
            }
        }
        private void MAllocator_ClientOffline(object sender, ServerEventArgs e)
        {

        }

        private void MObserver_DisposeRequestEvent(INConnector connector, string msg)
        {

        }

        private void MObserver_DisposeResponseEvent(INConnector connector, string msg)
        {

        }
        bool mTCPServerBind = false;
        private string mTCPServer_IP;
        private int mTCPServer_Port;

        private void butTCPServerBind_Click(object sender, EventArgs e)
        {
            if (mTCPServerBind == false)
            {
                if (mTCPClients == null) mTCPClients = new ConcurrentDictionary<string, MyTCPServerClientDetail>();
                mTCPServer_IP = cmbLocalIP.Text;
                mTCPServer_Port = int.Parse(txtTCPServerPort.Text);
                var tcp = new TCPServerDetail(mTCPServer_IP, mTCPServer_Port);
                mAllocator.OpenConnector(tcp);
                txtTCPServerPort.Enabled = false;
                cmbLocalIP.Enabled = false;
                mTCPServerBind = true;
            }
            else
            {
                var tcp = new TCPServerDetail(mTCPServer_IP, mTCPServer_Port);

                mAllocator.CloseConnector(tcp);

                var sKeys = mTCPClients.Keys.ToArray();
                foreach (var key in sKeys)
                {
                    TCPServerClientDetail cdt = new TCPServerClientDetail(key);
                    mAllocator.CloseConnector(cdt);
                }
                mTCPClients.Clear();
                cmbTCPClientList.Items.Clear();
                txtTCPServerPort.Enabled = true;
                cmbLocalIP.Enabled = true;
                mTCPServerBind = false;
            }
        }
    }
    public class MyTCPServerClientDetail : TCPServerClientDetail
    {
        public String SN;
        public MyTCPServerClientDetail(string sKey, IPEndPoint remoteIP) : base(sKey)
        {

        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(SN))
            {
                return Remote.ToString();
            }
            return $"{SN} {Remote}";
        }

    }

    public struct CommandResult
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string SN { get; set; }

        public string Remote { get; set; }

        public string Time { get; set; }
        public string Timemill { get; set; }
    }
}
