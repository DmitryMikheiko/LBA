using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;

namespace ESP8266_Programmer_ForStm32
{
 public class ESP8266_CommunicationProtocol
    {
     //Marker
        private const uint UOP_Marker = 0xFF00807F;
     //Commands
        private const byte UOP_GetStatus =  0x00;     
private const byte  UOP_GetStatus_cb     =  0x80;
private const byte  UOP_MCU_Reset        =  0x40;
private const byte  UOP_MCU_Reset_cb     =  0xC0;
private const byte  UOP_MCU_BootMode     =  0x41;
private const byte  UOP_MCU_BootMode_cb  =  0xC1;
private const byte  UOP_MCU_StartBoot    =  0x42;
private const byte  UOP_MCU_StartBoot_cb =  0xC2;
private const byte  UOP_MCU_StopBoot     =  0x43;
private const byte  UOP_MCU_StopBoot_cb  =  0xC3;
private const byte  UOP_MCU_Read         =  0x44;
private const byte  UOP_MCU_Read_cb      =  0xC4;
private const byte  UOP_MCU_Write        =  0x45;
private const byte  UOP_MCU_Write_cb     =  0xC5;
private const byte  UOP_MCU_ExtendedErase = 0x46;
private const byte  UOP_MCU_ExtendedErase_cb = 0xC6;
private const byte  UOP_MCU_GV           =  0x47;
private const byte  UOP_MCU_GV_cb        =  0xC7;
private const byte  UOP_MCU_GetID        =  0x48;
private const byte  UOP_MCU_GetID_cb     =  0xC8;
private const byte  UOP_MCU_Get          =  0x49;
private const byte  UOP_MCU_Get_cb       =  0xC9;
private const byte  UOP_MCU_ReadProtect  =  0x4A;
private const byte  UOP_MCU_ReadProtect_cb   = 0xCA;
private const byte  UOP_MCU_ReadUnProtect     = 0x4B;
private const byte  UOP_MCU_ReadUnProtect_cb  = 0xCB;
private const byte  UOP_MCU_WriteProtect      = 0x4C;
private const byte  UOP_MCU_WriteProtect_cb   = 0xCC;
private const byte  UOP_MCU_WriteUnProtect    = 0x4D;
private const byte  UOP_MCU_WriteUnProtect_cb = 0xCD;

private const byte  UOP_UART_Send         = 0x01;
private const byte  UOP_UART_Read         = 0x02;
private const byte  UOP_UART_SendRead     = 0x03;
private const byte  UOP_UART_Flush        = 0x04;
private const byte  UOP_TCP_Send          = 0x05;
private const byte  UOP_TCP_Receive       = 0x06;
private const byte  UOP_UDP_Send          = 0x07;
private const byte  UOP_UDP_Receive       = 0x08;
private const byte  UOP_GetTime           = 0x09;
private const byte  UOP_SetNtpServerIP    = 0x0A;

private const byte  UOP_FS_GetFreeSize    = 0x10;
private const byte  UOP_FS_CheckFile      = 0x11;
private const byte  UOP_FS_CreateFile     = 0x12;
private const byte  UOP_FS_DeleteFile     = 0x13;
private const byte  UOP_FS_WriteFile      = 0x14;
private const byte  UOP_FS_ReadFile       = 0x15;

private const byte  UOP_Stop              = 0x20;
private const byte  UOP_Start             = 0x21;

        //Enums
        public enum UOP_StatusType
       {
           Unknown,
           OK,
           Error,
           UnknownCommand,
           FS_FileDoesNotExist
       }
       
     //UOP Timings

       private const int UOP_WaitAnswer_Delay = 1;
     //UOP Constants
       private const int UOP_HeaderSize = 9;
       private const byte UOP_FlagDefault = 0;
       private const int UOP_Marker_Pos = 0;

       private const int UOP_Cmd_Pos = 4;
       private const int UOP_Flags_Pos = 5;
       private const int UOP_DataSize_Pos = 6;
       private const int UOP_CheckSum_Pos = 8;
       private const int UOP_Data_Pos = 9;

