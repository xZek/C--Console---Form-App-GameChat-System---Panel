using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace Do
{
    public partial class ChatLog : Form
    {
        public ChatLog()
        {
            InitializeComponent();
        }

        private void ChatLog_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var chatlog = File.ReadLines(@"C:\Users\Android-AP\Desktop\EMULATOR CONTROL SYSTEM\bin\Release\chatLog.txt", Encoding.GetEncoding("iso-8859-9"));
            foreach (var name in chatlog)
            {
                listBox1.Items.Add(name);
            }
        }
    }
}
