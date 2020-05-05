using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace VisualEffectsConverter
{
    class AnimationConverter
    {
        public class Frame
        {
            //public long StartPos;
            public int id;
            public byte[] Image;
            public Bitmap bitmap;
        }
        public class Animation
        {
            public int FramesCount;
            public int SizeX;
            public int SizeY;
            public int Fps;
            public List<Frame> frames;
        }
        private bool Lock = true;
        private int CurrentFrame = -1;
        public enum Version
        {
            v1
        }
        public enum PixelOrder
        {
            HS_TL,
            HS_TR,
            HS_BL,
            HS_BR,
            HL_TL,
            HL_TR,
            HL_BL,
            HL_BR,
            VL_TL,
            VL_TR,
            VL_BL,
            VL_BR,
            VS_TL,
            VS_TR,
            VS_BL,
            VS_BR
        }
        public enum ColorOrder
        {
            RGB,
            RBG,
            GRB,
            GBR,
            BRG,
            BGR
        }
        public enum DataType
        {
            Default,
            WithColorTable
        }

        public struct BMAFormat
        {
            public Version version;
            public PixelOrder pixelOrder;
            public ColorOrder colorOrder;
            public DataType dataType;
            public int FramesCount;
            public byte fps;
            public ushort Width;
            public ushort Height;
            public ushort ColorTableSize;
            public byte[,] ColorTable;
            public byte[] Data;
        }
        public Animation animation;
        public BMAFormat InputFile;
        private BMAFormat OutputFile;

        private int ColorTableMaxSize = 256;

        public AnimationConverter()
        {
            InputFile = new BMAFormat();
        }
        public AnimationConverter(ColorOrder colorOder,PixelOrder pixelOrder,DataType dataType)
        {
            InputFile = new BMAFormat();
            SetInputFileOptions(colorOder, pixelOrder, dataType);
        }
        public AnimationConverter(Bitmap bitmap)
        {
            InputFile.Width = (ushort)bitmap.Width;
            InputFile.Height = (ushort)bitmap.Height;
            InputFile.FramesCount = 1;
            InputFile.fps = 25;

            animation = new Animation();
            animation.SizeX = InputFile.Width;
            animation.SizeY = InputFile.Height;
            animation.FramesCount = InputFile.FramesCount;
            animation.Fps = InputFile.fps;
            animation.frames = new List<Frame>(animation.FramesCount);
            Frame frame = new Frame();
            frame.bitmap = bitmap;
            animation.frames.Add(frame);
        }
        public AnimationConverter(int Width, int Height)
        {
            InputFile.Width = (ushort)Width;
            InputFile.Height = (ushort)Height;
            InputFile.fps = 25;

            animation = new Animation();
            animation.SizeX = InputFile.Width;
            animation.SizeY = InputFile.Height;
            animation.Fps = InputFile.fps;
            animation.frames = new List<Frame>();
        }
        public void AddFrame(Bitmap bitmap)
        {
            if (animation == null) return;
            Frame frame = new Frame();
            frame.bitmap = bitmap;
            animation.frames.Add(frame);
            animation.FramesCount = animation.frames.Count;
        }
        public void LockAniamtion()
        {
            Lock = true;
        }
        public void UnlockAnimation()
        {
            Lock = false;
        }
        public bool Decode(string FilePath)
        {
            LockAniamtion();
            if(!File.Exists(FilePath)) return false;
            FileInfo f_info = new FileInfo(FilePath);
            switch(f_info.Extension)
            {
                case ".bma":
                 using (BinaryReader reader = new BinaryReader(File.Open(FilePath,FileMode.Open)))
                 {
                     int pos = 0;
                     //***check marker "bma"****
                     char c = reader.ReadChar();
                     if (c != 'b') return false;
                     c = reader.ReadChar();
                     if (c != 'm') return false;
                     c = reader.ReadChar();
                     if (c != 'a') return false;
                     //*************************
                     InputFile.version = (Version)reader.ReadByte();
                     InputFile.pixelOrder = (PixelOrder) reader.ReadByte();
                     InputFile.colorOrder = (ColorOrder) reader.ReadByte();
                     InputFile.dataType   = (DataType)   reader.ReadByte();
                     InputFile.FramesCount = reader.ReadInt32();
                     InputFile.fps = reader.ReadByte();
                     InputFile.Width = reader.ReadUInt16();
                     InputFile.Height = reader.ReadUInt16();
                     if (InputFile.dataType == DataType.Default)
                     {
                         int data_size =(int)(InputFile.FramesCount * InputFile.Width * InputFile.Height * 3);
                         //InputFile.Data = new byte[data_size];
                         InputFile.Data = reader.ReadBytes(data_size);

                     }
                     else if (InputFile.dataType == DataType.WithColorTable)
                     {
                         int ct_pos;
                         InputFile.ColorTableSize = reader.ReadUInt16();
                         byte[] ct_buf = reader.ReadBytes((int)InputFile.ColorTableSize);
                         InputFile.Data = reader.ReadBytes((int)(InputFile.FramesCount * InputFile.Width * InputFile.Height));
                         InputFile.ColorTableSize /=3;
                         InputFile.ColorTable = new byte[InputFile.ColorTableSize, 3];
                         pos=0;
                         for (ct_pos = 0; ct_pos < InputFile.ColorTableSize; ct_pos++)
                         {
                             InputFile.ColorTable[ct_pos, 0] = ct_buf[pos++];
                             InputFile.ColorTable[ct_pos, 1] = ct_buf[pos++];
                             InputFile.ColorTable[ct_pos, 2] = ct_buf[pos++];
                         }
                     }
                     else return false;

                 }
                 break;

                default: return false;
            }

            CurrentFrame = -1;

            int data_pos = 0;
            int buf_pos = 0;
            int frame_image_pos = 0;
            byte[] frame_buf=null;
            animation = new Animation();
            animation.SizeX = InputFile.Width;
            animation.SizeY = InputFile.Height;
            animation.FramesCount = InputFile.FramesCount;
            animation.Fps = InputFile.fps;
            animation.frames = new List<Frame>(animation.FramesCount);
            if (InputFile.dataType == DataType.WithColorTable) frame_buf = new byte[animation.SizeX * animation.SizeY];
            for (int frame_num = 0; frame_num < animation.FramesCount; frame_num++)
            {
                Frame frame = new Frame();
                frame.id = frame_num;
                frame.Image = new byte[animation.SizeX * animation.SizeY * 3];
                frame_image_pos = 0;

                if (InputFile.dataType == DataType.Default)
                {
                    Array.Copy(InputFile.Data, data_pos, frame.Image, 0, frame.Image.Length);
                    data_pos += frame.Image.Length;
                }
                else if (InputFile.dataType == DataType.WithColorTable)
                {
                    Array.Copy(InputFile.Data, data_pos, frame_buf, 0, frame_buf.Length);
                    data_pos += frame_buf.Length;
                    for(buf_pos=0;buf_pos<frame_buf.Length;buf_pos++)
                    {
                        frame.Image[frame_image_pos++] = InputFile.ColorTable[frame_buf[buf_pos], 0];
                        frame.Image[frame_image_pos++] = InputFile.ColorTable[frame_buf[buf_pos], 1];
                        frame.Image[frame_image_pos++] = InputFile.ColorTable[frame_buf[buf_pos], 2];
                    }
                }
                else return false;
                
                FrameRender(frame);
                animation.frames.Add(frame);
            }

            OutputFile.colorOrder = InputFile.colorOrder;
            OutputFile.pixelOrder = InputFile.pixelOrder;
            OutputFile.dataType = InputFile.dataType;
            OutputFile.fps = InputFile.fps;
            OutputFile.FramesCount = InputFile.FramesCount;
            OutputFile.Width = InputFile.Width;
            OutputFile.Height = InputFile.Height;
            OutputFile.version = Version.v1;

            UnlockAnimation();
            return true;
        }

        public bool Encode(string FilePath,int StartFrame,int EndFrame,ColorOrder colorOrder, PixelOrder pixelOrder, DataType dataType)
        {
            //if (Lock) return false;
            int x, y, data_pos = 0;
            Frame frame;

            OutputFile = new BMAFormat();
            OutputFile.colorOrder = colorOrder;
            OutputFile.dataType = dataType;
            OutputFile.pixelOrder = pixelOrder;
            OutputFile.fps = (byte) animation.Fps;
            OutputFile.FramesCount = EndFrame - StartFrame + 1;
            OutputFile.Width = (ushort) animation.SizeX;
            OutputFile.Height =(ushort) animation.SizeY;

            bool CT_Enable = (OutputFile.dataType == DataType.WithColorTable);

            if (CT_Enable)
            {
                if (!CreateColorTable()) return false;
            }

                OutputFile.Data = new byte[OutputFile.FramesCount * OutputFile.Height * OutputFile.Width * 3];
                for(int frame_num=StartFrame; frame_num<=EndFrame;frame_num++)
                {
                    frame = animation.frames[frame_num];
                    if(frame.bitmap == null) FrameRender(frame);
                    switch(OutputFile.pixelOrder)
                    {
                        case PixelOrder.VS_TR:
                            for (x = animation.SizeX - 1; x >= 0; x--)
                            {
                                for (y = 0; y < animation.SizeY; y++)
                                {
                                    WriteDataToOutput(x, y, frame.bitmap.GetPixel(x, y), CT_Enable, ref data_pos);
                                }
                                x--;
                                if (x < 0) break;
                                for (y = animation.SizeY - 1; y >= 0; y--)
                                {
                                    WriteDataToOutput(x, y, frame.bitmap.GetPixel(x, y), CT_Enable, ref data_pos);
                                }
                            }
                            break;
                        case PixelOrder.VS_TL:
                            for (x = 0; x < animation.SizeX; x++)
                            {
                                for (y = 0; y < animation.SizeY; y++)
                                {
                                    WriteDataToOutput(x, y, frame.bitmap.GetPixel(x, y), CT_Enable, ref data_pos);
                                }
                                x++;
                                if (x == animation.SizeX) break;
                                for (y = animation.SizeY - 1; y >= 0; y--)
                                {
                                    WriteDataToOutput(x, y, frame.bitmap.GetPixel(x, y), CT_Enable, ref data_pos);
                                }
                            }
                            break;
                        case PixelOrder.VS_BR:
                            for (x = animation.SizeX - 1; x >= 0; x--)
                            {
                                for (y = animation.SizeY - 1; y >= 0; y--)
                                {
                                    WriteDataToOutput(x, y, frame.bitmap.GetPixel(x, y), CT_Enable, ref data_pos);
                                }
                                x--;
                                if (x < 0) break;
                                for (y = 0; y < animation.SizeY; y++)
                                {
                                    WriteDataToOutput(x, y, frame.bitmap.GetPixel(x, y), CT_Enable, ref data_pos);
                                }
                            }
                            break;
                        case PixelOrder.VS_BL:
                            for (x = 0; x < animation.SizeX; x++)
                            {
                                for (y = animation.SizeY - 1; y >= 0; y--)
                                {
                                    WriteDataToOutput(x, y, frame.bitmap.GetPixel(x, y), CT_Enable, ref data_pos);
                                }
                                x++;
                                if (x == animation.SizeX) break;
                                for (y = 0; y < animation.SizeY; y++)
                                {
                                    WriteDataToOutput(x, y, frame.bitmap.GetPixel(x, y), CT_Enable, ref data_pos);
                                }
                            }
                            break;
                        case PixelOrder.HS_TL:
                            for (y = 0; y < animation.SizeY; y++)
                            {
                                for (x = 0; x < animation.SizeX; x++)
                                {
                                    WriteDataToOutput(x, y, frame.bitmap.GetPixel(x, y), CT_Enable, ref data_pos);
                                }
                                y++;
                                if (y == animation.SizeY) break;
                                for (x = animation.SizeX - 1; x >=0; x--)
                                {
                                    WriteDataToOutput(x, y, frame.bitmap.GetPixel(x, y), CT_Enable, ref data_pos);
                                }
                            }
                            break;
                        case PixelOrder.HS_TR:
                            for (y = 0; y < animation.SizeY; y++)
                            {
                                for (x = animation.SizeX - 1; x >= 0; x--)
                                {
                                    WriteDataToOutput(x, y, frame.bitmap.GetPixel(x, y), CT_Enable, ref data_pos);
                                }
                                y++;
                                if (y == animation.SizeY) break;
                                for (x = 0; x < animation.SizeX; x++)
                                {
                                    WriteDataToOutput(x, y, frame.bitmap.GetPixel(x, y), CT_Enable, ref data_pos);
                                }
                            }
                            break;
                        case PixelOrder.HS_BL:
                            for (y = animation.SizeY -1 ; y >= 0; y--)
                            {
                                for (x = 0; x < animation.SizeX; x++)
                                {
                                    WriteDataToOutput(x, y, frame.bitmap.GetPixel(x, y), CT_Enable, ref data_pos);
                                }
                                y--;
                                if (y < 0) break;
                                for (x = animation.SizeX - 1; x >= 0; x--)
                                {
                                    WriteDataToOutput(x, y, frame.bitmap.GetPixel(x, y), CT_Enable, ref data_pos);
                                }
                            }
                            break;
                        case PixelOrder.HS_BR:
                            for (y = animation.SizeY - 1; y >= 0; y--)
                            {
                                for (x = animation.SizeX - 1; x >= 0; x--)
                                {
                                    WriteDataToOutput(x, y, frame.bitmap.GetPixel(x, y), CT_Enable, ref data_pos);
                                }
                                y--;
                                if (y < 0) break;
                                for (x = 0; x < animation.SizeX; x++)
                                {
                                    WriteDataToOutput(x, y, frame.bitmap.GetPixel(x, y), CT_Enable, ref data_pos);
                                }
                            }
                            break;
                        case PixelOrder.HL_TL:
                            for (y = 0; y < animation.SizeY; y++)
                            {
                                for (x = 0; x < animation.SizeX; x++)
                                {
                                    WriteDataToOutput(x, y, frame.bitmap.GetPixel(x, y), CT_Enable, ref data_pos);
                                }                               
                            }
                            break;
                        case PixelOrder.HL_TR:
                            for (y = 0; y < animation.SizeY; y++)
                            {
                                for (x = animation.SizeX - 1; x >= 0; x--)
                                {
                                    WriteDataToOutput(x, y, frame.bitmap.GetPixel(x, y), CT_Enable, ref data_pos);
                                }
                            }
                            break;
                        case PixelOrder.HL_BL:
                            for (y = animation.SizeY - 1; y >= 0; y--)
                            {
                                for (x = 0; x < animation.SizeX; x++)
                                {
                                    WriteDataToOutput(x, y, frame.bitmap.GetPixel(x, y), CT_Enable, ref data_pos);
                                }
                            }
                            break;
                        case PixelOrder.HL_BR:
                            for (y = animation.SizeY - 1; y >= 0; y--)
                            {
                                for (x = animation.SizeX - 1; x >= 0; x--)
                                {
                                    WriteDataToOutput(x, y, frame.bitmap.GetPixel(x, y), CT_Enable, ref data_pos);
                                }
                            }
                            break;
                        case PixelOrder.VL_TR:
                            for (x = animation.SizeX - 1; x >= 0; x--)
                            {
                                for (y = 0; y < animation.SizeY; y++)
                                {
                                    WriteDataToOutput(x, y, frame.bitmap.GetPixel(x, y), CT_Enable, ref data_pos);
                                }
                            }
                            break;
                        case PixelOrder.VL_TL:
                            for (x = 0; x < animation.SizeX; x++)
                            {
                                for (y = 0; y < animation.SizeY; y++)
                                {
                                    WriteDataToOutput(x, y, frame.bitmap.GetPixel(x, y), CT_Enable, ref data_pos);
                                }
                            }
                            break;
                        case PixelOrder.VL_BR:
                            for (x = animation.SizeX - 1; x >= 0; x--)
                            {
                                for (y = animation.SizeY - 1; y >= 0; y--)
                                {
                                    WriteDataToOutput(x, y, frame.bitmap.GetPixel(x, y), CT_Enable, ref data_pos);
                                }
                            }
                            break;
                        case PixelOrder.VL_BL:
                            for (x = 0; x < animation.SizeX; x++)
                            {
                                for (y = animation.SizeY - 1; y >= 0; y--)
                                {
                                    WriteDataToOutput(x, y, frame.bitmap.GetPixel(x, y), CT_Enable, ref data_pos);
                                }
                            }
                            break;
                        default: break;
                    }
                }
            
            
            //*****************Save Animation to file*******************
          FileInfo f_info = new FileInfo(FilePath);
          switch(f_info.Extension)
          {
              case ".bma":
                  using(BinaryWriter writer = new BinaryWriter(File.Open(FilePath,FileMode.OpenOrCreate)))
                  {
                      writer.Flush();
                      writer.Seek(0, SeekOrigin.Begin);
                      writer.Write('b');
                      writer.Write('m');
                      writer.Write('a');
                      writer.Write((byte)OutputFile.version);
                      writer.Write((byte)OutputFile.pixelOrder);
                      writer.Write((byte)OutputFile.colorOrder);
                      writer.Write((byte)OutputFile.dataType);
                      writer.Write(OutputFile.FramesCount);
                      writer.Write(OutputFile.fps);
                      writer.Write(OutputFile.Width);
                      writer.Write(OutputFile.Height);
                      if (OutputFile.dataType == DataType.Default)
                      {
                          writer.Write(OutputFile.Data,0,(int)(OutputFile.FramesCount * OutputFile.Width * OutputFile.Height * 3));
                      }
                      else if (OutputFile.dataType == DataType.WithColorTable)
                      {
                          writer.Write((ushort)(OutputFile.ColorTableSize * 3));
                          for(int ct_pos=0;ct_pos< OutputFile.ColorTableSize;ct_pos++)
                          {
                              writer.Write(OutputFile.ColorTable[ct_pos, 0]);
                              writer.Write(OutputFile.ColorTable[ct_pos, 1]);
                              writer.Write(OutputFile.ColorTable[ct_pos, 2]);                             
                          }
                          writer.Write(OutputFile.Data, 0, (int)(OutputFile.FramesCount * OutputFile.Width * OutputFile.Height));
                      }
                      else return false;

                  }
                  return true;

              default: return false;
          }

        }
        private void WriteDataToOutput(int x,int y,Color pixel, bool CT_Enable,ref int data_pos)
        {
            int index;
            if (CT_Enable)
            {
                index = GetIndexInColorTable(OutputFile.ColorTable,pixel);
                if(index!=-1)
                {
                    OutputFile.Data[data_pos++] = (byte)index;
                }
            }
            else
            {
                CopyColorToBuf(OutputFile.Data, data_pos, pixel, OutputFile.colorOrder);
                data_pos += 3;
            }
        }
        private void CopyColorToBuf(byte[] image,int ImagePos,Color color,ColorOrder colorOrder)
        {
            CopyColorToBuf(image, ImagePos, color.R, color.G, color.B, colorOrder);
        }
        private void CopyColorToBuf(byte[] image,int ImagePos,byte R,byte G,byte B,ColorOrder colorOrder)
        {
            if (ImagePos > (image.Length - 3)) return;
            switch (colorOrder)
            {
                case ColorOrder.BGR:
                    image[ImagePos++] = B;
                    image[ImagePos++] = G;
                    image[ImagePos] = R;
                    break;
                case ColorOrder.BRG:
                    image[ImagePos++] = B;
                    image[ImagePos++] = R;
                    image[ImagePos] = G;
                    break;
                case ColorOrder.GBR:
                    image[ImagePos++] = G;
                    image[ImagePos++] = B;
                    image[ImagePos] = R;
                    break;
                case ColorOrder.GRB:
                    image[ImagePos++] = G;
                    image[ImagePos++] = R;
                    image[ImagePos] = B;
                    break;
                case ColorOrder.RBG:
                    image[ImagePos++] = R;
                    image[ImagePos++] = B;
                    image[ImagePos] = G;
                    break;
                case ColorOrder.RGB:
                    image[ImagePos++] = R;
                    image[ImagePos++] = G;
                    image[ImagePos] = B;
                    break;
                default:
                    image[ImagePos++] = R;
                    image[ImagePos++] = G;
                    image[ImagePos] = B;
                    break;
            }

        }
        private Color GetColorFromImage(byte[] image,int ImagePos, ColorOrder colorOrder)
        {
            Color color;     
            if (ImagePos > (image.Length - 3)) return Color.Black;
            int C1 = image[ImagePos++];
            int C2 = image[ImagePos++];
            int C3 = image[ImagePos];
            switch(colorOrder)
            {
                case ColorOrder.BGR:
                    color = Color.FromArgb(C3, C2, C1);
                    break;
                case ColorOrder.BRG:
                    color = Color.FromArgb(C2, C3, C1);
                    break;
                case ColorOrder.GBR:
                    color = Color.FromArgb(C3, C1, C2);
                    break;
                case ColorOrder.GRB:
                    color = Color.FromArgb(C2, C1, C3);
                    break;
                case ColorOrder.RBG:
                    color = Color.FromArgb(C1, C3, C2);
                    break;
                case ColorOrder.RGB:
                    color = Color.FromArgb(C1, C2, C3);
                    break;
                default:
                    color = Color.FromArgb(C1, C2, C3);
                    break;
            }
            return color;
        }
        
        private int GetIndexInColorTable(byte[,] ColorTable,Color color)
        {
            byte R = (byte)color.R;
            byte G = (byte)color.G;
            byte B = (byte)color.B;
            for(int pos=0;pos<ColorTable.GetLength(0);pos++)
            {
                if (ColorTable[pos, 0] == R &&
                    ColorTable[pos, 1] == G &&
                    ColorTable[pos, 2] == B)
                {
                    return pos;
                }
            }
            return -1;
        }
        private int GetIndexInColorTable(byte[,] ColorTable, int size, byte[] Image , int ImagePos)
        {
            for (int pos = 0; pos < size; pos++ )
            {
                if (ColorTable[pos, 0] == Image[ImagePos ] &&
                    ColorTable[pos, 1] == Image[ImagePos + 1] &&
                    ColorTable[pos, 2] == Image[ImagePos + 2])
                {
                    return pos;
                }
            }
           return -1;
        }
        private bool CreateColorTable()
        {
            int x,y;
            byte[,] ColorTable = new byte[ColorTableMaxSize, 3];
            int ColorTablePos = 0;
            int ImagePos;
            foreach (Frame frame in animation.frames)
            {
                ImagePos=0;
                for(y=0;y<animation.SizeY;y++)
                    for(x=0;x<animation.SizeX;x++,ImagePos+=3)
                    {
                        if(GetIndexInColorTable(ColorTable,ColorTablePos,frame.Image,ImagePos) == -1)
                        {
                            if (ColorTablePos == ColorTableMaxSize) return false;
                            ColorTable[ColorTablePos, 0]   = frame.Image[ImagePos];
                            ColorTable[ColorTablePos, 1]   = frame.Image[ImagePos + 1];
                            ColorTable[ColorTablePos++, 2] = frame.Image[ImagePos + 2];
                        }
                    }
            }
            OutputFile.ColorTable = ColorTable;
            OutputFile.ColorTableSize = (ushort)ColorTablePos;
            return true;
        }
        public void FrameRender(Frame frame)
        {
           // if (Lock) return;
            int x, y, pos=0;
            frame.bitmap = new Bitmap(animation.SizeX, animation.SizeY);

            switch(InputFile.pixelOrder)
            {
                case PixelOrder.VS_TR:
                for (x = animation.SizeX - 1; x >= 0; x--)
                {
                  for (y = 0; y < animation.SizeY; y++)
                  {
                    frame.bitmap.SetPixel(x,y,GetColorFromImage(frame.Image,pos,InputFile.colorOrder));
                    pos += 3;
                  }
                  x--;
                  if (x < 0) break;
                  for (y = animation.SizeY - 1; y >= 0; y--)
                  {
                      frame.bitmap.SetPixel(x, y, GetColorFromImage(frame.Image, pos, InputFile.colorOrder));
                      pos += 3;
                  }
                }
                    break;
                case PixelOrder.VS_TL:
                    for (x = 0; x < animation.SizeX; x++)
                    {
                        for (y = 0; y < animation.SizeY; y++)
                        {
                            frame.bitmap.SetPixel(x, y, GetColorFromImage(frame.Image, pos, InputFile.colorOrder));
                            pos += 3;
                        }
                        x++;
                        if (x == animation.SizeX) break;
                        for (y = animation.SizeY - 1; y >= 0; y--)
                        {
                            frame.bitmap.SetPixel(x, y, GetColorFromImage(frame.Image, pos, InputFile.colorOrder));
                            pos += 3;
                        }
                    }
                    break;
                case PixelOrder.VS_BR:
                    for (x = animation.SizeX - 1; x >= 0; x--)
                    {
                        for (y = animation.SizeY - 1; y >= 0; y--)
                        {
                            frame.bitmap.SetPixel(x, y, GetColorFromImage(frame.Image, pos, InputFile.colorOrder));
                            pos += 3;
                        }
                        x--;
                        if (x < 0) break;
                        for (y = 0; y < animation.SizeY; y++)
                        {
                            frame.bitmap.SetPixel(x, y, GetColorFromImage(frame.Image, pos, InputFile.colorOrder));
                            pos += 3;
                        }
                    }
                    break;
                case PixelOrder.VS_BL:
                    for (x = 0; x < animation.SizeX; x++)
                    {
                        for (y = animation.SizeY - 1; y >= 0; y--)
                        {
                            frame.bitmap.SetPixel(x, y, GetColorFromImage(frame.Image, pos, InputFile.colorOrder));
                            pos += 3;
                        }
                        x++;
                        if (x == animation.SizeX) break;
                        for (y = 0; y < animation.SizeY; y++)
                        {
                            frame.bitmap.SetPixel(x, y, GetColorFromImage(frame.Image, pos, InputFile.colorOrder));
                            pos += 3;
                        }
                    }
                    break;
                case PixelOrder.HS_TL:
                    for (y = 0; y < animation.SizeY; y++)
                    {
                        for (x = 0; x < animation.SizeX; x++)
                        {
                            frame.bitmap.SetPixel(x, y, GetColorFromImage(frame.Image, pos, InputFile.colorOrder));
                            pos += 3;
                        }
                        y++;
                        if (y == animation.SizeY) break;
                        for (x = animation.SizeX - 1; x >= 0; x--)
                        {
                            frame.bitmap.SetPixel(x, y, GetColorFromImage(frame.Image, pos, InputFile.colorOrder));
                            pos += 3;
                        }
                    }
                    break;
                case PixelOrder.HS_TR:
                    for (y = 0; y < animation.SizeY; y++)
                    {
                        for (x = animation.SizeX - 1; x >= 0; x--)
                        {
                            frame.bitmap.SetPixel(x, y, GetColorFromImage(frame.Image, pos, InputFile.colorOrder));
                            pos += 3;
                        }
                        y++;
                        if (y == animation.SizeY) break;
                        for (x = 0; x < animation.SizeX; x++)
                        {
                            frame.bitmap.SetPixel(x, y, GetColorFromImage(frame.Image, pos, InputFile.colorOrder));
                            pos += 3;
                        }
                    }
                    break;
                case PixelOrder.HS_BL:
                    for (y = animation.SizeY - 1; y >= 0; y--)
                    {
                        for (x = 0; x < animation.SizeX; x++)
                        {
                            frame.bitmap.SetPixel(x, y, GetColorFromImage(frame.Image, pos, InputFile.colorOrder));
                            pos += 3;
                        }
                        y--;
                        if (y < 0) break;
                        for (x = animation.SizeX - 1; x >= 0; x--)
                        {
                            frame.bitmap.SetPixel(x, y, GetColorFromImage(frame.Image, pos, InputFile.colorOrder));
                            pos += 3;
                        }
                    }
                    break;
                case PixelOrder.HS_BR:
                    for (y = animation.SizeY - 1; y >= 0; y--)
                    {
                        for (x = animation.SizeX - 1; x >= 0; x--)
                        {
                            frame.bitmap.SetPixel(x, y, GetColorFromImage(frame.Image, pos, InputFile.colorOrder));
                            pos += 3;
                        }
                        y--;
                        if (y < 0) break;
                        for (x = 0; x < animation.SizeX; x++)
                        {
                            frame.bitmap.SetPixel(x, y, GetColorFromImage(frame.Image, pos, InputFile.colorOrder));
                            pos += 3;
                        }
                    }
                    break;
                case PixelOrder.HL_TL:
                    for (y = 0; y < animation.SizeY; y++)
                    {
                        for (x = 0; x < animation.SizeX; x++)
                        {
                            frame.bitmap.SetPixel(x, y, GetColorFromImage(frame.Image, pos, InputFile.colorOrder));
                            pos += 3;
                        }
                    }
                    break;
                case PixelOrder.HL_TR:
                    for (y = 0; y < animation.SizeY; y++)
                    {
                        for (x = animation.SizeX - 1; x >= 0; x--)
                        {
                            frame.bitmap.SetPixel(x, y, GetColorFromImage(frame.Image, pos, InputFile.colorOrder));
                            pos += 3;
                        }
                    }
                    break;
                case PixelOrder.HL_BL:
                    for (y = animation.SizeY - 1; y >= 0; y--)
                    {
                        for (x = 0; x < animation.SizeX; x++)
                        {
                            frame.bitmap.SetPixel(x, y, GetColorFromImage(frame.Image, pos, InputFile.colorOrder));
                            pos += 3;
                        }
                    }
                    break;
                case PixelOrder.HL_BR:
                    for (y = animation.SizeY - 1; y >= 0; y--)
                    {
                        for (x = animation.SizeX - 1; x >= 0; x--)
                        {
                            frame.bitmap.SetPixel(x, y, GetColorFromImage(frame.Image, pos, InputFile.colorOrder));
                            pos += 3;
                        }
                    }
                    break;
                case PixelOrder.VL_TR:
                    for (x = animation.SizeX - 1; x >= 0; x--)
                    {
                        for (y = 0; y < animation.SizeY; y++)
                        {
                            frame.bitmap.SetPixel(x, y, GetColorFromImage(frame.Image, pos, InputFile.colorOrder));
                            pos += 3;
                        }
                    }
                    break;
                case PixelOrder.VL_TL:
                    for (x = 0; x < animation.SizeX; x++)
                    {
                        for (y = 0; y < animation.SizeY; y++)
                        {
                            frame.bitmap.SetPixel(x, y, GetColorFromImage(frame.Image, pos, InputFile.colorOrder));
                            pos += 3;
                        }
                    }
                    break;
                case PixelOrder.VL_BR:
                    for (x = animation.SizeX - 1; x >= 0; x--)
                    {
                        for (y = animation.SizeY - 1; y >= 0; y--)
                        {
                            frame.bitmap.SetPixel(x, y, GetColorFromImage(frame.Image, pos, InputFile.colorOrder));
                            pos += 3;
                        }
                    }
                    break;
                case PixelOrder.VL_BL:
                    for (x = 0; x < animation.SizeX; x++)
                    {
                        for (y = animation.SizeY - 1; y >= 0; y--)
                        {
                            frame.bitmap.SetPixel(x, y, GetColorFromImage(frame.Image, pos, InputFile.colorOrder));
                            pos += 3;
                        }
                    }
                    break;

                default: break;
            }
          
        }
        public void SetFirstFrame()
        {
             CurrentFrame = -1;
        }
        public Bitmap GetNextFrame()
        {
                if (animation == null || animation.FramesCount == 0)
            {
                CurrentFrame = -1;
                return null;
            }
            CurrentFrame++;
            if (CurrentFrame >= animation.FramesCount) CurrentFrame = 0;
            return animation.frames[CurrentFrame].bitmap;
        }
        public void SetInputFileOptions(ColorOrder colorOder,PixelOrder pixelOrder,DataType dataType)
        {
            InputFile.colorOrder = colorOder;
            InputFile.pixelOrder = pixelOrder;
            InputFile.dataType = dataType;
        }

    }
}
