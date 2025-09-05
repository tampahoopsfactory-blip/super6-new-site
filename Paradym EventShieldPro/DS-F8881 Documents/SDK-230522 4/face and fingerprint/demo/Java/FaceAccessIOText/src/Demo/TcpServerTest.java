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
import Door.Access.Data.AbstractTransaction;
import Door.Access.Data.BytesData;
import Door.Access.Data.INData;
import Door.Access.Door8800.Command.Card.Parameter.WriteCardListBySequence_Parameter;
import Door.Access.Door8800.Command.Data.*;
import Door.Access.Door8800.Command.Door8800CommandWatchResponse;
import Door.Access.Door8800.Door8800Identity;
import Door.Access.Door8800.Packet.Door8800Decompile;
import Door.Access.Door8800.Packet.Door8800PacketModel;
import Door.Access.Door89H.Command.Card.WriteCardListBySequence;
import Door.Access.Packet.INPacketModel;
import Door.Access.Packet.PacketDecompileAllocator;
import Face.Data.BodyTemperatureTransaction;
import Face.Data.CardTransaction;
import Face.Data.DoorSensorTransaction;
import Face.Data.FaceCommandWatchResponse;
import Face.Data.IdentificationData;
import Face.Data.Person;
import Face.Data.SystemTransaction;
import Face.FaceConnectorAllocator;
import Face.Person.AddPersonAndImage;
import Face.Person.DeletePerson;
import Face.Person.Parameter.AddPersonAndImage_Parameter;
import Face.Person.Parameter.DeletePerson_Parameter;
import Face.System.BeginWatch;
import Face.System.SendConnectTestResponse;
import io.netty.buffer.ByteBuf;
import io.netty.buffer.ByteBufUtil;
import java.io.DataInputStream;
import java.io.FileInputStream;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.HashMap;
import java.util.Map;
import java.util.concurrent.Semaphore;

public class TcpServerTest implements INConnectorEvent ,Runnable  {

    ConnectorAllocator _Allocator;

    Map<String,CommandDetail> map = new HashMap();

    public TcpServerTest() {
        _Allocator = FaceConnectorAllocator.GetAllocator();
        _Allocator.AddListener(this);
        _Allocator.Listen(9001);//绑定TCP server 端口
    }

