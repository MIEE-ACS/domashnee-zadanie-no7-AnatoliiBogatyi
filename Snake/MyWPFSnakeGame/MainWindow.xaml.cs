using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using SnakeClassLibrary;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Panel = System.Windows.Controls.Panel;
using System.Configuration;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Collections.Generic;

namespace MyWPFSnakeGame//Do you wanna have a bad time?
{
    public partial class MainWindow : Window
    {
        private bool timerFlag = false;
        private SnakeGame game;
        private Timer stepTimer;
        private int Score;
        private UIElementCollection cells;

        private static string fileName = ConfigurationSettings.AppSettings.AllKeys[0];

        private Stream f;
        
        private Records myRecords = new Records();

        int rows = 23;
        int columns = 51;

        class Images
        {
            public enum ImNames { Apple, Mandarin, Body, Bush, Grass, Head, HeadHelmet }
            static public readonly List<ImageSource> images;

            static Images()//Суть в том, что я загружаю картинки в оперативку, уверен, можно сделать как-то иначе и лучше, но я пришел к данной реализации
            {
                images = new List<ImageSource>();
                images.Add(Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.Apple.GetHbitmap(),
                                  IntPtr.Zero,
                                  Int32Rect.Empty,
                                  BitmapSizeOptions.FromEmptyOptions()));
                images.Add(Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.Mandarin.GetHbitmap(),
                                  IntPtr.Zero,
                                  Int32Rect.Empty,
                                  BitmapSizeOptions.FromEmptyOptions()));
                images.Add(Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.Body.GetHbitmap(),
                                  IntPtr.Zero,
                                  Int32Rect.Empty,
                                  BitmapSizeOptions.FromEmptyOptions()));
                images.Add(Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.Bush.GetHbitmap(),
                                  IntPtr.Zero,                                                               
                                  Int32Rect.Empty,
                                  BitmapSizeOptions.FromEmptyOptions()));
                images.Add(Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.Grass.GetHbitmap(),
                                  IntPtr.Zero,
                                  Int32Rect.Empty,
                                  BitmapSizeOptions.FromEmptyOptions()));
                images.Add(Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.Head.GetHbitmap(),
                                      IntPtr.Zero,                                                                     
                                      Int32Rect.Empty,
                                      BitmapSizeOptions.FromEmptyOptions()));
                images.Add(Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.HeadHelmet.GetHbitmap(),
                                      IntPtr.Zero,                                                                      
                                      Int32Rect.Empty,
                                      BitmapSizeOptions.FromEmptyOptions()));
            }
        }



        public MainWindow()
        {
            
            InitializeComponent();
            cells = AreaGrid.Children;
            TextScore.Text = Score.ToString();

            

            AreaGrid.Rows = rows;
            AreaGrid.Columns = columns;
            SetBackground(AreaGrid, "LightGray");

            DrawRegion(rows, columns);

            game = new SnakeGame();
            
            game.Draw += this.Draw;

            
            stepTimer = new System.Windows.Forms.Timer();
            stepTimer.Interval = 100;
            stepTimer.Enabled = false;
            stepTimer.Tick += OnStepTimer;
        }

        private void DrawRegion(int rows, int columns)
        {
            for (int y = 0; y < rows; y++)
            for (int x = 0; x < columns; x++)
                AreaGrid.Children.Add(CreateSectionCanvas(y, x));
        }

        private Canvas CreateSectionCanvas(int y, int x)
        {
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = Images.images[(int)Images.ImNames.Grass];
            Canvas section = new Canvas();
            section.Name = "Section_" + y.ToString() + "_" + x.ToString();
            section.Margin = new Thickness(1);
            section.Background = brush;
            
            return section;
        }

        private void SetBackground(Panel control, string color)
        {
            control.Background = (SolidColorBrush) new BrushConverter().ConvertFromString(color);
        }

        private void OnStepTimer(object sender, EventArgs e)
        {
            game.Step();
            if (game.State == GameState.End)
            {
                stepTimer.Stop();
            }
        }

        private void Draw(SnakeClassLibrary.Point point, Item item)
        {
            
            ImageBrush brush = new ImageBrush();
            switch (item)
            {
                case Item.Prize:
                    if (point.feature)
                    {
                        brush.ImageSource = Images.images[(int)Images.ImNames.Mandarin];
                    }
                    else
                    {
                        brush.ImageSource = Images.images[(int)Images.ImNames.Apple];
                    }
                    TextScore.Text = Score.ToString();
                    Score++;
                    break;
                case Item.SnakeSegment:

                    if (point.feature)
                    {
                        if (game.OneMoreRound == 0)
                        {
                            brush.ImageSource = Images.images[(int)Images.ImNames.Head];
                        }
                        else
                        {
                            brush.ImageSource = Images.images[(int)Images.ImNames.HeadHelmet];
                        }
                    }
                    else
                    {
                        brush.ImageSource = Images.images[(int)Images.ImNames.Body];
                    }
                    break;
                case Item.Zero:
                    brush.ImageSource = Images.images[(int)Images.ImNames.Grass];
                    break;
                default:
                    brush.ImageSource = Images.images[(int)Images.ImNames.Grass];
                    break;
            }

            
            foreach (Canvas c in cells)
            {
                if (c.Name == ("Section_" + point.Y + "_" + point.X))
                {
                    c.Background = brush;
                }
            }
        }

        private void GameWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    game.Instruction(Direction.Up);                    
                    break;
                case Key.Down:
                    game.Instruction(Direction.Down);
                    break;
                case Key.Left:
                    game.Instruction(Direction.Left);
                    break;
                case Key.Right:
                    game.Instruction(Direction.Right);
                    break;
            }
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            Score = 0;
            cells.Clear();//Xd
            cells = AreaGrid.Children;
            AreaGrid.Rows = rows;
            AreaGrid.Columns = columns;
            DrawRegion(rows, columns);


            game.Initialization();
            game.Start();
            stepTimer.Start();
        }


        private void ButtonPausePlay_Click(object sender, RoutedEventArgs e)
        {
            if (timerFlag)
                stepTimer.Start();
            else
                stepTimer.Stop();

            timerFlag = !timerFlag;
        }

    }
}

