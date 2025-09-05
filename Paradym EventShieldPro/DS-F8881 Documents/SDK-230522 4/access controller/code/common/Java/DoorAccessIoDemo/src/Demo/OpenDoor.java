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
import Door.Access.Connector.*;
import Door.Access.Connector.TCPClient.TCPClientDetail;
import Door.Access.Connector.TCPServer.TCPServerClientDetail;
import Door.Access.Data.INData;
import Door.Access.Door8800.Command.Data.Door8800WatchTransaction;
import Door.Access.Door8800.Command.Door.Parameter.DoorPort_Parameter;
import Door.Access.Door8800.Command.Door.Parameter.OpenDoor_Parameter;
import Door.Access.Door8800.Command.Door.Parameter.RemoteDoor_Parameter;
import Door.Access.Door8800.Command.Password.ReadPasswordDataBase;
import Door.Access.Door8800.Command.Door.ReadRelayReleaseTime;
import Door.Access.Door8800.Command.Door.Result.ReadRelayReleaseTime_Result;
import Door.Access.Door8800.Command.System.SearchEquptOnNetNum;
import Door.Access.Door8800.Door8800Identity;
import Door.Access.Door89H.Command.Data.PasswordDetail;
import Door.Access.Door89H.Command.Password.DeletePassword;
import Door.Access.Door89H.Command.Password.Parameter.DeletePassword_Parameter;
import Door.Access.Door89H.Command.Password.Parameter.WritePassword_Parameter;
import Door.Access.Door89H.Command.Password.WritePassword;
import java.util.Calendar;
import java.util.concurrent.Semaphore;
/**
 *
 * @author F
 */
public class OpenDoor  implements INConnectorEvent{
    private ConnectorAllocator _Allocator;
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
    public OpenDoor(ConnectorAllocator global) {
        if (global != null) {
            _Allocator = global;
        } else {
            System.out.println("命令对象不能为空");
            return;
        }
        //添加监听
        _Allocator.AddListener(this);
       

    }
    
    public  void readPasswordDataBase(){
          CommandDetail commandDetail = new CommandDetail();
        TCPClientDetail tcpClientDetail = new TCPClientDetail("192.168.1.59", 8000);//IP地址，端口(默认8000)
        commandDetail.Connector = tcpClientDetail;
        commandDetail.Identity = new Door8800Identity("AB-5824T25070244", "FFFFFFFF", E_ControllerType.Door8800);//设置SN(16位字符)，密码(8位十六进制字符)，设备类型
        CommandParameter par=new CommandParameter(commandDetail);
        ReadPasswordDataBase cmd=new ReadPasswordDataBase(par);
        _Allocator.AddCommand(cmd);
    }
    public void openDoor(int doorNum){
         //定义控制器连接信息
        CommandDetail commandDetail = new CommandDetail();
        TCPClientDetail tcpClientDetail = new TCPClientDetail("192.168.3.250", 8000);//IP地址，端口(默认8000)
        commandDetail.Connector = tcpClientDetail;
        commandDetail.Identity = new Door8800Identity("MC-5824T20044095", "FFFFFFFF", E_ControllerType.Door8800);//设置SN(16位字符)，密码(8位十六进制字符)，设备类型
        //定义命令参数
        OpenDoor_Parameter openDoor_parameter = new OpenDoor_Parameter(commandDetail);//初始化开门参数
        openDoor_parameter.Door.SetDoor(doorNum, 1);//设定1号门执行操作
       // openDoor_parameter.Door.SetDoor(2, 1);//设定2号门不执行操作
        Door.Access.Door8800.Command.Door.OpenDoor openDoor = new Door.Access.Door8800.Command.Door.OpenDoor(openDoor_parameter);
        //添加命令到队列
        _Allocator.AddCommand(openDoor);
    }

    public void CloseDoor() {
        CommandDetail commandDetail = new CommandDetail();
        TCPClientDetail tcpClientDetail = new TCPClientDetail("192.168.1.59", 8000);//IP地址，端口(默认8000)
        //定义控制器连接信息
        commandDetail.Connector = tcpClientDetail;
        commandDetail.Identity = new Door8800Identity("AB-5824T25070244", "FFFFFFFF", E_ControllerType.Door8800);//设置SN(16位字符)，密码(8位十六进制字符)，设备类型
        //定义命令参数
        RemoteDoor_Parameter par = new RemoteDoor_Parameter(commandDetail);//初始化开门参数
        par.Door.SetDoor(1, 1);//设定1号门执行操作
        par.Door.SetDoor(2, 1);//设定2号门不执行操作
        Door.Access.Door8800.Command.Door.CloseDoor closeDoor = new Door.Access.Door8800.Command.Door.CloseDoor(par);
        //添加命令到队列
        _Allocator.AddCommand(closeDoor);
    }

    public void ReleaseOD() {
        //删除监听
        _Allocator.DeleteListener(this);
        _Allocator = null;
    }
    public void readRelayReleaseTime (){
         CommandDetail commandDetail = new CommandDetail();
        TCPClientDetail tcpClientDetail = new TCPClientDetail("192.168.1.216", 8000);//IP地址，端口(默认8000)
        commandDetail.Connector = tcpClientDetail;
        commandDetail.Identity = new Door8800Identity("FC-8940H47124309", "FFFFFFFF", E_ControllerType.Door8800);//设置SN(16位字符)，密码(8位十六进制字符)，设备类型
        DoorPort_Parameter par=new DoorPort_Parameter (commandDetail,1);
        ReadRelayReleaseTime cmd =new ReadRelayReleaseTime(par);
        _Allocator.AddCommand(cmd);
    }
    
