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
        //Pieces
        const string CAPTURED_BLACK_SL = "CapturedBlackPiece";
        const string CAPTURED_WHITE_SL = "CapturedWhitePiece";
        const string COUNT_CAP_BLACK_LBL = "CountCapturedBlackPieces";
        const string COUNT_CAP_WHITE_LBL = "CountCapturedWhitePieces";

        //Board Size
        const int NUM_ROWS = 10, NUM_COLS = 10; //Game SetUP
        const int BOARD_ROWS = 8, BOARD_COLS = 8; //Game Board

        //Boxview
        BoxView currPieceSelected;


        //Global Variables for Starting Positions
        int[][] _startBlack = new int[3][] { new int[] {1, 3, 5, 7}, 
                                             new int[] {2, 4, 6, 8}, 
                                             new int[] {1, 3, 5, 7 } };

        int[][] _startWhite = new int[3][] { new int[] {2, 4, 6, 8},
                                             new int[] {1, 3, 5, 7},
                                             new int[] {2, 4, 6, 8} };


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
        }//End SetUpBoard

        //Create Player Pieces
        private void CreatePlayerPieces(Color colour, string styleID, 
                                        int[][] startPosition, int startRow)
        {
            //Declare Variables
            int r, startArrayCol, startArrayRow;

            //Tapped Gesture
            TapGestureRecognizer t = new TapGestureRecognizer();
            t.NumberOfTapsRequired = 1;
            t.Tapped += Piece_Tapped; //Creating Event Handler

            //Put a single boxview on the board - One piece in the Game
            BoxView b;

            #region Loop for Pieces
            //Loop for Pieces
            startArrayRow = 0;
            for (r = startRow; r < startRow + 3; r++)
            {
                //c is the index in array
                for(startArrayCol = 0; startArrayCol < 4; startArrayCol++)
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
                    b.SetValue(Grid.RowProperty, r);
                    b.SetValue(Grid.ColumnProperty, startPosition[startArrayRow][startArrayCol]);

                    b.GestureRecognizers.Add(t);

                    //Add Boxview to collection Children on the grid
                    GrdGameLayout.Children.Add(b);
                }
                startArrayRow++;
            }
            #endregion
        }//End CreatePlayerPiece


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
            }//End For Loop

            #region CapturedPieces
            //Stack Layout for Captured Pieces
            //Declare
            StackLayout sl;
            Label lbl;

            //Black Pieces
            sl = new StackLayout();
            sl.SetValue(Grid.RowProperty, 1);
            sl.SetValue(Grid.ColumnProperty, 0);
            sl.StyleId = CAPTURED_BLACK_SL; 

            //Label
            lbl = new Label();
            lbl.Text = "";
            lbl.TextColor = Color.White;
            lbl.FontAttributes = FontAttributes.Bold;
            lbl.HorizontalOptions = LayoutOptions.Center;
            // "CountCapturedBlackPieces"
            lbl.StyleId = COUNT_CAP_BLACK_LBL;
            
            //Add to Stacklayout and Grid Children
            sl.Children.Add(lbl);
            GrdGameLayout.Children.Add(sl);

            //White Pieces
            sl = new StackLayout();
            sl.SetValue(Grid.RowProperty, 8);
            sl.SetValue(Grid.ColumnProperty, 0);
            sl.StyleId = CAPTURED_WHITE_SL;
            
            //Label
            lbl = new Label();
            lbl.Text = "";
            lbl.TextColor = Color.White;
            lbl.FontAttributes = FontAttributes.Bold;
            lbl.HorizontalOptions = LayoutOptions.Center;
            lbl.StyleId = COUNT_CAP_WHITE_LBL;

            //Add to Stacklayout and Grid Children
            sl.Children.Add(lbl);
            GrdGameLayout.Children.Add(sl);

            #endregion
        }//End CreateSquaresOnBoard

        //Square Tapped
        private void Square_Tapped(object sender, EventArgs e)
        {
            //is there a current piece selected
            if (currPieceSelected == null)
                return;

            //Move current piece
            BoxView currSq = (BoxView)sender;

            //Where Trying to move to
            MoveSelectedPieceTo((int)currSq.GetValue(Grid.RowProperty),
                        (int)currSq.GetValue(Grid.ColumnProperty));
                //Can create variables and use them instead
        }//End Square_Tapped

        //Move Piece To
        private void MoveSelectedPieceTo(int destRow, int destColumn)
        {
            //Can only Move diagonally
            int piece_Row, piece_Col;

            //Multiplier for Up or Down Moving
            int multiplier = -1;//Default moving down

            if (currPieceSelected.StyleId.Contains("Black"))
                multiplier = 1;//Moving up

            //Piece Place
            piece_Row = (int)currPieceSelected.GetValue(Grid.RowProperty);
            piece_Col = (int)currPieceSelected.GetValue(Grid.ColumnProperty);

            #region MoveThePiece
            //Square Occupied
            if (IsSquareOccupied(destRow, destColumn) == true) return;

            //If JumpMove
            if (IsThisAJump(destRow, destColumn, multiplier) == true) return;

            //Only for Upwards
            //If Trying to move more than 1 Diagonally away- Return
            if (destRow + (1 * multiplier) != piece_Row) return;
            if ((destColumn - 1 != piece_Col) && (destColumn + 1 != piece_Col)) return;

            //Get and Set Grid properties
            currPieceSelected.SetValue(Grid.RowProperty, destRow);
            currPieceSelected.SetValue(Grid.ColumnProperty, destColumn);

            //King
            if (destRow == 1 || destRow == 8)
                MakeMeAKing();

            //Reset Piece
            ResetCurrentSelectedPiece();
            #endregion

            //Then set current selected to null
            currPieceSelected = null;
        }//End MovePieceTo

        //Is a Jump
        private bool IsThisAJump(int destR, int destC,int direction)
        {
            //Declare Variables
            bool IsJump = false;
            int originRow, originColumn;
            int diffRow = 0, diffColumn = 0;

            //Set Origin
            originRow = (int)currPieceSelected.GetValue(Grid.RowProperty);
            originColumn = (int)currPieceSelected.GetValue(Grid.ColumnProperty);

            //Set Difference in Rows & Columns
            diffRow = destR - originRow;
            diffColumn = destC - originColumn;

            //Bug Fixing for Jumping
            if (destR + (2 * direction) == originRow)
            {
                if ((Math.Abs(diffRow) == 2) && (Math.Abs(diffColumn) == 2))
                {  
                    //Check for a piece in the middle.
                    if (IsSquareOccupied((diffRow / 2) + originRow, (diffColumn / 2) + originColumn) == true)
                    {   
                        BoxView removePiece = GetThisPiece((diffRow / 2) + originRow, (diffColumn / 2) + originColumn);

                        //Check If Same Team
                        if (removePiece.StyleId != currPieceSelected.StyleId)
                        {
                            //Move Jumping Piece
                            currPieceSelected.SetValue(Grid.RowProperty, destR);
                            currPieceSelected.SetValue(Grid.ColumnProperty, destC);

                            //Reset
                            ResetCurrentSelectedPiece();

                            //Remove Piece
                            RemoveCapturedPieceFromBoard(removePiece);

                            //Set Return Variable
                            IsJump = true;
                        }
                    } //End if(IsSquareOccupied((diffRow / 2)
                } //End if((Math.Abs(diffRow) == 2
            } //End if((diffRow < 0 && direction < 0)

            //Return
            return IsJump;
        }//End IsAJump

        //Remove Captured Piece
        private void RemoveCapturedPieceFromBoard(BoxView piece)
        {
            //Declare
            StackLayout slCaptured = null;
            Label lblCaptured = null;

            //Black Piece is default
            string whichSL = CAPTURED_BLACK_SL;
            string whichLBL = COUNT_CAP_BLACK_LBL;

            //White Piece
            if(piece.StyleId.Contains("White"))
            {
                whichSL = CAPTURED_WHITE_SL;
                whichLBL = COUNT_CAP_WHITE_LBL;
            }

            //Check Board for Piece
            foreach (var c in GrdGameLayout.Children)
            {
                if (c.StyleId == whichSL)
                {
                    //Found Stacklayout
                    slCaptured = (StackLayout)c;

                    //Find the label in the children of the stack layout
                    foreach (var l in slCaptured.Children)
                    {
                        if (l.StyleId == whichLBL)
                        {
                            lblCaptured = (Label)l;
                            break;
                        }
                    }
                }
            }//End Foreach

            //Set Row and Column to Null
            piece.SetValue(Grid.RowProperty, null);
            piece.SetValue(Grid.ColumnProperty, null);
            GrdGameLayout.Children.Remove(piece);

            //Add to StackLayout
            slCaptured.Children.Add(piece);

            //Update Label with Count Pieces
            lblCaptured.Text = (slCaptured.Children.Count -1).ToString();

            //Make Invisible
            if (slCaptured.Children.Count > 2)
                piece.IsVisible = false;

            //Remove Tap
            piece.GestureRecognizers.Clear();
        }//End RemoveCapturedPiece

        //Get This Piece
        private BoxView GetThisPiece(int row, int column)
        {
            //Boxview
            BoxView b = null;

            //Check Grid Children for "Piece"
            foreach(var piece in GrdGameLayout.Children)
            {
                if(piece.StyleId.Contains("Piece"))
                {
                    if(row == (int)piece.GetValue(Grid.RowProperty) &&
                       column == (int)piece.GetValue(Grid.ColumnProperty))
                    {
                        //Piece Found
                        b = (BoxView)piece;
                        break;
                    }
                }
            }//End Foreach

            //Return
            return b;
        }//End GetThisPiece

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
                        //Set return
                        isOccupied = true;
                        break;
                    }
                }
            }

            //Return
            return isOccupied;
        }//End IsSquareOccupied

        //Reset Current Piece
        private void ResetCurrentSelectedPiece()
        {
            //If no Piece Selected
            if (currPieceSelected == null) return;

            //If Piece Selected
            currPieceSelected.Opacity = 1;
        }//End ResetCurrrentPiece

        //Tapped Event
        private void Piece_Tapped(object sender, EventArgs e)
        {
            //Tap was added to boxview
            BoxView currB = (BoxView)sender;

            //Select Piece
            SelectThisPiece(currB);
        }//End Piece_Tapped

        //Select This Piece
        private void SelectThisPiece(BoxView thisOne)
        {
            //Select Piece
            if (currPieceSelected == null)
            {
                //This Piece is Selected
                currPieceSelected = thisOne;

                //Change Opacity
                currPieceSelected.Opacity = 0.7;
            }

            //Deselect
            else
            {
                //Deselecting Piece
                currPieceSelected = null;

                //Reset Piece Selected
                ResetCurrentSelectedPiece();
            }
        }//End SelectThisPiece

        //Make King
        private void MakeMeAKing()
        {
            //Declare Variables
            double r = currPieceSelected.BackgroundColor.R;
            double g = currPieceSelected.BackgroundColor.G;
            double b = currPieceSelected.BackgroundColor.B;

            //Set BackgroundColour
            currPieceSelected.BackgroundColor = new Color(r * 0.75,
                                                          g * 0.75,
                                                          b * 0.75);

            //Set StyleId to King
            currPieceSelected.StyleId = "King" + currPieceSelected.StyleId;
        }
    }//End Main
}//End Namespace
