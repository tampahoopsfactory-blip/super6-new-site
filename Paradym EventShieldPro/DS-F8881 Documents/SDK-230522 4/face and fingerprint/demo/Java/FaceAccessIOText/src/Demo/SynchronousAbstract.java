/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Demo;

import Door.Access.Command.INCommand;
import Door.Access.Command.INCommandResult;
import Door.Access.Connector.ConnectorDetail;
import Door.Access.Connector.INConnectorEvent;
import Door.Access.Connector.TCPServer.TCPServerClientDetail;
import Door.Access.Data.INData;
import java.util.concurrent.Semaphore;

/**
 *
 * @author F
 */
public abstract class SynchronousAbstract implements INConnectorEvent {
    
     private final Semaphore available = new Semaphore(0, true);
    //等待
    public void syn() {
        try {
            available.acquire();
        } catch (Exception e) {
        }
    }
    //关闭等待
    public void release() {
        available.release();
    }
    
    @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
       
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

    @Override
    public void ClientOnline(ConnectorDetail client) {
        
    }

    @Override
    public void ClientOffline(ConnectorDetail client) {
       
    }
}
