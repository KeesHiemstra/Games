using System;
using System.IO;
using System.Reflection;

namespace Sudoku1.ViewModels
{
  public static class Log
  {
    static readonly string LogFile = $".\\{Assembly.GetExecutingAssembly().GetName().Name}.log";

    public static void Write(string message)
    {
      string Message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {message}";
      using (StreamWriter stream = new StreamWriter(LogFile, true))
      {
        stream.WriteLine(Message);
      }
    }
  }
}
