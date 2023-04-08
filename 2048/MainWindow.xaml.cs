using System;
using System.Collections.Generic;
using System.Data.Common;
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

namespace _2048
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
    

    public partial class MainWindow : Window
    {
        gamegrid theGameGrid = new gamegrid();
        Dictionary<int, Brush> valueToColor = new Dictionary<int, Brush>()
        {
            {2, Brushes.Blue},
            {4, Brushes.Red},
            {8, Brushes.Green},
            {16, Brushes.Orange},
            {32, Brushes.Purple},
            {64, Brushes.DarkBlue},
            {128, Brushes.LightGreen},
            {256, Brushes.Yellow},
            {512, Brushes.Magenta},
            {1024, Brushes.Brown},
            {2048, Brushes.Cyan},
            {4096, Brushes.Gray}
        };

        public MainWindow()
        {
            
            theGameGrid.Clear();
            InitializeComponent();
            

           

        }

        



        private void updateView()
        {
            // Assuming that the text boxes are stored in a 2D array named textBoxes
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    gamebox gameBox = theGameGrid[row, col];
                    TextBox textBox = (TextBox)grid.Children
                    .Cast<TextBox>()
                    .Single(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == col);
                     

                    if (gameBox == null)
                    {
                        textBox.Text = "";
                        textBox.Background = Brushes.White;
                    }
                    else
                    {
                        textBox.Text = gameBox.Value.ToString();
                        textBox.Background = valueToColor[gameBox.Value];
                    }
                }
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                theGameGrid.PushBoxes(Direction.Left);  
            }
            if (e.Key == Key.Right)
            {
                theGameGrid.PushBoxes(Direction.Right);
            }
            if (e.Key == Key.Up) 
            {
                theGameGrid.PushBoxes(Direction.Up);
            }
            if (e.Key == Key.Down){
                theGameGrid.PushBoxes(Direction.Down);
            }


            theGameGrid.generateNewBox();
            updateView();
            
            
        }


    }
}
