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

        //Global Variables for Starting Positions
        int[][] _startBlack = new int[3][] { new int[] {1, 3, 5, 7}, 
                                             new int[] {2, 4, 6, 8}, 
                                             new int[] {1, 3, 5, 7 } };

        int[][] _startWhite = new int[3][] { new int[] {2, 4, 6, 8},
                                             new int[] {1, 3, 5, 7},
                                             new int[] {2, 4, 6, 8} };

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
            int i;

            //Set up 10 Columns and 10 Rows to Grid in Xaml
            for (i = 0; i < NUM_COLS; i++)
            {
                GrdGameLayout.ColumnDefinitions.Add(new ColumnDefinition());
                GrdGameLayout.RowDefinitions.Add(new RowDefinition());
            }//Can Put together as Board a Square

            //Create Squares and Pieces on Board
            CreateSquaresOnBoard();
            //Black Pieces
            CreatePlayerPieces(Color.Red,
                               "BlackPiece",
                               _startBlack,
                               6);
            //White Pieces
            CreatePlayerPieces(Color.White,
                               "WhitePiece",
                               _startWhite,
                               1);

            //Just to Clear
            currPieceSelected = null;
        }

        //Create Player Pieces
        private void CreatePlayerPieces(Color colour, string styleID, 
                                        int[][] startPosition, int startRow)
        {
            //Declare Variables
            int r, c;

            //Tapped Gesture
            TapGestureRecognizer t = new TapGestureRecognizer();
            t.NumberOfTapsRequired = 1;
            t.Tapped += Piece_Tapped; //Creating Event Handler

            //Put a single boxview on the board - One piece in the Game
            BoxView b;

            #region Loop for Pieces
            //Loop for Pieces
            for (r = 0; r < 3; r++)
            {
                //c is the index in array
                for(c = 0; c < 4; c++)
                {
                    //Create Pieces
                    b = new BoxView();
                    b.BackgroundColor = colour;
                    b.StyleId = styleID;
                    b.HorizontalOptions = LayoutOptions.Center;
                    b.VerticalOptions = LayoutOptions.Center;
                    b.HeightRequest = 40;
                    b.WidthRequest = 40;
                    b.CornerRadius = 20;

                    //Piece Grid Properties
                    b.SetValue(Grid.RowProperty, r + startRow);
                    b.SetValue(Grid.ColumnProperty, startPosition[r][c]);

                    b.GestureRecognizers.Add(t);

                    //Add Boxview to collection Children on the grid
                    GrdGameLayout.Children.Add(b);
                }
                #endregion
            }
        }//End Create Piece


        //Create Squares on Board
        private void CreateSquaresOnBoard()
        {
            //Declare Variables
            int r, c;

            //Put Squares on the Board- Boxviews
            //Board starts at Col 1, Row 1 for 8 in each direction
            BoxView sq;

            //tap Gesture Recongizer
            TapGestureRecognizer t_sq = new TapGestureRecognizer();
            t_sq.NumberOfTapsRequired = 1;
            t_sq.Tapped += Square_Tapped;

            //Create Black Game Squares
            for (r = 1; r < BOARD_ROWS + 1; r++)
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
        }

        //Move to Square
        private void Square_Tapped(object sender, EventArgs e)
        {
            //is there a current piece selected
            if (currPieceSelected == null)
                return;

            //Move current piece
            BoxView currSq = (BoxView)sender;

            //Can only Move diagonally
            int sq_r, sq_c, piece_Row, piece_Col;
            int multiplier = -1;//Default moving down

            if (currPieceSelected.StyleId == "BlackPiece")
                multiplier = 1;//Moving up

            #region MoveThePiece
            //Where Trying to move to
            sq_r = (int)currSq.GetValue(Grid.RowProperty);
            sq_c = (int)currSq.GetValue(Grid.ColumnProperty);

            //Piece Place
            piece_Row = (int)currPieceSelected.GetValue(Grid.RowProperty);
            piece_Col = (int)currPieceSelected.GetValue(Grid.ColumnProperty);

            //Square Occupied
            if (IsSquareOccupied(sq_r, sq_c) == true) return;

            //Only for Upwards
            //If Trying to move more than 1 Diagonally away- Return
            if (sq_r + (1 * multiplier) != piece_Row) return;
            if ((sq_c - 1 != piece_Col) && (sq_c + 1 != piece_Col)) return;

            //Get and Set Grid properties
            currPieceSelected.SetValue(Grid.RowProperty, currSq.GetValue(Grid.RowProperty));
            currPieceSelected.SetValue(Grid.ColumnProperty, currSq.GetValue(Grid.ColumnProperty));

            //Reset Piece back
            //White
            if (currPieceSelected.StyleId.Contains("White"))
            {
                currPieceSelected.BackgroundColor = Color.White;
            }

            //Black
            if (currPieceSelected.StyleId.Contains("Black"))
            {
                currPieceSelected.BackgroundColor = Color.Red;
            }
            #endregion

            //Then set current selected to null
            currPieceSelected = null;
        }

        //Square Occupied
        private bool IsSquareOccupied(int sq_r, int sq_c)
        {
            //Check all Pieces on grid and check if trying to move there
            //Not Occupied
            bool isOccupied = false;

            //Is Occupied
            foreach(var piece in GrdGameLayout.Children)
            {
                //Check if a piece not a square- as squares are also children
                if(piece.StyleId.Contains("Piece"))
                {
                    //If a piece is on the square
                    if(sq_r == (int)piece.GetValue(Grid.RowProperty) && sq_c == (int)piece.GetValue(Grid.ColumnProperty))
                    {
                        isOccupied = true;
                        break;
                    }
                }
            }

            return isOccupied;
        }

        //Tapped Event
        private void Piece_Tapped(object sender, EventArgs e)
        {
            //Tap was added to boxview
            BoxView currB = (BoxView)sender;

            //Select Piece
            if(currPieceSelected == null)
            {
                currPieceSelected = currB;
                currB.BackgroundColor = Color.Blue;
            }

            //Deselect
            else
            {
                //Deselecting Piece
                currPieceSelected = null;

                //Changing back Colour
                //White
                if(currB.StyleId.Contains("White"))
                {
                    currB.BackgroundColor = Color.White;
                }

                //Black
                if (currB.StyleId.Contains("Black"))
                {
                    currB.BackgroundColor = Color.Red;
                }
            }
        }
    }
}
