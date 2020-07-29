using Letters.Models;
using Letters.ViewModels;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Letters
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    MainViewModel MainVM;

    public MainWindow()
    {
      InitializeComponent();

      MainVM = new MainViewModel(this);

      CreateLettersMasker(MainVM.LetterCount);
    }

    private void CreateLettersMasker(int letterCount)
    {
      for (int i = 0; i < letterCount; i++)
      {
        CreateMasker(LettersPanel, i);
        CreateMasker(WordPanel);
      }
    }

    /// <summary>
    /// Draw the game masker.
    /// </summary>
    /// <param name="panel"></param>
    private void CreateMasker(Panel panel, int? position = null)
    {
      TextBlock textBlock = new TextBlock()
      {
        HorizontalAlignment = HorizontalAlignment.Center,
        VerticalAlignment = VerticalAlignment.Center,
        FontSize = 30,
      };

      Border border = new Border()
      {
        Background = Brushes.Transparent,
        BorderBrush = Brushes.Gray,
        BorderThickness = new Thickness(0.5),
        CornerRadius = new CornerRadius(9),
        Height = 50,
        Width = 50,
        Margin = new Thickness(0, 0, 20, 0),
        Child = textBlock,
      };

      if (position.HasValue)
      {
        border.Name = $"BorderLetter_{position}";
      }

      panel.Children.Add(border);
    }

    /// <summary>
    /// Select a random vowel.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void VowelButton_Click(object sender, RoutedEventArgs e)
    {
      ((TextBlock)((Border)LettersPanel.Children[8 - MainVM.LetterCount]).Child).Text =
        MainVM.SelectLetter(GameViewModel.LetterType.Vowel);
    }

    /// <summary>
    /// Select a random consonant.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ConsonantButton_Click(object sender, RoutedEventArgs e)
    {
      ((TextBlock)((Border)LettersPanel.Children[8 - MainVM.LetterCount]).Child).Text =
        MainVM.SelectLetter(GameViewModel.LetterType.Consonant);
    }

    /// <summary>
    /// Act on pressed letter.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_KeyUp(object sender, KeyEventArgs e)
    {
      MainVM.DetectKey(e);        
    }
  }
}
