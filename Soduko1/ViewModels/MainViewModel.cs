using Sudoku.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sudoku.ViewModels
{
  public class MainViewModel
  {
    private readonly MainWindow MainView;
    private bool changed = true;
    public List<Cell> Sudoku { get; set; } = new List<Cell>();
    public Canvas Visual { get; set; } = new Canvas();

    public MainViewModel(MainWindow mainView)
    {
      MainView = mainView;
      CreateSudoku();
      ShowSudoku();

      FillCells();
    }

    private void CreateSudoku()
    {
      WrapPanel wrapPanel = new WrapPanel()
      {
        Width = 405,
        Height = 405,
      };
      for (byte i = 0; i < 81; i++)
      {
        Sudoku.Add(new Cell(i));
        wrapPanel.Children.Add(Sudoku[i].CellVisual);
      }
      Border border = new Border()
      {
        Width = 405,
        Height = 405,
        BorderThickness = new Thickness(1),
        BorderBrush = Brushes.Black,
        HorizontalAlignment = HorizontalAlignment.Stretch,
        VerticalAlignment = VerticalAlignment.Stretch,
        Child = wrapPanel,
      };
      Visual.Children.Add(border);

      wrapPanel = new WrapPanel()
      {
        Width = 405,
        Height = 405,
      };
      for (int i = 0; i < 9; i++)
      {
        border = new Border()
        {
          Width = 135,
          Height = 135,
          BorderThickness = new Thickness(1),
          BorderBrush = Brushes.Black,
        };
        wrapPanel.Children.Add(border);
      }
      Visual.Children.Add(wrapPanel);
    }

    private void ShowSudoku()
    {
      MainView.SudocuGrid.Children.Add(Visual);
    }

    private void FillCells()
    {
      FillCorrectedCell(1, 1, 1);
      FillCorrectedCell(2, 1, 9);
      FillCorrectedCell(5, 1, 8);
      FillCorrectedCell(8, 1, 7);
      FillCorrectedCell(9, 1, 3);
      FillCorrectedCell(4, 2, 3);
      FillCorrectedCell(6, 2, 1);
      FillCorrectedCell(3, 3, 2);
      FillCorrectedCell(7, 3, 6);
      FillCorrectedCell(1, 4, 2);
      FillCorrectedCell(5, 4, 9);
      FillCorrectedCell(9, 4, 7);
      FillCorrectedCell(5, 5, 2);
      FillCorrectedCell(2, 5, 4);
      FillCorrectedCell(8, 5, 6);
      FillCorrectedCell(3, 6, 1);
      FillCorrectedCell(5, 6, 3);
      FillCorrectedCell(7, 6, 5);
      FillCorrectedCell(4, 7, 9);
      FillCorrectedCell(5, 7, 1);
      FillCorrectedCell(6, 7, 6);
      FillCorrectedCell(5, 8, 5);
      FillCorrectedCell(1, 9, 8);
      FillCorrectedCell(5, 9, 7);
      FillCorrectedCell(9, 9, 9);
    }

    private void FillCorrectedCell(byte column, byte row, byte result, Cell.CellTypes action = Cell.CellTypes.Preset)
    {
      FillCell((byte)(column - 1), (byte)(row - 1), result, action);
    }

    private void FillCell(byte column, byte row, byte result, Cell.CellTypes action)
    {
      Sudoku[(byte)(column + row * 9)].SetCell(result, action);
      
      ClearNumbersArea(column, row, result);
      ClearNumbersColumn(column, result);
      ClearNumbersRow(row, result);
    }

    private void ClearNumbersColumn(byte column, byte result)
    {
      for (byte row = 0; row < 9; row++)
      {
        ClearNumbersCell(column, row, result);
      }
      CountNumbersColumn(column);
    }

    private void CountNumbersColumn(byte column)
    {
      Dictionary<byte, (byte counting, byte index)> count = 
        new Dictionary<byte, (byte counting, byte index)>();

      for (byte row = 0; row < 9; row++)
      {
        if (Sudoku[(byte)(column + row * 9)].Numbers.Count == 0) { continue; }
        foreach (Number item in Sudoku[(byte)(column + row * 9)].Numbers)
        {
          if (!count.ContainsKey(item.Digit))
          {
            count.Add(item.Digit, (1, row));
          }
          else
          {
            var value = count[item.Digit];
            value.counting++;
            count[item.Digit] = value;
          }
        }
      }

      foreach (var item in count.Where(x => x.Value.counting == 1))
      {
        FillCell(column, item.Value.index, item.Key, Cell.CellTypes.Calulated);
      }
    }

    private void ClearNumbersRow(byte row, byte result)
    {
      for (byte column = 0; column < 9; column++)
      {
        ClearNumbersCell(column, row, result);
      }
      CountNumbersRow(row);
    }

    private void CountNumbersRow(byte row)
    {
      Dictionary<byte, (byte counting, byte index)> count = 
        new Dictionary<byte, (byte counting, byte index)>();

      for (byte column = 0; column < 9; column++)
      {
        if (Sudoku[(byte)(column + row * 9)].Numbers.Count == 0) { continue; }
        foreach (Number item in Sudoku[(byte)(column + row * 9)].Numbers)
        {
          if (!count.ContainsKey(item.Digit))
          {
            count.Add(item.Digit, (1, column));
          }
          else
          {
            var value = count[item.Digit];
            value.counting++;
            count[item.Digit] = value;
          }
        }
      }

      foreach (var item in count.Where(x => x.Value.counting == 1))
      {
        FillCell(item.Value.index, row, item.Key, Cell.CellTypes.Calulated);
      }
    }

    private void ClearNumbersArea(byte column, byte row, byte result)
    {
      byte beginColumn = (byte)(((byte)(column / 3)) * 3);
      byte beginRow = (byte)(((byte)(row / 3)) * 3);

      for (byte j = 0; j < 3; j++)
      {
        for (byte i = 0; i < 3; i++)
        {
          ClearNumbersCell((byte)(i + beginColumn), (byte)(j + beginRow), result);
        }
      }
      CountNumbersArea(beginColumn, beginRow);
    }

    private void CountNumbersArea(byte _column, byte _row)
    {
      Dictionary<byte, (byte counting, byte index)> count =
        new Dictionary<byte, (byte counting, byte index)>();
      byte beginColumn = (byte)(((byte)(_column / 3)) * 3);
      byte beginRow = (byte)(((byte)(_row / 3)) * 3);

      for (byte column = beginColumn; column < beginColumn + 3; column++)
      {
        for (byte row = beginRow; row < beginRow + 3; row++)
        {
          if (Sudoku[(byte)(column + row * 9)].Numbers.Count == 0) { continue; }
          foreach (Number item in Sudoku[(byte)(column + row * 9)].Numbers)
          {
            if (!count.ContainsKey(item.Digit))
            {
              count.Add(item.Digit, (1, (byte)(column + row * 9)));
            }
            else
            {
              var value = count[item.Digit];
              value.counting++;
              count[item.Digit] = value;
            }
          }
        }
      }

      foreach (var item in count.Where(x => x.Value.counting == 1))
      {
        FillCell((byte)(item.Value.index % 9), (byte)(item.Value.index / 9), 
          item.Key, Cell.CellTypes.Calulated);
      }
    }

    private void ClearNumbersCell(byte column, byte row, byte result)
    {
      Sudoku[(byte)(column + row * 9)].RemoveNumber(result);
      if (Sudoku[(byte)(column + row * 9)].Numbers.Count == 1)
      {
        FillCell(column, row, GetNumbersCellResult(column, row), Cell.CellTypes.Calulated);
      }
    }

    private byte GetNumbersCellResult(byte column, byte row)
    {
      return Sudoku[(byte)(column + row * 9)].Numbers[0].Digit;
    }
  }
}
