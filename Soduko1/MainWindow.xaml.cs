using Sudoku.ViewModels;

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
  }
}
