using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client3
{
    public partial class Client3 : Form
    {
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.0.116"), 9050);

        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public Client3()
        {
            InitializeComponent();
            Connect();
            //Disconnect();
        }

        
        
        void Connect()
        {
            try
            {
                server.Connect(ipep);

            }
            catch (SocketException e)
            {
                Console.WriteLine("Unable to connect to server.");

                Console.WriteLine(e.ToString());

                return;
            }

            string stringData;
            byte[] data = new byte[1024];
            int recv = server.Receive(data);
            stringData = Encoding.UTF8.GetString(data, 0, recv);

        }

        //void Disconnect()
        //{
        //    try
        //    {
        //        server.Disconnect(ipep);
        //    }
        //    catch (SocketException e)
        //    {
        //        MessageBox.Show("DISCONNECT FROM SERVER");
        //        Close();
        //    }
        //}
        Encoding OutputEncoding = Encoding.UTF8;
        
        string input;
      
        private void btnConvert_Click(object sender, EventArgs e)
        {          
            input = txbNhap.Text;

            double n;

            if (!double.TryParse(input, out n))
            {
                MessageBox.Show("Sai định dạng", "Sai định dạng", MessageBoxButtons.OK);

                return;
            }

            if (rdbtnTV.Checked == true)
            {
                server.Send(Encoding.UTF8.GetBytes("tv " + input));
                byte[] data = new byte[1024];

                int recv = server.Receive(data);

                string stringData = Encoding.UTF8.GetString(data, 0, recv);

                byte[] utf8string = System.Text.Encoding.UTF8.GetBytes(stringData);

                txbKetqua.Text = stringData;

            }
            else if (rdbtnTA.Checked == true)
            {
                server.Send(Encoding.UTF8.GetBytes("ta " + input));
                byte[] data = new byte[1024];

                int recv = server.Receive(data);

                string stringData = Encoding.UTF8.GetString(data, 0, recv);

                byte[] utf8string = System.Text.Encoding.UTF8.GetBytes(stringData);

                txbKetqua.Text = stringData;

            }

        }

        private void txbNhap_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txbNhap.Clear();
            txbKetqua.Clear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txbNhap_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '-')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && txbNhap.Text.IndexOf('.') > -1) e.Handled = true;

            else if (e.KeyChar == '-' && txbNhap.Text.IndexOf('-') > -1) e.Handled = true;
        }
    }
}
