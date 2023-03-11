using System.Globalization;

namespace BytePacketExtractor;

public class Extractor
{
    private double previousTime;

    public Extractor()
    {
        previousTime = 0;
    }

    public IEnumerable<BytePacket> ExtractFromCsvFile(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            if (ExtractFromLine(line, out var packet))
            {
                yield return packet;                
            }
        }
    }
    private bool ExtractFromLine(string line, out BytePacket packet)
    {
        if (!ExtractionRegex.TimeData().IsMatch(line))
        {
            packet = new BytePacket();
            return false;
        }
        var match = ExtractionRegex.TimeData().Match(line);
        if (!double.TryParse(match.Groups[1].Value, CultureInfo.InvariantCulture, out var time))
        {
            packet = new BytePacket();
            return false;
        }
        // First packet waits 0 seconds
        var delta = previousTime != 0 ? time - previousTime : 0;
        var group2 = match.Groups[2].Value.Replace("�\\200\\246", "");
        packet = new BytePacket
        {
            Delta = delta* 1000,
            ByteData = ByteDataFromString(group2),
            PlainText = group2
        };
        previousTime = time;
        return true;
    }

    private static ByteData ByteDataFromString(string byteData)
    {
        
        var dataArray = Enumerable.Range(0, byteData.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(byteData.Substring(x, 2), 16))
            .ToArray();
        return new ByteData()
        {
            Header01 = dataArray[0],
            Header02 = dataArray[1],
            ID = dataArray[2],
            Command = dataArray[3],
            data = dataArray[4..]
        };
    }
}