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
  public class SnakePart
  {

    MainWindow mw;
    MainViewModel vm;
    private SolidColorBrush snakeBodyBrush = Brushes.Green;
    private SolidColorBrush snakeHeadBrush = Brushes.YellowGreen;
    private List<SnakePart> snakeParts = new List<SnakePart>();
    public MainWindow.SnakeDirection snakeDirection = MainWindow.SnakeDirection.Right;
    private int snakeLength;
    private int currentScore = 0;
    private Random rnd = new Random();
    private UIElement snakeFood = null;
    private SolidColorBrush foodBrush = Brushes.Red;

    public UIElement UiElement { get; set; }

    public Point Position { get; set; }

    public bool IsHead { get; set; }

    public SnakePart(MainWindow mainWindow, MainViewModel mainViewModel)
    {
      mw = mainWindow;
      vm = mainViewModel;
    }

    private void DrawSnake()
    {
      foreach (SnakePart snakePart in snakeParts)
      {
        if (snakePart.UiElement == null)
        {
          snakePart.UiElement = new Rectangle()
          {
            Width = vm.SnakeSquareSize,
            Height = vm.SnakeSquareSize,
            Fill = (snakePart.IsHead ? snakeHeadBrush : snakeBodyBrush)
          };
          mw.GameArea.Children.Add(snakePart.UiElement);
          Canvas.SetTop(snakePart.UiElement, snakePart.Position.Y);
          Canvas.SetLeft(snakePart.UiElement, snakePart.Position.X);
        }
      }
    }

    public void MoveSnake()
    {
      // Remove the last part of the snake, in preparation of the new part added below  
      while (snakeParts.Count >= snakeLength)
      {
        mw.GameArea.Children.Remove(snakeParts[0].UiElement);
        snakeParts.RemoveAt(0);
      }

      // Next up, we'll add a new element to the snake, which will be the (new) head  
      // Therefore, we mark all existing parts as non-head (body) elements and then  
      // we make sure that they use the body brush  
      foreach (SnakePart snakePart in snakeParts)
      {
        (snakePart.UiElement as Rectangle).Fill = snakeBodyBrush;
        snakePart.IsHead = false;
      }

      // Determine in which direction to expand the snake, based on the current direction  
      SnakePart snakeHead = snakeParts[snakeParts.Count - 1];
      double nextX = snakeHead.Position.X;
      double nextY = snakeHead.Position.Y;
      switch (snakeDirection)
      {
        case MainWindow.SnakeDirection.Left:
          nextX -= vm.SnakeSquareSize;
          break;
        case MainWindow.SnakeDirection.Right:
          nextX += vm.SnakeSquareSize;
          break;
        case MainWindow.SnakeDirection.Up:
          nextY -= vm.SnakeSquareSize;
          break;
        case MainWindow.SnakeDirection.Down:
          nextY += vm.SnakeSquareSize;
          break;
      }

      // Now add the new head part to our list of snake parts...  
      snakeParts.Add(new SnakePart(mw, vm)
      {
        Position = new Point(nextX, nextY),
        IsHead = true
      });
      //... and then have it drawn!  
      DrawSnake();

      DoCollisionCheck();          
    }

    public void StartNewGame()
    {
      // Remove potential dead snake parts and leftover food...
      foreach (SnakePart snakeBodyPart in snakeParts)
      {
        if (snakeBodyPart.UiElement != null)
          mw.GameArea.Children.Remove(snakeBodyPart.UiElement);
      }
      snakeParts.Clear();
      if (snakeFood != null)
        mw.GameArea.Children.Remove(snakeFood);

      // Reset stuff
      currentScore = 0;
      snakeLength = vm.SnakeStartLength;
      snakeDirection = MainWindow.SnakeDirection.Right;
      snakeParts.Add(new SnakePart(mw, vm) { Position = new Point(vm.SnakeSquareSize * 5, vm.SnakeSquareSize * 5) });
      mw.gameTickTimer.Interval = TimeSpan.FromMilliseconds(vm.SnakeStartSpeed);

      // Draw the snake again and some new food...
      DrawSnake();
      DrawSnakeFood();

      // Update status
      UpdateGameStatus();

      // Go!        
      mw.gameTickTimer.IsEnabled = true;
    }

    private Point GetNextFoodPosition()
    {
      int maxX = (int)(mw.GameArea.ActualWidth / vm.SnakeSquareSize);
      int maxY = (int)(mw.GameArea.ActualHeight / vm.SnakeSquareSize);
      int foodX = rnd.Next(0, maxX) * vm.SnakeSquareSize;
      int foodY = rnd.Next(0, maxY) * vm.SnakeSquareSize;

      foreach (SnakePart snakePart in snakeParts)
      {
        if ((snakePart.Position.X == foodX) && (snakePart.Position.Y == foodY))
          return GetNextFoodPosition();
      }

      return new Point(foodX, foodY);
    }

    private void DrawSnakeFood()
    {
      Point foodPosition = GetNextFoodPosition();
      snakeFood = new Ellipse()
      {
        Width = vm.SnakeSquareSize,
        Height = vm.SnakeSquareSize,
        Fill = foodBrush
      };
      mw.GameArea.Children.Add(snakeFood);
      Canvas.SetTop(snakeFood, foodPosition.Y);
      Canvas.SetLeft(snakeFood, foodPosition.X);
    }

    private void DoCollisionCheck()
    {
      SnakePart snakeHead = snakeParts[snakeParts.Count - 1];

      if ((snakeHead.Position.X == Canvas.GetLeft(snakeFood)) && (snakeHead.Position.Y == Canvas.GetTop(snakeFood)))
      {
        EatSnakeFood();
        return;
      }

      if ((snakeHead.Position.Y < 0) || (snakeHead.Position.Y >= mw.GameArea.ActualHeight) ||
      (snakeHead.Position.X < 0) || (snakeHead.Position.X >= mw.GameArea.ActualWidth))
      {
        EndGame();
      }

      foreach (SnakePart snakeBodyPart in snakeParts.Take(snakeParts.Count - 1))
      {
        if ((snakeHead.Position.X == snakeBodyPart.Position.X) && (snakeHead.Position.Y == snakeBodyPart.Position.Y))
          EndGame();
      }
    }

    private void EatSnakeFood()
    {
      snakeLength++;
      currentScore++;
      int timerInterval = Math.Max(vm.SnakeSpeedThreshold, 
        (int)mw.gameTickTimer.Interval.TotalMilliseconds - (currentScore * 2));
      mw.gameTickTimer.Interval = TimeSpan.FromMilliseconds(timerInterval);
      mw.GameArea.Children.Remove(snakeFood);
      DrawSnakeFood();
      UpdateGameStatus();
    }

    private void UpdateGameStatus()
    {
      mw.Title = "SnakeWPF - Score: " + currentScore + " - Game speed: " + 
        mw.gameTickTimer.Interval.TotalMilliseconds;
    }

    private void EndGame()
    {
      mw.gameTickTimer.IsEnabled = false;
      MessageBox.Show("Oooops, you died!\n\nTo start a new game, just press the Space bar...", "SnakeWPF");
    }



  }
}