    @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof BeginWatch) {
            System.out.println("打开监控成功");
        }

        if (cmd instanceof DeletePerson) {
            System.out.println("删除人员成功");
        }
    }

    @Override
    public void CommandProcessEvent(INCommand cmd) {

    }

    @Override
    public void ConnectorErrorEvent(INCommand cmd, boolean isStop) {

    }

    @Override
    public void ConnectorErrorEvent(ConnectorDetail detail) {

    }

    @Override
    public void CommandTimeout(INCommand cmd) {

    }

    @Override
    public void PasswordErrorEvent(INCommand cmd) {

    }

    @Override
    public void ChecksumErrorEvent(INCommand cmd) {

    }

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
            ArrayList<INPacketModel> oRetPack = new ArrayList<>(10);
            if (decompile.Decompile(dBuf, oRetPack)) {

                Door8800PacketModel m = (Door8800PacketModel) oRetPack.get(0);
                String sn=m.GetSN();
                CommandDetail commandDetail = new CommandDetail();
                commandDetail.Connector = detail;
                commandDetail.Identity = new Door8800Identity(sn, Long.toHexString(m.GetPassword()), E_ControllerType.Face_Fingerprint);
                //添加人脸解析器
                _Allocator.AddWatchDecompile(cd, PacketDecompileAllocator.GetDecompile(E_ControllerType.Face_Fingerprint));
                 beginWatch(commandDetail);
                 if(map.containsKey(sn)){
                    map.replace(sn,commandDetail);
                 }else {
                     map.put(sn,commandDetail);
                 }
                System.out.println("添加人脸解析器");
                System.out.println("客户端ID:" + cd.ClientID + "(" + sn + ")，收到数据包：" + ByteBufUtil.hexDump(dBuf));
            }
        }
    }

    public void deletePerson(CommandDetail commandDetail) {
        ArrayList<Integer> userCodeList = new ArrayList<Integer>();
        for (int i = 110; i < 500; i++) {
            userCodeList.add(i);
        }
        DeletePerson_Parameter parameter = new DeletePerson_Parameter(commandDetail, userCodeList);
        DeletePerson cmd = new DeletePerson(parameter);
        _Allocator.AddCommand(cmd);
    }

    public void addPersonAndImage(CommandDetail commandDetail) {
        // for (int i=0;i<10;i++) {
        IdentificationData[] data = new IdentificationData[1];
        Person person = new Person();
        person.PName = "陈" + 1;
        person.UserCode = 1000;
        byte[] data1 = ReadImage("D:\\图片\\103 - 副本.jpg");

        data[0] = new IdentificationData(1, 1, data1);
        // data[1] = new IdentificationData(2, 2, data2);
        AddPersonAndImage_Parameter parameter = new AddPersonAndImage_Parameter(commandDetail, person, data);
        AddPersonAndImage cmd = new AddPersonAndImage(parameter);
        _Allocator.AddCommand(cmd);
        // }
    }

    private byte[] ReadImage(String path) {
        try {
            DataInputStream stream = new DataInputStream(new FileInputStream(path));
            byte[] file = new byte[stream.available()];
            stream.read(file);
            return file;

        } catch (Exception ex) {

        }
        return null;
    }

    //非排序区
    /*  public void UploadCardList(CommandDetail commandDetail){

        //创建卡片列表对象
        ArrayList<CardDetail> mCardList = new ArrayList<CardDetail>();
        //创建卡片
       Door.Access.Door89H.Command.Data.  CardDetail cd = new Door.Access.Door89H.Command.Data.CardDetail();//设置卡号
        try{
            cd.SetCardData("2345678");
        }catch(Exception ex){
        }
        cd.Password = "";//设置卡密码
        Calendar CardExpiry = Calendar.getInstance();
        CardExpiry.set(2021,10-1, 28,10,10,10);//设置有效时间
        cd.Expiry = CardExpiry;
        cd.OpenTimes = 5;//开门时长
        cd.CardStatus = 0;//卡状态
        cd.SetDoor(1, true);//赋予开门权限
        cd.SetDoor(2,true);
        cd.SetNormal();//设定卡片没有特权
        cd.HolidayUse = true;//设定受节假日限制。
        mCardList.add(cd);
        WriteCardListBySequence_Parameter par = new WriteCardListBySequence_Parameter(commandDetail, mCardList);
        WriteCardListBySequence cmd = new WriteCardListBySequence(par);
        //添加命令到队列
        _Allocator.AddCommand(cmd);
    } */
    public void beginWatch(CommandDetail commandDetail) {

        CommandParameter parameter = new CommandParameter(commandDetail);
        BeginWatch cmd = new BeginWatch(parameter);
        _Allocator.AddCommand(cmd);
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
            //   AbstractTransaction tr = (AbstractTransaction) watchEvent.EventData;
            System.out.println(watchEvent.SN + " 收到监控事件：" + watchEvent.EventData.getClass().toString());
            switch (watchEvent.CmdIndex) {
                case 1://认证记录
                    Face.Data.CardTransaction card = (Face.Data.CardTransaction) watchEvent.EventData;
                    System.out.println(card);
                    break;
                case 2://门磁记录
                    DoorSensorTransaction DoorSensor = (DoorSensorTransaction) watchEvent.EventData;
                    System.out.println(DoorSensor);
                    break;
                case 3://系统记录
                    SystemTransaction system = (SystemTransaction) watchEvent.EventData;
                    System.out.println(system);
                    break;
                case 4://体温记录
                    BodyTemperatureTransaction bodyTemp = (BodyTemperatureTransaction) watchEvent.EventData;
                    System.out.println(bodyTemp);
                    break;
                default:
                    DefinedTransaction dt = (DefinedTransaction) watchEvent.EventData;
            }
            if (watchEvent.CmdIndex == 0xA0) {
                //"保活包消息，远端信息：0x22
                //"连接测试消息，远端信息：0xA0
                CommandDetail commandDetail = new CommandDetail();
                /**
                 * 此函数超时时间设定长一些
                 */
                commandDetail.Timeout = 8000;

                commandDetail.Connector = detail;
                /**
                 * 设置SN(16位字符)，密码(8位十六进制字符)，设备类型
                 */
                Door8800Identity idt = new Door8800Identity(watchEvent.SN, "FFFFFFFF", E_ControllerType.Door8900);
                commandDetail.Identity = idt;

                SendConnectTestResponse cmd = new SendConnectTestResponse(new CommandParameter(commandDetail));
                _Allocator.AddCommand(cmd);
                System.out.println("测试响应");
                // deletePerson(commandDetail);
            }
        } else {
            System.out.println("testio.FCardIO.FCardIOTest.WatchEvent() -- 未知消息");
        }
    }

    @Override
    public void ClientOnline(ConnectorDetail client) {
        // client.
        TCPServerClientDetail tClient = (TCPServerClientDetail) client;
        System.out.println("有客户端上线：" + tClient.Remote.toString() + "，客户端ID：" + tClient.ClientID);
    }

    @Override
    public void ClientOffline(ConnectorDetail client) {
        TCPServerClientDetail tClient = (TCPServerClientDetail) client;
        System.out.println("有客户端离线：" + tClient.Remote.toString() + "，客户端ID：" + tClient.ClientID);
    }

    @Override
    public void run() {

    }
}
