package Demo;

import Door.Access.Command.CommandDetail;
import Door.Access.Command.CommandParameter;
import Door.Access.Command.INCommand;
import Door.Access.Command.INCommandResult;
import Door.Access.Connector.ConnectorAllocator;
import Door.Access.Connector.ConnectorDetail;
import Door.Access.Connector.E_ControllerType;
import Door.Access.Connector.INConnectorEvent;
import Door.Access.Connector.TCPClient.TCPClientDetail;
import Door.Access.Connector.TCPServer.TCPServerClientDetail;
import Door.Access.Data.AbstractTransaction;
import Door.Access.Data.BytesData;
import Door.Access.Data.INData;
import Door.Access.Door8800.Command.Data.*;
import Door.Access.Door8800.Command.Door.Parameter.OpenDoor_Parameter;
import Door.Access.Door8800.Command.System.BeginWatch;
import Door.Access.Door8800.Command.System.Parameter.WriteKeepAliveInterval_Parameter;
import Door.Access.Door8800.Command.System.Parameter.WriteTCPSetting_Parameter;
import Door.Access.Door8800.Command.System.ReadTCPSetting;
import Door.Access.Door8800.Command.System.Result.ReadTCPSetting_Result;
import Door.Access.Door8800.Command.System.WriteKeepAliveInterval;
import Door.Access.Door8800.Command.System.WriteTCPSetting;
import Door.Access.Door8800.Door8800Identity;
import Door.Access.Door8800.Packet.Door8800Decompile;
import Door.Access.Door8800.Packet.Door8800PacketModel;
import Door.Access.Packet.INPacketModel;
import Door.Access.Packet.PacketDecompileAllocator;
import io.netty.buffer.ByteBuf;
import io.netty.buffer.ByteBufUtil;
import java.net.InetAddress;
import java.net.UnknownHostException;

import java.util.ArrayList;
import java.util.Dictionary;
import java.util.HashMap;
import java.util.concurrent.Semaphore;
import java.util.logging.Level;
import java.util.logging.Logger;
/**
 * 服务器模式使用流程
 * 1、设置设备的服务器IP和端口
 * 2、打开设备监控状态
 * 3、设置保活
 * 4、等待设备连接
 * 5、发送命令给设备
 * @author F
 */
public class TcpServerTest implements INConnectorEvent {

    ConnectorAllocator _Allocator;
    private final Semaphore available = new Semaphore(0, true);
    private String LocalIP;

    private int LocalPort;
    HashMap<String, TCPServerClientDetail> TcpDictionary = new HashMap<String, TCPServerClientDetail>();
    ///等待

    public void syn() {
        try {
            available.acquire();
        } catch (Exception e) {
        }

    }
//关闭等待

    public void release() {
        _Allocator.Release();
        available.release();
    }