       private const int UOP_RW_Addr     = 0;
       private const int UOP_RW_Addr_cb  = 1;
       private const int UOP_RW_Size = 4;
       private const int UOP_RW_Size_cb = 5;
       private const int UOP_W_Data = 5;
       private const int UOP_R_Data = 6;
       private const int UOP_RW_DataHeader_Size = 5;
       private const int UOP_RW_DataHeader_Size_cb = 6;
       private const int UOP_EErase_Count    = 0;
       private const int UOP_EErase_DataHeader_Size = 2;
       private const int UOP_WriteProtect_Count = 0;
       private const int UOP_WriteProtect_DataHeader_Size = 1;
       private const int UOP_UART_Read_DataHeader_Size_cb = 3;
       private const int UOP_UART_SendRead_DataHeader_Size =6;
       private const int UOP_UART_SendRead_DataHeader_Size_cb =7;
       private const int UOP_UART_SendRead_DataSize =0;
       private const int UOP_UART_SendRead_DataMaxSize =2;
       private const int UOP_UART_SendRead_TimeOut =4;
       private const int UOP_UART_SendRead_DataSize_cb =1;
       private const int UOP_UART_SendRead_Data =6;
       private const int UOP_UART_SendRead_Data_cb =3;
       private const int UOP_GetTime_DataHeader_Size_cb =2;
       private const int UOP_GetTime_DataSize_cb =1;
      
       private const int UOP_FS_WriteFile_Flags_Create  = 1;
       private const int UOP_FS_WriteFile_Flags         = 0;
       private const int UOP_FS_WriteFile_NameLength    = 1;
       private const int UOP_FS_WriteFile_Name          = 8;
       private const int UOP_FS_WriteFile_DataPos       = 2;
       private const int UOP_FS_WriteFile_DataSize      = 6;
       private const int UOP_FS_WriteFile_HeaderSize    = 8;
       private const int UOP_FS_WriteFile_BlockSize   = 1024;

       private const int UOP_FS_DeleteFile_HeaderSize   = 1;
       private const int UOP_FS_DeleteFile_NameLength   = 0;
       private const int UOP_FS_DeleteFile_Name         = 1;


        //Structures
        public struct UOP_FS_WriteFileHeader
        {
            public byte Flags;
            public byte NameLength;
            public uint DataPos;
            public UInt16 DataSize;
        }
        public struct UOP_Package
       {
         public bool Received;
         public uint Marker;
         public byte Cmd;
         public byte Flags;
         public UInt16 DataSize;
         public byte CheckSum;
         public byte[] Data;
       };
     public struct BootGetStruct
       {
         public byte BootVersion;
         public byte CommandsCount;
         public byte[] Commands;
       };
     public BootGetStruct BootInfo;
     public struct BootGVStruct
       {
         public byte BootVersion;
         public byte OptionByte1;
         public byte OptionByte2;
       };
     public BootGVStruct BootGV;
      
     public struct BootIDStruct
     {
	   public byte Count;
	   public byte[] ID;
     };
     public BootIDStruct BootID;
     //**********************************************************

        public delegate bool SendData(byte[] data,int size);
        public delegate int ReadData(byte[] data);
        public delegate void WriteFileProgressDelegate(int part);

        public SendData _Send;
        public ReadData _Read;
        public WriteFileProgressDelegate WriteFileProgress;
       
     public UOP_StatusType GetStatus()
        {
            if (SendMsg(UOP_GetStatus, null, 0))
            {
                UOP_Package pack = ReadPackage();
                if (pack.Received)
                {
                    int p = UOP_HeaderSize;
                    if ((UOP_StatusType)pack.Data[p++] == UOP_StatusType.OK)
                    {
                        return UOP_StatusType.OK;
                    }
                }
            }
            return UOP_StatusType.Error;
        }

