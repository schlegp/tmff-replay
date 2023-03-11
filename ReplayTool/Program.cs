// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Text;
using BytePacketExtractor;
using LibUsbDotNet;
using LibUsbDotNet.LibUsb;
using LibUsbDotNet.Main;

///
/// Args[0] = ProductId of your Thrustmaster Wheel
/// Args[1] = csv-File to read data from
/// 

var csvFile = File.ReadAllLines(args[0]);
// var csvFile = File.ReadAllLines("Unlock100Set100SetDamping1ExtremeTestFFBSetDamping0TestFFB.csv");
var packetExtractor = new Extractor();
var packets = packetExtractor.ExtractFromCsvFile(csvFile).ToList();

var VendorId = 0x044F;
var ProductId = Convert.ToInt32(args[0], 16);
using (var context = new UsbContext())
{
    context.SetDebugLevel(LogLevel.Info);
    var usbDeviceCollection = context.List();
    var selectedDevice = usbDeviceCollection.FirstOrDefault(d => d.ProductId == ProductId && d.VendorId == VendorId);
    
    selectedDevice.Open();
    selectedDevice.ClaimInterface(selectedDevice.Configs[0].Interfaces[0].Number);
    
    var writeEndpoint = selectedDevice.OpenEndpointWriter(WriteEndpointID.Ep01);
    var sw = new Stopwatch();
    foreach (var packet in packets)
    {
        sw.Stop();
        var waitTime = Math.Max(0, (int)(packet.Delta - sw.ElapsedMilliseconds));
        sw.Reset();
        Console.WriteLine($"{packet.Delta}ms | {packet.PlainText}");
        Thread.Sleep(waitTime);
        sw.Start();
        writeEndpoint.Write(packet.ToByteArray(), 5, out var _);
    }
    
}