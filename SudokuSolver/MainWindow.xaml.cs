using Sudoku.ViewModels;

using Sudoku1.ViewModels;

using System.Threading;
using System.Threading.Tasks;
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

    private void Window_LayoutUpdated(object sender, System.EventArgs e)
    {
    }

  }
}
