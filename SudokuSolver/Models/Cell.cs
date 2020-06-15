using Sudoku.ViewModels;

using Sudoku1.ViewModels;

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
  public class Cell
  {
    public enum CellTypes { Open, Preset, Calulated };
    public List<Number> Numbers { get; private set; } = new List<Number>();
    public Canvas CellVisual { get; set; } = new Canvas() { Width = 45, Height = 45 };
    public byte Result { get; private set; }
    public CellTypes Type { get; set; }

    public Cell(byte index)
    {
      WrapPanel wrapPanel = new WrapPanel()
      {
        Width = 45,
        Height = 45
      };
      for (byte i = 0; i < 9; i++)
      {
        Numbers.Add(new Number(i));
        wrapPanel.Children.Add(Numbers[i].Visual);
      }
      Border border = new Border()
      {
        Name = $"Cell_{index}",
        Width = 45,
        Height = 45,
        BorderThickness = new Thickness(1),
        BorderBrush = Brushes.DarkGray,
        HorizontalAlignment = HorizontalAlignment.Stretch,
        VerticalAlignment = VerticalAlignment.Stretch,
        Child = wrapPanel,
      };
      CellVisual.Children.Add(border);
      Type = CellTypes.Open;
    }

    public void SetCell(byte result, CellTypes type)
    {
      TextBox textBox = new TextBox()
      {
        Text = result.ToString(),
        FontSize = 25,
        FontWeight = FontWeights.Bold,
        Foreground = type == CellTypes.Calulated ? Brushes.Blue : Brushes.Black,
        HorizontalAlignment = HorizontalAlignment.Center,
        VerticalAlignment = VerticalAlignment.Center,
        BorderThickness = new Thickness(0),
      };

      Border border = (Border)CellVisual.Children[0];
      border.Child = textBox;

      Numbers.Clear();
      Result = result;
      Type = type;

      Log.Write($"{border.Name} got {result} with {type}");
     }

    public bool RemoveNumber(byte number)
    {
      bool removed = false;

      if (Numbers.Count == 0) { return false; }

      for (int i = 0; i < Numbers.Count; i++)
      {
        if (Numbers[i].Digit == number)
        {
          Border border = (Border)CellVisual.Children[0];
          WrapPanel wrapPanel = (WrapPanel)border.Child;
          wrapPanel.Children.Remove(Numbers[i].Visual);
          Numbers.RemoveAt(i);
          removed = true;
          Log.Write($"Remove {number} from {border.Name} ==> {Numbers.Count} numbers left");
          break;
        }
      }
      return removed;
    }

  }
}
