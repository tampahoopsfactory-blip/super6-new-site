package Face;

import Door.Access.Connector.ConnectorAllocator;
import Door.Access.Connector.E_ControllerType;
import Door.Access.Packet.PacketDecompileAllocator;
import Face.Data.FaceCommandWatchResponse;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
/**
 *
 * @author FCARD
 */
public class FaceConnectorAllocator {

    static boolean IsAddDecompile = false;

    /**
     * 获取人脸分配器
     *
     * @return
     */
    public static ConnectorAllocator GetAllocator() {
        if (IsAddDecompile == false) {
            PacketDecompileAllocator.AddDecompile(E_ControllerType.Face_Fingerprint, FaceCommandWatchResponse.class); //增加解析器
            IsAddDecompile = true;
        }
        return ConnectorAllocator.GetAllocator();
    }
}
