﻿using System;
using System.Windows.Forms;
using SharpPcap;
using PacketDotNet;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Microsoft.VisualBasic;
using NetworkTrafficCSharpForm.Properties;
using System.Linq;
//if scaled up make sure pid's are only being passed for the current machine's IP.
//Things to change between nodes  333
namespace NetworkTrafficCSharpForm
{
    public partial class Form1 : Form
    {
        //  SqlConnection connection = null;
        // Define your connection string to connect to your SQL Server database
        string connectionString = Settings.Default.CS;

        // Define the SQL query to insert the data into the database
        string query = "INSERT INTO IPLog (Organization, OrgName, OrgId, Address, City, StateProv, PostalCode, Country, SourceIP, DestinationIP, Protocol, PacketSize, PacketColor, HasPayloadPacket, HasPayloadData, IsPayloadInitialized, HeaderLength, HeaderData, HopLimit, PayloadDataLength, PayloadPacket, TimeToLive, TotalLength, TotalPacketLength, Version) " +
                "VALUES (@Organization, @OrgName, @OrgId, @Address, @City, @StateProv, @PostalCode, @Country, @SourceIP, @DestinationIP, @Protocol, @PacketSize, @PacketColor, @HasPayloadPacket, @HasPayloadData, @IsPayloadInitialized, @HeaderLength, @HeaderData, @HopLimit, @PayloadDataLength, @PayloadPacket, @TimeToLive, @TotalLength, @TotalPacketLength, @Version)";
        // Import the kernel32.dll library
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private ICaptureDevice captureDevice;
        public Form1()
        {
            InitializeComponent();
            AllocConsole();
            // Redirect the standard output stream to the console window
            StreamWriter writer = new StreamWriter(Console.OpenStandardOutput())
            {
                AutoFlush = true
            };
            Console.SetOut(writer);         
        }
        private void OnPacketArrival(object sender, PacketCapture e)
        {
            // Get the captured packet
            var rawCapture = e.GetPacket();
            var packet = Packet.ParsePacket(rawCapture.LinkLayerType, rawCapture.Data);

            // Check if the packet is an IP packet
            if (packet.PayloadPacket is IPPacket ipPacket)
            {
                string sourceordest = string.Empty;
                // Check if the packet is coming from or going to a computer on your network
                if (ipPacket.DestinationAddress.ToString().StartsWith(TextBox1.Text) || ipPacket.SourceAddress.ToString().StartsWith(TextBox1.Text))
                {
                    if (!ipPacket.SourceAddress.ToString().StartsWith(TextBox1.Text.Split('.').First()))
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            // Create the SqlCommand object with the connection
                            SqlCommand cmd = new SqlCommand
                            {
                                Connection = connection,
                                // Set the command text and parameters
                                CommandText = "SELECT COUNT(*) FROM IPLog WHERE SourceIP = @SourceIP"
                            };
                            // Add any necessary parameters
                            cmd.Parameters.AddWithValue("@SourceIP", ipPacket.SourceAddress.ToString());
                            // Execute the scalar query
                            int count = (int)cmd.ExecuteScalar();
                            connection.Close();
                            sourceordest = "source";
                            if (count == 0)
                            {                               
                                GetIPData(ipPacket.SourceAddress.ToString());
                            }
                        }
                    }
                    else if (!ipPacket.DestinationAddress.ToString().StartsWith(TextBox1.Text.Split('.').First()))
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            // Create the SqlCommand object with the connection
                            SqlCommand cmd = new SqlCommand
                            {
                                Connection = connection,

                                // Set the command text and parameters
                                CommandText = "SELECT COUNT(*) FROM IPLog WHERE DestIP = @DestIP"
                            };
                            // Add any necessary parameters
                            cmd.Parameters.AddWithValue("@DestIP", ipPacket.DestinationAddress.ToString());
                            // Execute the scalar query
                            int count1 = (int)cmd.ExecuteScalar();
                            connection.Close();
                            sourceordest = "dest";
                            if (count1 == 0)
                            {                               
                                GetIPData(ipPacket.DestinationAddress.ToString());
                            }
                        }
                    }