      public UOP_StatusType MCU_GV()
     {
         if (SendMsg(UOP_MCU_GV, null, 0))
         {
             UOP_Package pack = ReadPackage();
             if (pack.Received)
             {
                 int p = UOP_HeaderSize;
                 if ((UOP_StatusType)pack.Data[p++] == UOP_StatusType.OK)
                 {
                     BootGV.BootVersion = pack.Data[p];
                     return UOP_StatusType.OK;
                 }
             }
         }
         return UOP_StatusType.Error;
     }
      public UOP_StatusType MCU_GetID()
      {
          if (SendMsg(UOP_MCU_GetID, null, 0))
          {
              UOP_Package pack = ReadPackage();
              if (pack.Received)
              {
                  int p = UOP_HeaderSize;
                  if ((UOP_StatusType)pack.Data[p++] == UOP_StatusType.OK)
                  {
                      BootID.ID = new byte[2];
                      BootID.ID[0] = pack.Data[p++];
                      BootID.ID[1] = pack.Data[p++];
                      BootID.Count = 2;
                      return UOP_StatusType.OK;
                  }
              }
          }
          return UOP_StatusType.Error;
      }
      public UOP_StatusType MCU_Get()
      {
          if (SendMsg(UOP_MCU_Get, null, 0))
          {
              UOP_Package pack = ReadPackage();
              if (pack.Received)
              {
                  int p = UOP_HeaderSize;
                  if ((UOP_StatusType)pack.Data[p++] == UOP_StatusType.OK)
                  {
                      BootInfo.BootVersion = pack.Data[p++];
                      BootInfo.CommandsCount = pack.Data[p++];
                      BootInfo.Commands = new byte[BootInfo.CommandsCount];
                      for (int n = 0; n < BootInfo.CommandsCount; n++) BootInfo.Commands[n] = pack.Data[p++];
                          return UOP_StatusType.OK;
                  }
              }
          }
         return UOP_StatusType.Error;
      }
      public UOP_StatusType MCU_ReadProtect()
      {
          if (SendMsg(UOP_MCU_ReadProtect, null, 0))
          {
              UOP_Package pack = ReadPackage();
              if (pack.Received)
              {
                  int p = UOP_HeaderSize;
                  if ((UOP_StatusType)pack.Data[p] == UOP_StatusType.OK)
                  {
                      return UOP_StatusType.OK;
                  }
              }
          }
          return UOP_StatusType.Error;
      }
        public UOP_StatusType MCU_ReadUnProtect()
        {
            if (SendMsg(UOP_MCU_ReadUnProtect, null, 0))
            {
                UOP_Package pack = ReadPackage();
                if (pack.Received)
                {
                    int p = UOP_HeaderSize;
                    if ((UOP_StatusType)pack.Data[p] == UOP_StatusType.OK)
                    {
                        return UOP_StatusType.OK;
                    }
                }
            }
            return UOP_StatusType.Error;
        }
        public UOP_StatusType MCU_WriteUnProtect()
        {
            if (SendMsg(UOP_MCU_WriteUnProtect, null, 0))
            {
                UOP_Package pack = ReadPackage();
                if (pack.Received)
                {
                    int p = UOP_HeaderSize;
                    if ((UOP_StatusType)pack.Data[p] == UOP_StatusType.OK)
                    {
                        return UOP_StatusType.OK;
                    }
                }
            }
            return UOP_StatusType.Error;
        }
        public UOP_StatusType MCU_WriteProtect(byte Count, byte[] sectors)
        {
            if (Count == 0) return UOP_StatusType.Error;
            byte[] data = new byte[UOP_WriteProtect_DataHeader_Size + Count];
            data[UOP_WriteProtect_Count] =(byte) (Count - 1);
            sectors.CopyTo(data, UOP_WriteProtect_DataHeader_Size);
            if (SendMsg(UOP_MCU_WriteProtect, data, UOP_WriteProtect_DataHeader_Size + Count))
            {
                UOP_Package pack = ReadPackage();
                if (pack.Received)
                {
                    return (UOP_StatusType)pack.Data[UOP_HeaderSize];
                }
            }
            return UOP_StatusType.Error;
        }
        public UOP_StatusType MCU_StartBoot()
      {
          if (SendMsg(UOP_MCU_StartBoot, null, 0))
          {
              UOP_Package pack = ReadPackage();
              if (pack.Received)
              {
                  int p = UOP_HeaderSize;
                  if ((UOP_StatusType)pack.Data[p] == UOP_StatusType.OK)
                  {
                      return UOP_StatusType.OK;
                  }
              }
          }
          return UOP_StatusType.Error;
      }
      public UOP_StatusType MCU_StopBoot()
      {
          if (SendMsg(UOP_MCU_StopBoot, null, 0))
          {
              UOP_Package pack = ReadPackage();
              if (pack.Received)
              {
                  int p = UOP_HeaderSize;
                  if ((UOP_StatusType)pack.Data[p] == UOP_StatusType.OK)
                  {
                      return UOP_StatusType.OK;
                  }
              }
          }
          return UOP_StatusType.Error;
      }
     public UOP_StatusType MCU_Read(int Addr,byte Count,ref byte[] buf)
      {
         byte[] data = new byte[UOP_RW_DataHeader_Size];
         BitConverter.GetBytes(Addr).CopyTo(data, UOP_RW_Addr);
         data[UOP_RW_Size] = Count;
         if (SendMsg(UOP_MCU_Read, data, UOP_RW_DataHeader_Size))
         {
             UOP_Package pack = ReadPackage();
             if (pack.Received)
             {
                 int p = UOP_HeaderSize;
                 if ((UOP_StatusType)pack.Data[p++] == UOP_StatusType.OK)
                 {
                     int Addr2 = BitConverter.ToInt32(pack.Data, UOP_HeaderSize + UOP_RW_Addr_cb);
                     byte Count2 = pack.Data[UOP_HeaderSize + UOP_RW_Size_cb];
                     if(Count == Count2 && Addr == Addr2)
                     {
                         buf = new byte[Count];
                         Array.Copy(pack.Data, UOP_HeaderSize + UOP_RW_DataHeader_Size_cb, buf, 0, Count);
                         return UOP_StatusType.OK;
                     }
                 }              
             }
         }
          return UOP_StatusType.Error;
      }
     public UOP_StatusType MCU_Write(int Addr, byte Count, ref byte[] buf)
     {
         byte[] data = new byte[UOP_RW_DataHeader_Size + Count];
         BitConverter.GetBytes(Addr).CopyTo(data, UOP_RW_Addr);
         data[UOP_RW_Size] = Count;
         Array.Copy(buf, 0, data, UOP_RW_DataHeader_Size, Count);
         if (SendMsg(UOP_MCU_Write, data, UOP_RW_DataHeader_Size + Count))
         {
             UOP_Package pack = ReadPackage();
             if (pack.Received)
             {
                 int p = UOP_HeaderSize;
                 if ((UOP_StatusType)pack.Data[p++] == UOP_StatusType.OK)
                 {
                     int Addr2 = BitConverter.ToInt32(pack.Data, UOP_HeaderSize + UOP_RW_Addr_cb);
                     byte Count2 = pack.Data[UOP_HeaderSize + UOP_RW_Size_cb];
                     if (Count == Count2 && Addr == Addr2)
                     {                       
                         return UOP_StatusType.OK;
                     }
                 }
             }
         }
         return UOP_StatusType.Error;
     }
     public UOP_StatusType MCU_EraseAll()
     {
         ushort Count = 0xFFFF; // for global mass erase
         byte[] data = new byte[UOP_EErase_DataHeader_Size];
         BitConverter.GetBytes(Count).CopyTo(data, UOP_EErase_Count);

         if (SendMsg(UOP_MCU_ExtendedErase, data, UOP_EErase_DataHeader_Size))
         {
             UOP_Package pack = ReadPackage();
             if (pack.Received)
             {
                 return (UOP_StatusType)pack.Data[UOP_HeaderSize];
             }
         }
         return UOP_StatusType.Error;
     }
     public UOP_StatusType MCU_ExtendedErase(UInt16 Count, UInt16[] pages)
     {
         if(Count >=0xFFF0 || Count==0) return UOP_StatusType.Error;
         byte[] data = new byte[UOP_EErase_DataHeader_Size + Count * 2];
         BitConverter.GetBytes(Count-1).CopyTo(data, UOP_EErase_Count);
         int p=UOP_EErase_DataHeader_Size;
         for (int n = 0; n < Count;n++,p+=2 )
         {
             BitConverter.GetBytes(pages[n]).CopyTo(data, p);
         }
         if (SendMsg(UOP_MCU_ExtendedErase, data, UOP_EErase_DataHeader_Size + Count*2))
         {             
             UOP_Package pack = ReadPackage();
             if (pack.Received)
             {
               return (UOP_StatusType)pack.Data[UOP_HeaderSize];                 
             }
         }
        return UOP_StatusType.Error;
     }
     public UOP_StatusType UART_Send(UInt16 Count, ref byte[] buf)
     {
         if (Count > 0 && buf != null)
         {
             byte[] data = new byte[Count + 2];
             BitConverter.GetBytes(Count).CopyTo(data, 0);
             Array.Copy(buf, 0, data, 2, Count);
             if (SendMsg(UOP_UART_Send, data, Count + 2))
             {
                 UOP_Package pack = ReadPackage();
                 if (pack.Received)
                 {
                     return (UOP_StatusType)pack.Data[UOP_HeaderSize];
                 }
                 
             }
         }
         return UOP_StatusType.Error;
     }
     public string GetTime()
     {
         string time = "";
         if (SendMsg(UOP_GetTime, null,0))
         {
             UOP_Package pack = ReadPackage();
             if (pack.Received)
             {
                 if((UOP_StatusType)pack.Data[UOP_HeaderSize] == UOP_StatusType.OK)
                 {
                     int len = pack.Data[UOP_HeaderSize + 1];
                     int p = 0;
                     for (; p < len; p++) time += (char)pack.Data[UOP_HeaderSize + 2 + p];                     
                 }
             }

         }

         return time;
     }
     public UOP_StatusType WriteFile(string FileName,bool Create)
     {
         using(BinaryReader file = new BinaryReader(File.Open(FileName, FileMode.Open)))
         {
             byte[] buf;
             int FileSize = (int)file.BaseStream.Length;
             UOP_FS_WriteFileHeader header =new UOP_FS_WriteFileHeader();
             FileInfo finfo = new FileInfo(FileName);
             FileName = finfo.Name;
             if (Create) header.Flags = 1;
             if (FileName.Length > 255) return UOP_StatusType.Error;
             header.NameLength =(byte)FileName.Length;
             int BlocksCount =( FileSize / UOP_FS_WriteFile_BlockSize);
             if((FileSize % UOP_FS_WriteFile_BlockSize) > 0) BlocksCount++;
             uint pos = 0;
             for (int block = 0; block < BlocksCount; block++, pos += UOP_FS_WriteFile_BlockSize)
             {
                 header.DataPos = pos;
                 if ((FileSize - pos) < UOP_FS_WriteFile_BlockSize) header.DataSize =(ushort) (FileSize - pos);
                 else header.DataSize = UOP_FS_WriteFile_BlockSize;
                 buf = new byte[UOP_FS_WriteFile_HeaderSize + header.NameLength + header.DataSize];

                 buf[UOP_FS_WriteFile_Flags] = header.Flags;
                 buf[UOP_FS_WriteFile_NameLength] = header.NameLength;
                 /*byte[] ValueArr = BitConverter.GetBytes(header.DataPos);
                 Array.Reverse(ValueArr);
                 ValueArr.CopyTo(buf, UOP_FS_WriteFile_DataPos);
                 ValueArr = BitConverter.GetBytes(header.DataSize);
                 Array.Reverse(ValueArr);
                 ValueArr.CopyTo(buf, UOP_FS_WriteFile_DataPos);*/

                 BitConverter.GetBytes(header.DataPos).CopyTo(buf, UOP_FS_WriteFile_DataPos);
                 BitConverter.GetBytes(header.DataSize).CopyTo(buf, UOP_FS_WriteFile_DataSize);
                 Encoding.UTF8.GetBytes(FileName).CopyTo(buf, UOP_FS_WriteFile_HeaderSize);                
                 file.ReadBytes(header.DataSize).CopyTo(buf, UOP_FS_WriteFile_HeaderSize + header.NameLength);

                 for (int try_count = 0; try_count < 2; try_count++)
                 {
                     if (SendMsg(UOP_FS_WriteFile, buf, buf.Length))
                     {
                         UOP_Package pack = ReadPackage();
                         if (pack.Received)
                         {
                             if ((UOP_StatusType)pack.Data[UOP_HeaderSize] == UOP_StatusType.OK)
                             {
                                 break;
                             }
                         }
                     }

                         if (try_count < 1)
                         {
                             Thread.Sleep(5000);
                             if (LB_Stop() != ESP8266_CommunicationProtocol.UOP_StatusType.OK) return UOP_StatusType.Error;
                         }
                         else return UOP_StatusType.Error;
                     
                 }
                 if (WriteFileProgress != null) WriteFileProgress(((block + 1) * 100) / BlocksCount);
                 Thread.Sleep(10);
             }
         }
         return UOP_StatusType.OK;
         
     }
     public UOP_StatusType DeleteFile(string FileName)
     {
         FileName = Path.GetFileName(FileName);
         byte[] data = new byte[UOP_FS_DeleteFile_HeaderSize + FileName.Length];
         if (FileName.Length > 255) return UOP_StatusType.Error;
         data[UOP_FS_DeleteFile_NameLength] = (byte)FileName.Length;
         Encoding.UTF8.GetBytes(FileName).CopyTo(data, UOP_FS_DeleteFile_Name);
         if (SendMsg(UOP_FS_DeleteFile, data,data.Length))
         {
             UOP_Package pack = ReadPackage();
             if (pack.Received)
             {
                 int p = UOP_HeaderSize;
                 if ((UOP_StatusType)pack.Data[p] == UOP_StatusType.OK || (UOP_StatusType)pack.Data[p] == UOP_StatusType.FS_FileDoesNotExist)
                 {
                     return UOP_StatusType.OK;
                 }
             }
         }
         return UOP_StatusType.Error;
     }
     public UOP_StatusType LB_Stop()
     {
         if (SendMsg(UOP_Stop, null, 0))
         {
             UOP_Package pack = ReadPackage();
             if (pack.Received)
             {
                 int p = UOP_HeaderSize;
                 if ((UOP_StatusType)pack.Data[p++] == UOP_StatusType.OK)
                 {
                     return UOP_StatusType.OK;
                 }
             }
         }
         return UOP_StatusType.Error;
     }
     public UOP_StatusType LB_Start()
     {
         if (SendMsg(UOP_Start, null, 0))
         {
             UOP_Package pack = ReadPackage();
             if (pack.Received)
             {
                 int p = UOP_HeaderSize;
                 if ((UOP_StatusType)pack.Data[p++] == UOP_StatusType.OK)
                 {
                     return UOP_StatusType.OK;
                 }
             }
         }
         return UOP_StatusType.Error;
     }
     public bool SendMsg(byte Command,byte[] Data,int DataSize)
     {
         int size = UOP_HeaderSize + DataSize;
         byte[] buf = new byte[size];
         int p = 0;
         buf[p++] = (byte)(UOP_Marker & 0xFF);
         buf[p++] = (byte)((UOP_Marker >> 8) & 0xFF);
         buf[p++] = (byte)((UOP_Marker >> 16) & 0xFF);
         buf[p++] = (byte)((UOP_Marker >> 24) & 0xFF);
         buf[p++] = Command;
         buf[p++] = UOP_FlagDefault;
         buf[p++] = (byte)(DataSize & 0xFF);
         buf[p++] = (byte)(DataSize >> 8);        
         buf[p++] = 0;
         if(Data != null && DataSize > 0)
         {
             int dp=0;
             while(dp<DataSize) buf[p++] = Data[dp++];
         }

         buf[UOP_CheckSum_Pos] = GetCheckSum(buf, size);
         
         return Send(buf, size);
     }
     public byte GetCheckSum(byte[] data,int size)
     {
         byte csum = 0;
         for(int n=0;n<size;n++)
         {
             if (n == UOP_CheckSum_Pos) continue;
             csum ^= data[n];
         }
         return csum;
     }
     private bool Send(byte[] data,int size) // Check user method and call it with data
     {
         if (_Send == null) return false;
         int WaitCounter = 0;
         while (WaitCounter++ < UOP_WaitAnswer_Delay)
         {
             if (_Send(data, size)) return true;
             Thread.Sleep(1);
         }
        return false;
     }

