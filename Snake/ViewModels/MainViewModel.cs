using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Snake.ViewModels
{
  public class MainViewModel
  {

    public readonly int SnakeSquareSize = 20;
    public readonly int SnakeStartLength = 3;
    public readonly int SnakeStartSpeed = 400;
    public readonly int SnakeSpeedThreshold = 100;

    readonly MainWindow mw;
    public SnakePart snakePart;

    public MainViewModel(MainWindow mainWindow)
    {
      mw = mainWindow;
      snakePart = new SnakePart(mw, this);
    }

    internal void DrawGameArea()
    {

      bool doneDrawingBackground = false;
      int nextX = 0;
      int nextY = 0;
      int rowCounter = 0;
      bool nextIsOdd = false;

      while (doneDrawingBackground == false)
      {
        Rectangle rect = new Rectangle
        {
          Width = SnakeSquareSize,
          Height = SnakeSquareSize,
          Fill = nextIsOdd ? Brushes.White : Brushes.LightGray
        };
        mw.GameArea.Children.Add(rect);
        Canvas.SetTop(rect, nextY);
        Canvas.SetLeft(rect, nextX);

        nextIsOdd = !nextIsOdd;
        nextX += SnakeSquareSize;
        if (nextX >= mw.GameArea.ActualWidth)
        {
          nextX = 0;
          nextY += SnakeSquareSize;
          rowCounter++;
          nextIsOdd = (rowCounter % 2 != 0);
        }

        if (nextY >= mw.GameArea.ActualHeight)
          doneDrawingBackground = true;
      }

    }


  }
}
