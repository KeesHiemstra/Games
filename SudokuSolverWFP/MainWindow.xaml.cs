using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace SudokuSolverWFP
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public SodukuSolverOptions options;

		public MainWindow()
		{
			options = new SodukuSolverOptions();
			InitializeComponent();
			AddGridField();
			TextBoxFileName.Text = options.FileName;
		}

		private void AddGridField()
		{
			TextBlock[] textBoxes = new TextBlock[2];
			textBoxes[0] = new TextBlock { Text = "0" };
			textBoxes[1] = new TextBlock { Text = "1" };

			GridField.Children.Add(textBoxes[0]);
			GridField.Children.Add(textBoxes[1]);
		}

		private void ButtonOpen_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				FileName = options.FileName
			};

			Nullable<bool> Result = openFileDialog.ShowDialog();

			if (Result ?? false)
			{
				options.FileName = openFileDialog.FileName;
				ImportJsonFile(options.FileName);
			}
		}

		private bool ImportJsonFile(string fileName)
		{
			try
			{
				using (FileStream Json = File.Open(fileName, FileMode.Open))
				{
					DataContractJsonSerializer Import = new DataContractJsonSerializer(typeof(List<DataGridCell>));

				}
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// The color of the TextBox will be black if the file exists, others it will be gray.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TextBoxFileName_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (File.Exists(TextBoxFileName.Text))
			{
				TextBoxFileName.Foreground = Brushes.Black;
			}
			else
			{
				TextBoxFileName.Foreground = Brushes.Gray;
			}
		}

		private void Window_Initialized(object sender, EventArgs e)
		{
			options.FileName = @"C:\Temp\Soducko page 44.2 2018-02-18 1808.json";
			//options.FileName = @"<no file loaded>";
		}
	}
}
