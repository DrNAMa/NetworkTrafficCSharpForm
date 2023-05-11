using System;
using System.Windows.Forms;
using SharpPcap;
using PacketDotNet;
using System.Runtime.InteropServices;
using System.Net;
using HtmlAgilityPack;
using System.Diagnostics.Eventing.Reader;
using System.Threading;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Security.Cryptography;
using PacketDotNet.Ieee80211;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using static NetworkTrafficCSharpForm.Form1;
using System.Linq;
//might try to just straight up netstat -a -n -o to get the information you need to find the responsible .exe's
//using MaxMind.GeoIP2;
//PREEETY MUCH TELL THIS APP TO IGNORE NOT PUTTING YOUR OWN IP RANGE AS BLANK AND NOT RUNNING THEM THROUGH THE INFORMATION FINDER
//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
namespace NetworkTrafficCSharpForm
{
    public partial class Form1 : Form
    {
        SqlConnection connection = null;
        // Define your connection string to connect to your SQL Server database
        string connectionString =  "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\holyh\\source\\repos\\NetworkTrafficCSharpForm\\NetworkTrafficCSharpForm\\IPLogs.mdf;Integrated Security=True"; //"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\holyh\\Source\\Repos\\DrNAMa\\NetworkTrafficCSharpForm\\NetworkTrafficCSharpForm\\IPLogs.mdf;Integrated Security=True";

        // Define the SQL query to insert the data into the database
        string query = "INSERT INTO IPLog (Organization, OrgName, OrgId, Address, City, StateProv, PostalCode, Country, SourceIP, DestinationIP, Protocol, PacketSize, PacketColor, HasPayloadPacket, HasPayloadData, IsPayloadInitialized, HeaderLength, HeaderData, HopLimit, PayloadDataLength, PayloadPacket, TimeToLive, TotalLength, TotalPacketLength, Version) " +
                "VALUES (@Organization, @OrgName, @OrgId, @Address, @City, @StateProv, @PostalCode, @Country, @SourceIP, @DestinationIP, @Protocol, @PacketSize, @PacketColor, @HasPayloadPacket, @HasPayloadData, @IsPayloadInitialized, @HeaderLength, @HeaderData, @HopLimit, @PayloadDataLength, @PayloadPacket, @TimeToLive, @TotalLength, @TotalPacketLength, @Version)";
        // Import the kernel32.dll library
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private ICaptureDevice captureDevice;
        [StructLayout(LayoutKind.Sequential)]
        public struct MIB_TCPROW_OWNER_PID
        {
            public uint dwState;
            public uint dwLocalAddr;
            public uint dwLocalPort;
            public uint dwRemoteAddr;
            public uint dwRemotePort;
            public uint dwOwningPid;
        }

        //[DllImport("iphlpapi.dll")]
        //public static extern int GetExtendedTcpTable(IntPtr pTcpTable, ref int pdwSize, bool bOrder, int ulAf, TcpTableClass tableClass, uint reserved = 0);

        //public enum TcpTableClass : int
        //{
        //    TCP_TABLE_OWNER_PID_ALL
        //}
        //public enum TCP_TABLE_CLASS : int
        //{
        //    TCP_TABLE_BASIC_LISTENER,
        //    TCP_TABLE_BASIC_CONNECTIONS,
        //    TCP_TABLE_BASIC_ALL,
        //    TCP_TABLE_OWNER_PID_LISTENER,
        //    TCP_TABLE_OWNER_PID_CONNECTIONS,
        //    TCP_TABLE_OWNER_PID_ALL,
        //    TCP_TABLE_OWNER_MODULE_LISTENER,
        //    TCP_TABLE_OWNER_MODULE_CONNECTIONS,
        //    TCP_TABLE_OWNER_MODULE_ALL
        //}

        //[DllImport("iphlpapi.dll")]
        //public static extern int GetExtendedUdpTable(IntPtr pUdpTable, ref int pdwSize, bool bOrder, int ulAf, UdpTableClass tableClass, uint reserved = 0);

        //public enum UdpTableClass
        //{
        //    UDP_TABLE_OWNER_PID
        //}
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
            // Test the console output
          //  Console.WriteLine("This is a test message");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
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
        private void GetIPData(string eyepee, int port)
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
      

