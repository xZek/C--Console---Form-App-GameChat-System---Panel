﻿using Do.chat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.IO;

namespace Do
{
    public partial class KICK : Form
    {
        public User user;
        private readonly byte[] _buffer = new byte[2048];
       IAsyncResult result;
       public Socket _handler;

        public KICK()
        {
            InitializeComponent();
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            foreach (var pair in Program.chatUsers)
            {
          
                var bytesread = 1;
                var content = Encoding.UTF8.GetString(_buffer, 0, bytesread);

                if (content == "")
                {
                    return;
                }
               
                var packet = content.Replace("@", "%").Split('%');

                if (String.Equals(pair.Value.Username, textBox1.Text, StringComparison.CurrentCultureIgnoreCase))
                {
                 
                    pair.Value.Send("as%#");
                    pair.Value._handler.Shutdown(SocketShutdown.Both);
                    pair.Value._handler.Close();
                    pair.Value._handler = null;

                    Program.chatUsers.Remove(pair.Key);
                    foreach (var usar in Program.chatUsers)
                    {

                        usar.Value.Send("fk%997@" + textBox1.Text + "  kicked.#");
                     
                    }
                    return;
                }
            }
            MessageBox.Show(textBox1.Text + "Adlı Kullanıcı Başarıyla Yasaklandı", "BASARI");
            StreamWriter Info = new StreamWriter(@"C:\Users\Android-AP\Desktop\EMULATOR CONTROL SYSTEM\adminislem.txt");
            Info.Write("[" + DateTime.Now + "] - [" + "SYSTEM" + "] >> " + "" + ":: '" + textBox1.Text + "   KICK FROM SYSTEM" + "'");
            Info.Close();
        }

        private void KICK_Load(object sender, EventArgs e)
        {
              
        }
    }
}
