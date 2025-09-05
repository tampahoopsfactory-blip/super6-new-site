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
import Door.Access.Door8800.Command.Data.E_WeekDay;
import Door.Access.Door8800.Command.Data.TimeGroup.DayTimeGroup;
import Door.Access.Door8800.Command.Data.TimeGroup.TimeSegment;
import Door.Access.Door8800.Command.Data.TimeGroup.WeekTimeGroup;
import Door.Access.Door8800.Command.TimeGroup.AddTimeGroup;
import Door.Access.Door8800.Command.TimeGroup.Parameter.AddTimeGroup_Parameter;
import Door.Access.Door8800.Door8800Identity;
import java.util.ArrayList;
import java.util.concurrent.Semaphore;

/**
 *
 * @author F
 */
public class GroupTime implements INConnectorEvent {

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

    public GroupTime() {
        _Allocator = ConnectorAllocator.GetAllocator();
        _Allocator.AddListener(this);
    }

    public void addGroupTime() {
        CommandDetail commandDetail = new CommandDetail();
        commandDetail.Timeout = 8000;//此函数超时时间设定长一些
        TCPClientDetail tcpClientDetail = new TCPClientDetail("192.168.1.65", 8000);//IP  ， 端口(默认8000)

        commandDetail.Connector = tcpClientDetail;
        commandDetail.Identity = new Door8800Identity("FC-8940H48120001", "FFFFFFFF", E_ControllerType.Door8900);//设置SN(16位字符)，密码(8位十六进制字符)，设备类型
        ArrayList<WeekTimeGroup> list = new ArrayList<WeekTimeGroup>();
        for (int i = 0; i < 64; i++) {//64个时段
            WeekTimeGroup group = new WeekTimeGroup(8);//每个时段里有7天和8个时间点
            DayTimeGroup dayTimeGroup=  group.GetItem(E_WeekDay.Monday);//获取星期一的时间点
            dayTimeGroup.GetItem(7);//获取第7个时间点
            TimeSegment ts=   new TimeSegment();//具体时段
            ts.SetBeginTime(0, 0);
            ts.SetEndTime(23, 59);
            dayTimeGroup.SetItem(ts, 1);
            group.SetIndex(i);//当前时段的索引
            list.add(group);
        }
        AddTimeGroup_Parameter par = new AddTimeGroup_Parameter(commandDetail, list);
        AddTimeGroup cmd = new AddTimeGroup(par);

    }

    @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
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
