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
import Door.Access.Door8800.Command.Data.Door8800WatchTransaction;
import Door.Access.Door8800.Door8800Identity;
import Door.Access.Door8800.Packet.Door8800Decompile;
import Door.Access.Door8800.Packet.Door8800PacketModel;
import Door.Access.Packet.INPacketModel;
import Door.Access.Packet.PacketDecompileAllocator;
import Face.AdditionalData.Parameter.ReadFeatureCode_Parameter;
import Face.AdditionalData.ReadFeatureCode;
import Face.AdditionalData.Result.ReadFeatureCode_Result;
import Face.Data.FaceCommandWatchResponse;
import Face.FaceConnectorAllocator;
import Face.Person.DeletePerson;
import Face.Person.Parameter.DeletePerson_Parameter;
import Face.System.BeginWatch;
import Face.System.ReadVersion;
import Face.System.Result.ReadVersion_Result;
import Face.System.SendConnectTestResponse;
import io.netty.buffer.ByteBuf;
import io.netty.buffer.ByteBufUtil;

import java.util.ArrayList;
import java.util.concurrent.Semaphore;

public class UDPServerTest implements INConnectorEvent {

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

    public UDPServerTest() {
        _Allocator = FaceConnectorAllocator.GetAllocator();
        _Allocator.AddListener(this);
        _Allocator.UDPBind("192.168.1.130", 9000);//绑定UDP server 端口
      //  PacketDecompileAllocator.AddDecompile(E_ControllerType.Face_Fingerprint, FaceCommandWatchResponse.class);//增加解析类，人脸指纹SDK需要手动进行添加
        //_Allocator.
        //System.out.print("打印");
    }

    @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (result instanceof ReadVersion_Result) {
            System.out.println("读取版本号完成");
        }
        if (result instanceof ReadFeatureCode_Result) {
            System.out.println("读取指纹特征完成");
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

        ByteBuf dBuf = b.GetBytes();
        if (dBuf.getByte(0) == 0x7e) {
            Door8800Decompile decompile = new Door8800Decompile();
            ArrayList<INPacketModel> oRetPack = new ArrayList<>(10);
            if (decompile.Decompile(dBuf, oRetPack)) {

                Door8800PacketModel m = (Door8800PacketModel) oRetPack.get(0);

               
                    UDPDetail cd = (UDPDetail) detail;
                    //_Allocator.
                    //添加人脸解析器
                    _Allocator.AddWatchDecompile(cd, PacketDecompileAllocator.GetDecompile(E_ControllerType.Face_Fingerprint));
                    if ("192.168.1.143".equals(cd.IP)) {
                        CommandDetail commandDetail = new CommandDetail();
                        commandDetail.Connector = detail;
                        commandDetail.Identity = new Door8800Identity(m.GetSN(), Long.toHexString(m.GetPassword()), E_ControllerType.Face_Fingerprint);
                        readFeatureCode(commandDetail);
                    }

                    // beginWatch(commandDetail);
                    System.out.println("添加人脸解析器");
                    // deletePerson(commandDetail);
                
                // m.GetSN() -- 这个就是此控制器的SN号，
                //  System.out.println("客户端ip:" + cd.ip + "(" + m.GetSN() + ")，收到数据包：" + ByteBufUtil.hexDump(dBuf));
            }
        }
    }

    public void readFeatureCode(CommandDetail commandDetail) {
        ReadFeatureCode cmd = new ReadFeatureCode(new ReadFeatureCode_Parameter(commandDetail, 235, 2, 0));
        _Allocator.AddCommand(cmd);
    }

    public void ReadVersion(CommandDetail commandDetail) {
        CommandParameter par = new CommandParameter(commandDetail);
        ReadVersion cmd = new ReadVersion(par);
        _Allocator.AddCommand(cmd);
    }

    public void deletePerson(CommandDetail commandDetail) {
        ArrayList<Integer> userCodeList = new ArrayList<Integer>();
        userCodeList.add(1);
        //  userCodeList.add(100024);
        DeletePerson_Parameter parameter = new DeletePerson_Parameter(commandDetail, userCodeList);
        DeletePerson cmd = new DeletePerson(parameter);
        _Allocator.AddCommand(cmd);
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
            AbstractTransaction tr = (AbstractTransaction) watchEvent.EventData;
            System.out.println(watchEvent.SN + " 收到监控事件：" + tr.getClass().toString());
//            switch (watchEvent.CmdIndex) {
//                case 1://认证记录
//                    CardTransaction card = (CardTransaction) watchEvent.EventData;
//
//                    break;
//                case 2://门磁记录
//                    DoorSensorTransaction DoorSensor = (DoorSensorTransaction) watchEvent.EventData;
//                    break;
//                case 3://系统记录
//                    SystemTransaction system = (SystemTransaction) watchEvent.EventData;
//                    break;
//                case 4://包活/连接测试记录
//                    System.out.println("保活包消息.................");
//                    break;
//                default:
//                    DefinedTransaction dt = (DefinedTransaction) watchEvent.EventData;
//
//            }
            if (watchEvent.CmdIndex == 0x22 || watchEvent.CmdIndex == 0xA0) {
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
                Door8800Identity idt = new Door8800Identity(watchEvent.SN, "FFFFFFFF", E_ControllerType.Face_Fingerprint);
                commandDetail.Identity = idt;

                SendConnectTestResponse cmd = new SendConnectTestResponse(new CommandParameter(commandDetail));
                _Allocator.AddCommand(cmd);
                System.out.println("测试响应");
            }
        } else {
            System.out.println("testio.FCardIO.FCardIOTest.WatchEvent() -- 未知消息");
        }
    }

    @Override
    public void ClientOnline(ConnectorDetail client) {
        UDPDetail uClient = (UDPDetail) client;
        System.out.println("有客户端上线：" + uClient.IP + "，客户端ID：" + uClient.Port);
    }

    @Override
    public void ClientOffline(ConnectorDetail client) {
        UDPDetail uClient = (UDPDetail) client;
        System.out.println("有客户端离线：" + uClient.IP + "，客户端ID：" + uClient.Port);
    }
}
