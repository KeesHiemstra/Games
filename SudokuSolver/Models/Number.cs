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
  public class Number
  {
    public byte Digit { get; set; }
    public Canvas Visual { get; set; } = new Canvas() { Width = 15, Height = 15 };

    public Number(byte digit)
    {
      Digit = ++digit;
      TextBlock visual = new TextBlock()
      {
        Text = digit.ToString(),
        FontSize = 8,
      };
      Border border = new Border()
      {
        Name = $"D_{Digit}",
        Width = 15,
        Height = 15,
        HorizontalAlignment = HorizontalAlignment.Center,
        VerticalAlignment = VerticalAlignment.Center,
        Child = visual,
      };
      Visual.Children.Add(border);
    }
  }
}
