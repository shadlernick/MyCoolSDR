namespace MyCoolSDR.Services;

public class DataParserService
{
    private ParsedFrame ParseFrame(byte[] rawData)
    {
        if (rawData.Length < FRAME_SIZE)
        {
            return new ParsedFrame();
        }

        UInt64 createdOn = BitConverter.ToUInt64(rawData, 0);
        UInt64 frequency = BitConverter.ToUInt64(rawData, 8);
        UInt32 bandwidth = BitConverter.ToUInt32(rawData, 16);
        float snr = (float)BitConverter.ToDouble(rawData, 20); // SNR is transferred as double (8 bytes) but we convert it to float for our model

        return new ParsedFrame
        {
            CreatedOn = createdOn,
            Frequency = frequency,
            Bandwidth = bandwidth,
            SNR = snr
        };
    }

    private const int FRAME_SIZE = 28; // Each frame is 28 bytes: 8 (createdOn) + 8 (frequency) + 4 (bandwidth) + 8 (SNR)
    private const int HEADER_SIZE = 2;
    private const int TYPE_MASK = 0x07;
    private const int TYPE_SHIFT = 5;
    private const int MSB_MASK = 0x1F;
    
    public List<ParsedFrame> ParseMultipleFrames(byte[] rawData)
    {
        var parsedFrames = new List<ParsedFrame>();
        int offset = 0;

        while (offset < rawData.Length)
        {
            // Check if there's enough data for header (2 bytes: LSB, type+MSB)
            if (offset + HEADER_SIZE > rawData.Length)
            {
                break;
            }

            // Parse header: reconstruct size from LSB and MSB
            byte lsb = rawData[offset];
            byte byte2 = rawData[offset + 1];
            
            byte type = (byte)((byte2 >> TYPE_SHIFT) & TYPE_MASK);  // Extract 3-bit type
            byte msb = (byte)(byte2 & MSB_MASK);          // Extract 5-bit MSB
            
            int payloadSize = lsb | (msb << 8);
            offset += HEADER_SIZE;

            // Verify there's enough data for the payload
            if (offset + payloadSize > rawData.Length)
            {
                break;
            }

            // Parse each 28-byte frame within the payload
            for (int i = 0; i < payloadSize; i += FRAME_SIZE)
            {
                if (i + FRAME_SIZE <= payloadSize)
                {
                    byte[] frameData = new byte[FRAME_SIZE];
                    Array.Copy(rawData, offset + i, frameData, 0, FRAME_SIZE);
                    var parsedFrame = ParseFrame(frameData);
                    parsedFrames.Add(parsedFrame);
                }
            }

            offset += payloadSize;
        }

        return parsedFrames;
    }
}

public class ParsedFrame
{
    public UInt64 CreatedOn { get; set; }
    public UInt64 Frequency { get; set; }
    public UInt32 Bandwidth { get; set; }
    public float SNR { get; set; }
}