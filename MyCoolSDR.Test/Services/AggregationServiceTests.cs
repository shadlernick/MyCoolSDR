using MyCoolSDR.Services;

namespace MyCoolSDR.Test.Services;

[TestClass]
public class AggregationServiceTests
{
    [TestMethod]
    public void AggregateByFirst_WithNull_ThrowsArgumentNullException()
    {
        // Act
        Assert.ThrowsExactly<ArgumentNullException>(() => AggregationService.AggregateByFirst(null!));
    }

    [TestMethod]
    public void AggregateByFirst_WithEmptyList_ReturnsEmptyList()
    {
        // Arrange
        var frames = new List<ParsedFrame>();

        // Act
        var result = AggregationService.AggregateByFirst(frames);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsEmpty(result);
    }

    [TestMethod]
    public void AggregateByFirst_WithSingleFrame_ReturnsSingleGroup()
    {
        // Arrange
        var frames = new List<ParsedFrame>
        {
            new() { CreatedOn = 1000, Frequency = 433000000, Bandwidth = 125000, SNR = 10 }
        };

        // Act
        var result = AggregationService.AggregateByFirst(frames);

        // Assert
        Assert.HasCount(1, result);
        Assert.AreEqual(1, result[0].Count);
        Assert.AreEqual(433000000UL, result[0].Frequency);
    }

    [TestMethod]
    public void AggregateByFirst_WithFramesInRange_GroupsThem()
    {
        // Arrange
        var frames = new List<ParsedFrame>
        {
            new() { CreatedOn = 1000, Frequency = 433000000, Bandwidth = 125000, SNR = 10 },
            new() { CreatedOn = 1001, Frequency = 433001000, Bandwidth = 125000, SNR = 11 }, // Within range
            new() { CreatedOn = 1002, Frequency = 433062500, Bandwidth = 125000, SNR = 12 }, // Within range
        };

        // Act
        var result = AggregationService.AggregateByFirst(frames);

        // Assert
        Assert.HasCount(1, result, "All frames should be grouped into one");
        Assert.AreEqual(3, result[0].Count, "Group should contain 3 frames");
    }

    [TestMethod]
    public void AggregateByFirst_WithFramesOutOfRange_CreatesMultipleGroups()
    {
        // Arrange
        var frames = new List<ParsedFrame>
        {
            new() { CreatedOn = 1000, Frequency = 433000000, Bandwidth = 125000, SNR = 10 },
            new() { CreatedOn = 1001, Frequency = 435000000, Bandwidth = 125000, SNR = 11 }, // Different frequency, out of range
        };

        // Act
        var result = AggregationService.AggregateByFirst(frames);

        // Assert
        Assert.HasCount(2, result, "Should create two separate groups");
        Assert.AreEqual(1, result[0].Count);
        Assert.AreEqual(1, result[1].Count);
    }

    [TestMethod]
    public void AggregateByFirst_PreservesFrameData()
    {
        // Arrange
        var frames = new List<ParsedFrame>
        {
            new() 
            { 
                CreatedOn = 1000, 
                Frequency = 433000000, 
                Bandwidth = 125000, 
                SNR = 10.5f 
            }
        };

        // Act
        var result = AggregationService.AggregateByFirst(frames);

        // Assert
        Assert.AreEqual(1000UL, result[0].CreatedOn);
        Assert.AreEqual(433000000UL, result[0].Frequency);
        Assert.AreEqual(125000UL, result[0].Bandwidth);
        Assert.AreEqual(10.5f, result[0].SNR, 0.01f);
    }

    [TestMethod]
    public void AggregateByFirst_LargeDataSet_PerformsEfficiently()
    {
        // Arrange
        var frames = new List<ParsedFrame>();
        
        // Generate 1000 frames
        for (int i = 0; i < 1000; i++)
        {
            frames.Add(new ParsedFrame
            {
                CreatedOn = (ulong)i,
                Frequency = 433000000 + (ulong)(i * 1000),
                Bandwidth = 125000,
                SNR = 10f + i % 20
            });
        }

        // Act
        var result = AggregationService.AggregateByFirst(frames);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotEmpty(result, "Should have grouped frames");
        Assert.AreEqual(1000, result.Sum(g => g.Count), "Total count should match input");
    }
}
