using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Do.chat;
namespace Do
{
    public partial class TEXT : Form
    {
        public  User user;


        public TEXT()
        {
            InitializeComponent();
        }
       
        private void TEXT_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("GLOBAL'E GONDERILDI....","BASARI");
            Console.WriteLine("SYSTEM" + ": " + textBox1.Text  + " in room: " + 997);
                 foreach (var pair in Program.chatUsers)
                            {

                                pair.Value.Send("j%" + 997 + "@" + "SYSTEM" + "@" + textBox1.Text + "#");
                 
                               }


        }

        private void btn_tr_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TR'YE GONDERILDI....", "BASARI");
            Console.WriteLine("SYSTEM" + ": " + textBox1.Text + " in room: " + 250);
            foreach (var pair in Program.chatUsers)
            {

                pair.Value.Send("j%" + 250 + "@" + "SYSTEM" + "@" + textBox1.Text + "#");

            }
        }

        private void btn_en_Click(object sender, EventArgs e)
        {
            MessageBox.Show("INGILIZ'E GONDERILDI....", "BASARI");
            Console.WriteLine("SYSTEM" + ": " + textBox1.Text + " in room: " + 256);
            foreach (var pair in Program.chatUsers)
            {

                pair.Value.Send("j%" + 256 + "@" + "SYSTEM" + "@" + textBox1.Text + "#");

            }
        }

        private void btn_isp_Click(object sender, EventArgs e)
        {
          MessageBox.Show("ISPANYOL'A GONDERILDI....", "BASARI");
          Console.WriteLine("SYSTEM" + ": " + textBox1.Text + " in room: " + 255);
            foreach (var pair in Program.chatUsers)
            {

                pair.Value.Send("j%" + 255 + "@" + "SYSTEM" + "@" + textBox1.Text + "#");

            }  
        }

        private void button6_Click(object sender, EventArgs e)
        {
            KICK kick = new KICK();
            kick.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ChatLog log = new ChatLog();
            log.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BAN ban = new BAN();
            ban.Show();
        }
    }
}
