using MyCoolSDR.Converters;
using System.Globalization;

namespace MyCoolSDR.Test.Converters;

[TestClass]
public class HzToMhzConverterTests
{
    private HzToMhzConverter _converter = null!;

    [TestInitialize]
    public void Setup()
    {
        _converter = new HzToMhzConverter();
    }

    [TestMethod]
    public void Convert_With433MHz_ReturnsFormattedString()
    {
        // Arrange
        ulong hz = 433000000;

        // Act
        var result = _converter.Convert(hz, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("433.000", result);
    }

    [TestMethod]
    public void Convert_With435MHz_ReturnsFormattedString()
    {
        // Arrange
        ulong hz = 435000000;

        // Act
        var result = _converter.Convert(hz, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.AreEqual("435.000", result);
    }

    [TestMethod]
    public void Convert_WithZero_ReturnsZeroString()
    {
        // Arrange
        ulong hz = 0;

        // Act
        var result = _converter.Convert(hz, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.AreEqual("0.000", result);
    }

    [TestMethod]
    public void Convert_WithLargeValue_ReturnsFormattedString()
    {
        // Arrange
        ulong hz = 1234567890;

        // Act
        var result = _converter.Convert(hz, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.AreEqual("1234.568", result); // 1234567890 / 1,000,000 = 1234.56789
    }

    [TestMethod]
    public void Convert_WithNull_ReturnsNull()
    {
        // Act
        var result = _converter.Convert(null, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void Convert_WithNonUlongType_ReturnsOriginalValue()
    {
        // Arrange
        int value = 433000;

        // Act
        var result = _converter.Convert(value, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void ConvertBack_Throws()
    {
        // Act
        Assert.ThrowsExactly<NotImplementedException>(() => _converter.ConvertBack("433.000", typeof(ulong), null, CultureInfo.InvariantCulture));
    }
}

[TestClass]
public class HzToKhzConverterTests
{
    private HzToKhzConverter _converter = null!;

    [TestInitialize]
    public void Setup()
    {
        _converter = new HzToKhzConverter();
    }

    [TestMethod]
    public void Convert_With125kHz_ReturnsFormattedString()
    {
        // Arrange
        ulong hz = 125000;

        // Act
        var result = _converter.Convert(hz, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.AreEqual("125.000", result);
    }

    [TestMethod]
    public void Convert_With200kHz_ReturnsFormattedString()
    {
        // Arrange
        ulong hz = 200000;

        // Act
        var result = _converter.Convert(hz, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.AreEqual("200.000", result);
    }

    [TestMethod]
    public void Convert_WithZero_ReturnsZeroString()
    {
        // Arrange
        ulong hz = 0;

        // Act
        var result = _converter.Convert(hz, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.AreEqual("0.000", result);
    }

    [TestMethod]
    public void Convert_WithNull_ReturnsNull()
    {
        // Act
        var result = _converter.Convert(null, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void Convert_WithNonUlongType_ReturnsOriginalValue()
    {
        // Arrange
        int value = 125000;

        // Act
        var result = _converter.Convert(value, typeof(string), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void ConvertBack_Throws()
    {
        // Act
        Assert.ThrowsExactly<NotImplementedException>(() => _converter.ConvertBack("125.000", typeof(ulong), null, CultureInfo.InvariantCulture));
    }
}

[TestClass]
public class UInt64TimestampToDateTimeConverterTests
{
    private UInt64TimestampToDateTime _converter = null!;

    [TestInitialize]
    public void Setup()
    {
        _converter = new UInt64TimestampToDateTime();
    }

    [TestMethod]
    public void Convert_WithValidUnixTimestamp_ReturnsDateTime()
    {
        // Arrange
        ulong timestamp = 1000UL; // Unix timestamp: January 1, 1970 + 1000 seconds

        // Act
        var result = _converter.Convert(timestamp, typeof(DateTime), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<DateTime>(result);
        var dateTime = (DateTime)result;
        Assert.IsGreaterThan(DateTime.UnixEpoch, dateTime);
    }

    [TestMethod]
    public void Convert_WithZeroTimestamp_ReturnsUnixEpoch()
    {
        // Arrange
        ulong timestamp = 0UL;

        // Act
        var result = _converter.Convert(timestamp, typeof(DateTime), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.IsNotNull(result);
        var dateTime = (DateTime)result;
        Assert.AreEqual(DateTime.UnixEpoch, dateTime);
    }

    [TestMethod]
    public void Convert_WithNull_ReturnsNull()
    {
        // Act
        var result = _converter.Convert(null, typeof(DateTime), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void Convert_WithNonUlongType_ReturnsOriginalValue()
    {
        // Arrange
        int value = 1000;

        // Act
        var result = _converter.Convert(value, typeof(DateTime), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void ConvertBack_Throws()
    {
        // Act
        Assert.ThrowsExactly<NotImplementedException>(() => _converter.ConvertBack(DateTime.Now, typeof(ulong), null, CultureInfo.InvariantCulture));
    }
}
