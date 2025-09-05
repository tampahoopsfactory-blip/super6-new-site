/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Demo;

import Door.Access.Connector.ConnectorAllocator;
import Door.Access.Util.ByteUtil;
import Door.Access.Util.StringUtil;
import com.sun.org.apache.xalan.internal.xsltc.dom.BitArray;
import java.io.File;

import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.lang.reflect.Array;
import java.net.InetAddress;
import java.net.ServerSocket;
import java.net.Socket;
import java.nio.file.Files;
import java.security.KeyFactory;
import java.security.interfaces.RSAPublicKey;
import java.security.spec.X509EncodedKeySpec;
import java.util.Arrays;
import java.util.BitSet;
import java.util.Calendar;
import java.util.GregorianCalendar;
import java.util.Scanner;

import javax.crypto.Cipher;

/**
 * @author F
 */
public class Main1 {

    public static void main(String[] args) throws IOException {
      //  long crc=ByteUtil.CreateCRC32(new byte[100],50, 50);
       // System.out.println(crc);

      //  byte[] b=new byte[]{1,2,3,4,5,6};
   //     crc32();
       //SystemTest test=new SystemTest();
   //    test.writeFaceMouthmufflePar();
   //    test.readFaceMouthmufflePar();
   //    test.writeFaceBodyTemperaturePar();
   //    test.readFaceBodyTemperaturePar();
 //     test.syn();
     // DoorTest test=new DoorTest();
    //  test.ReadVersion();
    //  test.syn();
/*   ConnectorAllocator   _Allocator = ConnectorAllocator.GetAllocator();
      _Allocator.UDPBind("192.168.1.130", 8801);

        Scanner input=new Scanner(System.in);
        while (true) {  
            String inputString= input.next();         
            test.OpenDoor();
        }*/
       // DemoTest test = new DemoTest();

      //  test.addPersonAndImage();
       // test.syn();
       // test.readTransactionDatabase();
       // test.writeTransactionDatabaseReadIndex();
        //test.addPerson();
        //test.deletePerson();
        //  test.HoldDoor();
        //  test.UnlockDoor();
        
        // test.WriteReaderOption();
        // test.ReadReaderOption();
        //test.writeRelayOption();
        // test.readRelayOption();
        //test.writeUnlockingTime();
        //  test.readUnlockingTime();

      //  test.readExemptionVerificationOpen();
       // test.writeExemptionVerificationOpen();
       // test.readVoiceBroadcastSetting();
     //   test.writeVoiceBroadcastSetting();
        //test.readReaderIntervalTime();
      //  test.writeReaderIntervalTime();
        //test.readExpirationPrompt();
      //  test.addPerson();
     //  DoorTest doorTest=new DoorTest();
      // doorTest.beginWatch();
      //  doorTest.syn();
       // doorTest.readTransactionDatabase();
      //  doorTest.HoldDoor();
   /*     String publicKey="MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArfOzx+LjKF+BZKcbriPu" +
                "rWwmlrif+fQvZyqQq6hG8SWZRE52Ahp++3Fem79XdAu3U5jumvOeEKAfXMCClsfV" +
                "G9EqhLNSVA7Xb8zgnVelSHMPg9r2LX73nPSK28r3SoHAAuVNrva8f94koCYV8zym" +
                "I6W3duhDL/bfQDUkFS3MJcUb8bQcaxupKPLkxImBYGAjI3ceSMi984CFCcS8D6yU" +
                "tWGnxqy/nZVrfws7eI72FSpa2JaWkp7Bqm27bAMnirMx27rRN9uatHLjGBS60yrO" +
                "kZ1UJDkffi9tyOEIaEbNvUJWMH9rSAqiMpWH9Qdo9Vre4heMwNaxcFheYfty/o8Q" +
                "aQIDAQAB";
        try {
            System.out.println(encrypt("Fcard,system,0000",publicKey));
        } catch (Exception e) {
            e.printStackTrace();
        }*/

  /* byte b = 53;
    byte[] array = new byte[8];
    for (int i = 7; i >= 0; i--)
    { 
        array[i] = (byte)(b & 1);
        b >>= 1;
    }
        System.out.println(Arrays.toString(array));
        
        String bits =  Integer.toBinaryString(b);
        System.out.println(bits);
        System.out.println(Integer.parseInt(bits,2));*/

        ElevatorDemo  test=new ElevatorDemo();
       test.WritePersonElevatorAccessTest();
      //  test.ReadPersonElevatorAccessTest();

       
    }
    public  static void crc32() throws IOException{
        File faceFile=new File("D:\\Fcard软件\\FC指纹人脸调试器\\tmpPhoto.jpg");
        byte[] faceData= Files.readAllBytes(faceFile.toPath());
        long lcrc32=ByteUtil.CreateCRC32(faceData,0,faceData.length);
        System.out.println(Long.toHexString(lcrc32)); 
    }
/*    public static String encrypt( String str, String publicKey ) throws Exception{
        //base64编码的公钥
        byte[] decoded = Base64.decodeBase64(publicKey);
        RSAPublicKey pubKey = (RSAPublicKey) KeyFactory.getInstance("RSA").generatePublic(new X509EncodedKeySpec(decoded));
        //RSA加密
        Cipher cipher = Cipher.getInstance("RSA");
        cipher.init(Cipher.ENCRYPT_MODE, pubKey);
        String outStr = Base64.encodeBase64String(cipher.doFinal(str.getBytes("UTF-8")));
        return outStr;
    }*/
}
