using Sudoku.Models;

using Sudoku1.ViewModels;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sudoku.ViewModels
{
  public class MainViewModel
	{
		private readonly MainWindow MainView;
		private Dictionary<byte, byte> solution = new Dictionary<byte, byte>();
		public Dictionary<byte, byte> ToSet = new Dictionary<byte, byte>();
		public List<Cell> Sudoku { get; set; } = new List<Cell>();
		public Canvas Visual { get; set; } = new Canvas();

		public MainViewModel(MainWindow mainView)
		{
			MainView = mainView;
			CreateSudoku();
			ShowSudoku();

			LoadSudoku();
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
			MainView.SudokuBorder.Child = Visual;
		}

		private void LoadSudoku()
		{
			Log.DeleteLog();
			string path = @"C:\Users\chi\OneDrive\Data\Sudoku\2020080901.Sudoku";
			byte row = 0;

			Log.Write($"Using '{path}'");
			using (StreamReader stream = File.OpenText(path))
			{
				while (row < 9 && !stream.EndOfStream)
				{
					string sudoku = stream.ReadLine();
					for (byte column = 0; column < sudoku.Length; column++)
					{
						string charDigit = sudoku[column].ToString();
						if ("123456789".Contains(charDigit))
						{
							byte result = byte.Parse(charDigit);
							Log.Write($"Set {charDigit} to ({column},{row}) form file");
							FillCell(column, row, result, Cell.CellTypes.Preset);
						}
					}
					row++;
				}
			}

			Run();
      if (solution.Count == 81)
      {
				Log.Write("Solved");
      }
		}

		private void FillCell(byte column, byte row, byte result, Cell.CellTypes action)
		{
			Sudoku[(byte)(column + row * 9)].SetCell(result, action);
      if (!solution.ContainsKey((byte)(column + row * 9)))
      {
				solution.Add((byte)(column + row * 9), result);
			}

			ClearNumbersColumn(column, result);
			ClearNumbersRow(row, result);
			ClearNumbersArea(column, row, result);

			CountNumbersColumn(column);
			CountNumbersRow(row);
			CountNumbersArea(column, row);
		}

		private void Run()
    {
      while (ToSet.Count > 0)
      {
				ActOnToSet();

        for (byte i = 0; i < 9; i++)
        {
					CountNumbersArea((byte)((i % 3) * 3), (byte)((i / 3) * 3));
        }
        if (ToSet.Count > 0) { continue; }

        for (byte i = 0; i < 9; i++)
        {
					CountNumbersColumn(i);
        }
				if (ToSet.Count > 0) { continue; }

				for (byte i = 0; i < 9; i++)
				{
					CountNumbersRow(i);
				}
				if (ToSet.Count > 0) { continue; }
			}
		}

    #region [ Column ]

    private void ClearNumbersColumn(byte column, byte result)
		{
			Log.Write($"Start removing {result} in column {column}");

			for (byte row = 0; row < 9; row++)
			{
				ClearNumbersCell(column, row, result);
			}
		}

		public void CountNumbersColumn(byte column)
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
				RememberToSet(column, item.Value.index, item.Key, "column");
			}

			//Cleanup count
			count.Clear();
			count = null;
		}

    #endregion

    #region [ Row ]

    private void ClearNumbersRow(byte row, byte result)
		{
			Log.Write($"Start removing {result} in row {row}");

			for (byte column = 0; column < 9; column++)
			{
				ClearNumbersCell(column, row, result);
			}
		}

		public void CountNumbersRow(byte row)
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
				RememberToSet(item.Value.index, row, item.Key, "row");
			}

			//Cleanup count
			count.Clear();
			count = null;
		}

    #endregion

    #region [ Area ]

    private void ClearNumbersArea(byte _column, byte _row, byte result)
		{
			byte beginColumn = (byte)(((byte)(_column / 3)) * 3);
			byte beginRow = (byte)(((byte)(_row / 3)) * 3);

      Log.Write($"Start removing {result} in area ({_column},{_row}) starting ({beginColumn},{beginRow})");

			for (byte row = beginRow; row < beginRow + 3; row++)
			{
				for (byte column = beginColumn; column < beginColumn + 3; column++)
				{
					ClearNumbersCell((byte)column, (byte)row, result);
				}
			}
		}

		public void CountNumbersArea(byte _column, byte _row)
		{
			Dictionary<byte, (byte counting, byte index)> count =
				new Dictionary<byte, (byte counting, byte index)>();
			byte beginColumn = (byte)(((byte)(_column / 3)) * 3);
			byte beginRow = (byte)(((byte)(_row / 3)) * 3);

			Log.Write($"Start counting in area ({_column},{_row}) starting ({beginColumn},{beginRow})");

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
				Log.Write($"Found {item.Key} with one possibly in ({item.Value.index % 9}, " +
					$"{item.Value.index / 9}) => {item.Value.index}");

				RememberToSet((byte)(item.Value.index % 9), (byte)(item.Value.index / 9), item.Key, "area");
			}

			//Cleanup count
			count.Clear();
			count = null;
		}

		#endregion

		private void ClearNumbersCell(byte column, byte row, byte result)
		{
      if (Sudoku[(byte)(column + row * 9)].Numbers.Count == 0) { return; }

			Log.Write($"Remove {result} from ({column},{row}) => {(column + row * 9)}");

			Sudoku[(byte)(column + row * 9)].RemoveNumber(result);
			if (Sudoku[(byte)(column + row * 9)].Numbers.Count == 1)
			{
				RememberToSet(column, row, GetNumbersCellResult(column, row), "?");
			}
		}

		private void RememberToSet(byte column, byte row, byte result, string from)
    {
			Log.Write($"RememberToSet ({column},{row}) with {result} from {from}");

			if (!ToSet.ContainsKey((byte)(column + row * 9)))
			{
				ToSet.Add((byte)(column + row * 9), result);
			}
			else
			{
				if (ToSet[(byte)(column + row * 9)] != result)
				{
					MessageBox.Show($"Setting {column + row * 9} is wrong");
				}
			}
		}

		public void ActOnToSet()
    {
			Dictionary<byte, byte> toSet = new Dictionary<byte, byte>(ToSet);
      ToSet.Clear();

      foreach (var item in toSet)
      {
				FillCell((byte)(item.Key % 9), (byte)(item.Key / 9), item.Value, Cell.CellTypes.Calulated);
      }
    }

		private byte GetNumbersCellResult(byte column, byte row)
		{
			return Sudoku[(byte)(column + row * 9)].Numbers[0].Digit;
		}

		
	}
}
