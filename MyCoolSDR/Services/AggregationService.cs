using MyCoolSDR.Models;

namespace MyCoolSDR.Services;

public class AggregationService
{
    public static List<ParsedFrameGrouped> AggregateByFirst(List<ParsedFrame> rawFrames)
    {
        ArgumentNullException.ThrowIfNull(rawFrames);

        var groupedFrames = new List<ParsedFrameGrouped>();

        foreach (var frame in rawFrames)
        {
            var existingGroup = groupedFrames.FirstOrDefault(gf => gf.InRange(frame.Frequency));

            if (existingGroup != null)
            {
                existingGroup.Count++;
            }
            else
            {
                groupedFrames.Add(new ParsedFrameGrouped
                {
                    CreatedOn = frame.CreatedOn,
                    Frequency = frame.Frequency,
                    Bandwidth = frame.Bandwidth,
                    SNR = frame.SNR,
                    Count = 1
                });
            }
        }

        return groupedFrames;
    }
}
