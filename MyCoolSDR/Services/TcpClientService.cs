using System.Diagnostics;

namespace MyCoolSDR.Services;

public class TcpClientService(string serverAddress = "127.0.0.1", int serverPort = 5000)
{
    private readonly string _serverAddress = serverAddress;
    private readonly int _serverPort = serverPort;

    public async Task<byte[]> DownloadFileAsync(string filename)
    {
        try
        {
            Debug.WriteLine($"[Mock] Connecting to TCP server at {_serverAddress}:{_serverPort}");

            // Simulate network delay
            await Task.Delay(100);

            Debug.WriteLine($"[Mock] Connected to TCP server");
            Debug.WriteLine($"[Mock] Sent request for file: {filename}");

            var testData = GenerateTestData();

            Debug.WriteLine($"[Mock] Server response size: {testData.Length} bytes");
            Debug.WriteLine($"[Mock] Successfully received {testData.Length} bytes from TCP server");

            return testData;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"TCP Mock Error: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Generates a byte array containing sample data frames for testing purposes.
    /// </summary>
    /// <remarks>The generated frames include both textual and binary payloads with varying type
    /// identifiers. This method is intended for use in test scenarios and should not be used in production
    /// environments.</remarks>
    /// <returns>A byte array representing multiple test data frames, each containing a type identifier and associated
    /// payload.</returns>
    private byte[] GenerateTestData()
    {
        var frames = new List<byte>();

        // Generate 12 similar data frames (Group 1: 433 MHz range)
        for (int i = 0; i < 12; i++)
        {
            ulong unixTimestamp = (ulong)DateTimeOffset.UtcNow.ToUnixTimeSeconds() + (ulong)i;
            ulong frequency = 433000000 + (ulong)(i * 1000); // Vary frequency slightly: 433 MHz, 433.001 MHz, etc.
            UInt32 bandwidth = 125000; // 125 kHz
            double snr = 10.5 + i; // Vary SNR: 10.5, 11.5, 12.5, etc.

            frames.AddRange(BitConverter.GetBytes(unixTimestamp));     // 8 bytes
            frames.AddRange(BitConverter.GetBytes(frequency));         // 8 bytes
            frames.AddRange(BitConverter.GetBytes(bandwidth));         // 4 bytes
            frames.AddRange(BitConverter.GetBytes(snr));               // 8 bytes
        }

        // Generate 6 frames with significantly different frequency (Group 2: 435 MHz range)
        for (int i = 0; i < 6; i++)
        {
            ulong unixTimestamp = (ulong)DateTimeOffset.UtcNow.ToUnixTimeSeconds() + (ulong)(12 + i);
            ulong frequency = 435000000 + (ulong)(i * 1000); // Significantly different frequency: 435 MHz
            UInt32 bandwidth = 125000; // 125 kHz
            double snr = 8.5 + i; // Different SNR range: 8.5, 9.5, 10.5, etc.

            frames.AddRange(BitConverter.GetBytes(unixTimestamp));     // 8 bytes
            frames.AddRange(BitConverter.GetBytes(frequency));         // 8 bytes
            frames.AddRange(BitConverter.GetBytes(bandwidth));         // 4 bytes
            frames.AddRange(BitConverter.GetBytes(snr));               // 8 bytes
        }

        // Generate additional frames with significantly different frequency (Group 1: 433 MHz range)
        for (int i = 0; i < 2; i++)
        {
            ulong unixTimestamp = (ulong)DateTimeOffset.UtcNow.ToUnixTimeSeconds() + (ulong)i;
            ulong frequency = 433000000 + (ulong)(i * 1000); // Vary frequency slightly: 433 MHz, 433.001 MHz, etc.
            UInt32 bandwidth = 125000; // 125 kHz
            double snr = 10.5 + i; // Vary SNR: 10.5, 11.5, 12.5, etc.

            frames.AddRange(BitConverter.GetBytes(unixTimestamp));     // 8 bytes
            frames.AddRange(BitConverter.GetBytes(frequency));         // 8 bytes
            frames.AddRange(BitConverter.GetBytes(bandwidth));         // 4 bytes
            frames.AddRange(BitConverter.GetBytes(snr));               // 8 bytes
        }

        // Generate additional frames with significantly different frequency (Group 2: 435 MHz range)
        for (int i = 0; i < 3; i++)
        {
            ulong unixTimestamp = (ulong)DateTimeOffset.UtcNow.ToUnixTimeSeconds() + (ulong)(12 + i);
            ulong frequency = 435000000 + (ulong)(i * 1000); // Significantly different frequency: 435 MHz
            UInt32 bandwidth = 125000; // 125 kHz
            double snr = 8.5 + i; // Different SNR range: 8.5, 9.5, 10.5, etc.

            frames.AddRange(BitConverter.GetBytes(unixTimestamp));     // 8 bytes
            frames.AddRange(BitConverter.GetBytes(frequency));         // 8 bytes
            frames.AddRange(BitConverter.GetBytes(bandwidth));         // 4 bytes
            frames.AddRange(BitConverter.GetBytes(snr));               // 8 bytes
        }

        byte type = 0x01;
        int size = frames.Count();

        byte lsb = (byte)(size & 0xFF);        // lsb 8 біт
        byte msb = (byte)((size >> 8) & 0x1F); // msb 5 біт
        byte byte2 = (byte)((type << 5) | msb);

        byte[] header = [lsb, byte2];

        frames.InsertRange(0, header); // Add header at the beginning

        return [.. frames];
    }
}