        private List<String> carrylist = new List<string>();
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
                if (ipPacket.DestinationAddress.ToString().StartsWith("192.168.1.") || ipPacket.SourceAddress.ToString().StartsWith("192.168.1."))
                {
                    if (!ipPacket.SourceAddress.ToString().StartsWith("192"))
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            // Create the SqlCommand object with the connection
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = connection;
                            // Set the command text and parameters
                            cmd.CommandText = "SELECT COUNT(*) FROM IPLog WHERE SourceIP = @SourceIP";
                            // Add any necessary parameters
                            cmd.Parameters.AddWithValue("@SourceIP", ipPacket.SourceAddress.ToString());
                            // Execute the scalar query
                            int count = (int)cmd.ExecuteScalar();
                            connection.Close();
                            sourceordest = "source";
                            if (count == 0)
                            {
                                int sourceport = 0;
                               if (ipPacket.PayloadPacket is TcpPacket tcpPacket)
                                {
                                    sourceport = tcpPacket.SourcePort;                                    
                                }
                                if (ipPacket.PayloadPacket is UdpPacket updPacket)
                                {
                                    sourceport = updPacket.SourcePort;
                                }
                                GetIPData(ipPacket.SourceAddress.ToString(), sourceport);

                                int[] pids = { 996,4,3588,3588,7180,4,4520,1976,10500,2988,808,716,1316,1932,2988,788,924,4,4,3456,14856,3456,3448,3448,10500,4,10764,18728,3568,20104,3752,10500,20104,8036,20104,20104,22324,20104,20104,20104,20104,20104,20104,14232,14232,14232,14232,14232,14232,14232,14232,14232,14232,4,4,996,4,4,4520,1976,2988,808,716,1316,1932,2988,788,924,4,4,4,4,4,4,4,4,4,4,4,4,4356,4356,4356,4356,9304,4356,9304,9304,9304,9304,7180,2704,15248,15248,15248,2704,1976,10500,20104,20104,10500,0500,724,3224,724,4,4,724,724,4,4,724,724,4,4,724,724,724,4520,724 };
                                string[] programNames = GetProgramNames(pids);

                                for (int i = 0; i < pids.Length; i++)
                                {
                                    Console.WriteLine("PID: {0}, Program Name: {1}", pids[i], programNames[i]);
                                }
                                GetProgramNames(pids);
                              //  GetIPData2(ipPacket.SourceAddress.ToString());
                            }
                        }
                    }
                    else if (!ipPacket.DestinationAddress.ToString().StartsWith("192"))
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            // Create the SqlCommand object with the connection
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = connection;

