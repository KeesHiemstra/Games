using Letters.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Letters.ViewModels
{
  class MainViewModel
  {
    private MainWindow MainV;
    private string Vowel;
    private string Consonant;
    private List<TextBlock> RandomLetters = new List<TextBlock>();
    private List<int> SelectedLetters = new List<int>();
    private string RandomWord = string.Empty;
    private string RemainLetters = string.Empty;
    private string SelectedWord = string.Empty;

    public int GamePlay = 1;
    public int GamePlayer = 0;
    public int[] GameScore = new int[2];
    //Don't re-random too often
    public Random Rand = new Random();

    public GameViewModel Game = new GameViewModel();
    public byte LetterCount = 8;

    public MainViewModel(MainWindow mainV)
    {
      MainV = mainV;
      Vowel = Game.Vowel;
      Consonant = Game.Consonant;
    }

    public string SelectLetter(GameViewModel.LetterType letterType)
    {
      if (LetterCount == 0)
      {
        return string.Empty;
      }

      string letters;
      string selectedLetter;

      switch (letterType)
      {
        case GameViewModel.LetterType.Vowel:
          letters = Vowel;
          break;
        default:
          letters = Consonant;
          break;
      }

      int randomLetter = Rand.Next(letters.Length);

      selectedLetter = letters[randomLetter].ToString().ToUpper();
      letters = letters.Remove(randomLetter, 1);

      switch (letterType)
      {
        case GameViewModel.LetterType.Vowel:
          Vowel = letters;
          break;
        default:
          Consonant = letters;
          break;
      }

      RandomLetters.Add((TextBlock)((Border)MainV.LettersPanel.Children[8 - LetterCount]).Child);
      RemainLetters += selectedLetter;

      LetterCount--;
      if (LetterCount == 0)
      {
        RandomWord = RemainLetters;
        ShowWordPanel();
      }

      //Hide the result of selected word
      MainV.ResultStackPanel.Visibility = Visibility.Hidden;

      return selectedLetter;
    }

    private void ShowWordPanel()
    {
      for (int i = 0; i < RandomLetters.Count; i++)
      {
        ((Border)MainV.LettersPanel.Children[i]).MouseLeftButtonUp += border_MouseLeftButtonUp;
      }

      MainV.GameButtonsPanel.Visibility = Visibility.Hidden;
      MainV.WordPanel.Visibility = Visibility.Visible;
    }

    /// <summary>
    /// Mouse handling.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    internal void border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      int position = int.Parse(((Border)sender).Name.Replace("BorderLetter_", ""));
      MakeWord(position);
    }

    /// <summary>
    /// Keyboard handling.
    /// </summary>
    /// <param name="e"></param>
    internal void DetectKey(KeyEventArgs e)
    {
      if (LetterCount > 0)
      {
        return;
      }

      if (e.Key == Key.OemQuestion)
      {
        MessageBox.Show(Game.ProposeWord(RandomWord),
          "Easter egg - Don't press <Enter>");
        return;
      }

      if (e.Key == Key.Back && SelectedWord.Length > 0)
      {
        UndoLetter();
        return;
      }

      if (e.Key == Key.Enter)
      {
        FinishWord();
        return;
      }

      if (!(RemainLetters.Contains(e.Key.ToString())))
      {
        return;
      }

      MakeWord(e.Key.ToString());
    }

    /// <summary>
    /// From keyword.
    /// </summary>
    /// <param name="letter"></param>
    internal void MakeWord(string letter)
    {
      int pos = RemainLetters.IndexOf(letter);
      MakeWord(pos);
    }

    internal void MakeWord(int position)
    {
      ((Border)MainV.LettersPanel.Children[position]).MouseLeftButtonUp -= border_MouseLeftButtonUp;

      string letter = RandomWord.Substring(position, 1);
      SelectedLetters.Add(position);
      SelectedWord = SelectedWord.Insert(SelectedWord.Length, letter);

      RandomLetters[position].Foreground = Brushes.LightGray;
      
      RemainLetters = RemainLetters.Remove(position, 1);
      RemainLetters = RemainLetters.Insert(position, " ");

      ((TextBlock)((Border)(MainV.WordPanel.Children[SelectedWord.Length - 1])).Child).Text =
        letter;
    }

    internal void UndoLetter()
    {
      int pos = SelectedWord.Length - 1;
      char undoLetter = SelectedWord[pos];

      ((TextBlock)((Border)(MainV.WordPanel.Children[pos])).Child).Text = "";
      RandomLetters[SelectedLetters[pos]].Foreground = Brushes.Black;

      RemainLetters = RemainLetters.Remove(SelectedLetters[pos], 1);
      RemainLetters = RemainLetters.Insert(SelectedLetters[pos], undoLetter.ToString());

      ((Border)MainV.LettersPanel.Children[SelectedLetters[pos]]).MouseLeftButtonUp += 
        border_MouseLeftButtonUp;
     SelectedLetters.Remove(SelectedLetters[pos]);

      SelectedWord = SelectedWord.Substring(0, pos);
    }

    internal void FinishWord()
    {
      if (Game.CheckWord(SelectedWord.ToLower()))
      {
        MainV.ResultTextBlock.Text = "correct";
        MainV.ResultTextBlock.Foreground = Brushes.DarkGreen;
        GameScore[GamePlayer] += SelectedWord.Length;
        switch (GamePlayer)
        {
          case 1:
            MainV.GameScoreRightTextBlock.Text = GameScore[GamePlayer].ToString();
            break;
          default:
            MainV.GameScoreLeftTextBlock.Text = GameScore[GamePlayer].ToString();
            break;
        }
      }
      else
      {
        MainV.ResultTextBlock.Text = "incorrect";
        MainV.ResultTextBlock.Foreground = Brushes.Red;
      }

      string proposeWord = Game.ProposeWord(RandomWord);
      if (proposeWord.Length > SelectedWord.Length)
      {
        MainV.ProposeWordTextBlock.Text = proposeWord;
        MainV.ProposeStackPanel.Visibility = Visibility.Visible;
      }
      else
      {
        MainV.ProposeStackPanel.Visibility = Visibility.Hidden;
      }

      MainV.ResultStackPanel.Visibility = Visibility.Visible;

      NewGame();
    }

    private void NewGame()
    {
      foreach (var letter in RandomLetters)
      {
        letter.Text = "";
        letter.Foreground = Brushes.Black;
      }

      for (int i = 0; i < RandomLetters.Count; i++)
      {
        ((Border)MainV.LettersPanel.Children[i]).MouseLeftButtonUp -= 
          border_MouseLeftButtonUp;
      }
      RemainLetters = "";
      RandomLetters.Clear();
      SelectedLetters.Clear();
      SelectedWord = "";
      LetterCount = 8;

      for (int i = 0; i < LetterCount; i++)
      {
        ((TextBlock)((Border)(MainV.WordPanel.Children[i])).Child).Text = "";
      }

      if (Vowel.Length < 7)
      {
        Vowel = Game.Vowel;
        Consonant = Game.Consonant;
      }

      MainV.GameButtonsPanel.Visibility = Visibility.Visible;
      MainV.WordPanel.Visibility = Visibility.Hidden;

      GamePlayer++;
      if (GamePlayer == GameScore.Length)
      {
        GamePlayer = 0;
        GamePlay++;
      }

      switch (GamePlayer)
      {
        case 1:
          MainV.LeftBorder.Background = Brushes.Transparent;
          MainV.RightBorder.Background = Brushes.LightSalmon;
          break;
        default:
          MainV.LeftBorder.Background = Brushes.LightSalmon;
          MainV.RightBorder.Background = Brushes.Transparent;
          break;
      }

      MainV.GamePlayTextBlock.Text = GamePlay.ToString();
    }
  }
}
