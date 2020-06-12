using Sudoku.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sudoku.Models
{
  public class Area
  {
    public List<Cell> Cells { get; set; } = new List<Cell>();
    public Canvas Visual { get; set; } = new Canvas() { Width = 90, Height = 90 };

    public Area(byte index)
    {
      WrapPanel wrapPanel = new WrapPanel()
      {
        Width = 90,
        Height = 90,
      };
      for (byte i = 0; i < 9; i++)
      {
        Cells.Add(new Cell(i));
        wrapPanel.Children.Add(Cells[i].CellVisual);
      }
      Border border = new Border()
      {
        Width = 90,
        Height = 90,
        BorderThickness = new Thickness(1),
        BorderBrush = Brushes.Black,
        HorizontalAlignment = HorizontalAlignment.Stretch,
        VerticalAlignment = VerticalAlignment.Stretch,
        Child = wrapPanel,
      };
      Visual.Children.Add(border);
    }

  }
}
