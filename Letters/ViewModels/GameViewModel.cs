using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Letters.Models
{
  public class GameViewModel
  {
    private string RandomWord;
    private List<string> FoundWords = new List<string>();

    public enum LetterType  { Vowel, Consonant };
    public List<string> WordList { get; set; } = new List<string>();    
    public string Vowel { get; set; } = "aaaaaeeeeeeeeeeiiioooouuy";
    public string Consonant { get; set; } = "bbccddddffgggghhjkkkllllmmnnnnnppprrrrsssttttvvwz";

    public GameViewModel()
    {
      string[] wordList = Letters.Properties.Resources.WordList
        .Replace("\r\n", "\n")
        .Split('\n');
      foreach (string word in wordList)
      {
        WordList.Add(word);
      }
    }

    internal bool CheckWord(string selectedWord)
    {
      return WordList.Where(x => x == selectedWord).Count() == 1;
    }

    public string ProposeWord(string randomWord)
    {
      string result = string.Empty;
      RandomWord = randomWord.ToLower();
      FoundWords.Clear();
      SearchWords();
      List<string> results = FoundWords.OrderByDescending(x => x.Length).ToList();
      if (results.Count > 0)
      {
        result = results[0];
      }
      return result;
    }

    /// <summary>
    /// Search for words with the matrix letters of 2 letters.
    /// </summary>
    private void SearchWords()
    {
      for (int x = 0; x < RandomWord.Length; x++)
      {
        for (int y = 0; y < RandomWord.Length; y++)
        {
          if (x == y) { continue; }
          AddFoundWord(RandomWord[x].ToString() + RandomWord[y].ToString());
        }
      }
    }

    /// <summary>
    /// Does the found word exists the right letters.
    /// </summary>
    /// <param name="search"></param>
    private void AddFoundWord(string search)
    {
      foreach (string found in WordList.Where(x => x.Contains(search)))
      {
        if (!FoundWords.Contains(found))
        {
          if (DoesLetterExist(found) && MatchWord(found))
          {
            FoundWords.Add(found);
          }
        }
      }
    }

    /// <summary>
    /// Contains the word the letters?
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    private bool DoesLetterExist(string word)
    {
      bool add = true;
      foreach (char letter in word)
      {
        add = add && RandomWord.Contains(letter);
        if (!add) { break; }
      }
      return add;
    }

    /// <summary>
    /// Match the word the letters?
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    private bool MatchWord(string word)
    {
      bool add = true;
      string random = RandomWord;

      foreach (char letter in word)
      {
        int pos = random.IndexOf(letter);
        if (pos > 0)
        {
          random = random.Remove(pos, 1);
        }
        else
        {
          add = false;
          break;
        }
      }
      return add;
    }
  }
}
