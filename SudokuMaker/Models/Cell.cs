using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SudokuMaker.Models
{
  public class Cell : INotifyPropertyChanged
  {

    #region [ Fields ]


    #endregion

    #region [ Properties ]

    public Border CellBorder { get; set; }
    public List<Number> Numbers { get; set; } = new List<Number>();

    #endregion

    #region [ Public event ]

    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged(string propertyName = "")
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

    #region [ Construction ]

    public Cell()
    {
    }

    #endregion

    #region [ Public methods ]

    public async Task<Border> CreateCell(int index)
    {
      CellBorder = new Border()
      {
        Name = $"Cell_{index}",
        Width = 3 * 15,
        Height = 3 * 15,
        BorderThickness = new Thickness(0.5),
        BorderBrush = Brushes.Black,
      };
      return CellBorder;
    }

    #endregion

    private async Task CreateCellNumbers()
    {

    }

  }
}
