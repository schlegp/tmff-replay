namespace BytePacketExtractor;

public class BytePacket
{
    public double Delta { get; set; }
    public ByteData ByteData { get; set; }
    public string PlainText { get; set; }
    public byte[] ToByteArray()
    {
        var value = new List<byte>{ByteData.Header01, ByteData.Header02, ByteData.ID, ByteData.Command};
        value.AddRange(ByteData.data);
        return value.ToArray();
    }
}

public struct ByteData
{
    public byte Header01;
    public byte Header02;
    public byte ID;
    public byte Command;
    public byte[] data;
}

