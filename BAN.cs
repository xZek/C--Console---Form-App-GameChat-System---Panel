using Do.chat;
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

namespace Do
{
    public partial class BAN : Form
    {
        public User user;
        private readonly byte[] _buffer = new byte[2048];
        IAsyncResult result;
        public Socket _handler;

        public BAN()
        {
            InitializeComponent();
        }

        private void BAN_Load(object sender, EventArgs e)
        {

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

                    pair.Value.Send("at%#");
                    pair.Value._handler.Shutdown(SocketShutdown.Both);
                    pair.Value._handler.Close();
                    pair.Value._handler = null;
                    Program.chatUsers.Remove(pair.Key);
                   
                    Program.BanUser(pair.Value.UserId, pair.Value.Username);
                    return;
                 
                }
            }
        }

      
    }
}
