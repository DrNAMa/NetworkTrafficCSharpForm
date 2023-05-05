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

namespace NetworkTrafficCSharpForm
{
    public partial class Form1 : Form
    {
        SqlConnection connection = null;
        // Define your connection string to connect to your SQL Server database
        string connectionString = "Data Source=(local);Initial Catalog=YourDatabaseName;Integrated Security=True";

        // Define the SQL query to insert the data into the database
        string query = "INSERT INTO YourTableName (Organization, OrgName, OrgId, Address, City, StateProv, PostalCode, Country, SourceIP, DestinationIP, Protocol, PacketSize, PacketColor, HasPayloadPacket, HasPayloadData, IsPayloadInitialized, HeaderLength, HeaderData, HopLimit, PayloadDataLength, PayloadPacket, TimeToLive, TotalLength, TotalPacketLength, Version) " +
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
            // Test the console output
            Console.WriteLine("This is a test message");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
           
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
        }
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

                // Check if the packet is coming from or going to a computer on your network
                if (ipPacket.DestinationAddress.ToString().StartsWith("192.168.1.") || ipPacket.SourceAddress.ToString().StartsWith("192.168.1."))
                {
                    if (!ipPacket.SourceAddress.ToString().StartsWith("192"))
                    {
                        GetIPData(ipPacket.SourceAddress.ToString());
                    }
                    else
                    {
                        GetIPData(ipPacket.DestinationAddress.ToString());
                    }
                    // Display the source and destination IP addresses and the protocol of the captured packet
                    Console.WriteLine("Source IP: " + ipPacket.SourceAddress.ToString());
                    Console.WriteLine("Destination IP: " + ipPacket.DestinationAddress.ToString());
                    Console.WriteLine("Protocol: " + ipPacket.Protocol.ToString());
                    Console.WriteLine("Packet Size: " + ipPacket.Bytes.Length.ToString());
                    Console.WriteLine("Packet Color: " + ipPacket.Color.ToString());
                    Console.WriteLine("Has Payload Packet: " + ipPacket.HasPayloadPacket.ToString());
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
                    bool yee = ipPacket.HasPayloadData;
                    if (yee)                        
                    {
                        Console.WriteLine("Payload Data:");
                        foreach (byte b in ipPacket.PayloadData)
                        {
                            Console.Write(b.ToString("X2") + " ");
                        }
                       
                    }
                    Console.WriteLine("Payload Packet: " + ipPacket.PayloadPacket.ToString());
                    Console.WriteLine("TimeToLive: " + ipPacket.TimeToLive.ToString());
                    Console.WriteLine("Total Length: " + ipPacket.TotalLength.ToString());
                    Console.WriteLine("Total Packet Length: " + ipPacket.TotalPacketLength.ToString());
                    Console.WriteLine("Version: " + ipPacket.Version.ToString());
                    Console.WriteLine("");
                  //  Console.WriteLine("Protocol: " + ipPacket.Bytes.Length.ToString());
                }
            }
        }
        private void BtnStartCapture_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the list of available capture devices
                var devices = CaptureDeviceList.Instance;

                // Select the first available device
                captureDevice = devices[2];
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

        private void InsertPacketToDatabase(string sourceIP, string destIP, string protocol, int packetSize, string packetColor, bool hasPayloadPacket, bool hasPayloadData, bool isPayloadInitialized, int headerLength, string headerData, int hopLimit, int payloadDataLength, string payloadPacket, int timeToLive, int totalLength, int totalPacketLength, string version, string organization, string orgName, string orgId, string address, string city, string stateProv, string postalCode, string country)
        {
            // Check if the IP and company already exist in the database
            string query = "SELECT COUNT(*) FROM Packets WHERE SourceIP = @SourceIP OR DestIP = @DestIP OR Organization = @Organization";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@SourceIP", sourceIP);
            cmd.Parameters.AddWithValue("@DestIP", destIP);
            cmd.Parameters.AddWithValue("@Organization", organization);
            int count = (int)cmd.ExecuteScalar();

            // If the IP and company do not exist in the database, insert a new row
            if (count == 0)
            {
                query = "INSERT INTO Packets (SourceIP, DestIP, Protocol, PacketSize, PacketColor, HasPayloadPacket, HasPayloadData, IsPayloadInitialized, HeaderLength, HeaderData, HopLimit, PayloadDataLength, PayloadPacket, TimeToLive, TotalLength, TotalPacketLength, Version, Organization, OrgName, OrgId, Address, City, StateProv, PostalCode, Country) VALUES (@SourceIP, @DestIP, @Protocol, @PacketSize, @PacketColor, @HasPayloadPacket, @HasPayloadData, @IsPayloadInitialized, @HeaderLength, @HeaderData, @HopLimit, @PayloadDataLength, @PayloadPacket, @TimeToLive, @TotalLength, @TotalPacketLength, @Version, @Organization, @OrgName, @OrgId, @Address, @City, @StateProv, @PostalCode, @Country)";
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
    class EthernetHeader
    {
        public string DestinationMAC { get; set; }
        public string SourceMAC { get; set; }
    }
}