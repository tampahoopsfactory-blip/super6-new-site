Attribute VB_Name = "ModRC4"
Option Explicit
Public Declare Function CreateCRC32 Lib "RC4.dll" (ByRef pdata As Byte, ByVal datalen As Long) As Long


Public Function Uint32(ByVal iLongValue As Long) As Double
    Dim iUint32 As Double
    If iLongValue < 0 Then
        iUint32 = 2147483648#
        iUint32 = iUint32 + (iUint32 + iLongValue)
    Else
        iUint32 = iLongValue
    End If
    Uint32 = iUint32
End Function
