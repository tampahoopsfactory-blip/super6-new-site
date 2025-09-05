package Demo;

import Door.Access.Command.CommandDetail;
import Door.Access.Command.CommandParameter;
import Door.Access.Command.INCommand;
import Door.Access.Command.INCommandResult;
import Door.Access.Connector.ConnectorAllocator;
import Door.Access.Connector.ConnectorDetail;
import Door.Access.Connector.E_ControllerType;
import Door.Access.Connector.INConnectorEvent;
import Door.Access.Connector.TCPServer.TCPServerClientDetail;
import Door.Access.Connector.UDP.UDPDetail;
import Door.Access.Data.INData;
import Door.Access.Door8800.Door8800Identity;
import Face.Data.OEMDetail;
import Face.Data.e_TransactionDatabaseType;
import Face.Door.CloseDoor;
import Face.Door.HoldDoor;
import Face.System.*;
import Face.System.Parameter.WriteOEM_Parameter;
import Face.Transaction.Parameter.ReadTransactionDatabase_Parameter;
import Face.Transaction.ReadTransactionDatabase;

import java.io.UnsupportedEncodingException;
import java.util.Calendar;
import java.util.concurrent.Semaphore;

/**
 * 门方法测试
 */
public class DoorTest implements INConnectorEvent {

    String LocalIp = "192.168.1.130";
    int LocalPort = 8100;
    ConnectorAllocator _Allocator;
    private final Semaphore available = new Semaphore(0, true);
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

    private CommandDetail getDetail() {
        CommandDetail commandDetail = new CommandDetail();
        /**
         * 此函数超时时间设定长一些
         */
        commandDetail.Timeout = 8000;
        UDPDetail cDetail = new UDPDetail("192.168.1.174", 8686, LocalIp, LocalPort);
        commandDetail.Connector = cDetail;
        /**
         * 设置SN(16位字符)，密码(8位十六进制字符)，设备类型
         */
        Door8800Identity idt = new Door8800Identity("FC-8600T20124011", "FFFFFFFF", E_ControllerType.Face_Fingerprint);
        commandDetail.Identity = idt;
        return commandDetail;
    }

    public DoorTest() {
        _Allocator = ConnectorAllocator.GetAllocator();
        _Allocator.AddListener(this);
        _Allocator.UDPBind(LocalIp, LocalPort);

    }

    /**
     * 远程关门
     */
    public void CloseDoor() {
        CommandParameter par = new CommandParameter(getDetail());
        CloseDoor cmd = new CloseDoor(par);
        _Allocator.AddCommand(cmd);
    }

    /**
     * 门常开
     */
    public void HoldDoor() {
        CommandParameter par = new CommandParameter(getDetail());
        HoldDoor cmd = new HoldDoor(par);
        _Allocator.AddCommand(cmd);
    }

    public void ReadSystemRunStatus() {
        CommandParameter par = new CommandParameter(getDetail());
        ReadSystemRunStatus cmd = new ReadSystemRunStatus(par);
        _Allocator.AddCommand(cmd);
    }
    public void ReadVersion() {
        CommandParameter par = new CommandParameter(getDetail());
        ReadVersion cmd = new ReadVersion(par);
        _Allocator.AddCommand(cmd);
    }
    public void readTransactionDatabase() {
        ReadTransactionDatabase_Parameter parameter = new ReadTransactionDatabase_Parameter(getDetail(), e_TransactionDatabaseType.OnBodyTemperatureTransaction);
        // parameter.PacketSize=60;
        parameter.Quantity = 10;
        ReadTransactionDatabase cmd = new ReadTransactionDatabase(parameter);
        _Allocator.AddCommand(cmd);
    }
    public  void readOem(){
        CommandParameter parameter=new CommandParameter(getDetail());
        ReadOEM cmd=new ReadOEM(parameter);
        _Allocator.AddCommand(cmd);
    }
    public  void writeOem(){
        OEMDetail detail=new OEMDetail();
        detail.Manufacturer="岗顶赛格";
        detail.WebAddress="www.baidu.com";
        detail.DeliveryDate= Calendar.getInstance();
        WriteOEM_Parameter parameter=new WriteOEM_Parameter(getDetail(),detail);
        WriteOEM cmd= null;
        try {
            cmd = new WriteOEM(parameter);
        } catch (UnsupportedEncodingException e) {
            e.printStackTrace();
        }
        _Allocator.AddCommand(cmd);
    }
    public  void beginWatch(){
        CommandParameter parameter=new  CommandParameter(getDetail());
        BeginWatch cmd=new BeginWatch(parameter);
        _Allocator.AddCommand(cmd);
    }
    public  void closeWatch(){
        CommandParameter parameter=new  CommandParameter(getDetail());
       CloseWatch cmd=new CloseWatch(parameter);
       _Allocator.AddCommand(cmd);
    }
    /**
     * 命令执行成功返回
     *
     * @param cmd    此次事件所关联的命令详情
     * @param result 命令包含的结果
     */
    @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        System.out.println("命令执行成功");
    }

    @Override
    public void CommandProcessEvent(INCommand cmd) {
        int processMax = cmd.getProcessMax();
        int processStep = cmd.getProcessStep();
        System.out.println("当前执行进度：" + processMax + "/" + processStep);
    }

    @Override
    public void ConnectorErrorEvent(INCommand cmd, boolean isStop) {
        System.out.println("命令错误");
    }

    @Override
    public void ConnectorErrorEvent(ConnectorDetail detail) {
        System.out.println("连接错误");
    }

    @Override
    public void CommandTimeout(INCommand cmd) {
        System.out.println("连接超时");
    }

    @Override
    public void PasswordErrorEvent(INCommand cmd) {
        System.out.println("通讯密码错误");
    }

    @Override
    public void ChecksumErrorEvent(INCommand cmd) {

    }

    @Override
    public void WatchEvent(ConnectorDetail detail, INData event) {

    }

    @Override
    public void ClientOnline(ConnectorDetail client) {
       //  TCPServerClientDetail tClient=(TCPServerClientDetail)client;
      //  System.out.println("有客户端上线：" + tClient.Remote.toString() + "，客户端ID：" + tClient.ClientID);

    }

    @Override
    public void ClientOffline(ConnectorDetail client) {
       //  TCPServerClientDetail tClient=(TCPServerClientDetail)client;
      //  System.out.println("有客户端离线：" + tClient.Remote.toString() + "，客户端ID：" + tClient.ClientID);
    }
}
