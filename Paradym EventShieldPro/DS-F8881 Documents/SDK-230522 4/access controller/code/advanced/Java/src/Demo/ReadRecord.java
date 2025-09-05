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
import Door.Access.Data.INData;
import Door.Access.Door8800.Command.Card.ReadCardDatabaseDetail;
import Door.Access.Door89H.Command.Data.CardTransaction;
import Door.Access.Door8800.Command.Transaction.Parameter.ReadTransactionDatabase_Parameter;
import Door.Access.Door89H.Command.Transaction.ReadTransactionDatabase;
import Door.Access.Door8800.Command.Transaction.Result.ReadTransactionDatabase_Result;
import Door.Access.Door8800.Command.Transaction.e_TransactionDatabaseType;
import Door.Access.Door8800.Door8800Identity;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.Semaphore;

/**
 *
 * @author F
 */
public class ReadRecord implements INConnectorEvent {

    private ConnectorAllocator _Allocator;
private ConcurrentHashMap<String, CommandResultCallback> CommandResult;

    private interface CommandResultCallback {

        public void ResultToLog(StringBuilder strBuf, INCommandResult result);
    }  
    private boolean mIsClose;
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
    public ReadRecord(ConnectorAllocator global) {
        if (global != null) {
            _Allocator = global;
        } else {
            System.out.println("命令对象不能为空");
            return;
        }
        //添加监听
        _Allocator.AddListener(this);
    }

    public void Read() {
        CommandDetail commandDetail = new CommandDetail();
        commandDetail.Timeout = 8000;//此函数超时时间设定长一些
        TCPClientDetail tcpClientDetail = new TCPClientDetail("192.168.1.65", 8000);//IP地址，端口(默认8000)
        commandDetail.Connector = tcpClientDetail;
        commandDetail.Identity = new Door8800Identity("FC-8940H48120001", "FFFFFFFF", E_ControllerType.Door8900);//设置SN(16位字符)，密码(8位十六进制字符)，设备类型
        ReadTransactionDatabase_Parameter parameter = new ReadTransactionDatabase_Parameter(commandDetail, e_TransactionDatabaseType.OnCardTransaction);
        ReadTransactionDatabase cmd = new ReadTransactionDatabase(parameter);
        _Allocator.AddCommand(cmd);
    }

    public void ReadPasswordDetail() {
        CommandDetail commandDetail = new CommandDetail();
        commandDetail.Timeout = 8000;//此函数超时时间设定长一些
        TCPClientDetail tcpClientDetail = new TCPClientDetail("192.168.1.216", 8000);//IP地址，端口(默认8000)
        commandDetail.Connector = tcpClientDetail;
        commandDetail.Identity = new Door8800Identity("FC-8940H47124309", "FFFFFFFF", E_ControllerType.Door8900);//设置SN(16位字符)，密码(8位十六进制字符)，设备类型
        CommandParameter parameter = new CommandParameter(commandDetail);
        ReadCardDatabaseDetail cmd = new ReadCardDatabaseDetail(parameter);
        _Allocator.AddCommand(cmd);
    }

    @Override
    public void CommandCompleteEvent(INCommand inc, INCommandResult incr) {
        if (incr instanceof ReadTransactionDatabase_Result) {
            ReadTransactionDatabase_Result result = (ReadTransactionDatabase_Result) incr;
            for (int i = 0; i < result.TransactionList.size(); i++) {
                CardTransaction cardTransaction = (CardTransaction) result.TransactionList.get(i);
                System.out.println("卡号/密码:" + cardTransaction.CardData);
                System.out.println("门号:" + cardTransaction.DoorNum());
                System.out.println("读卡器号:" + cardTransaction.Reader);
                System.out.println("进出方向:" + cardTransaction.IsEnter());
                System.out.println("出入时间:" + cardTransaction.TransactionDate().getTime());
            }
        }
    }

    @Override
    public void CommandProcessEvent(INCommand inc) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void ConnectorErrorEvent(INCommand inc, boolean bln) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void ConnectorErrorEvent(ConnectorDetail cd) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void CommandTimeout(INCommand inc) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void PasswordErrorEvent(INCommand inc) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void ChecksumErrorEvent(INCommand inc) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void WatchEvent(ConnectorDetail cd, INData indata) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void ClientOnline(TCPServerClientDetail tcpscd) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void ClientOffline(TCPServerClientDetail tcpscd) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

}
