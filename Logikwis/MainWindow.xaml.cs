using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Logikwis
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			BuildField();
		}

		private void BuildField()
		{
			//			<TextBlock Name="X1"
			//Text = "Naam"
			//Grid.Row = "0"
			//Grid.Column = "2"
			//Grid.ColumnSpan = "5" />

			string[] FieldName = new string[4];
			FieldName[0] = "Dag";
			FieldName[1] = "Naam";
			FieldName[2] = "Gelegenheid";
			FieldName[3] = "Fout";

			TextBlock[] FieldText = new TextBlock[6];

			FieldText[1].Text = FieldName[1];

		}
	}
}
