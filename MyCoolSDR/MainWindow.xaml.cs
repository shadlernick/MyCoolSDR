using System.Collections.ObjectModel;
using System.Windows;
using MyCoolSDR.Models;
using MyCoolSDR.Services;

namespace MyCoolSDR;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private TcpClientService _tcpClient;
    private DataParserService _parser;

    public ObservableCollection<ParsedFrameGrouped> Results { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        Results = [];
        this.DataContext = this;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        InitializeServices();
    }

    private void InitializeServices()
    {
        _tcpClient = new TcpClientService();
        _parser = new DataParserService();
    }

    private async void FetchData_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            FetchDataButton.IsEnabled = false;
            StatusText.Text = "Fetching data...";

            var rawData = await _tcpClient.DownloadFileAsync("testfile.bin");

            var rawFrames = _parser.ParseMultipleFrames(rawData);
            var groupedFrames = AggregationService.AggregateByFirst(rawFrames);

            Results.Clear();
            foreach (var group in groupedFrames)
            {
                Results.Add(group);
            }
            StatusText.Text = $"Successfully loaded {rawFrames.Count} frames in {groupedFrames.Count} group(s)";
        }
        catch (Exception ex)
        {
            StatusText.Text = $"Error: {ex.Message}";
            MessageBox.Show($"Error: {ex.Message}", "File Error");
        }
        finally
        {
            FetchDataButton.IsEnabled = true;
        }
    }
}