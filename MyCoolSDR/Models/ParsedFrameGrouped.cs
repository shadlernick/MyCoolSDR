namespace MyCoolSDR.Models;

public class ParsedFrameGrouped
{
    public ulong CreatedOn { get; set; }
    public ulong Frequency { get; set; }
    public ulong Bandwidth { get; set; }
    public float SNR { get; set; }
    public int Count { get; set; }

    public bool InRange(ulong frequency)
    {
        var lowerBound = this.Frequency - (this.Bandwidth / 2);
        var upperBound = this.Frequency + (this.Bandwidth / 2);

        return lowerBound <= frequency && upperBound >= frequency;
    }
}
