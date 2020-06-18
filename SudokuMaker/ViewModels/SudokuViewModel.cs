using SudokuMaker.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SudokuMaker.ViewModels
{
  public class SudokuViewModel : INotifyPropertyChanged
  {

    public const double NUMBERWIDTH = 15;

    #region [ Fields ]

    MainWindow MainView;

    #endregion

    #region [ Properties ]

    public Border Field { get; set; }

    #endregion

    #region [ Public event ]

    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged(string propertyName = "")
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

    #region [ Construction ]

    public SudokuViewModel(MainWindow mainView)
    {
      MainView = mainView;
      CreateField();
    }

    #endregion

    #region [ Public methods ]


    #endregion

    private async Task CreateField() 
    {
      WrapPanel wrapPanel = new WrapPanel()
      {
        Width = 9 * 3 * NUMBERWIDTH,
        Height = 9 * 3 * NUMBERWIDTH,
      };

      Field = new Border()
      {
        Name = "SudokuField",
        Width = 9 * 3 * NUMBERWIDTH,
        Height = 9 * 3 * NUMBERWIDTH,
        BorderThickness = new Thickness(2),
        BorderBrush = Brushes.Black,
      };
      Field.Child = wrapPanel;
      
      await CreateCells();

      MainView.SudokoCanvas.Children.Add(Field);
    }

    private async Task CreateCells()
    {
      for (int i = 0; i < 81; i++)
      {
        WrapPanel wrapPanel = (WrapPanel)Field.Child;
        wrapPanel.Children.Add(await new Cell().CreateCell(i));
      }
    }
  }
}
