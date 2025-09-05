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
import Door.Access.Data.AbstractTransaction;
import Door.Access.Data.BytesData;
import Door.Access.Data.INData;
import Door.Access.Door8800.Command.Data.*;
import Door.Access.Door8800.Command.Door.Parameter.OpenDoor_Parameter;
import Door.Access.Door8800.Door8800Identity;
import Door.Access.Door8800.Packet.Door8800Decompile;
import Door.Access.Door8800.Packet.Door8800PacketModel;
import Door.Access.Packet.INPacketModel;
import Door.Access.Packet.PacketDecompileAllocator;
import io.netty.buffer.ByteBuf;
import io.netty.buffer.ByteBufUtil;

import java.util.ArrayList;
import java.util.Dictionary;
import java.util.HashMap;
import java.util.concurrent.Semaphore;

public class TcpServerTest implements INConnectorEvent {

    ConnectorAllocator _Allocator;
    private final Semaphore available = new Semaphore(0, true);
    
    HashMap<String,TCPServerClientDetail> TcpDictionary=new  HashMap<String, TCPServerClientDetail>();
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
        _Allocator.Listen("192.168.1.130",9000); //绑定监听IP和端口
    }
/**
 * 命令执行成功回调
 * @param cmd cmd是执行命令的命令对象 
 * @param result  是命令执行后返回的结果
 */
    @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        //命令执行成功之后会执行
        System.out.println("命令执行完成");
    }
/**
 * 命令执行进度回调
 * @param cmd 
 */
    @Override
    public void CommandProcessEvent(INCommand cmd) {
        System.out.println("命令执行进度:"+cmd.getProcessMax()+"/"+ cmd.getProcessStep());
    }
    /**
     * 连接出错
     * @param cmd
     * @param isStop 是否是用户停止时发生的
     */
    @Override
    public void ConnectorErrorEvent(INCommand cmd, boolean isStop) {
        System.out.println("Demo.TcpServerTest.ConnectorErrorEvent()");
    }
/**
 * 连接出错
 * @param detail 
 */
    @Override
    public void ConnectorErrorEvent(ConnectorDetail detail) {

    }
/**
 * 命令超时
 * @param cmd 
 */
    @Override
    public void CommandTimeout(INCommand cmd) {

    }
/**
 * 密码错误
 * @param cmd 
 */
    @Override
    public void PasswordErrorEvent(INCommand cmd) {
        System.out.println("通讯密码错误");
    }
/**
 * 通讯校验和错误
 * @param cmd 
 */
    @Override
    public void ChecksumErrorEvent(INCommand cmd) {
  System.out.println("通讯校验和错误");
    }
/**
 * 数据监控
 * @param detail
 * @param event 
 */
    @Override
    public void WatchEvent(ConnectorDetail detail, INData event) {
        if (event instanceof BytesData) {
            authentication(detail,event);
        }else {
            Transaction(detail,event);
        }
    }

    /**
     * 身份验证
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
                String snString=m.GetSN();
               // if(TcpDictionary.containsKey(snString)){
                    TcpDictionary.put(snString, cd);
               // }
                    _Allocator.AddWatchDecompile(cd, PacketDecompileAllocator.GetDecompile(E_ControllerType.Door8900));
                    System.out.println("添加控制板解析器");               
                System.out.println("客户端ID:" + cd.ClientID + "(" + m.GetSN() + ")，收到数据包：" + ByteBufUtil.hexDump(dBuf));
            }

        }
    }
    /**
     * 监控消息处理
     * @param detail
     * @param event
     */
    private  void Transaction(ConnectorDetail detail, INData event){
        if (event instanceof Door8800WatchTransaction) {
            Door8800WatchTransaction watchEvent = (Door8800WatchTransaction) event;
            AbstractTransaction tr = (AbstractTransaction) watchEvent.EventData;
            System.out.println(watchEvent.SN+" 收到监控事件：" + tr.getClass().toString());
            switch (watchEvent.CmdIndex) {
                case 1://认证记录
                    CardTransaction card = (CardTransaction) watchEvent.EventData;
                   if("4972917".equals(card.CardData)){    //判断是否是有效卡号
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
                case 2://门磁记录
                    DoorSensorTransaction DoorSensor = (DoorSensorTransaction) watchEvent.EventData;
                    break;
                case 3://系统记录
                    SystemTransaction system = (SystemTransaction) watchEvent.EventData;
                    break;
                case 4://包活/连接测试记录
                    System.out.println("保活包消息.................");
                    break;
                default:
                    DefinedTransaction dt = (DefinedTransaction) watchEvent.EventData;
            }
        } else {
            System.out.println("testio.FCardIO.FCardIOTest.WatchEvent() -- 未知消息");
        }
    }
/**
 * 设备上线
 * @param client 
 */
    @Override
    public void ClientOnline(TCPServerClientDetail client) {
       // client.
        System.out.println("有客户端上线：" + client.Remote.toString() + "，客户端ID：" + client.ClientID);
    }
/**
 * 设备离线
 * @param client 
 */
    @Override
    public void ClientOffline(TCPServerClientDetail client) {
        System.out.println("有客户端离线：" + client.Remote.toString() + "，客户端ID：" + client.ClientID);
    }
}
