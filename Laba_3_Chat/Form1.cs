using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

namespace KSIS_3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        const int typeSize = 1;
        const int typeShift = 0;
        const int lengthSize = 2;
        const int lengthShift = 1;
        const int hdrSize = 3;
        const int dataShift = 3;
        const int PORT_UDP= 9876;
        const int PORT_TCP = 9875;

        public string ipAddressBrodcast;
        public string IEPMemoried;
        public bool IsConnected = false;
        public List<Client> clients = new List<Client>();
        Task UdpRec;
        Task TcpRec;

        public class Client
        {
            public NetworkStream stream;
            public IPEndPoint iep;
            public String name;
            public Client(IPEndPoint iepc, string str, NetworkStream streamlocal)
            {
                stream = streamlocal;
                iep = iepc;
                name = str;
            }
        }

        class Packet
        {
            public byte type;
            public UInt16 length;
            public byte[] data;

            public Packet(byte typelocal)
            {
                type = typelocal;
                length = hdrSize;
                data = null;
            }

            public Packet(byte typelocal, string datalocal)
            {
                type = typelocal;
                data = Encoding.Unicode.GetBytes(datalocal);
                length = (UInt16)(hdrSize + data.Length);
            }

            public Packet(byte[] datalocal)
            {
                type = datalocal[0];
                length = BitConverter.ToUInt16(datalocal, typeSize);
                data = new byte[length - hdrSize];
                Buffer.BlockCopy(datalocal, hdrSize, data, 0, length - hdrSize);
            }
            public byte[] getBytes()
            {
                byte[] datalocal = new byte[length];
                Buffer.BlockCopy(BitConverter.GetBytes(type), 0, datalocal, typeShift, typeSize);
                Buffer.BlockCopy(BitConverter.GetBytes(length), 0, datalocal, lengthShift, lengthSize);
                if (data != null)
                    Buffer.BlockCopy(data, 0, datalocal, dataShift, length - hdrSize);
                return datalocal;
            }
        }

        public static String GetName(IPEndPoint iep, List<Client> list)
        {
            foreach(Client clientcheck in list)
            {
                if (IPEndPoint.Equals(clientcheck.iep,iep))
                {
                    return clientcheck.name;
                }
            }
            return null;
        }

        private void Connect()
        {
            UdpClient client = new UdpClient();
            client.EnableBroadcast = true;
            try
            {
                //высчитывание широковещательного адреса
                int intMyIP = BitConverter.ToInt32(IPAddress.Parse(GetLocalIPAddress()).GetAddressBytes(), 0);
                int intMask = BitConverter.ToInt32(IPAddress.Parse("255.255.255.0").GetAddressBytes(), 0);
                int intMaskReversed = BitConverter.ToInt32(IPAddress.Parse("0.0.0.255").GetAddressBytes(), 0);
                int intMulted = intMyIP & intMask;
                intMulted = intMulted ^ intMaskReversed;
                ipAddressBrodcast = new IPAddress(BitConverter.GetBytes(intMulted)).ToString();
                MessageBox.Show(ipAddressBrodcast);
                Packet MessageCache = new Packet(1, textBoxName.Text);
                client.Send(MessageCache.getBytes(), MessageCache.length, ipAddressBrodcast, PORT_UDP);
                client.Close();

                if (!IsConnected)
                {
                    IsConnected = true;
                    UdpRec = new Task(UdpReceive);
                    UdpRec.Start();
                    TcpRec = new Task(TcpReceive);
                    TcpRec.Start();
                }
                btnSend.Enabled = true;
            }
            catch (Exception ex)
            {
                client.Close();
                MessageBox.Show(ex.Message);
            }
        }
        private void UdpReceive()
        {
            while (true)
            {
                UdpClient client = new UdpClient(PORT_UDP);
                client.EnableBroadcast = true;
                IPEndPoint iep = null;
                byte[] data = client.Receive(ref iep);
                Packet MessageBroadCast = new Packet(data);
                if (String.Compare(iep.Address.ToString(), GetLocalIPAddress()) == 0)
                    return;
                client.Close();    
                switch (MessageBroadCast.type)
                {
                    case 1:
                        TcpClient clientTcp = new TcpClient(iep.Address.ToString(), PORT_TCP);
                        NetworkStream stream = clientTcp.GetStream();
                        Client cl = new Client(new IPEndPoint(iep.Address, PORT_TCP), Encoding.Unicode.GetString(MessageBroadCast.data), stream);
                        IEPMemoried = cl.iep.ToString();
                        bool flag = true;
                        if (clients.Contains(cl))
                        {
                            flag = false;
                        }
                        if (flag)
                        {
                            clients.Add(cl);
                            this.Invoke(new MethodInvoker(() =>
                            {
                                listUsers.Items.Add(cl.name + "(" + cl.iep.ToString() + ")");
                                listChat.Items.Add(DateTime.Now + " " + cl.name + ": подключился");
                            }));

                            MessageBroadCast = new Packet(1,textBoxName.Text);  
                            stream.Write(MessageBroadCast.getBytes(), 0, MessageBroadCast.getBytes().Length);
                            stream.Close();
                        }
                        break;
                    case 0:
                        foreach (Client ClientCheck in clients)
                        {
                            if (ClientCheck.iep.ToString() == IEPMemoried)
                            {
                                clients.Remove(ClientCheck);
                                this.Invoke(new MethodInvoker(() =>
                                {
                                    listUsers.Items.Remove(ClientCheck.name + "(" + ClientCheck.iep.ToString() + ")");
                                    listChat.Items.Add(DateTime.Now + " " + ClientCheck.name + ": покинул чат");
                                }));
                                break;
                            }
                        }
                        break;
                }
            }
        }

        private void TcpReceive()
        {
            while (IsConnected)
            {
                TcpListener listener = new TcpListener(IPAddress.Parse(GetLocalIPAddress()), PORT_TCP);
                listener.Start();
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream stream = client.GetStream();
                byte[] data = new byte[65536];
                stream.Read(data, 0, data.Length);
                Packet MessageTcp = new Packet(data);
                IPEndPoint iep = new IPEndPoint(IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()), PORT_TCP);
                switch (MessageTcp.type)
                {
                    case 1:
                        Client ClientCheck = new Client(iep, Encoding.Unicode.GetString(MessageTcp.data), stream);
;                        if (!clients.Contains(ClientCheck))
                        {
                            clients.Add(ClientCheck);
                            this.Invoke(new MethodInvoker(() =>
                            {
                                listUsers.Items.Add(ClientCheck.name + "(" + ClientCheck.iep.ToString() + ")");
                                listChat.Items.Add(DateTime.Now + " " + ClientCheck.name + ": присоединился");
                            }));
                        }
                        break;
                    case 2:
                        foreach (Client ClientsCheck in clients)
                        {
                            if (ClientsCheck.iep.Equals(iep))
                            {
                                this.Invoke(new MethodInvoker(() =>
                                {
                                    listChat.Items.Add(DateTime.Now + " " + ClientsCheck.name + ": " + Encoding.Unicode.GetString(MessageTcp.data));
                                }));
                                break;
                            }
                        }
                        break;
                }
                client.Close();
                stream.Close();
                listener.Stop();
            }
        }
        public static string GetLocalIPAddress()
        {
            string localIP;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0);
            try
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint Point = socket.LocalEndPoint as IPEndPoint;
                localIP = Point.Address.ToString();
            }
            catch
            {
                IPEndPoint Point = socket.LocalEndPoint as IPEndPoint;
                localIP = Point.Address.ToString();
            }
            return localIP;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            Connect();
            btnFind.Enabled = false;       
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Packet MessageTextBox = new Packet(2,textInputMessage.Text);
            listChat.Items.Add(DateTime.Now + " " + "Вы: " + textInputMessage.Text);
            textInputMessage.Text = "";  
            foreach (Client ClientCheck in clients)
            {
                TcpClient clientTcp = new TcpClient(ClientCheck.iep.Address.ToString(), PORT_TCP);
                NetworkStream stream = clientTcp.GetStream();
                stream.Write(MessageTextBox.getBytes(), 0, MessageTextBox.getBytes().Length);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (String.Compare(textBoxName.Text, "") != 0)
            {
                btnFind.Enabled = true;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            UdpClient client = new UdpClient();
            client.EnableBroadcast = true;
            try
            {
                Packet MessageBroadCast = new Packet(0);
                client.Send(MessageBroadCast.getBytes(), MessageBroadCast.length, ipAddressBrodcast, PORT_UDP);
            }
            catch (Exception ex)
            {
                if (IsConnected)
                  MessageBox.Show(ex.Message);
            }
            finally
            {
                client.Close();
            }
        }

    }
}
