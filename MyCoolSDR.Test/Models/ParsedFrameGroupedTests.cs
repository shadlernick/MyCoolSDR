using MyCoolSDR.Models;

namespace MyCoolSDR.Test.Models;

[TestClass]
public class ParsedFrameGroupedTests
{
    [TestMethod]
    public void InRange_WithFrequencyInRange_ReturnsTrue()
    {
        // Arrange
        var frame = new ParsedFrameGrouped
        {
            Frequency = 433000000,
            Bandwidth = 125000
        };
        ulong testFrequency = 433000000; // Exact center

        // Act
        var result = frame.InRange(testFrequency);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void InRange_WithFrequencyAtLowerBound_ReturnsTrue()
    {
        // Arrange
        var frame = new ParsedFrameGrouped
        {
            Frequency = 433000000,
            Bandwidth = 125000
        };
        ulong testFrequency = 433000000 - (125000 / 2); // Lower bound

        // Act
        var result = frame.InRange(testFrequency);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void InRange_WithFrequencyAtUpperBound_ReturnsTrue()
    {
        // Arrange
        var frame = new ParsedFrameGrouped
        {
            Frequency = 433000000,
            Bandwidth = 125000
        };
        ulong testFrequency = 433000000 + (125000 / 2); // Upper bound

        // Act
        var result = frame.InRange(testFrequency);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void InRange_WithFrequencyBelowLowerBound_ReturnsFalse()
    {
        // Arrange
        var frame = new ParsedFrameGrouped
        {
            Frequency = 433000000,
            Bandwidth = 125000
        };
        ulong testFrequency = 433000000 - (125000 / 2) - 1; // Just below lower bound

        // Act
        var result = frame.InRange(testFrequency);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void InRange_WithFrequencyAboveUpperBound_ReturnsFalse()
    {
        // Arrange
        var frame = new ParsedFrameGrouped
        {
            Frequency = 433000000,
            Bandwidth = 125000
        };
        ulong testFrequency = 433000000 + (125000 / 2) + 1; // Just above upper bound

        // Act
        var result = frame.InRange(testFrequency);

        // Assert
        Assert.IsFalse(result);
    }
}
