using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESP8266_Programmer_ForStm32;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace LED_Board_Access
{
    public partial class LED_Board_Manager : Form
    {
        private ESP8266_CommunicationProtocol uop;
        TcpClient client;
        NetworkStream tcp_stream;
        BinaryWriter tcp_writer;
        BinaryReader tcp_reader;
        string IP;
        int Port;
        string UploadFilePath = "";
        const int TCP_Port = 80;

        Thread uploadThread;
        Thread uploadFilesThread;

        List<string> files;
        public LED_Board_Manager()
        {
            InitializeComponent();
            uop = new ESP8266_CommunicationProtocol();
            files = new List<string>(20);           
        }
        public void AddFile(string path)
        {
            if(!files.Exists((x => x== path))) files.Add(path);
        }
        public void ClearFiles()
        {
            files.Clear();
        }
        private bool Connect()
        {
            return Connect(IP_textBox.Text, TCP_Port);
        }
        private bool Connect(string _IP, int _Port)
        {
            if (client != null && client.Connected) return true;
            try
            {
                IP = _IP;
                Port = _Port;
                client = new TcpClient();
                IPAddress ipAddr = IPAddress.Parse(IP);
                IPEndPoint endPoint = new IPEndPoint(ipAddr, Convert.ToInt32(Port));

                client.Connect(endPoint);
                tcp_stream = client.GetStream();
                tcp_writer = new BinaryWriter(tcp_stream);
                tcp_reader = new BinaryReader(tcp_stream);

                uop._Send = new ESP8266_CommunicationProtocol.SendData(SendData);
                uop._Read = new ESP8266_CommunicationProtocol.ReadData(ReadData);
                uop.WriteFileProgress = new ESP8266_CommunicationProtocol.WriteFileProgressDelegate(DisplayWriteFileProgress);

                if (uop.GetStatus() != ESP8266_CommunicationProtocol.UOP_StatusType.OK)
                {
                    MessageBox.Show("LED Board doesn't answer", "Connection status", MessageBoxButtons.OK);
                    Disconnect();
                    return false;
                }
                return true;
            }
            catch
            {
                MessageBox.Show("Network error", "Connection status", MessageBoxButtons.OK);
                return false;
            }
        }
        private void Disconnect()
        {
            if (client == null) return;

            if (tcp_writer != null)
            {
                tcp_writer.Flush();
                tcp_writer.Close();
            }
            if (tcp_reader != null)
            {
                tcp_reader.Close();
            }
            if (tcp_stream != null) tcp_stream.Close();
            client.Close();
        }
        public bool SendData(byte[] data, int size)
        {
            if (client == null || !client.Connected)
            {
                if (!Connect(IP, Port)) return false;
                while (!client.Connected) ;
            }
            try
            {
                tcp_writer.Write(data, 0, size);
                return true;
            }
            catch
            {
                return false;
            }

        }
        int ReadData(byte[] data)
        {
            if (client == null || !client.Connected)
            {
                if (!Connect(IP, Port)) return -1;
            }
            try
            {
                tcp_stream.ReadTimeout = 5000;
                return tcp_stream.Read(data, 0, data.Length);
            }
            catch
            {
                return (int)-1;
            }
        }
        private void UploadFile(object path)
        {
            string file = path as string;
            if (File.Exists(file))
            {
                uop.DeleteFile(file);
                /*if (uop.DeleteFile(file) == ESP8266_CommunicationProtocol.UOP_StatusType.Error) 
                {
                    MessageBox.Show(file + " delete error");
                    return;
                }*/
                if (uop.WriteFile(file, true) == ESP8266_CommunicationProtocol.UOP_StatusType.OK)
                {
                    return;
                }

            }
            Thread.Sleep(5);
        }
        private void UploadFiles()
        {
            FileInfo finfo;
            int fnum = 0;
            bool Loaded = true;
            if (!Connect()) return;
            if (uop.LB_Stop() != ESP8266_CommunicationProtocol.UOP_StatusType.OK)
            {
                buttonLoadProjectEnabled(true);
                buttonRestartEnabled(true);
                buttonUpdateSKDEnabled(true);
                return;
            }
            foreach (string file in files)
            {
          
                finfo = new FileInfo(file);
                DisplayWriteFilesProgress(finfo.Name, (int)finfo.Length,(fnum * 100) / files.Count);
                uploadThread = new Thread(UploadFile);
                uploadThread.Start(file);
                uploadThread.Join();
                if (progressBar_File.Value != 100)
                {
                    MessageBox.Show("Upload error");
                    Loaded = false;
                    break;
                }
                fnum++;
                DisplayWriteFilesProgress(finfo.Name, (int)finfo.Length, (fnum * 100) / files.Count);
            }

           // if (uop.LB_Start() != ESP8266_CommunicationProtocol.UOP_StatusType.OK) MessageBox.Show("Can't run LED Board", "LEB Board Info", MessageBoxButtons.OK);
            //if (Loaded)
           // {
                uop.MCU_StopBoot();
                //MessageBox.Show("Project Loaded", "LEB Board Info", MessageBoxButtons.OK);
            //}
            buttonLoadProjectEnabled(true);
            buttonRestartEnabled(true);
            buttonUpdateSKDEnabled(true);
            Disconnect();  
        }
        private void UploadProjectFile()
        {
            FileInfo finfo;
            if (!Connect()) return;
            if (uop.LB_Stop() != ESP8266_CommunicationProtocol.UOP_StatusType.OK)
            {
                buttonLoadProjectEnabled(true);
                buttonRestartEnabled(true);
                buttonUpdateSKDEnabled(true);
                return;
            }
            string file = "";
            foreach (string s in files)
            {
                if (Path.GetExtension(s) == ".lbprj")
                {
                    file = s;
                    break;
                }
            }
            if (file == "") return;

                finfo = new FileInfo(file);
                DisplayWriteFilesProgress(finfo.Name, (int)finfo.Length, 0);
                uploadThread = new Thread(UploadFile);
                uploadThread.Start(file);
                uploadThread.Join();
                if (progressBar_File.Value != 100)
                {
                    MessageBox.Show("Upload error");
                }
                else   DisplayWriteFilesProgress(finfo.Name, (int)finfo.Length, 100);
                       
            uop.MCU_StopBoot();

            buttonLoadProjectEnabled(true);
            buttonRestartEnabled(true);
            buttonUpdateSKDEnabled(true);
            Disconnect();
        }
        private void button_check_Click(object sender, EventArgs e)
        {
            if (Connect(IP_textBox.Text, TCP_Port))
            {
                MessageBox.Show("Connection: Ok", "Connection status", MessageBoxButtons.OK);
                Disconnect();
            }
        }

        private void button_Restart_Click(object sender, EventArgs e)
        {
            if(Connect())
            {
                if (uop.MCU_StopBoot() == ESP8266_CommunicationProtocol.UOP_StatusType.OK)
               {
                   MessageBox.Show("Restart: Ok", "LED Board Info", MessageBoxButtons.OK);
               }
                Disconnect();
            }
        }

        private void button_LoadProject_Click(object sender, EventArgs e)
        {
            this.Size = new Size(425,307);
            progressBar_File.Value = 0;
            progressBar_Files.Value = 0;
            uploadFilesThread = new Thread(new ThreadStart(UploadFiles));
            uploadFilesThread.Start();
            button_LoadProject.Enabled = false;
            button_Restart.Enabled = false;
            button_UpdateSKD.Enabled = false;
        }
        private void buttonLoadProjectEnabled(bool state)
        {
            if(button_LoadProject.InvokeRequired)
            {
                button_LoadProject.BeginInvoke(new Action<bool> ((s)=>button_LoadProject.Enabled=s),state);
            }
            else
            {
                button_LoadProject.Enabled=state;
            }
        }
        private void buttonRestartEnabled(bool state)
        {
            if (button_Restart.InvokeRequired)
            {
                button_Restart.BeginInvoke(new Action<bool>((s) => button_Restart.Enabled = s), state);
            }
            else
            {
                button_Restart.Enabled = state;
            }
        }
        private void buttonUpdateSKDEnabled(bool state)
        {
            if (button_UpdateSKD.InvokeRequired)
            {
                button_UpdateSKD.BeginInvoke(new Action<bool>((s) => button_UpdateSKD.Enabled = s), state);
            }
            else
            {
                button_UpdateSKD.Enabled = state;
            }
        }
        private void DisplayWriteFilesProgress(string FileName,int FileSize,int part)
        {
            Action action = () =>
            {
                label_filesProgress.Text = FileName + " (";
                if (FileSize < 1024)
                {
                    label_filesProgress.Text += FileSize.ToString() + "bytes) ";
                }
                else if (FileSize < 1024 * 1024)
                {
                    label_filesProgress.Text += ((double)FileSize / 1024.0).ToString("f2") + "KB) ";
                }
                else
                {
                    label_filesProgress.Text += ((double)FileSize / (1024.0 * 1024.0)).ToString("f2") + "MB) ";
                }
                progressBar_Files.Value = part;
            };
            if (progressBar_File.InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }
        private void DisplayWriteFileProgress(int part)
        {
            Action action = () => 
            {
                label_fileProgress.Text = part.ToString() + "%";
                progressBar_File.Value = part;
            };
            if (progressBar_File.InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }

        private void button_UpdateSchedule_Click(object sender, EventArgs e)
        {
            this.Size = new Size(425, 307);
            progressBar_File.Value = 0;
            progressBar_Files.Value = 0;
            uploadFilesThread = new Thread(new ThreadStart(UploadProjectFile));
            uploadFilesThread.Start();
            button_LoadProject.Enabled = false;
            button_Restart.Enabled = false;
            button_UpdateSKD.Enabled = false;
                        
        }
    }
}
