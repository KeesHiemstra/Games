using Snake.ViewModels;
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
using System.Windows.Threading;

namespace Snake
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public DispatcherTimer gameTickTimer = new DispatcherTimer();
    public MainViewModel MainVM;
    public enum SnakeDirection { Left, Right, Up, Down };

    public MainWindow()
    {
      InitializeComponent();

      MainVM = new MainViewModel(this);
      gameTickTimer.Tick += GameTickTimer_Tick;
    }

    private void Window_ContentRendered(object sender, EventArgs e)
    {
      MainVM.DrawGameArea();
    }

    public void GameTickTimer_Tick(object sender, EventArgs e)
    {
      MainVM.snakePart.MoveSnake();
    }

    public void Window_KeyUp(object sender, KeyEventArgs e)
    {

      var originalSnakeDirection = MainVM.snakePart.snakeDirection;

      switch (e.Key)
      {
        case Key.Up:
          if (MainVM.snakePart.snakeDirection != SnakeDirection.Down)
            MainVM.snakePart.snakeDirection = SnakeDirection.Up;
          break;
        case Key.Down:
          if (MainVM.snakePart.snakeDirection != SnakeDirection.Up)
            MainVM.snakePart.snakeDirection = SnakeDirection.Down;
          break;
        case Key.Left:
          if (MainVM.snakePart.snakeDirection != SnakeDirection.Right)
            MainVM.snakePart.snakeDirection = SnakeDirection.Left;
          break;
        case Key.Right:
          if (MainVM.snakePart.snakeDirection != SnakeDirection.Left)
            MainVM.snakePart.snakeDirection = SnakeDirection.Right;
          break;
        case Key.Space:
          MainVM.snakePart.StartNewGame();
          break;
      }
      if (MainVM.snakePart.snakeDirection != originalSnakeDirection)
        MainVM.snakePart.MoveSnake();
    }

  }
}