    public TcpServerTest() {
        _Allocator = ConnectorAllocator.GetAllocator();//获取通道对象
        _Allocator.AddListener(this);//添加监听
        try {
            InetAddress addr = InetAddress.getLocalHost();//获取本机IP
            LocalIP = addr.getHostAddress();
            LocalPort = 9000;
            _Allocator.Listen(LocalIP, LocalPort); //绑定TCP监听IP和端口
              System.out.println("设置流程开始");
            readTCPSetting();
        } catch (UnknownHostException ex) {
            Logger.getLogger(TcpServerTest.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

    private CommandDetail GetCommandDetail() {
        CommandDetail commandDetail = new CommandDetail();
        commandDetail.Timeout = 5000;//此函数超时时间设定长一些
        commandDetail.RestartCount = 0;
        TCPClientDetail tcpClientDetail = new TCPClientDetail("192.168.1.190", 8000);//设备的IP和 端口(默认8000) 
        tcpClientDetail.Timeout = 5000;
        tcpClientDetail.RestartCount = 0;
        commandDetail.Connector = tcpClientDetail;
        Door8800Identity idt = new Door8800Identity("MC-5912T10064245", "FFFFFFFF", E_ControllerType.Door8900);//设备SN(16位字符)，密码(8位十六进制字符)，设备类型
        commandDetail.Identity = idt;
        return commandDetail;
    }

    /**
     * 读取TPC/IP参数
     */
    public void readTCPSetting() {

        CommandParameter parameter = new CommandParameter(GetCommandDetail());
        ReadTCPSetting cmd = new ReadTCPSetting(parameter);
        _Allocator.AddCommand(cmd);
        System.out.println("开始读取设备TCP参数");

    }

    /**
     * 写入TPC/IP参数
     *
     * @param detail
     */
    private void writeTCPSetting(TCPDetail detail) {
        detail.SetServerAddr(LocalIP);
        detail.SetServerIP(LocalIP);
        detail.SetServerPort(LocalPort);
        WriteTCPSetting_Parameter parameter = new WriteTCPSetting_Parameter(GetCommandDetail(), detail);
        WriteTCPSetting cmd = new WriteTCPSetting(parameter);
        _Allocator.AddCommand(cmd);
        System.out.println("开始写入设备TCP参数");
    }

    /**
     * 开启监控
     */
    private void beginWatch() {
        BeginWatch cmd = new BeginWatch(new CommandParameter((GetCommandDetail())));
        _Allocator.AddCommand(cmd);

    }

    /**
     * 写入保活间隔时间
     */
    private void writeKeepAliveInterval() {
        WriteKeepAliveInterval_Parameter par = new WriteKeepAliveInterval_Parameter(GetCommandDetail());
        par.IntervalTime = 30;//取值范围：0-3600,0--关闭功能 
        WriteKeepAliveInterval cmd = new WriteKeepAliveInterval(par);
        _Allocator.AddCommand(cmd);
    }

    /**
     * 开门
     *
     * @param snString
     * @param detail
     */
    private void openDoor(String snString, TCPServerClientDetail detail) {
        CommandDetail commandDetail = new CommandDetail();
        commandDetail.Timeout = 5000;//此函数超时时间设定长一些
        commandDetail.RestartCount = 0;
        commandDetail.Connector = detail;
        Door8800Identity idt = new Door8800Identity(snString, "FFFFFFFF", E_ControllerType.Door8900);//设备SN(16位字符)，密码(8位十六进制字符)，设备类型
        commandDetail.Identity = idt;
        OpenDoor_Parameter parameter = new OpenDoor_Parameter(commandDetail);
        parameter.Door.SetDoor(1, 1);//打开一门
        Door.Access.Door8800.Command.Door.OpenDoor cmd = new Door.Access.Door8800.Command.Door.OpenDoor(parameter);
        _Allocator.AddCommand(cmd);
         System.out.println("开始执行开门");
    }

    /**
     * 命令执行成功回调
     *
     * @param cmd cmd是执行命令的命令对象
     * @param result 是命令执行后返回的结果
     */
    @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        //命令执行成功之后会执行
        if (result instanceof ReadTCPSetting_Result) {
            System.out.println("读取设备TCP参数成功");
            ReadTCPSetting_Result tcpResult = (ReadTCPSetting_Result) result;
            writeTCPSetting(tcpResult.TCP);
        }
        if (cmd instanceof WriteTCPSetting) {
            System.out.println("写入设备TCP参数成功");
            beginWatch();
        }
        if (cmd instanceof BeginWatch) {
            System.out.println("开启设备监控成功");
            writeKeepAliveInterval();
        }
        if (cmd instanceof WriteKeepAliveInterval) {
            System.out.println("写入保活间隔时间成功");
             System.out.println("设置流程结束");
            System.out.println("等待设备连接");
        }
         if (cmd instanceof Door.Access.Door8800.Command.Door.OpenDoor) {
            System.out.println("执行开门成功");
        }
    }

    /**
     * 命令执行进度回调
     *
     * @param cmd
     */
    @Override
    public void CommandProcessEvent(INCommand cmd) {
        System.out.println("命令执行进度:" + cmd.getProcessMax() + "/" + cmd.getProcessStep());
    }

    /**
     * 连接出错
     *
     * @param cmd
     * @param isStop 是否是用户停止时发生的
     */
    @Override
    public void ConnectorErrorEvent(INCommand cmd, boolean isStop) {
        System.out.println("Demo.TcpServerTest.ConnectorErrorEvent()");
    }

    /**
     * 连接出错
     *
     * @param detail
     */
    @Override
    public void ConnectorErrorEvent(ConnectorDetail detail) {

    }

    /**
     * 命令超时
     *
     * @param cmd
     */
    @Override
    public void CommandTimeout(INCommand cmd) {

    }

    /**
     * 密码错误
     *
     * @param cmd
     */
    @Override
    public void PasswordErrorEvent(INCommand cmd) {
        System.out.println("通讯密码错误");
    }

    /**
     * 通讯校验和错误
     *
     * @param cmd
     */
    @Override
    public void ChecksumErrorEvent(INCommand cmd) {
        System.out.println("通讯校验和错误");
    }

    /**
     * 数据监控
     *
     * @param detail
     * @param event
     */
    @Override
    public void WatchEvent(ConnectorDetail detail, INData event) {
        if (event instanceof BytesData) {
            authentication(detail, event);
        } else {
            Transaction(detail, event);
        }
    }

    /**
     * 身份验证
     *
     * @param detail
     * @param event
     */
    private void authentication(ConnectorDetail detail, INData event) {
        BytesData b = (BytesData) event;
        TCPServerClientDetail cd = (TCPServerClientDetail) detail;
        ByteBuf dBuf = b.GetBytes();
        if (dBuf.getByte(0) == 0x7e) {
            Door8800Decompile decompile = new Door8800Decompile();

            ArrayList<INPacketModel> oRetPack = new ArrayList<INPacketModel>(10);
            if (decompile.Decompile(dBuf, oRetPack)) {
                Door8800PacketModel m = (Door8800PacketModel) oRetPack.get(0);
                String snString = m.GetSN();
                // if(TcpDictionary.containsKey(snString)){
                TcpDictionary.put(snString, cd);
                // }
                _Allocator.AddWatchDecompile(cd, PacketDecompileAllocator.GetDecompile(E_ControllerType.Door8900));
                System.out.println("添加控制板解析器");
                System.out.println("客户端ID:" + cd.ClientID + "(" + m.GetSN() + ")，收到数据包：" + ByteBufUtil.hexDump(dBuf));
                openDoor(snString, cd);
            }

        }
    }

    /**
     * 监控消息处理
     *
     * @param detail
     * @param event
     */
    private void Transaction(ConnectorDetail detail, INData event) {
        if (event instanceof Door8800WatchTransaction) {
            Door8800WatchTransaction watchEvent = (Door8800WatchTransaction) event;
            AbstractTransaction tr = (AbstractTransaction) watchEvent.EventData;
            System.out.println(watchEvent.SN + " 收到监控事件：" + tr.getClass().toString());
            switch (watchEvent.CmdIndex) {
                case 1://认证记录
                    CardTransaction card = (CardTransaction) watchEvent.EventData;
                    if ("4972917".equals(card.CardData)) {    //判断是否是有效卡号
                        //开门操作
                        CommandDetail commandDetail = new CommandDetail();
                        commandDetail.Connector = TcpDictionary.get(watchEvent.SN);
                        commandDetail.Identity = new Door8800Identity(watchEvent.SN, "FFFFFFFF", E_ControllerType.Door8900);//设置SN(16位字符)，密码(8位十六进制字符)，设备类型
                        OpenDoor_Parameter openDoor_parameter = new OpenDoor_Parameter(commandDetail);//初始化开门参数
                        openDoor_parameter.Door.SetDoor(card.DoorNum(), 1);//设定1号门执行操作
                        Door.Access.Door8800.Command.Door.OpenDoor openDoor = new Door.Access.Door8800.Command.Door.OpenDoor(openDoor_parameter);
                        _Allocator.AddCommand(openDoor);
                    }
                    break;
                case 2://出门开关信息
                    ButtonTransaction Software=(ButtonTransaction) watchEvent.EventData;
                    break;
                case 3://门磁信息
                    DoorSensorTransaction DoorSensor = (DoorSensorTransaction) watchEvent.EventData;
                    break;
                case 4://软件操作 包括远程开门之类的一些命令
                    SoftwareTransaction softwareTransaction=(SoftwareTransaction) watchEvent.EventData;
                    System.out.println("远程开门消息.................");
                    break;
                case 5://报警记录
                    AlarmTransaction alarmTransaction= (AlarmTransaction) watchEvent.EventData;
                    break;
                case 6://系统记录
                    SystemTransaction  systemTransaction= (SystemTransaction) watchEvent.EventData;
                    break;
                case 34://保活消息
                    System.out.println("保活包消息.................");
                    break;
                default:
                    DefinedTransaction dt = (DefinedTransaction) watchEvent.EventData;
                    break;
            }
        } else {
            System.out.println("testio.FCardIO.FCardIOTest.WatchEvent() -- 未知消息");
        }
    }

    /**
     * 设备上线
     *
     * @param client
     */
    @Override
    public void ClientOnline(TCPServerClientDetail client) {
        // client.
        System.out.println("有客户端上线：" + client.Remote.toString() + "，客户端ID：" + client.ClientID);
    }

    /**
     * 设备离线
     *
     * @param client
     */
    @Override
    public void ClientOffline(TCPServerClientDetail client) {
        System.out.println("有客户端离线：" + client.Remote.toString() + "，客户端ID：" + client.ClientID);
    }
}
