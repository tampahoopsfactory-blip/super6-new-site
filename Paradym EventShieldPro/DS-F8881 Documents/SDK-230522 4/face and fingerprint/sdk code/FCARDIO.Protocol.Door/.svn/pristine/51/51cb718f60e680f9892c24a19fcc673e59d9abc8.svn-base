
#include <stdio.h>
#include <memory.h>
#include <malloc.h>
#include <string.h>
#include "DES2.h"
void Byte2Hex(char *dstHex, char* srcBits, unsigned int sizeBits);
void Hex2Byte(char *Hex, char *bytes, unsigned int size);

void main()
{
	unsigned char key[9] =  {'1','2','3','4','5','6','7','8','\0'}; //设置密钥
	unsigned char *data="abcdefg123478";
	unsigned char *DesBuf,*Hex,*Data2;int iSize=0;
	char *HexStr=0;unsigned int iHexLen;
	int i=0;
	

	printf("密钥：%s\n",key);

	DES_Initialize(); //初始化DES库
	DES_InitializeKey(key);//初始化密钥
	
	//*************************加密*********************
	DES_EncryptAnyLength(data,15); //数据加密
	DesBuf=DES_GetCiphertextAnyLength();//取出加密的数据
	iSize=DES_GetCiphertextAnyLengthSize();//取出加密后数据长度
	
	//输出显示
	Hex= (char *)malloc(iSize*2+1);
	Byte2Hex(Hex,DesBuf,iSize);
	printf("加密后数据：%s\n",Hex);
	free(Hex);
	//**************************************************

	//****************解密*********************
	data=0;
	DES_DecryptAnyLength(DesBuf,iSize);
	data=DES_GetPlaintextAnyLength();
	iSize=DES_GetPlaintextAnyLengthSize();
	printf("解密后数据：%s，返回长度：%d\n",data,iSize);
	//*****************************************
	printf("\n");
	DES_Initialize(); //销毁申请的动态空间



	//转换十六进制数据为字节数组
	HexStr = "A0AAEE30BB16E2D86ADCF925E2772B8A";
	iHexLen=strlen(HexStr);
	DesBuf= (char *)malloc(iHexLen/2);
	Hex2Byte(HexStr,DesBuf,iHexLen);
	
	//解密
	DES_Initialize(); //初始化DES库
	DES_InitializeKey(key);//初始化密钥
	DES_DecryptAnyLength(DesBuf,iHexLen/2);
	DesBuf=DES_GetPlaintextAnyLength();
	iSize=DES_GetPlaintextAnyLengthSize();
	
	Data2= (char *)malloc(iSize);
	memcpy(Data2,DesBuf,iSize);
	DES_Initialize(); //销毁申请的动态空间

	//输出显示
	Hex= (char *)malloc(iSize*2+1);
	Byte2Hex(Hex,Data2,iSize);
	printf("解密后数据：%s\n",Hex);
	free(Hex);
	
	free(Data2);

}

void Byte2Hex(char *dstHex, char* srcBytes, unsigned int size)
{
	unsigned int i=0,j=0,index=0;
	char *mHexDigits="0123456789ABCDEF";
	memset(dstHex,0,size*2+1);
	for(i=0; i < size; i++) 
	{
		dstHex[j] =mHexDigits[(unsigned char)srcBytes[i] / 16]; j++;
		dstHex[j] =mHexDigits[(unsigned char)srcBytes[i] % 16];  j++;
	}

}

void Hex2Byte(char *Hex, char *bytes, unsigned int size)
{
	//size--十六进制字符串长度
	unsigned char mByteDigits[256]="",*tmp,bData;
	unsigned int i=0,j=0;
	
	//创建解码表
	tmp="0123456789ABCDEF";
	for(i=0;i<16;i++)
	{
		mByteDigits[tmp[i]]=i;
	}
	tmp="abcdef";
	for(i=0;i<6;i++)
	{
		mByteDigits[tmp[i]]=i+10;
	}

	memset(bytes,0,size/2);

	for(i=0;i<size;i++)
	{
		bData = mByteDigits[Hex[i]]*16;
		i++;
		bData+=mByteDigits[Hex[i]];

		bytes[j]=bData;
		j++;
	}
}