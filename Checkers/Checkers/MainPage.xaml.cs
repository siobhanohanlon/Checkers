using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Checkers
{
    public partial class MainPage : ContentPage
    {
        //Declare Constants
        const int NUM_ROWS = 10, NUM_COLS = 10; //Game SetUP
        const int BOARD_ROWS = 8, BOARD_COLS = 8; //Game Board

        public MainPage()
        {
            InitializeComponent();
            SetUpBoard();
        }

        //Set up Game Board
        private void SetUpBoard()
        {
            //Declare Variables
            int i, r, c;

            //Set up 10 Columns and 10 Rows to Grid in Xaml
            for(i = 0; i < NUM_COLS; i++)
            {
                GrdGameLayout.ColumnDefinitions.Add(new ColumnDefinition());
                GrdGameLayout.RowDefinitions.Add(new RowDefinition());
            }//Can Put together as Board a Square

            //Put Squares on the Board- Boxviews
            //Board starts at Col 1, Row 1 for 8 in each direction
            BoxView sq;
            
            //Create Black Game Squares
            for(r = 1; r < BOARD_ROWS + 1; r++)
            {
                for (c = 1; c < BOARD_COLS + 1; c++)
                {
                    sq = new BoxView();
                    //Make all black
                    sq.BackgroundColor = Color.Black;
                    //Decide which are white
                    if ((r + c) % 2 == 0) sq.BackgroundColor = Color.White;
                    
                    sq.SetValue(Grid.RowProperty, r);
                    sq.SetValue(Grid.ColumnProperty, c);
                    sq.StyleId = "BoardSquare";
                    GrdGameLayout.Children.Add(sq);
                }
            }

            //Put a single boxview on the board - One piece in the Game
            BoxView b = new BoxView();
            b.BackgroundColor = Color.Red;
            b.HorizontalOptions = LayoutOptions.Center;
            b.VerticalOptions = LayoutOptions.Center;
            b.HeightRequest = 40;
            b.WidthRequest = 40;
            b.CornerRadius = 20;
            b.SetValue(Grid.RowProperty, 2);
            b.SetValue(Grid.ColumnProperty, 2);

            //Add Boxview to collection Children on the grid
            GrdGameLayout.Children.Add(b);
        }

    }
}
