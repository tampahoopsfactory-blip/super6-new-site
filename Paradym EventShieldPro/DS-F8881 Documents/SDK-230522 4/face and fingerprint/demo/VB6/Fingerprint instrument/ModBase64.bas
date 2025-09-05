Attribute VB_Name = "ModBase64"
Option Explicit

Private Const BASE64CHR As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"
Private psBase64Chr(0 To 63) As String
Private mDecodeBase64Table(255) As Byte, mIniDecodeBase64Table As Boolean



'从一个经过Base64的字符串中解码到源字符串
Public Function DecodeBase64String(ByVal str2Decode As String) As String
    DecodeBase64String = StrConv(DecodeBase64Byte(str2Decode), vbUnicode)
End Function


'从一个经过Base64的字符串中解码到源字节数组
Public Function DecodeBase64Byte(ByVal sBase64 As String) As Byte()

    Dim lPtr As Long
    Dim iValue As Byte
    Dim iLen As Long, iBase64Len As Long
    Dim iCtr As Long, iCtrIndex As Long
    Dim Bits(1 To 4) As Byte
    Dim sChar As String, iAsc As Long
    Dim Output() As Byte


    Dim iIndex As Long
    Dim i As Long

    Call InitBase

    '除去回车
    sBase64 = Replace(sBase64, vbNewLine, "")
    iBase64Len = Len(sBase64)
    '申请数据空间
    ReDim Output(iBase64Len)
    iIndex = 0
    
    '每4个字符一组（4个字节表示3个字节）
    For lPtr = 1 To iBase64Len Step 4
        iLen = 4
        For iCtr = 1 To 4
            iCtrIndex = (lPtr + (iCtr - 1))
            If iCtrIndex <= iBase64Len Then
                sChar = Mid$(sBase64, iCtrIndex, 1)
                '查找字符在BASE64字符串中的位置
                iAsc = Asc(sChar)
                If iAsc > 127 Then
                    Exit Function '有非法字符
                End If
                
                iValue = mDecodeBase64Table(iAsc)
                Select Case iValue 'A~Za~z0~9+/
                    Case 1 To 64
                        Bits(iCtr) = iValue - 1
                        
                    Case 65 '=
                        Bits(iCtr) = 0
                        iLen = iLen - 1
                        
                    Case 0: Exit Function '没有发现 有非法字符
                End Select
            Else
                Bits(iCtr) = 0
                iLen = iLen - 1
            End If
        Next

        '转换4个6比特数成为3个8比特数
        Bits(1) = Bits(1) * &H4 + (Bits(2) And &H30) \ &H10
        Bits(2) = (Bits(2) And &HF) * &H10 + (Bits(3) And &H3C) \ &H4
        Bits(3) = (Bits(3) And &H3) * &H40 + Bits(4)


        If iLen < 2 Then Exit Function '无法解析
        
        For i = 1 To iLen - 1
            Output(iIndex) = Bits(i)
            iIndex = iIndex + 1
        Next
    Next
    
    ReDim Preserve Output(iIndex - 1)
    
    DecodeBase64Byte = Output
End Function



'将一个字节数组进行Base64编码，并返回字符串
Public Function EncodeBase64Byte(sValue() As Byte) As String
    Dim lCtr As Long
    Dim lPtr As Long
    Dim lLen As Long
    Dim sEncoded As cStringBulider
    Set sEncoded = New cStringBulider
    sEncoded.Capacity = UBound(sValue) * 1.4
    Dim Bits8(1 To 3) As Byte
    Dim Bits6(1 To 4) As Byte

    Dim i As Long

    Call InitBase
    'base64 就是把3个字节转换为4个6bit的字节，所以转换时每次取原数组的3个字节，然后进行运算变换为4个字节
    Dim iIndex As Long
    For lCtr = 0 To UBound(sValue) Step 3
        '从数组取3个字节
        lLen = 0 '指示本次转换取了多少字节
        For i = 1 To 3
            iIndex = lCtr + (i - 1)
            If iIndex <= UBound(sValue) Then
                Bits8(i) = sValue(iIndex)
                lLen = lLen + 1
            Else
                Bits8(i) = 0
            End If
        Next


        '//转换字符串为数组，然后转换为4个6位(0-63)
        Bits6(1) = (Bits8(1) And &HFC) \ 4 '第一个字节是取原数据的第一字节前6位 这里是的意思是舍去低2位，然后右移2位
        Bits6(2) = (Bits8(1) And &H3) * &H10 + (Bits8(2) And &HF0) \ &H10  '取原数据的第一字节低2位，和第二字节高4位，合并在一起
        Bits6(3) = (Bits8(2) And &HF) * 4 + (Bits8(3) And &HC0) \ &H40 '取第二字节低4位和第三字节高2位 合并在一起
        Bits6(4) = Bits8(3) And &H3F '取第三字节低6位。

        '//添加4个新字符
        For lPtr = 1 To lLen + 1
            sEncoded.Append psBase64Chr(Bits6(lPtr))
        Next
    Next

    '//不足4位，以=填充
    Select Case lLen + 1
        Case 2: sEncoded.Append "=="
        Case 3: sEncoded.Append "="
        Case 4
    End Select

    EncodeBase64Byte = sEncoded.ToString()
End Function


'对字符串进行Base64编码并返回字符串
Public Function EncodeBase64String(ByVal str2Encode As String) As String
    Dim sValue() As Byte
    sValue = StrConv(str2Encode, vbFromUnicode)
    EncodeBase64String = EncodeBase64Byte(sValue)
End Function

Private Sub InitBase()
    Dim iPtr As Integer
    If mIniDecodeBase64Table Then
        Exit Sub
    End If
    mIniDecodeBase64Table = True
    Dim sChar As String
    '初始化 BASE64数组
    For iPtr = 0 To 63
        sChar = Mid$(BASE64CHR, iPtr + 1, 1)
        psBase64Chr(iPtr) = sChar
        mDecodeBase64Table(Asc(sChar)) = iPtr + 1
    Next
    mDecodeBase64Table(61) = 65 '=占位符

End Sub


