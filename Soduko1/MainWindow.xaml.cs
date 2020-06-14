using Sudoku.ViewModels;

using Sudoku1.ViewModels;

using System.Windows;

namespace Sudoku
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainViewModel MainVM { get; set; }

    public MainWindow()
    {
      InitializeComponent();
      MainVM = new MainViewModel(this);
      DataContext = MainVM;
    }

    private void ActionButton_Click(object sender, RoutedEventArgs e)
    {
      byte location = (byte)((byte)(LocationListBox.SelectedIndex));
      switch (DirectionListBox.SelectedIndex)
      {
        case 0:
          Log.Write($"Manual start counting column {location}");
          MainVM.CountNumbersColumn(location);
          break;
        case 1:
          Log.Write($"Manual start counting row {location}");
          MainVM.CountNumbersRow(location);
          break;
        case 2:
          Log.Write($"Manual start counting area {location}");
          byte column = (byte)((location % 3) * 3);
          byte row = (byte)((location / 3) * 3);
          MainVM.CountNumbersArea(column, row);
          break;
        default:
          break;
      }
    }

    private void GoToSetButton_Click(object sender, RoutedEventArgs e)
    {
      MainVM.ActOnToSet();
    }
  }
}
