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

        BoxView currPieceSelected;

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

            //tap Gesture Recongizer
            TapGestureRecognizer t_sq = new TapGestureRecognizer();
            t_sq.NumberOfTapsRequired = 1;
            t_sq.Tapped += Square_Tapped;
            
            //Create Black Game Squares
            for(r = 1; r < BOARD_ROWS + 1; r++)
            {
                for (c = 1; c < BOARD_COLS + 1; c++)
                {
                    sq = new BoxView();
                    //Make all bWhite
                    sq.BackgroundColor = Color.White;

                    //Decide which are Black
                    if ((r + c) % 2 != 0)
                    {
                        sq.BackgroundColor = Color.Black;
                        //Tap Gesture
                        sq.GestureRecognizers.Add(t_sq);
                    }
                    
                    //Set Grid Values
                    sq.SetValue(Grid.RowProperty, r);
                    sq.SetValue(Grid.ColumnProperty, c);
                    sq.StyleId = "BoardSquare";
                    GrdGameLayout.Children.Add(sq);
                }
            }

            //Tapped Gesture
            TapGestureRecognizer t = new TapGestureRecognizer();
            t.NumberOfTapsRequired = 1;
            t.Tapped += Piece_Tapped; //Creating Event Handler

            //Put a single boxview on the board - One piece in the Game
            BoxView b = new BoxView();
            b.BackgroundColor = Color.Red;
            b.StyleId = "Red";
            b.HorizontalOptions = LayoutOptions.Center;
            b.VerticalOptions = LayoutOptions.Center;
            b.HeightRequest = 40;
            b.WidthRequest = 40;
            b.CornerRadius = 20;
            b.SetValue(Grid.RowProperty, 8);
            b.SetValue(Grid.ColumnProperty, 3);
            b.GestureRecognizers.Add(t);

            //Add Boxview to collection Children on the grid
            GrdGameLayout.Children.Add(b);

            //Just to Clear
            currPieceSelected = null;
        }

        private void Square_Tapped(object sender, EventArgs e)
        {
            //is there a current piece selected
            if (currPieceSelected == null)
                return;

            //Move current piece
            BoxView currB = (BoxView)sender;
            currPieceSelected.SetValue(Grid.RowProperty, currB.GetValue(Grid.RowProperty));
            currPieceSelected.SetValue(Grid.ColumnProperty, currB.GetValue(Grid.ColumnProperty));
            currPieceSelected.BackgroundColor = Color.Red;
            currPieceSelected.StyleId = "Red";

            //Then set current selected to null
            currPieceSelected = null;
        }

        //Tapped Event
        private void Piece_Tapped(object sender, EventArgs e)
        {
            //Tap was added to boxview
            BoxView currB = (BoxView)sender;

            //Save current piece selected
            currPieceSelected = currB;

            //Decide what to do when tapped
            if (currB.StyleId == "Red")
            {
                currB.BackgroundColor = Color.Blue;
                currB.StyleId = "Blue";
            }

            else if (currB.StyleId == "Blue")
            {
                currB.BackgroundColor = Color.Red;
                currB.StyleId = "Red";
            }
        }
    }
}