     public int Read(byte[] data) // User must call this method with received data
     {
         int WaitCounter = 0;
         int size;
         if (_Read == null) return -1;
         while (WaitCounter++ < UOP_WaitAnswer_Delay)
         {
             size =_Read(data);
             if(size>0) return size;
            // Thread.Sleep(1);
         }
         return -1;
     }
     public UOP_Package ReadPackage()
     {
         UOP_Package pack = new UOP_Package();
         byte[] buf = new byte[2048];
         pack.Received = false;
         int size = Read(buf);
         if(size >= UOP_HeaderSize)
         {
             pack.Marker =(uint)( ((int)buf[UOP_Marker_Pos]) );
             pack.Marker += (uint)(((int)buf[UOP_Marker_Pos+1]) << 8);
             pack.Marker += (uint)(((int)buf[UOP_Marker_Pos+2]) << 16);
             pack.Marker += (uint)(((int)buf[UOP_Marker_Pos+3]) << 24);
             pack.Cmd = buf[UOP_Cmd_Pos];
             pack.Flags = buf[UOP_Flags_Pos];
             pack.DataSize = (UInt16)((int)buf[UOP_DataSize_Pos] );
             pack.DataSize += (UInt16)((int)buf[UOP_DataSize_Pos+1] << 8);
             pack.CheckSum = buf[UOP_CheckSum_Pos];
             pack.Data = buf;
             if (pack.CheckSum == GetCheckSum(buf, size)) pack.Received = true;
             return pack;
         }        
         return pack;
     }
     
    }
}
