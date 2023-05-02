using System;
using System.Windows.Forms;
using SharpPcap;
using PacketDotNet;

namespace NetworkTrafficCSharpForm
{
    public partial class Form1 : Form
    {
        private ICaptureDevice captureDevice;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
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
            // Stop capturing packets
            captureDevice.StopCapture();

            // Close the capture device
            captureDevice.Close();

           // MessageBox.Show("Packet capture stopped.");
        }
    }
}