                    int sport = 0;
                    int dport = 0;
                    string proto = string.Empty;
                    if (ipPacket.PayloadPacket is TcpPacket tcpPacket1)
                    {
                        sport = tcpPacket1.SourcePort;
                        dport = tcpPacket1.DestinationPort;
                        proto = "tcp";
                    }
                    if (ipPacket.PayloadPacket is UdpPacket updPacket1)
                    {
                        sport = updPacket1.SourcePort;
                        dport = updPacket1.DestinationPort;
                        proto = "udp";
                    }
                    string yar = string.Empty;
                    if (ipPacket.DestinationAddress.ToString().StartsWith(TextBox3.Text)) //333
                    {
                        yar = YaMum(dport, proto);
                    }
                    else if (ipPacket.SourceAddress.ToString().StartsWith(TextBox3.Text))
                    {
                        yar = YaMum(sport, proto);
                    }
                    string[] splits = yar.Split(' ');
                    int lastindex = splits.Length - 1;
                    string exe = string.Empty;
                    int pid = 99999;
                    if (lastindex > 0)
                    {
                        exe = splits[0];
                        pid = int.Parse(splits[lastindex]);
                    }
                    //   ipPacket.ParentPacket.PayloadPacket.
                    // Display the source and destination IP addresses and the protocol of the captured packet                    
                    Console.WriteLine("Program: " + exe);
                    Console.WriteLine("Pid: " + pid);
                    Console.WriteLine("Source IP: " + ipPacket.SourceAddress.ToString() + ":" + sport);
                    Console.WriteLine("Destination IP: " + ipPacket.DestinationAddress.ToString() + ":" + dport);
                    Console.WriteLine("Protocol: " + ipPacket.Protocol.ToString());
                    Console.WriteLine("Packet Size: " + ipPacket.Bytes.Length.ToString());
                    Console.WriteLine("Packet Color: " + ipPacket.Color.ToString());
                    Console.WriteLine("Has Payload Packet: " + ipPacket.HasPayloadPacket.ToString());
                    Console.WriteLine("Has Sub Payload Packet: " + ipPacket.PayloadPacket.HasPayloadPacket.ToString());
                    Console.WriteLine("Has Payload Data: " + ipPacket.HasPayloadData.ToString());
                    Console.WriteLine("Is Payload Initialized: " + ipPacket.IsPayloadInitialized.ToString());
                    Console.WriteLine("Header Length: " + ipPacket.HeaderLength.ToString());
                    Console.WriteLine("Header Data:");
                    string bitten = string.Empty;
                    foreach (byte b in ipPacket.HeaderData)
                    {
                        bitten += b.ToString("X2") + " ";
                        Console.Write(b.ToString("X2") + " ");
                    }
                    Console.WriteLine();
                    Console.WriteLine("Hop Limit: " + ipPacket.HopLimit.ToString());
                    // Console.WriteLine("Parent Packet: " + ipPacket.ParentPacket.ToString());
                    Console.WriteLine("Payload Data Length: " + ipPacket.PayloadLength.ToString());
                    bool yee = ipPacket.PayloadPacket.HasPayloadData;
                    if (yee)
                    {
                        Console.Write("Payload Data: ");
                        foreach (byte b in ipPacket.PayloadPacket.PayloadData)
                        {
                            Console.Write(b.ToString("X2") + " ");
                        }

                        Console.WriteLine();
                    }
                    Console.WriteLine("Payload Packet: " + ipPacket.PayloadPacket.ToString());
                    Console.WriteLine("TimeToLive: " + ipPacket.TimeToLive.ToString());
                    Console.WriteLine("Total Length: " + ipPacket.TotalLength.ToString());
                    Console.WriteLine("Total Packet Length: " + ipPacket.TotalPacketLength.ToString());
                    Console.WriteLine("Version: " + ipPacket.Version.ToString());
                    Console.WriteLine("");
                    //  Console.WriteLine("Protocol: " + ipPacket.Bytes.Length.ToString());
                    if (carrylist.Count > 0)
                    {
                        InsertPacketToDatabase(exe, pid, sourceordest, ipPacket.SourceAddress.ToString(), ipPacket.DestinationAddress.ToString(), ipPacket.Protocol.ToString(), ipPacket.Bytes.Length, ipPacket.Color.ToString(),
                                             ipPacket.HasPayloadPacket, ipPacket.HasPayloadData, ipPacket.IsPayloadInitialized, ipPacket.HeaderLength, bitten,
                                             ipPacket.HopLimit, ipPacket.PayloadLength, ipPacket.PayloadPacket.ToString(), ipPacket.TimeToLive, ipPacket.TotalLength, ipPacket.TotalPacketLength,
                                             ipPacket.Version.ToString(), carrylist[0], carrylist[1], carrylist[2], carrylist[3], carrylist[4], carrylist[5], carrylist[6], carrylist[7]);
                    }
                    else
                    {
                        InsertPacketToDatabase(exe, pid, sourceordest, ipPacket.SourceAddress.ToString(), ipPacket.DestinationAddress.ToString(), ipPacket.Protocol.ToString(), ipPacket.Bytes.Length, ipPacket.Color.ToString(),
                                             ipPacket.HasPayloadPacket, ipPacket.HasPayloadData, ipPacket.IsPayloadInitialized, ipPacket.HeaderLength, bitten,
                                             ipPacket.HopLimit, ipPacket.PayloadLength, ipPacket.PayloadPacket.ToString(), ipPacket.TimeToLive, ipPacket.TotalLength, ipPacket.TotalPacketLength,
                                             ipPacket.Version.ToString(), null, null, null, null, null, null, null, null);
                    }
                    carrylist.Clear();
                }
                else
                {
                    Console.WriteLine("Stray Packet ALERT " + ipPacket.DestinationAddress + ":DestIP  &  SourceIP:" + ipPacket.SourceAddress.ToString());
                }

            }
        }

        private static string YaMum(int port, string proto)
        { 
        //int port = 8080; // Specify the port number you're interested in

            string netstatOutput = RunNetstatCommand(port, "ESTABLISHED", proto);

            if (netstatOutput == string.Empty)
            {
                netstatOutput = RunNetstatCommand(port, "LISTENING", proto);
            }
            if (netstatOutput == string.Empty)
            {
                return "";
            }

            int lastSpaceIndex = netstatOutput.LastIndexOf(' '); // Find the index of the last space
            string lastWord = netstatOutput.Substring(lastSpaceIndex + 1); // Extract the substring starting from the index after the last space
            int[] yee = { 0 };
            bool isNumeric = int.TryParse(lastWord, out _);
            if (isNumeric)
            {
                int geeze = int.Parse(lastWord);
                yee[0] += geeze;              
                // Console.WriteLine(yee[1]);
                string[] yeee = GetProgramNames(yee);
                if (yeee.Length > 0)
                {
                    //var yeet = GetProgramNames(yee)[0];
                    netstatOutput = yeee[0] + " : " + netstatOutput;
                }
               
            }
            //Console.WriteLine(netstatOutput.Trim());
            return netstatOutput;
        }
         
        public static string RunNetstatCommand(int port, string type, string proto)
        {
            // Create a new process to execute the netstat command
        Process process = new Process();
        string arguments = $@"/C netstat -ano -p {proto} | findstr {type} | findstr :{port}"; 
        // Set the start info for the process
        process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = arguments; //@"-ano -p tcp | findstr LISTENING | findstr :" + port;

        // Redirect the output to read the command result
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        // Start the process
        process.Start();

        // Read the output
        string output = process.StandardOutput.ReadToEnd();

        // Wait for the process to exit
        process.WaitForExit();

        return output;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            var devices = CaptureDeviceList.Instance;
            ComboBox1.Items.AddRange(devices.ToArray());


            int desiredIndex = Settings.Default.Device; // Specify the desired index

            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            if (desiredIndex >= 0 && desiredIndex < networkInterfaces.Length)
            {
                NetworkInterface desiredInterface = networkInterfaces[desiredIndex];

                // Get the IP properties of the desired interface
                IPInterfaceProperties ipProperties = desiredInterface.GetIPProperties();

                // Retrieve the first IPv4 address assigned to the interface
                IPAddress localIPAddress = ipProperties
                    .UnicastAddresses
                    .FirstOrDefault(addr => addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.Address;

                if (localIPAddress != null)
                {
                    TextBox2.Text = localIPAddress.ToString();
                    // Use the local IP address as needed
                }
            }           

            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();

                foreach (UnicastIPAddressInformation unicastAddress in ipProperties.UnicastAddresses)
                {
                    if (unicastAddress.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        IPAddress ipAddress = unicastAddress.Address;
                        if (IsLocalNetworkIP(ipAddress))
                            TextBox3.Text = ipAddress.ToString();
                    }
                }
            }
        }
        bool IsLocalNetworkIP(IPAddress ipAddress)
        {
            // Check if the IP address belongs to the local network
            // You can customize this logic based on your specific network configuration
            // Example: Check if the IP address starts with a specific subnet range
            string ipAddressString = ipAddress.ToString();
            if (ipAddressString.StartsWith(TextBox1.Text) || ipAddressString.StartsWith("10."))
            {
                return true;
            }

            return false;
        }
        private void Buttviewpackets_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            //string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\\Users\\holyh\\source\\repos\\NetworkTrafficCSharpForm\\NetworkTrafficCSharpForm\\IPLogs.mdf;Integrated Security=True"; //"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\holyh\\source\\repos\\NetworkTrafficCSharpForm\\NetworkTrafficCSharpForm\\IPLogs.mdf;Integrated Security=True"; //
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT * FROM IPLog";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;

        }
        private void GetIPData(string eyepee)
        {
            // URL of the webpage to scrape
            string url = "https://www.whois.com/whois/" + eyepee;

            // Download the webpage as an HTML document
            WebClient client = new WebClient();
            string html = client.DownloadString(url);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            // Extract the text from the <div class="df-block"> element
            var divElement = doc.DocumentNode.SelectSingleNode("//div[@class='df-block']");
            string text = divElement?.InnerText;
            if (text != null)
            {
                IsolateData(text);
            }
         //  int jamimah = GetProcessIdOrFileName(eyepee, port);
        } 
      

        readonly List<String> carrylist = new List<string>();
        private void IsolateData(string derter)
        {
            string[] searchWords = { "Organization:", "OrgName:", "OrgId:", "Address:", "City:", "StateProv:", "PostalCode:", "Country:" };
            foreach (string word in searchWords)
            {
                int index = derter.IndexOf(word);
               // Console.WriteLine("Word: " + word + " Index: " + index);
                if (index >= 0)
                {
                    int endIndex = derter.IndexOf("\n", index);
                    if (endIndex < 0)
                    {
                        endIndex = derter.Length;
                    }
                    string value = derter.Substring(index + word.Length, endIndex - index - word.Length).Trim();
                    Console.WriteLine(word + " " + value);
                    carrylist.Add(value);
                }
            }
        }

        public static string[] GetProgramNames(int[] pids)
        {
            string[] programNames = new string[pids.Length];
            for (int i = 0; i < pids.Length; i++)
            {
                try
                {
                    Process process = Process.GetProcessById(pids[i]);
                    programNames[i] = process.ProcessName;
                }
                catch (ArgumentException)
                {
                    programNames[i] = "N/A";
                }
            }
            return programNames;
        }    
        private void BtnStartCapture_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the list of available capture devices
                var devices = CaptureDeviceList.Instance;
                int devind = Settings.Default.Device;
                // Select the first available device 0 for W  2? for H
                captureDevice = devices[devind]; //222
                DeviceModes mode = DeviceModes.None;
                int read_timeout = 1000;
                var configuration = new DeviceConfiguration()
                {
                    Mode = mode,
                    ReadTimeout = read_timeout,
                };
                // Open the capture device for capturing packets
                captureDevice.Open(configuration);

                // Set a filter to capture only the traffic going to and from the computers on your network
                captureDevice.Filter = "ip"; //"tcp"; //net 192.168.0.0/24";

                // Add a handler for the PacketArrival event, which will be called for each captured packet
                captureDevice.OnPacketArrival += OnPacketArrival;

                // Start capturing packets
                captureDevice.StartCapture();        

              //  MessageBox.Show("Packet capture started.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting packet capture: " + ex.Message);
            }
        }
        private void BtnStopCapture_Click(object sender, EventArgs e)
        {     
            captureDevice.StopCapture();       
            captureDevice.Close();       
        }
        static byte[] CaptureEthernetPacket()
        {
            // Use the Ping class to send an ICMP Echo Request to localhost
            Ping pingSender = new Ping();
            string data = "Ping test data";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            PingOptions options = new PingOptions(64, true);
            PingReply reply = pingSender.Send("localhost", timeout, buffer, options);
            // Get the raw packet data from the reply buffer
            byte[] packetData = reply.Buffer;
            return packetData;
        }

        static EthernetHeader DecodeEthernetHeader(byte[] packetData)
        {
            // Create a new EthernetHeader object
            EthernetHeader header = new EthernetHeader
            {
                // Decode the destination MAC address
                DestinationMAC = BitConverter.ToString(packetData, 0, 6),

                // Decode the source MAC address
                SourceMAC = BitConverter.ToString(packetData, 6, 6)
            };

            // Return the EthernetHeader object
            return header;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            // Capture an Ethernet packet
            byte[] packetData = CaptureEthernetPacket();

            // Decode the Ethernet header
            EthernetHeader header = DecodeEthernetHeader(packetData);

            // Display the source and destination MAC addresses
            Console.WriteLine("Source MAC: {0}", header.SourceMAC);
            Console.WriteLine("Destination MAC: {0}", header.DestinationMAC);
        }

        private void InsertPacketToDatabase(string program, int pid, string sourceordest, string sourceIP, string destIP, string protocol, int packetSize, string packetColor, bool hasPayloadPacket, bool hasPayloadData, bool isPayloadInitialized, int headerLength, string headerData, int hopLimit, int payloadDataLength, string payloadPacket, int timeToLive, int totalLength, int totalPacketLength, string version, string organization, string orgName, string orgId, string address, string city, string stateProv, string postalCode, string country)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Create the SqlCommand object with the connection
                SqlCommand cmd = new SqlCommand
                {
                    Connection = connection
                };
                if (sourceordest == "source")
                {
                    // Set the command text and parameters
                    cmd.CommandText = "SELECT COUNT(*) FROM IPLog WHERE SourceIP = @SourceIP";
                    // Add any necessary parameters
                    cmd.Parameters.AddWithValue("@SourceIP", sourceIP);
                }
                else
                {
                    // Set the command text and parameters
                    cmd.CommandText = "SELECT COUNT(*) FROM IPLog WHERE DestIP = @DestIP";
                    // Add any necessary parameters
                    cmd.Parameters.AddWithValue("@DestIP", destIP);
                }
                // Execute the scalar query
                int count = (int)cmd.ExecuteScalar();

                // Check if the IP and company already exist in the database
                // string query = "SELECT COUNT(*) FROM Packets WHERE SourceIP = @SourceIP OR DestIP = @DestIP";
                // SqlCommand cmd = new SqlCommand(query, connection);
                // cmd.Parameters.AddWithValue("@SourceIP", sourceIP);
                // cmd.Parameters.AddWithValue("@DestIP", destIP);          
                // int count = (int)cmd.ExecuteScalar();

                // If the IP and company do not exist in the database, insert a new row
                if (count == 0)
                {
                    if (organization is null)
                    {
                        query = "INSERT INTO IpLog (Program, Pid, SourceIP, DestIP, Protocol, PacketSize, PacketColor, HasPayloadPacket, HasPayloadData, IsPayloadInitialized, HeaderLength, HeaderData, HopLimit, PayloadDataLength, PayloadPacket, TimeToLive, TotalLength, TotalPacketLength, Version) VALUES (@Program, @Pid, @SourceIP, @DestIP, @Protocol, @PacketSize, @PacketColor, @HasPayloadPacket, @HasPayloadData, @IsPayloadInitialized, @HeaderLength, @HeaderData, @HopLimit, @PayloadDataLength, @PayloadPacket, @TimeToLive, @TotalLength, @TotalPacketLength, @Version)";
                        cmd = new SqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@Program", program);
                        cmd.Parameters.AddWithValue("@Pid", pid);
                        cmd.Parameters.AddWithValue("@SourceIP", sourceIP);
                        cmd.Parameters.AddWithValue("@DestIP", destIP);
                        cmd.Parameters.AddWithValue("@Protocol", protocol);
                        cmd.Parameters.AddWithValue("@PacketSize", packetSize);
                        cmd.Parameters.AddWithValue("@PacketColor", packetColor);
                        cmd.Parameters.AddWithValue("@HasPayloadPacket", hasPayloadPacket);
                        cmd.Parameters.AddWithValue("@HasPayloadData", hasPayloadData);
                        cmd.Parameters.AddWithValue("@IsPayloadInitialized", isPayloadInitialized);
                        cmd.Parameters.AddWithValue("@HeaderLength", headerLength);
                        cmd.Parameters.AddWithValue("@HeaderData", headerData);
                        cmd.Parameters.AddWithValue("@HopLimit", hopLimit);
                        cmd.Parameters.AddWithValue("@PayloadDataLength", payloadDataLength);
                        cmd.Parameters.AddWithValue("@PayloadPacket", payloadPacket);
                        cmd.Parameters.AddWithValue("@TimeToLive", timeToLive);
                        cmd.Parameters.AddWithValue("@TotalLength", totalLength);
                        cmd.Parameters.AddWithValue("@TotalPacketLength", totalPacketLength);
                        cmd.Parameters.AddWithValue("@Version", version);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        query = "INSERT INTO IpLog (Program, Pid, SourceIP, DestIP, Protocol, PacketSize, PacketColor, HasPayloadPacket, HasPayloadData, IsPayloadInitialized, HeaderLength, HeaderData, HopLimit, PayloadDataLength, PayloadPacket, TimeToLive, TotalLength, TotalPacketLength, Version, Organization, OrgName, OrgId, Address, City, StateProv, PostalCode, Country) VALUES (@Program, @Pid, @SourceIP, @DestIP, @Protocol, @PacketSize, @PacketColor, @HasPayloadPacket, @HasPayloadData, @IsPayloadInitialized, @HeaderLength, @HeaderData, @HopLimit, @PayloadDataLength, @PayloadPacket, @TimeToLive, @TotalLength, @TotalPacketLength, @Version, @Organization, @OrgName, @OrgId, @Address, @City, @StateProv, @PostalCode, @Country)";
                        cmd = new SqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@Program", program);
                        cmd.Parameters.AddWithValue("@Pid", pid);
                        cmd.Parameters.AddWithValue("@SourceIP", sourceIP);
                        cmd.Parameters.AddWithValue("@DestIP", destIP);
                        cmd.Parameters.AddWithValue("@Protocol", protocol);
                        cmd.Parameters.AddWithValue("@PacketSize", packetSize);
                        cmd.Parameters.AddWithValue("@PacketColor", packetColor);
                        cmd.Parameters.AddWithValue("@HasPayloadPacket", hasPayloadPacket);
                        cmd.Parameters.AddWithValue("@HasPayloadData", hasPayloadData);
                        cmd.Parameters.AddWithValue("@IsPayloadInitialized", isPayloadInitialized);
                        cmd.Parameters.AddWithValue("@HeaderLength", headerLength);
                        cmd.Parameters.AddWithValue("@HeaderData", headerData);
                        cmd.Parameters.AddWithValue("@HopLimit", hopLimit);
                        cmd.Parameters.AddWithValue("@PayloadDataLength", payloadDataLength);
                        cmd.Parameters.AddWithValue("@PayloadPacket", payloadPacket);
                        cmd.Parameters.AddWithValue("@TimeToLive", timeToLive);
                        cmd.Parameters.AddWithValue("@TotalLength", totalLength);
                        cmd.Parameters.AddWithValue("@TotalPacketLength", totalPacketLength);
                        cmd.Parameters.AddWithValue("@Version", version);
                        cmd.Parameters.AddWithValue("@Organization", organization);
                        cmd.Parameters.AddWithValue("@OrgName", orgName);
                        cmd.Parameters.AddWithValue("@OrgId", orgId);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.Parameters.AddWithValue("@City", city);
                        cmd.Parameters.AddWithValue("@StateProv", stateProv);
                        cmd.Parameters.AddWithValue("@PostalCode", postalCode);
                        cmd.Parameters.AddWithValue("@Country", country);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void DataConnButt_Click(object sender, EventArgs e)
        {
            connectionString  = Interaction.InputBox("Enter your Database Connection String here as such: \nData Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\IPLogs.mdf;Integrated Security=True", "Connection String Required");
            Settings.Default.CS = connectionString;
            Settings.Default.Save();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.Device = ComboBox1.SelectedIndex;
            Settings.Default.Save();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.LN = TextBox1.Text;
            Settings.Default.Save();
        }
    }
    class EthernetHeader
    {
        public string DestinationMAC { get; set; }
        public string SourceMAC { get; set; }
    }
}