                            // Set the command text and parameters
                            cmd.CommandText = "SELECT COUNT(*) FROM IPLog WHERE DestIP = @DestIP";
                            // Add any necessary parameters
                            cmd.Parameters.AddWithValue("@DestIP", ipPacket.DestinationAddress.ToString());
                            // Execute the scalar query
                            int count1 = (int)cmd.ExecuteScalar();
                            connection.Close();
                            sourceordest = "dest";
                            if (count1 == 0)
                            {
                                int destport = 0;
                                if (ipPacket.PayloadPacket is TcpPacket tcpPacket)
                                {
                                    destport = tcpPacket.DestinationPort;
                                }
                                if (ipPacket.PayloadPacket is UdpPacket updPacket)
                                {
                                    destport = updPacket.DestinationPort;
                                }
                                GetIPData(ipPacket.DestinationAddress.ToString(), destport);
                                int[] pids = { 996, 4, 3588, 3588, 7180, 4, 4520, 1976, 10500, 2988, 808, 716, 1316, 1932, 2988, 788, 924, 4, 4, 3456, 14856, 3456, 3448, 3448, 10500, 4, 10764, 18728, 3568, 20104, 3752, 10500, 20104, 8036, 20104, 20104, 22324, 20104, 20104, 20104, 20104, 20104, 20104, 14232, 14232, 14232, 14232, 14232, 14232, 14232, 14232, 14232, 14232, 4, 4, 996, 4, 4, 4520, 1976, 2988, 808, 716, 1316, 1932, 2988, 788, 924, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4356, 4356, 4356, 4356, 9304, 4356, 9304, 9304, 9304, 9304, 7180, 2704, 15248, 15248, 15248, 2704, 1976, 10500, 20104, 20104, 10500, 0500, 724, 3224, 724, 4, 4, 724, 724, 4, 4, 724, 724, 4, 4, 724, 724, 724, 4520, 724 };
                                string[] programNames = GetProgramNames(pids);

                                for (int i = 0; i < pids.Length; i++)
                                {
                                    Console.WriteLine("PID: {0}, Program Name: {1}", pids[i], programNames[i]);
                                }
                                //  GetProgramNames(pids);
                              
                             //   int port = 8080; // Specify the port number you're interested in


                                //   GetIPData2(ipPacket.DestinationAddress.ToString());
                            }
                        }
                    }
                 //   ipPacket.ParentPacket.PayloadPacket.
                    // Display the source and destination IP addresses and the protocol of the captured packet
                    Console.WriteLine("Source IP: " + ipPacket.SourceAddress.ToString());
                    Console.WriteLine("Destination IP: " + ipPacket.DestinationAddress.ToString());
                    Console.WriteLine("Protocol: " + ipPacket.Protocol.ToString());
                    Console.WriteLine("Packet Size: " + ipPacket.Bytes.Length.ToString());
                    Console.WriteLine("Packet Color: " + ipPacket.Color.ToString());
                    Console.WriteLine("Has Payload Packet: " + ipPacket.HasPayloadPacket.ToString());
                    Console.WriteLine("Has Sub Payload Packet: " + ipPacket.PayloadPacket.HasPayloadPacket.ToString());
                    Console.WriteLine("Has Payload Data: " + ipPacket.HasPayloadData.ToString());
                    Console.WriteLine("Is Payload Initialized: " + ipPacket.IsPayloadInitialized.ToString());
                    Console.WriteLine("Header Length: " + ipPacket.HeaderLength.ToString());
                    Console.WriteLine("Header Data:");
                    foreach (byte b in ipPacket.HeaderData)
                    {
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
                        InsertPacketToDatabase(sourceordest, ipPacket.SourceAddress.ToString(), ipPacket.DestinationAddress.ToString(), ipPacket.Protocol.ToString(), ipPacket.Bytes.Length, ipPacket.Color.ToString(),
                                             ipPacket.HasPayloadPacket, ipPacket.HasPayloadData, ipPacket.IsPayloadInitialized, ipPacket.HeaderLength, ipPacket.HeaderData.ToString(),
                                             ipPacket.HopLimit, ipPacket.PayloadLength, ipPacket.PayloadPacket.ToString(), ipPacket.TimeToLive, ipPacket.TotalLength, ipPacket.TotalPacketLength,
                                             ipPacket.Version.ToString(), carrylist[0], carrylist[1], carrylist[2], carrylist[3], carrylist[4], carrylist[5], carrylist[6], carrylist[7]);
                    }
                    else
                    {
                        InsertPacketToDatabase(sourceordest, ipPacket.SourceAddress.ToString(), ipPacket.DestinationAddress.ToString(), ipPacket.Protocol.ToString(), ipPacket.Bytes.Length, ipPacket.Color.ToString(),
                                             ipPacket.HasPayloadPacket, ipPacket.HasPayloadData, ipPacket.IsPayloadInitialized, ipPacket.HeaderLength, ipPacket.HeaderData.ToString(),
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

        

    //    int GetProcessIdOrFileName(string sourceIp, int sourcePort)
    //{
    //    IPAddress localIpAddress = IPAddress.Parse(sourceIp);
    //    ushort localPort = (ushort)sourcePort;

    //    int bufferSize = 0;
    //    GetExtendedTcpTable(IntPtr.Zero, ref bufferSize, true, 2, TcpTableClass.TCP_TABLE_OWNER_PID_ALL);
    //    IntPtr tcpTable = Marshal.AllocHGlobal(bufferSize);

    //    try
    //    {
    //        int result = GetExtendedTcpTable(tcpTable, ref bufferSize, true, 2, TcpTableClass.TCP_TABLE_OWNER_PID_ALL);
    //        if (result == 0)
    //        {
    //            int rowCount = Marshal.ReadInt32(tcpTable);
    //            IntPtr rowPtr = IntPtr.Add(tcpTable, 4);

    //            for (int i = 0; i < rowCount; i++)
    //            {
    //                    MIB_TCPROW_OWNER_PID tcpRow = Marshal.PtrToStructure<MIB_TCPROW_OWNER_PID>(rowPtr);
    //                    ushort port = (ushort)(((tcpRow.dwLocalPort & 0xFF00) >> 8) | ((tcpRow.dwLocalPort & 0x00FF) << 8));
    //                    IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(sourceIp), sourcePort);  

    //                    if (!localEndPoint.Equals("0.0.0.0:0"))
    //                    {
    //                    int processId = (int)tcpRow.dwOwningPid;

    //                    try
    //                    {
    //                        Process process = Process.GetProcessById(processId);
    //                        string processFileName = process.MainModule.FileName;
    //                        // Alternatively, you can use process.ProcessName to get the process name without the file path.

    //                        Console.WriteLine($"Process ID: {processId}");
    //                        Console.WriteLine($"Process File Name: {processFileName}");

    //                        return processId;
    //                    }
    //                    catch (ArgumentException)
    //                    {
    //                        // Process with the given ID does not exist
    //                        return -1;
    //                    }
    //                }

    //                rowPtr = IntPtr.Add(rowPtr, Marshal.SizeOf(tcpRow));
    //            }
    //        }
    //    }
    //    finally
    //    {
    //        Marshal.FreeHGlobal(tcpTable);
    //    }

    //    return -1; // Process ID not found
    //}


    
        private void BtnStartCapture_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the list of available capture devices
                var devices = CaptureDeviceList.Instance;

                // Select the first available device 0 for W  2? for H
                captureDevice = devices[0];
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

        private void InsertPacketToDatabase(string sourceordest, string sourceIP, string destIP, string protocol, int packetSize, string packetColor, bool hasPayloadPacket, bool hasPayloadData, bool isPayloadInitialized, int headerLength, string headerData, int hopLimit, int payloadDataLength, string payloadPacket, int timeToLive, int totalLength, int totalPacketLength, string version, string organization, string orgName, string orgId, string address, string city, string stateProv, string postalCode, string country)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Create the SqlCommand object with the connection
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
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
                        query = "INSERT INTO IpLog (SourceIP, DestIP, Protocol, PacketSize, PacketColor, HasPayloadPacket, HasPayloadData, IsPayloadInitialized, HeaderLength, HeaderData, HopLimit, PayloadDataLength, PayloadPacket, TimeToLive, TotalLength, TotalPacketLength, Version) VALUES (@SourceIP, @DestIP, @Protocol, @PacketSize, @PacketColor, @HasPayloadPacket, @HasPayloadData, @IsPayloadInitialized, @HeaderLength, @HeaderData, @HopLimit, @PayloadDataLength, @PayloadPacket, @TimeToLive, @TotalLength, @TotalPacketLength, @Version)";
                        cmd = new SqlCommand(query, connection);
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
                        query = "INSERT INTO IpLog (SourceIP, DestIP, Protocol, PacketSize, PacketColor, HasPayloadPacket, HasPayloadData, IsPayloadInitialized, HeaderLength, HeaderData, HopLimit, PayloadDataLength, PayloadPacket, TimeToLive, TotalLength, TotalPacketLength, Version, Organization, OrgName, OrgId, Address, City, StateProv, PostalCode, Country) VALUES (@SourceIP, @DestIP, @Protocol, @PacketSize, @PacketColor, @HasPayloadPacket, @HasPayloadData, @IsPayloadInitialized, @HeaderLength, @HeaderData, @HopLimit, @PayloadDataLength, @PayloadPacket, @TimeToLive, @TotalLength, @TotalPacketLength, @Version, @Organization, @OrgName, @OrgId, @Address, @City, @StateProv, @PostalCode, @Country)";
                        cmd = new SqlCommand(query, connection);
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
//    using (var reader = new DatabaseReader("C:\\Users\\holyh\\AppData\\Roaming\\Wireshark\\GeoLite2-City.mmdb"))
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