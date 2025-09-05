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
import Door.Access.Connector.UDP.UDPDetail;
import Door.Access.Data.INData;
import Door.Access.Door8800.Door8800Identity;
import Face.System.Parameter.WriteFaceBodyTemperaturePar_Parameter;
import Face.System.Parameter.WriteFaceMouthmufflePar_Parameter;
import Face.System.ReadFaceBodyTemperaturePar;
import Face.System.ReadFaceMouthmufflePar;
import Face.System.Result.ReadFaceBodyTemperaturePar_Result;
import Face.System.Result.ReadFaceMouthmufflePar_Result;
import Face.System.WriteFaceBodyTemperaturePar;
import Face.System.WriteFaceMouthmufflePar;
import java.util.concurrent.Semaphore;

/**
 *
 * @author FCARD
 */
public class SystemTest implements INConnectorEvent {

    String LocalIp = "192.168.1.130"; //当前主机IP
    int LocalPort = 8801; //当前主机监听端口
    ConnectorAllocator _Allocator;
    private final Semaphore available = new Semaphore(0, true);

    public void syn() {
        try {
            available.acquire();
        } catch (Exception e) {
        }

    }
//关闭等待

    public void release() {
        //  _Allocator.Release();
        available.release();
    }

    public SystemTest() {
        _Allocator = ConnectorAllocator.GetAllocator();
        _Allocator.AddListener(this);
        _Allocator.UDPBind(LocalIp, LocalPort);

    }

    private CommandDetail getDetail() {
        CommandDetail commandDetail = new CommandDetail();
        /**
         * 此函数超时时间设定长一些
         */
        commandDetail.Timeout = 8000;
        //人脸机的目标IP和UDP端口，默认端口是8101
        UDPDetail cDetail = new UDPDetail("192.168.1.150", 8686, LocalIp, LocalPort);
        cDetail.Timeout = 20000;
        cDetail.RestartCount = 0;
        commandDetail.Connector = cDetail;
        /**
         * 设置SN(16位字符)，密码(8位十六进制字符)，设备类型
         */
        Door8800Identity idt = new Door8800Identity("FC-8300T22014009", "FFFFFFFF", E_ControllerType.Face_Fingerprint);
        commandDetail.Identity = idt;
        return commandDetail;
    }

    /**
     * 读取口罩识别开关
     */
    public void readFaceMouthmufflePar() {
        CommandDetail cmDetail = getDetail();
        ReadFaceMouthmufflePar cmd = new ReadFaceMouthmufflePar(new CommandParameter(cmDetail));
        _Allocator.AddCommand(cmd);
    }

    /**
     * 设置口罩识别开关
     */
    public void writeFaceMouthmufflePar() {
        CommandDetail cmDetail = getDetail();
        //口罩识别开关 0--禁止；1--启用
        WriteFaceMouthmufflePar_Parameter parameter = new WriteFaceMouthmufflePar_Parameter(cmDetail, 1);
        WriteFaceMouthmufflePar cmd = new WriteFaceMouthmufflePar(parameter);
        _Allocator.AddCommand(cmd);
    }

    /**
     * 设置体温开关及体温格式
     */
    public void writeFaceBodyTemperaturePar() {
        CommandDetail cmDetail = getDetail();
        //体温检测开关及格式 0--禁止；1--摄氏度（默认值）；2--华氏度
        WriteFaceBodyTemperaturePar_Parameter parameter = new WriteFaceBodyTemperaturePar_Parameter(cmDetail, 1);
        WriteFaceBodyTemperaturePar cmd = new WriteFaceBodyTemperaturePar(parameter);
          _Allocator.AddCommand(cmd);
    }

    /**
     * 读取体温开关及体温格式
     */
    public void readFaceBodyTemperaturePar() {
        CommandDetail cmDetail = getDetail();
        ReadFaceBodyTemperaturePar cmd = new ReadFaceBodyTemperaturePar(new CommandParameter(cmDetail));
        _Allocator.AddCommand(cmd);
    }

    @Override
    public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
        if (cmd instanceof ReadFaceMouthmufflePar) {
            ReadFaceMouthmufflePar_Result mResult = (ReadFaceMouthmufflePar_Result) result;
            String[] mouthmufflePar = new String[]{"禁止", "启用"};
            System.out.println("口罩识别开关状态:" + mouthmufflePar[mResult.Mouthmuffle]);
        }
        if (cmd instanceof WriteFaceMouthmufflePar) {

            System.out.println("设置口罩识别开关成功");
        }

        if (cmd instanceof ReadFaceBodyTemperaturePar) {
            ReadFaceBodyTemperaturePar_Result mResult = (ReadFaceBodyTemperaturePar_Result) result;
            String[] bodyTemperaturePars = new String[]{"禁止", "摄氏度（默认值）", "华氏度"};
            System.out.println("体温检测开关及体温格式:" + bodyTemperaturePars[mResult.BodyTemperaturePar]);
        }
        if (cmd instanceof WriteFaceBodyTemperaturePar) {
            System.out.println("设置体温检测开关及体温格式成功");
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

    @Override
    public void ClientOnline(ConnectorDetail client) {

    }

    @Override
    public void ClientOffline(ConnectorDetail client) {

    }

}
