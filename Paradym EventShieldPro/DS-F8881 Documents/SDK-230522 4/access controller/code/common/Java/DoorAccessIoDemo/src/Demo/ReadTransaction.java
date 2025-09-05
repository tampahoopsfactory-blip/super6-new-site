/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Demo;

import Door.Access.Command.CommandDetail;
import Door.Access.Command.INCommand;
import Door.Access.Command.INCommandResult;
import Door.Access.Connector.ConnectorAllocator;
import Door.Access.Connector.ConnectorDetail;
import Door.Access.Connector.E_ControllerType;
import Door.Access.Connector.INConnectorEvent;
import Door.Access.Connector.TCPClient.TCPClientDetail;
import Door.Access.Connector.TCPServer.TCPServerClientDetail;
import Door.Access.Data.INData;
import Door.Access.Door8800.Command.Transaction.Parameter.ReadTransactionDatabaseByIndex_Parameter;
import Door.Access.Door8800.Command.Transaction.Result.ReadTransactionDatabaseByIndex_Result;
import Door.Access.Door8800.Command.Transaction.e_TransactionDatabaseType;
import Door.Access.Door8800.Door8800Identity;
import Door.Access.Door89H.Command.Data.CardTransaction;
import Door.Access.Door89H.Command.Transaction.ReadTransactionDatabaseByIndex;
import java.util.concurrent.Semaphore;

/**
 *
 * @author F
 */
public class ReadTransaction implements INConnectorEvent {

    
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
    
     public ReadTransaction() {
        _Allocator = ConnectorAllocator.GetAllocator();
        _Allocator.AddListener(this);
       
    }
     
     public void readTransaction(){
            CommandDetail commandDetail = new CommandDetail();
        commandDetail.Timeout=8000;//此函数超时时间设定长一些
        TCPClientDetail tcpClientDetail = new TCPClientDetail("192.168.1.65",8000);//IP  ， 端口(默认8000)
        
        commandDetail.Connector = tcpClientDetail;
        commandDetail.Identity = new Door8800Identity("FC-8940H48120001","FFFFFFFF",E_ControllerType.Door8900);//设置SN(16位字符)，密码(8位十六进制字符)，设备类型
        ReadTransactionDatabaseByIndex_Parameter parameter=new  ReadTransactionDatabaseByIndex_Parameter(commandDetail,e_TransactionDatabaseType.OnCardTransaction );
        parameter.ReadIndex=0;
        parameter.Quantity=20;
        ReadTransactionDatabaseByIndex cmd=new ReadTransactionDatabaseByIndex(parameter);
        _Allocator.AddCommand(cmd);
     }
    
    @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
       ReadTransactionDatabaseByIndex_Result transaction=(ReadTransactionDatabaseByIndex_Result) result;
        for (int i = 0; i < transaction.TransactionList.size(); i++) {
         CardTransaction card=(CardTransaction)transaction.TransactionList.get(i);
             System.out.println("序号："+ card.SerialNumber);
            System.out.println("卡号："+card.CardData);
            System.out.println("读头："+card.Reader);
            System.out.println("读卡方向："+(card.IsEnter()?"进门读卡":"出门读卡"));
            System.out.println("时间："+card.TransactionDate().getTime());
              System.out.println("类型："+card.TransactionCode());
              
              System.out.println("------------------------");
               System.out.println("------------------------");
        }
    }

    @Override
    public void CommandProcessEvent(INCommand cmd) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void ConnectorErrorEvent(INCommand cmd, boolean isStop) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void ConnectorErrorEvent(ConnectorDetail detail) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void CommandTimeout(INCommand cmd) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void PasswordErrorEvent(INCommand cmd) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void ChecksumErrorEvent(INCommand cmd) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void WatchEvent(ConnectorDetail detail, INData event) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void ClientOnline(TCPServerClientDetail client) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void ClientOffline(TCPServerClientDetail client) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }
    
}
