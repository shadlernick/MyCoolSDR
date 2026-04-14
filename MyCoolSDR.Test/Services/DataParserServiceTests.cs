using MyCoolSDR.Services;

namespace MyCoolSDR.Test.Services;

[TestClass]
public class DataParserServiceTests
{
    private DataParserService _service = null!;

    [TestInitialize]
    public void Setup()
    {
        _service = new DataParserService();
    }

    [TestMethod]
    public void ParseFrame_WithTooShortData_HandlesGracefully()
    {
        // Arrange
        byte[] rawData = new byte[10]; // Too short

        // Act
        var result = _service.ParseMultipleFrames(rawData);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsEmpty(result);
    }

    [TestMethod]
    public void ParseMultipleFrames_WithValidPayload_ParsesAllFrames()
    {
        // Arrange
        var frames = new List<byte>();

        // Header: LSB = 28 (1 frame of 28 bytes), type=0, MSB=0
        frames.Add(28);      // LSB (payload size = 28 bytes)
        frames.Add(0x00);    // byte2: type=0 (upper 3 bits), MSB=0 (lower 5 bits)

        // 28-byte frame data (Unix timestamp, frequency, bandwidth, SNR)
        byte[] frameData = new byte[28];
        BitConverter.GetBytes(1000UL).CopyTo(frameData, 0);      // CreatedOn
        BitConverter.GetBytes(433000000UL).CopyTo(frameData, 8); // Frequency
        BitConverter.GetBytes(125000U).CopyTo(frameData, 16);    // Bandwidth
        BitConverter.GetBytes(10.5).CopyTo(frameData, 20);       // SNR

        frames.AddRange(frameData);

        // Act
        var result = _service.ParseMultipleFrames(frames.ToArray());

        // Assert
        Assert.IsNotNull(result);
        Assert.HasCount(1, result);
        Assert.AreEqual(433000000UL, result[0].Frequency);
    }

    [TestMethod]
    public void ParseMultipleFrames_WithMultipleFrames_ParsesAll()
    {
        // Arrange
        var frames = new List<byte>();

        // Create 2 frames
        for (int frameIdx = 0; frameIdx < 2; frameIdx++)
        {
            frames.Add(28);      // LSB (payload size)
            frames.Add(0x00);    // byte2

            byte[] frameData = new byte[28];
            BitConverter.GetBytes((ulong)(1000 + frameIdx)).CopyTo(frameData, 0);
            BitConverter.GetBytes((ulong)(433000000 + frameIdx * 1000)).CopyTo(frameData, 8);
            BitConverter.GetBytes(125000U).CopyTo(frameData, 16);
            BitConverter.GetBytes(10.5 + frameIdx).CopyTo(frameData, 20);

            frames.AddRange(frameData);
        }

        // Act
        var result = _service.ParseMultipleFrames(frames.ToArray());

        // Assert
        Assert.HasCount(2, result);
        Assert.AreEqual(433000000UL, result[0].Frequency);
        Assert.AreEqual(433001000UL, result[1].Frequency);
    }

    [TestMethod]
    public void ParseMultipleFrames_WithEmptyData_ReturnsEmpty()
    {
        // Arrange
        byte[] emptyData = new byte[0];

        // Act
        var result = _service.ParseMultipleFrames(emptyData);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsEmpty(result);
    }

    [TestMethod]
    public void ParseMultipleFrames_WithIncompleteHeader_ReturnsEmpty()
    {
        // Arrange
        byte[] incompleteHeader = new byte[1] { 28 };

        // Act
        var result = _service.ParseMultipleFrames(incompleteHeader);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsEmpty(result);
    }

    [TestMethod]
    public void ParseMultipleFrames_WithIncompletePayload_SkipsIncompleteFrame()
    {
        // Arrange
        var frames = new List<byte>();
        frames.Add(28);      // LSB
        frames.Add(0x00);    // byte2
        frames.AddRange(new byte[10]); // Incomplete payload (only 10 bytes instead of 28)

        // Act
        var result = _service.ParseMultipleFrames(frames.ToArray());

        // Assert
        Assert.IsNotNull(result);
        Assert.IsEmpty(result); // Should skip incomplete frames
    }
}
