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
import Door.Access.Connector.TCPClient.TCPClientDetail;
import Door.Access.Connector.TCPServer.TCPServerClientDetail;
import Door.Access.Connector.UDP.UDPDetail;
import Door.Access.Data.INData;
import Door.Access.Door8800.Command.System.Parameter.SearchEquptOnNetNum_Parameter;
import Door.Access.Door8800.Command.System.ReadSN;
import Door.Access.Door8800.Command.System.ReadTCPSetting;
import Door.Access.Door8800.Command.System.Result.ReadSN_Result;
import Door.Access.Door8800.Command.System.SearchEquptOnNetNum;
import Door.Access.Door8800.Door8800Identity;
import com.sun.org.apache.xalan.internal.xsltc.dom.BitArray;
import java.util.BitSet;
import java.util.concurrent.Semaphore;

/**
 *
 * @author F
 */
public class ReadSn implements INConnectorEvent {

    private ConnectorAllocator _Allocator;
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

    public ReadSn() {
        _Allocator = ConnectorAllocator.GetAllocator();
        _Allocator.AddListener(this);
      //  _Allocator.UDPBind("192.168.1.140", 8100);
    }

    public void GetSn() {

          CommandDetail commandDetail = new CommandDetail();
          commandDetail.Timeout = 500;//此函数超时时间设定长一些
          commandDetail.RestartCount=0;
           TCPClientDetail tcpClientDetail = new TCPClientDetail("192.168.1.48",8000);//IP  ， 端口(默认8000)
           tcpClientDetail.Timeout=500;
           tcpClientDetail.RestartCount=0;
        //   UDPDetail cDetail = new UDPDetail("255.255.255.255", 996, "192.168.1.140", 8100);
           commandDetail.Connector = tcpClientDetail;

           Door8800Identity idt = new Door8800Identity("FC-8600T20046115", "FFFFFFFF", E_ControllerType.Door8900);//设置SN(16位字符)，密码(8位十六进制字符)，设备类型
            idt.Broadcast = true;
            commandDetail.Identity = idt;
      //      SearchEquptOnNetNum_Parameter par = new SearchEquptOnNetNum_Parameter(commandDetail, getRanom());
     //    SearchEquptOnNetNum cmd = new SearchEquptOnNetNum(par);
          CommandParameter parameter = new CommandParameter(commandDetail);
             ReadTCPSetting cmd = new ReadTCPSetting(parameter);
          _Allocator.AddCommand(cmd);
    }

    public BitSet IntToBitSet(int n) {
        BitSet bSet = new BitSet();
        int index = 0;
        while (n != 0) {
            if (n % 2 == 0) {
                bSet.set(index, false);
            } else {
                bSet.set(index, true);
            }
            n = n / 2;
            index++;
        }
        return bSet;
    }

    public int getRanom() {
        int max = 65535, min = 1;
        return (int) (Math.random() * (max - min) + min);
    }

    @Override
    public void CommandCompleteEvent(INCommand inc, INCommandResult incr) {
        ReadSN_Result result = (ReadSN_Result) incr;
        System.out.println("Demo.ReadSn.CommandCompleteEvent()" + result.SN);

    }

    @Override
    public void CommandProcessEvent(INCommand inc) {
        throw new UnsupportedOperationException("Not supported yet.1"); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void ConnectorErrorEvent(INCommand inc, boolean bln) {
        throw new UnsupportedOperationException("Not supported yet.2"); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void ConnectorErrorEvent(ConnectorDetail cd) {
        throw new UnsupportedOperationException("Not supported yet.3"); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void CommandTimeout(INCommand inc) {
        throw new UnsupportedOperationException("Not supported yet.4"); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void PasswordErrorEvent(INCommand inc) {
        throw new UnsupportedOperationException("Not supported yet.5"); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void ChecksumErrorEvent(INCommand inc) {
        throw new UnsupportedOperationException("Not supported yet.6"); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void WatchEvent(ConnectorDetail cd, INData indata) {
        throw new UnsupportedOperationException("Not supported yet.7"); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void ClientOnline(TCPServerClientDetail tcpscd) {
        throw new UnsupportedOperationException("Not supported yet.8"); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void ClientOffline(TCPServerClientDetail tcpscd) {
        throw new UnsupportedOperationException("Not supported yet.9"); //To change body of generated methods, choose Tools | Templates.
    }

}
