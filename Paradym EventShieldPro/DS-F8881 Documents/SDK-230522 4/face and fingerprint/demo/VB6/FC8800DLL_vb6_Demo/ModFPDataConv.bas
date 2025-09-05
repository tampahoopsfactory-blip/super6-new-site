Attribute VB_Name = "ModFPDataConv"
Option Explicit

'Success return value for the dynamic library
Public Const FPResultOK As Long = 0


'Fingerprint signature, algorithm version number
Public Const DATA_VER_70 As Long = &H70
Public Const DATA_VER_80 As Long = &H80
Public Const DATA_VER_85 As Long = &H85
Public Const DATA_VER_89 As Long = &H89

'Maximum fingerprint buffer length
Public Const MAX_FTR_LEN As Long = 2048


'Fingerprint algorithm conversion dynamic library

'Dynamic library parameter initialization, called every time a transformation is needed
Private Declare Sub FPCONV_Init Lib "FpDataConv.dll" ()


'Get the dynamic library version number
Private Declare Function FPCONV_GetConvDLLModel Lib "FpDataConv.dll" (ByVal strModel As String) As Long



'Check whether the data buffer contains fingerprint data to verify the validity of fingerprint data
'apFpDataBuffer - fingerprint buffer
'Return value 0- success; Non-0 - failure
Private Declare Function FPCONV_GetFpDataValidity Lib "FpDataConv.dll" (ByRef bBuf As Byte) As Long


'Through the fingerprint buffer, the algorithm version number and the actual length of the fingerprint data are obtained
'apFpDataBuffer - fingerprint buffer
'apnVersion - The algorithm version number of the data represented by the apFpDataBuffer
'apnSize - Buffer size
'Return value 0- success; Non-0 - failure
Private Declare Function FPCONV_GetFpDataVersionAndSize Lib "FpDataConv.dll" (ByRef bBuf As Byte, ByRef apnVersion As Long, ByRef apnSize As Long) As Long


'Get the length of the fingerprint buffer for different versions
'anFpDataVersion -- in fingerprint algorithm version
'apnFpDataSize -- The buffer length used by the out fingerprinting algorithm
'Return value 0- success; Non-0 - failure
Private Declare Function FPCONV_GetFpDataSize Lib "FpDataConv.dll" (ByVal anFpDataVersion As Long, ByRef apnFpDataSize As Long) As Long


'Algorithm conversion
'anSrcVer- source algorithm version number
'apSrcFpData - Source fingerprint buffer
'anDestVer - target algorithm version number
'apDestFpData - target fingerprint buffer
'Return value 0- success; Non-0 - failure
Private Declare Function FPCONV_Convert Lib "FpDataConv.dll" (ByVal anSrcVer As Long, ByRef apSrcFpData As Byte, ByVal anDestVer As Long, ByRef apDestFpData As Byte) As Long




'Checks if the input bSrcFPDataBuf is a valid fingerprint and converts to the specified fingerprint algorithm version
'Exit Doeturns true The conversion succeeded, false the conversion failed
'returns true and the target fingerprint algorithm will be placed in bDestFPDataBuf
Public Function FPAlgorithmConvert(ByRef bSrcFPDataBuf() As Byte, bDestFPDataBuf() As Byte, ByVal lDestFPVer As Long) As Boolean
    FPAlgorithmConvert = False
    
    Select Case lDestFPVer
        Case 700
            lDestFPVer = DATA_VER_70
        Case 800
            lDestFPVer = DATA_VER_80
        Case 850
            lDestFPVer = DATA_VER_85
        Case 890
            lDestFPVer = DATA_VER_89
        Case Else
            Exit Function
    End Select
    
    Call FPCONV_Init
    
    
    Dim iRec As Long
    iRec = FPCONV_GetFpDataValidity(bSrcFPDataBuf(0))
    If iRec <> FPResultOK Then
        Exit Function
    End If
    
    If iRec <> FPResultOK Then
        Exit Function
    End If
    
    
    Dim lfpVer As Long, lFpSize As Long
    iRec = FPCONV_GetFpDataVersionAndSize(bSrcFPDataBuf(0), lfpVer, lFpSize)
    If iRec <> FPResultOK Then
        Exit Function
    End If
    
    If lfpVer = lDestFPVer Then
        FPAlgorithmConvert = True
        bDestFPDataBuf = bSrcFPDataBuf
        Exit Function
    End If
    
    'Start conversion
    
    
    
    'Gets the target buffer size
    Dim lDestSize As Long
    iRec = FPCONV_GetFpDataSize(lDestFPVer, lDestSize)
    
    
    ReDim bDestFPDataBuf(lDestSize - 1)
    iRec = FPCONV_Convert(lfpVer, bSrcFPDataBuf(0), lDestFPVer, bDestFPDataBuf(0))
    
    If iRec <> FPResultOK Then
        Exit Function
    End If
    
    FPAlgorithmConvert = True
End Function
