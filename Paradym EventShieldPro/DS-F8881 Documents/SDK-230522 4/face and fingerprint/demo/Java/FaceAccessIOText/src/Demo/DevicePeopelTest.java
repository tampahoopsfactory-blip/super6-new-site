/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
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
import Face.Data.FaceCommandWatchResponse;
import Face.Door.OpenDoor;
import Face.System.SendConnectTestResponse;
import io.netty.buffer.ByteBuf;
import io.netty.buffer.ByteBufUtil;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;
import java.util.concurrent.Semaphore;

/**
 *
 * @author FCARD
 */
public class DevicePeopelTest implements INConnectorEvent {

    ConnectorAllocator _Allocator;
    private final Semaphore available = new Semaphore(0, true);
    ///等待
    HashMap<Integer, ConnectorDetail> TcpClinetMap = new HashMap<Integer, ConnectorDetail>();

    HashMap<String, Integer> DeviceTcpMap = new HashMap<>();

    private CommandDetail getDetail(String sn) {

        int clientId = 0;
        if (DeviceTcpMap.containsKey(sn)) {
            clientId = DeviceTcpMap.get(sn);           
        }
        ConnectorDetail connDtl = null;
        if (clientId != 0 && TcpClinetMap.containsKey(clientId)) {
            connDtl = TcpClinetMap.get(clientId);
        }
        if(connDtl==null){
            return  null;
        }
        CommandDetail commandDetail = new CommandDetail();
        /**
         * 此函数超时时间设定长一些
         */
        commandDetail.Timeout = 8000;
        //  UDPDetail cDetail = new UDPDetail("192.168.1.174", 8686, LocalIp, LocalPort);
        commandDetail.Connector = connDtl;
        /**
         * 设置SN(16位字符)，密码(8位十六进制字符)，设备类型
         */
        Door8800Identity idt = new Door8800Identity(sn, "FFFFFFFF", E_ControllerType.Face_Fingerprint);
        commandDetail.Identity = idt;
        return commandDetail;
    }

    public void openDoor(String sn){
        CommandDetail cmmDtl=getDetail(sn);
        if(cmmDtl==null){
               System.out.println("沒有找到該設備");
            return;
        }
        OpenDoor cmd=new OpenDoor(new CommandParameter(cmmDtl));
        _Allocator.AddCommand(cmd);
    }
    
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

    public DevicePeopelTest() {
        _Allocator = ConnectorAllocator.GetAllocator();
        _Allocator.AddListener(this);
        _Allocator.Listen(8100);//绑定TCP server 端口
    }

    @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if(cmd instanceof OpenDoor ){
            System.out.println("開門成功");
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
                Door8800Identity idt = new Door8800Identity(watchEvent.SN, "FFFFFFFF", E_ControllerType.Door8900);
                commandDetail.Identity = idt;

                SendConnectTestResponse cmd = new SendConnectTestResponse(new CommandParameter(commandDetail));
                _Allocator.AddCommand(cmd);
                System.out.println("测试响应");
            }
        } else {
            System.out.println("testio.FCardIO.FCardIOTest.WatchEvent() -- 未知消息");
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
                String Sn = m.GetSN();
                CommandDetail commandDetail = new CommandDetail();
                commandDetail.Connector = detail;
                commandDetail.Identity = new Door8800Identity(Sn, Long.toHexString(m.GetPassword()), E_ControllerType.Door8900);
                if (m.GetSN().contains("FC-8940H")) {
                    _Allocator.AddWatchDecompile(cd, PacketDecompileAllocator.GetDecompile(E_ControllerType.Door8900));
                    System.out.println("添加控制板解析器");
                    // UploadCardList(commandDetail);
                } else {
                    //添加人脸解析器
                    _Allocator.AddWatchDecompile(cd, PacketDecompileAllocator.GetDecompile(E_ControllerType.Face_Fingerprint));
                    DeviceTcpMap.put(Sn, cd.ClientID);
                    // beginWatch(commandDetail);
                    System.out.println("添加人脸解析器");
                    // deletePerson(commandDetail);
                }
                // m.GetSN() -- 这个就是此控制器的SN号，
                System.out.println("客户端ID:" + cd.ClientID + "(" + m.GetSN() + ")，收到数据包：" + ByteBufUtil.hexDump(dBuf));
            }
        }
    }

    @Override
    public void ClientOnline(ConnectorDetail client) {
        //保存連接上來的設備
        if (client instanceof TCPServerClientDetail) {
            TCPServerClientDetail tcpServerClient = (TCPServerClientDetail) client;
            if (!TcpClinetMap.containsKey(tcpServerClient.ClientID)) {
                TcpClinetMap.put(tcpServerClient.ClientID, tcpServerClient);
            }
        }
    }

    @Override
    public void ClientOffline(ConnectorDetail client) {
        //設備離線，將它移除
        if (client instanceof TCPServerClientDetail) {
            TCPServerClientDetail tcpServerClient = (TCPServerClientDetail) client;
            if (TcpClinetMap.containsKey(tcpServerClient.ClientID)) {
                TcpClinetMap.remove(tcpServerClient.ClientID);
            }
        }
    }

}