    public void deletePassword() {
        CommandDetail commandDetail = new CommandDetail();
        commandDetail.Timeout = 2000;
        TCPClientDetail tcpClientDetail = new TCPClientDetail("192.168.1.216", 8000);//IP地址，端口(默认8000)
        commandDetail.Connector = tcpClientDetail;
        commandDetail.Identity = new Door8800Identity("FC-8940H47124309", "FFFFFFFF", E_ControllerType.Door8900);//设置SN(16位字符)，密码(8位十六进制字符)，设备类型
        String[] passwords={"14784579"};
        DeletePassword_Parameter par = new DeletePassword_Parameter(commandDetail,passwords );
        DeletePassword cmd = new DeletePassword(par);
        _Allocator.AddCommand(cmd);
    }

        public void addPassword() {
        CommandDetail commandDetail = new CommandDetail();
        commandDetail.Timeout = 5000;
        TCPClientDetail tcpClientDetail = new TCPClientDetail("192.168.1.216", 8000);//IP地址，端口(默认8000)
        commandDetail.Connector = tcpClientDetail;
        commandDetail.Identity = new Door8800Identity("FC-8940H47124309", "FFFFFFFF", E_ControllerType.Door8900);//设置SN(16位字符)，密码(8位十六进制字符)，设备类型
        PasswordDetail detail = new PasswordDetail();
        detail.Password = "14784579";//八位数字密码
        detail.OpenTimes = 65535;//65535表示无限制
        detail.Expiry = Calendar.getInstance();
        detail.Expiry.set(2089, 12 - 1, 31, 23, 59, 59);
        detail.SetDoor(1, false);
        detail.SetDoor(2, true);
        detail.SetDoor(3, true);
        detail.SetDoor(4, true);
        WritePassword_Parameter par = new WritePassword_Parameter(commandDetail, detail);
        WritePassword cmd = new WritePassword(par);
        _Allocator.AddCommand(cmd);
    }

    @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        //throw new UnsupportedOperationException("Not supported yet.");
           System.out.println("执行成功"); //门号  
        if(result instanceof ReadRelayReleaseTime_Result){
                    ReadRelayReleaseTime_Result incr=(ReadRelayReleaseTime_Result)result;
                 System.out.println("门号"+incr.Door); //门号  
                  System.out.println("开门保持时间"+incr.ReleaseTime); //开门保持时间，取值范围：0-65535；0表示0.5秒。单位：秒
        }

    }

    @Override
    public void CommandProcessEvent(INCommand cmd) {
        try {
            StringBuilder strBuf = new StringBuilder(100);
            strBuf.append("<html>");
            strBuf.append("当前命令：");
            strBuf.append("<br/>正在处理： ");
            strBuf.append(cmd.getProcessStep());
            strBuf.append(" / ");
            strBuf.append(cmd.getProcessMax());
            strBuf.append("</html>");
            strBuf = null;
        } catch (Exception e) {
            System.out.println("doorAccessiodemo.frmMain.CommandProcessEvent() -- 发生错误：" + e.toString());
        }

    }

    @Override
    public void ConnectorErrorEvent(INCommand cmd, boolean isStop) {
        try {
            StringBuilder strBuf = new StringBuilder(100);
            if (isStop) {
                strBuf.append("命令已手动停止!");
            } else {
                strBuf.append("网络连接失败!");
            }
            strBuf = null;
        } catch (Exception e) {
            System.out.println("doorAccessiodemo.frmMain.ConnectorErrorEvent() --- " + e.toString());
        }

    }

    @Override
    public void ConnectorErrorEvent(ConnectorDetail detial) {
        try {
            StringBuilder strBuf = new StringBuilder(100);
            strBuf.append("网络通道故障，IP信息：");
            strBuf = null;
        } catch (Exception e) {
            System.out.println("doorAccessiodemo.frmMain.ConnectorErrorEvent() -- " + e.toString());
        }

    }

    @Override
    public void CommandTimeout(INCommand cmd) {
        try {
            if (cmd instanceof SearchEquptOnNetNum) {
                return;
            }
            StringBuilder strBuf = new StringBuilder(100);
            strBuf.append("命令超时，已失败！");
            strBuf = null;
        } catch (Exception e) {
            System.out.println("doorAccessiodemo.frmMain.CommandTimeout() -- " + e.toString());
        }

    }

    @Override
    public void PasswordErrorEvent(INCommand cmd) {
        try {
            StringBuilder strBuf = new StringBuilder(100);
            strBuf.append("通讯密码错误，已失败！");
            strBuf = null;
        } catch (Exception e) {
            System.out.println("doorAccessiodemo.frmMain.PasswordErrorEvent() -- " + e.toString());
        }

    }

    @Override
    public void ChecksumErrorEvent(INCommand cmd) {
        try {
            StringBuilder strBuf = new StringBuilder(100);
            strBuf.append("命令返回的校验和错误，已失败！");
            strBuf = null;
        } catch (Exception e) {
            System.out.println("doorAccessiodemo.frmMain.ChecksumErrorEvent() -- " + e.toString());
        }

    }

    @Override
    public void WatchEvent(ConnectorDetail detial, INData event) {
        try {
            StringBuilder strBuf = new StringBuilder(100);
            strBuf.append("数据监控:");
            if (event instanceof Door8800WatchTransaction) {
                Door8800WatchTransaction WatchTransaction = (Door8800WatchTransaction) event;
                strBuf.append("，SN：");
                strBuf.append(WatchTransaction.SN);
                strBuf.append("\n");
            } else {
                strBuf.append("，未知事件：");
                strBuf.append(event.getClass().getName());
            }
            strBuf = null;
        } catch (Exception e) {
            System.out.println("doorAccessiodemo.frmMain.WatchEvent() -- " + e.toString());
        }
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
