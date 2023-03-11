using System.Text.RegularExpressions;

namespace BytePacketExtractor;

public partial class ExtractionRegex
{
    [GeneratedRegex("\".*\",\"(.*)\",\".*\",\".*\",\".*\",\".*\",\"(.*)\",\".*\"")]
    public static partial Regex TimeData();
}