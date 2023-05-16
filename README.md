<h1> This is an open source project, using Visual Studio with C#, that I'm building to help secure the little guy free of charge, malware, ad-ware, bloatware and any other warez that make downloading tools dreadful.
    <br>
    <br>
  <h2>What does it do?
    <h3>It Captures each incoming packet, sends foreign IP information off for analysis and then everything is stored in a database.
      <br>
      <br>
  <h2> How to Set it Up? (assuming we're using visual studio)
    <h3> When running the program, run it or visual studio as administrator<br>	  
    <h3> Right Click on NetworkTrafficCSharpForm >> Add >> New Item >> Service-based Database >> Name the database IPLogs.mdf <br>
    <h3> Create a new Table under this database and name it IPLog<br>   
      <br>
      Select ID, and under it's properties, expand Identiy Specification and make sure it is set to true and 1's.
      <br>
      Insert Columns with these Headers and attributes.
      <br>
  Id int	False	<br>
	Program	nvarchar(50)	True	<br>
	Pid	int	True	<br>
	Organization	nvarchar(MAX)	True	<br>
	OrgName	nvarchar(MAX)	True	<br>
	OrgId	nvarchar(50)	True	<br>
	Address	nvarchar(MAX)	True	<br>
	City	nvarchar(MAX)	True	<br>
	StateProv	nvarchar(50)	True	<br>
	PostalCode	nvarchar(50)	True	<br>
	Country	nvarchar(50)	True	<br>
	SourceIP	nvarchar(50)	True	<br>
	DestIP	nvarchar(50)	True	<br>
	Protocol	nvarchar(50)	True	<br>
	PacketSize	int	True	<br>
	PacketColor	nvarchar(50)	True	<br>
	HasPayloadPacket	nvarchar(50)	True	<br>
	HasPayloadData	nvarchar(50)	True	<br>
	IsPayloadInitialized	nvarchar(50)	True	<br>
	HeaderLength	int	True	<br>
	HeaderData	nvarchar(MAX)	True	<br>
	HopLimit	int	True	<br>
	PayloadDataLength	int	True	<br>
	PayloadPacket	nvarchar(MAX)	True	<br>
	TimeToLive	int	True	<br>
	TotalLength	int	True	<br>
	TotalPacketLength	int	True	<br>
	Version	nvarchar(50)	True	<br>
<br>
    <br>  
      Or use the script below
      <br>
      <br>
      CREATE TABLE [dbo].[IPLog] ( <br>
    [Id]                   INT            IDENTITY (1, 1) NOT NULL, <br>
    [Program]              NVARCHAR (50)  NULL,<br>
    [Pid]                  INT            NULL,<br>
    [Organization]         NVARCHAR (MAX) NULL,<br>
    [OrgName]              NVARCHAR (MAX) NULL,<br>
    [OrgId]                NVARCHAR (50)  NULL,<br>
    [Address]              NVARCHAR (MAX) NULL,<br>
    [City]                 NVARCHAR (MAX) NULL,<br>
    [StateProv]            NVARCHAR (50)  NULL,<br>
    [PostalCode]           NVARCHAR (50)  NULL,<br>
    [Country]              NVARCHAR (50)  NULL,<br>
    [SourceIP]             NVARCHAR (50)  NULL,<br>
    [DestIP]               NVARCHAR (50)  NULL,<br>
    [Protocol]             NVARCHAR (50)  NULL,<br>
    [PacketSize]           INT            NULL,<br>
    [PacketColor]          NVARCHAR (50)  NULL,<br>
    [HasPayloadPacket]     NVARCHAR (50)  NULL,<br>
    [HasPayloadData]       NVARCHAR (50)  NULL,<br>
    [IsPayloadInitialized] NVARCHAR (50)  NULL,<br>
    [HeaderLength]         INT            NULL,<br>
    [HeaderData]           NVARCHAR (MAX) NULL,<br>
    [HopLimit]             INT            NULL,<br>
    [PayloadDataLength]    INT            NULL,<br>
    [PayloadPacket]        NVARCHAR (MAX) NULL,<br>
    [TimeToLive]           INT            NULL,<br>
    [TotalLength]          INT            NULL,<br>
    [TotalPacketLength]    INT            NULL,<br>
    [Version]              NVARCHAR (50)  NULL,<br>
    PRIMARY KEY CLUSTERED ([Id] ASC)<br>
);<br>
      <br>
      <br>
      From there, enter what part of your network you want to monitor ie: (with the dot at the end)<br>
      192.168.1. or 192.168.<br>
      Select Your Capture (Network) Device.<br>
      Hit The Start Capture Button<br>
      Hit The View Packet Button<br>
      <br>
      <br>
      <br>
      <h2>Things that I'll probably Continue to work on...<br>
      <br>
      <h3>Upgrading the Gui<br>
      Get the datagridview to update automatically<br>
      Decoding/encoding packet data.<br>
      <br>
      <h2>Quirks:<br>
	      <h3>At the moment, you have to press the View Packet Button again to update the data view.<br>
	      <h3>The Capture Ethernet Packets Button is there for library capability testing.<br> 
      <br>
      I'd like to be able to get better location data as the whois this is connected to gets caught up with bot checks, any suggestions?<br>
      It'd be great to see live graphs on the incoming data statistics.
      <br>
   
      
    
      