//MAXMIND API METHOD
//private void GetIPData2(string eyepee)
//{
//    using (var reader = new DatabaseReader("C:\\Users\\????\\AppData\\Roaming\\Wireshark\\GeoLite2-City.mmdb"))
//    {

//        try
//        {
//            var response = reader.City(eyepee);
//            Console.WriteLine(response.Country.IsoCode);
//            Console.WriteLine(response.Country.Name);
//        //    Console.WriteLine(response.Country.Names.Keys);
//            Console.WriteLine(response.Country.GeoNameId);
//          //  Console.WriteLine(response.Country.Confidence.Value.ToString());
//            Console.WriteLine(response.Country.IsInEuropeanUnion);
//            Console.WriteLine(response.Continent.Name);
//            Console.WriteLine(response.Continent.Code);
//          //  Console.WriteLine(response.Continent.Names);
//            Console.WriteLine(response.Continent.GeoNameId);
//          //  Console.WriteLine(response.City.Name);
//           // Console.WriteLine(response.City.Names.Values);
//        //    Console.WriteLine(response.City.GeoNameId);
//         //   Console.WriteLine(response.City.Confidence.Value);
//       //     Console.WriteLine(response.MostSpecificSubdivision.Name);
//         //   Console.WriteLine(response.MostSpecificSubdivision.Names.Values);
//        //    Console.WriteLine(response.MostSpecificSubdivision.GeoNameId);
//         //   Console.WriteLine(response.MostSpecificSubdivision.Confidence.Value);
//        //    Console.WriteLine(response.Postal.Code);
//         //   Console.WriteLine(response.Postal.Confidence.Value);
//    //        Console.WriteLine(response.RegisteredCountry.Name);
//         //   Console.WriteLine(response.RegisteredCountry.Names.Values);
//       //     Console.WriteLine(response.RegisteredCountry.GeoNameId);
//        //    Console.WriteLine(response.RegisteredCountry.Confidence.Value);
//        //    Console.WriteLine(response.RegisteredCountry.IsInEuropeanUnion);
//     //       Console.WriteLine(response.RegisteredCountry.IsoCode);
//       //     Console.WriteLine(response.RepresentedCountry.Name);
//       //     Console.WriteLine(response.RepresentedCountry.Names.Values);
//       //     Console.WriteLine(response.RepresentedCountry.GeoNameId);
//        //    Console.WriteLine(response.RepresentedCountry.Confidence.Value);
//       //     Console.WriteLine(response.RepresentedCountry.IsInEuropeanUnion);
//       //     Console.WriteLine(response.RepresentedCountry.IsoCode);
//        //    Console.WriteLine(response.RepresentedCountry.Type);
//       //     Console.WriteLine(response.Subdivisions.Count);
//      //      Console.WriteLine(response.Traits.Isp);
//      //      Console.WriteLine(response.Traits.Domain);
//       //     Console.WriteLine(response.Traits.AutonomousSystemNumber);
//       //     Console.WriteLine(response.Traits.AutonomousSystemOrganization);
//       //     Console.WriteLine(response.Traits.ConnectionType);
//            Console.WriteLine(response.Traits.IPAddress);
//            Console.WriteLine(response.Traits.IsAnonymous);
//            Console.WriteLine(response.Traits.IsAnonymousProxy);
//            Console.WriteLine(response.Traits.IsAnonymousVpn);
//            Console.WriteLine(response.Traits.IsHostingProvider);
//            Console.WriteLine(response.Traits.IsLegitimateProxy);
//            Console.WriteLine(response.Traits.IsPublicProxy);
//            Console.WriteLine(response.Traits.IsResidentialProxy);
//            Console.WriteLine(response.Traits.IsSatelliteProvider);
//            Console.WriteLine(response.Traits.IsTorExitNode);
//           // Console.WriteLine(response.Traits.MobileCountryCode);
//            Console.WriteLine(response.Traits.Network);
//        //    Console.WriteLine(response.Traits.Organization);
//        //    Console.WriteLine(response.Traits.StaticIPScore);
//        //    Console.WriteLine(response.Traits.UserCount);
//        //    Console.WriteLine(response.Traits.UserType);
//            Console.WriteLine(response.Location.HasCoordinates);
//            Console.WriteLine(response.Location.TimeZone);                 
//            Console.WriteLine(response.Location.AccuracyRadius);
//        //    Console.WriteLine(response.Location.AverageIncome);
//            Console.WriteLine(response.Location.Latitude);
//            Console.WriteLine(response.Location.Longitude);
//        //    Console.WriteLine(response.Location.MetroCode);
//        //    Console.WriteLine(response.Location.PopulationDensity);                    
//        }

//        catch (Exception ex)
//        {
//            GetIPData(eyepee);
//        }
//    }
//}