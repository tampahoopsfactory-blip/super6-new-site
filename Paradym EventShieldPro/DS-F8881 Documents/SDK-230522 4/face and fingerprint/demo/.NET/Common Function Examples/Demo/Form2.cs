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
using OneCard;


namespace Demo
{
    public partial class Form2 : Form
    {
        ConnectorAllocator mAllocator;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Init();
            Task.Delay(2000).ContinueWith(x => AutoTask());
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


        private void AddLog(string sLog)
        {
            if (txtLog.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    AddLog(sLog);
                }));
                return;
            }
            else
            {
                txtLog.AppendText($"{DateTime.Now:HH:mm:ss.fff} -- {sLog} {Environment.NewLine}");
            }

        }

        private void mAllocator_CommandCompleteEvent(object sender, CommandEventArgs e)
        {
            AddLog($" {e.Command.GetType().Name} 命令成功");
        }

        private void mAllocator_CommandErrorEvent(object sender, CommandEventArgs e)
        {
            AddLog($" {e.Command.GetType().Name} 命令错误");
        }

        private void mAllocator_CommandProcessEvent(object sender, CommandEventArgs e)
        {
            //命令进度
        }

        private void mAllocator_CommandTimeout(object sender, CommandEventArgs e)
        {
            AddLog($" {e.Command.GetType().Name} 命令超时");
        }

        private void MAllocator_AuthenticationErrorEvent(object sender, CommandEventArgs e)
        {
            AddLog($" {e.Command.GetType().Name} 设备通讯密码错误");

        }

        private void MAllocator_TransactionMessage(INConnectorDetail connector, INData EventData)
        {

        }

        private void mAllocator_ConnectorConnectedEvent(object sender, INConnectorDetail connector)
        {
            if (!connector.IsFaulted && connector is UDPServerDetail)
            {


                this.Invoke(new Action(() =>
                {
                    AddLog($" UDP 绑定成功： {connector.ToString()}");

                    //开始自动循环发送人员
                    LoopSendPeople();

                }));
            }
        }



        private void mAllocator_ConnectorClosedEvent(object sender, INConnectorDetail connector)
        {
        }

        private void mAllocator_ConnectorErrorEvent(object sender, INConnectorDetail connector)
        {
            if (connector is UDPServerDetail)
            {
                AddLog($" UDP 绑定失败： {connector.ToString()}");
            }
        }

        private void MAllocator_ClientOnline(object sender, ServerEventArgs e)
        {
        }

        private void MAllocator_ClientOffline(object sender, ServerEventArgs e)
        {

        }


        private void AutoTask()
        {
            BindUDP();
        }

        private string mServerIP;
        private int mServerPort;
        private void BindUDP()
        {
            //先绑定一个UDP端口  这里应该读配置文件获取
            mServerPort = 9001;//本机DP端口号
            mServerIP = "192.168.1.86";//本机IP

            UDPServerDetail detail = new UDPServerDetail(mServerIP, mServerPort);
            //打开UDP服务器
            mAllocator.OpenConnector(detail);
            //等待 mAllocator_ConnectorConnectedEvent 事件

        }


        private void LoopSendPeople()
        {
            UploadPerson();
        }


        /// <summary>
        /// 获取 设备SN\设备通讯密码IP地址\端口号\
        /// </summary>
        /// <returns></returns>
        public INCommandDetail GetCommandDetail()
        {
            string sn, password;
            //这里应该读配置文件或者数据库获取
            sn = "0000000000000000";//设备SN
            password = "FFFFFFFF";//设备密码 Door8800Command.NULLPassword; 没有密码
            string addr = "192.168.1.182";//设备IP
            int port = 8101;//设备UDP端口号，可在机器上查看，默认是8101

            CommandDetailFactory.ConnectType connectType = CommandDetailFactory.ConnectType.UDPClient;
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


        uint UserCode = 0;
        /// <summary>
        /// 添加人员
        /// </summary>
        private void UploadPerson()
        {
            var cmdDtl = GetCommandDetail();//获取 设备SN\设备通讯密码IP地址\端口号\

            //组装人员信息
            UserCode = (uint)Form1.GetRandomNum();
            Person person = new Person(UserCode, "测试_" + UserCode);//设置人员编号和人名，具体信息看 Person定义
            person.CardData = 11223344;
            byte[] datas = System.IO.File.ReadAllBytes("person.jpg");//读取人员照片
            datas = ImageTool.ConvertImage(datas, 480, 640, 122880);//格式和图片尺寸检查
            IdentificationData id = new IdentificationData(1, datas);

            var par = new AddPersonAndImage_Parameter(person, id);
            par.WaitRepeatMessage = true;//固件版本v4.28以上才能用
            var cmd = new AddPeosonAndImage(cmdDtl, par);//组装命令
            mAllocator.AddCommand(cmd);//将命令加入到队列,准备发送

            AddLog($"开始添加人员，编号:{UserCode}");

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>//命令完成后会回调
            {

                var result = cmde.Result as AddPersonAndImage_Result;

                // 识别信息上传状态 1--上传完毕；2--特征码无法识别；3--人员照片不可识别；4--人员照片或特征码重复
                if (result != null)
                {
                    var ids = result.IdDataUploadStatus;

                    AddLog($"添加人员--命令成功,编号:{UserCode}  人员添加状态:{result.UserUploadStatus},照片上传状态:{ids[0]}");
                    if (ids[0] == 4)
                    {
                        AddLog($"添加人员--照片重复，重复的人员编号:{result.IdDataRepeatUser[0]}");
                    }

                    //自动删除人员
                    DeletePerson();
                }
            };
        }
        /// <summary>
        /// 删除人员
        /// </summary>
        private void DeletePerson()
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
            AddLog($"开始删除人员，编号:{UserCode}");

            cmdDtl.CommandCompleteEvent += (sdr, cmde) =>//命令完成后会回调
            {
                AddLog($"删除人员--命令成功,人员,编号:{UserCode}");

                UploadPerson();
            };
        }

    }
}
