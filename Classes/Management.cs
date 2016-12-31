using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B16_Ex05
{
    public delegate void BoardTextChanged(Piece i_Piece, int i_Rows, int i_Cols);

    public delegate void DeleteTextFromBoard(int i_Rows, int i_Cols);

    public delegate void EnableButtonWhenColumnIsFull(int i_Column);

    public class Management
    {
        private static readonly bool sr_GameOn = true;

        private readonly Player r_PlayerOne = null;

        private readonly Player r_PlayerTwo = null;

        private Board m_MyBoard;

        private Random m_Random = new Random();

        private Player m_CurrentPlayer = null;

        private bool isPlayerOneTurn = true;

        public event BoardTextChanged CellBoardTextChanged = null;

        public event DeleteTextFromBoard DeleteText = null;

        public event EnableButtonWhenColumnIsFull EnableButton = null;

        private string winningType = string.Empty;
         
        public Board MyBoard
        {
            ////properties
            get
            {
                return m_MyBoard;
            }

            set
            {
                m_MyBoard = value;
            }
        }
        ////constructor
        public Management(Board i_InputBoard)
        {
            m_MyBoard = i_InputBoard;
            r_PlayerOne = i_InputBoard.PlayerOne;
            r_PlayerTwo = i_InputBoard.PlayerTwo;
        }

        /*
        check if the cell is occupied.
        also check's if it's able to put the piece in the cell according to:
        if it is in the last row or if the cell beneath it is occupied.
        */
        private bool IsCellOccupied(Board i_CurrentBoard, int i_Row, int i_Column)
        {
            if (i_Column != -1 && i_CurrentBoard.MyBoard[i_Row, i_Column].PlayerPiece == Piece.ePlayerPiece.NoPiece)
            {
                if (i_Row == i_CurrentBoard.Rows - 1)
                {
                    return false;
                }

                if (i_CurrentBoard.MyBoard[i_Row + 1, i_Column].PlayerPiece != Piece.ePlayerPiece.NoPiece)
                {
                    return false;
                }
            }

            return true;
        }

        /*
        "insert" a piece to the board in the column chosen.
        return's false in case the column is full. 
        */
        public bool InsertToBoard(Board i_CurrentBoard, int i_SelectedColumn, Player i_CurrentPlayer, out int i_CellRowWhereDroped)
        {
            for (int i = i_CurrentBoard.Rows - 1; i >= 0; i--)
            {
                if (i_CurrentBoard.MyBoard[i, i_SelectedColumn].PlayerPiece == Piece.ePlayerPiece.NoPiece)
                {
                    i_CurrentBoard.MyBoard[i, i_SelectedColumn].PlayerPiece = i_CurrentPlayer.Piece;

                    if (CellBoardTextChanged != null)
                    {
                        CellBoardTextChanged.Invoke(i_CurrentBoard.MyBoard[i, i_SelectedColumn], i, i_SelectedColumn);
                    }

                    i_CellRowWhereDroped = i;
                    return true;
                }
            }          
  
            i_CellRowWhereDroped = -1;
            return false;
        }

        /*
        check if the board is full.
        */
        public bool IsAllBoardFull(Board i_CurrentBoard)
        {
            for (int i = 0; i < i_CurrentBoard.Rows; i++)
            {
                for (int j = 0; j < i_CurrentBoard.Colmuns; j++)
                {
                    if (i_CurrentBoard.MyBoard[i, j].PlayerPiece == Piece.ePlayerPiece.NoPiece)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /*
        check if there is a winning in the current round.
        also return's the type of the flush (vertically, horizontally or diagonally).
        */
        public bool Decision(Board i_CurrentBoard, Player i_CurrentPlayer, int i_RowSquareDroped, int i_ColumnSquareDroped, out string i_WinningType)
        {
            if (i_RowSquareDroped != -1)
            {
                if (this.VerticallyFlush(i_CurrentBoard, i_CurrentPlayer, i_RowSquareDroped, i_ColumnSquareDroped, out i_WinningType)
                    ||
                    this.HorizontallyFlush(i_CurrentBoard, i_CurrentPlayer, i_RowSquareDroped, i_ColumnSquareDroped, out i_WinningType)
                    ||
                    this.DiagonallyFlush(i_CurrentBoard, i_CurrentPlayer, i_RowSquareDroped, i_ColumnSquareDroped, out i_WinningType))
                {
                    i_CurrentPlayer.PlayerScore++;
                    return true;
                }
            }

            i_WinningType = string.Empty;
            return false;
        }

        /*
        check if there is a vertically flush in the current round.
        */
        private bool VerticallyFlush(Board i_CurrentBoard, Player i_CurrentPlayer, int i_RowSquareDroped, int i_ColumnSquareDroped, out string i_WinningType)
        {
            if ((i_CurrentBoard.Rows - i_RowSquareDroped) >= 4)
            {
                int squaresInFlush = 1;
                for (int i = i_RowSquareDroped + 1; (i < i_CurrentBoard.Rows) && (squaresInFlush <= 4); i++)
                {
                    if (i_CurrentBoard.MyBoard[i, i_ColumnSquareDroped].PlayerPiece == i_CurrentPlayer.Piece)
                    {
                        squaresInFlush++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (squaresInFlush >= 4)
                {
                    i_WinningType = "by Vertically Flush";
                    return true;
                }
            }

            i_WinningType = string.Empty;
            return false;
        }

        /*
        check if there is a Horizontally flush in the current round.
        */
        private bool HorizontallyFlush(Board i_CurrentBoard, Player i_CurrentPlayer, int i_RowSquareDroped, int i_ColumnSquareDroped, out string i_WinningType)
        {
            int squaresInFlush = 1;
            for (int i = i_ColumnSquareDroped + 1; (i < i_CurrentBoard.Colmuns) && (squaresInFlush <= 4); i++)
            {
                if (i_CurrentBoard.MyBoard[i_RowSquareDroped, i].PlayerPiece != i_CurrentPlayer.Piece)
                {
                    break;
                }

                squaresInFlush++;
            }

            for (int i = i_ColumnSquareDroped - 1; i >= 0 && squaresInFlush <= 4; i--)
            {
                if (i_CurrentBoard.MyBoard[i_RowSquareDroped, i].PlayerPiece != i_CurrentPlayer.Piece)
                {
                    break;
                }

                squaresInFlush++;
            }

            if (squaresInFlush >= 4)
            {
                i_WinningType = "by Horizontally Flush";
                return true;
            }
            else
            {
                i_WinningType = string.Empty;
                return false;
            }
        }

        /*
        check if there is a Diagonally flush in the current round.
        it check's for right & left diagonally flushs.
        */
        private bool DiagonallyFlush(Board i_CurrentBoard, Player i_CurrentPlayer, int i_RowSquareDroped, int i_ColumnSquareDroped, out string i_WinningType)
        {
            if (this.RightDiagonallyFlush(i_CurrentBoard, i_CurrentPlayer, i_RowSquareDroped, i_ColumnSquareDroped, out i_WinningType) == true
                ||
                this.LeftDiagonallyFlush(i_CurrentBoard, i_CurrentPlayer, i_RowSquareDroped, i_ColumnSquareDroped, out i_WinningType) == true)
            {
                return true;
            }

            return false;
        }

        /*
        check if there is a right Diagonally flush in the current round.
        */
        private bool RightDiagonallyFlush(Board i_CurrentBoard, Player i_CurrentPlayer, int i_RowSquareDroped, int i_ColumnSquareDroped, out string i_WinningType)
        {
            byte flushCounter = 1;
            int row = i_RowSquareDroped + 1;
            int column = i_ColumnSquareDroped + 1;
            if (row != i_CurrentBoard.Rows)
            {
                while (row < i_CurrentBoard.Rows && column < i_CurrentBoard.Colmuns && flushCounter <= 4)
                {
                    if (i_CurrentBoard.MyBoard[row, column].PlayerPiece != i_CurrentPlayer.Piece)
                    {
                        break;
                    }

                    flushCounter++;
                    row++;
                    column++;
                }
            }

            row = i_RowSquareDroped - 1;
            column = i_ColumnSquareDroped - 1;
            if (i_ColumnSquareDroped != 0)
            {
                while (row >= 0 && column >= 0 && flushCounter <= 4)
                {
                    if (i_CurrentBoard.MyBoard[row, column].PlayerPiece != i_CurrentPlayer.Piece)
                    {
                        break;
                    }

                    flushCounter++;
                    row--;
                    column--;
                }
            }

            if (flushCounter >= 4)
            {
                i_WinningType = "by Diagonally Flush";
                return true;
            }
            else
            {
                i_WinningType = string.Empty;
                return false;
            }
        }

        /*
        check if there is a left Diagonally flush in the current round.
        */
        private bool LeftDiagonallyFlush(Board i_CurrentBoard, Player i_CurrentPlayer, int i_RowSquareDroped, int i_ColumnSquareDroped, out string i_WinningType)
        {
            byte flushCounter = 1;
            int row = i_RowSquareDroped - 1;
            int column = i_ColumnSquareDroped + 1;
            ////if in the right border
            if (column != i_CurrentBoard.Colmuns)
            {
                while (column < i_CurrentBoard.Colmuns && row >= 0 && flushCounter <= 4)
                {
                    if (i_CurrentBoard.MyBoard[row, column].PlayerPiece != i_CurrentPlayer.Piece)
                    {
                        break;
                    }

                    flushCounter++;
                    row--;
                    column++;
                }
            }

            row = i_RowSquareDroped + 1;
            column = i_ColumnSquareDroped - 1;
            ////if in the bottom
            if (row != i_CurrentBoard.Rows)
            {
                while (row < i_CurrentBoard.Rows && column >= 0 && flushCounter <= 4)
                {
                    if (i_CurrentBoard.MyBoard[row, column].PlayerPiece != i_CurrentPlayer.Piece)
                    {
                        break;
                    }

                    flushCounter++;
                    row++;
                    column--;
                }
            }

            if (flushCounter >= 4)
            {
                i_WinningType = "by Diagonally Flush";
                return true;
            }
            else
            {
                i_WinningType = string.Empty;
                return false;
            }
        }

        /*
        check if the currnt column is full.
        */
        public bool IsColumnFull(Board i_CurrentBoard, Player i_CurrentPlayer, int i_Column)
        {
            for (int i = i_CurrentBoard.Rows - 1; i >= 0; i--)
            {
                if (i_CurrentBoard.MyBoard[i, i_Column].PlayerPiece == Piece.ePlayerPiece.NoPiece)
                {
                    return false;
                }
            }

            return true;
        }
        ////AI for the computer
        /*
        check if there is a 3-pieces-flush in the current round in order to block it.
        the 3-pieces-flush checked can be vertically, horizontally or diagonally.
        */
        public bool AiCheckForFlushBlocking(Board i_CurrentBoard, out int i_ColumnToDrop)
        {
            if (this.AiThreeVerticallyFlush(i_CurrentBoard, out i_ColumnToDrop) == true)
            {
                return true;
            }
            else if (this.AiThreeHorizontallyFlush(i_CurrentBoard, out i_ColumnToDrop) == true)
            {
                return true;
            }
            else if (this.AiThreeDiagonallyFlush(i_CurrentBoard, out i_ColumnToDrop) == true)
            {
                return true;
            }

            return false;
        }

        /*
        check if there is a 3-pieces vertically flush in the current round.
        */
        private bool AiThreeVerticallyFlush(Board i_CurrentBoard, out int i_ColumnToDrop)
        {
            byte flushCounter = 0;
            int j = 0;
            int i = i_CurrentBoard.Rows - 1;
            Piece currentPiece = new Piece(Piece.ePlayerPiece.NoPiece);
            for (j = 0; j < i_CurrentBoard.Colmuns; j++)
            {
                i = i_CurrentBoard.Rows - 1;
                flushCounter = 1;
                currentPiece.PlayerPiece = i_CurrentBoard.MyBoard[i, j].PlayerPiece;
                for (i -= 1; i >= 0 && currentPiece.PlayerPiece != Piece.ePlayerPiece.NoPiece; i--)
                {
                    if (currentPiece.PlayerPiece == i_CurrentBoard.MyBoard[i, j].PlayerPiece)
                    {
                        flushCounter++;
                    }
                    else
                    {
                        currentPiece.PlayerPiece = i_CurrentBoard.MyBoard[i, j].PlayerPiece;
                        flushCounter = 1;
                    }

                    if (flushCounter == 3 && i - 1 >= 0)
                    {
                        if (IsCellOccupied(i_CurrentBoard, i - 1, j) == false)
                        {
                            i_ColumnToDrop = j;
                            return true;
                        }

                        flushCounter = 1;
                    }
                }
            }

            i_ColumnToDrop = -1;
            return false;
        }

        /*
        check if there is a 3-pieces Horizontally flush in the current round.
        */
        private bool AiThreeHorizontallyFlush(Board i_CurrentBoard, out int i_ColumnToDrop)
        {
            byte flushCounter = 1;
            int j = 0;
            int savedColumnToDrop = 0;
            int i = i_CurrentBoard.Rows - 1;
            Piece currentPiece = new Piece(Piece.ePlayerPiece.NoPiece); 
            bool isFlushWithBlankSquare = false;
            byte oneBlank = 1;
            for (; i >= 0; i--, j = 0)
            {
                do
                {
                    currentPiece.PlayerPiece = i_CurrentBoard.MyBoard[i, j++].PlayerPiece;
                }
                while (currentPiece.PlayerPiece == Piece.ePlayerPiece.NoPiece && j < i_CurrentBoard.Colmuns);
                for (; j < i_CurrentBoard.Colmuns && currentPiece.PlayerPiece != Piece.ePlayerPiece.NoPiece; j++)
                {
                    if (currentPiece.PlayerPiece == i_CurrentBoard.MyBoard[i, j].PlayerPiece)
                    {
                        flushCounter++;
                    }
                    else if (i_CurrentBoard.MyBoard[i, j].PlayerPiece != Piece.ePlayerPiece.NoPiece)
                    {
                        currentPiece.PlayerPiece = i_CurrentBoard.MyBoard[i, j].PlayerPiece;
                        flushCounter = 1;
                        oneBlank = 1;
                        isFlushWithBlankSquare = false;
                    }
                    else if (oneBlank == 1)
                    {
                        isFlushWithBlankSquare = true;
                        savedColumnToDrop = j;
                        oneBlank = 0;
                        continue;
                    }
                    else
                    {
                        currentPiece.PlayerPiece = i_CurrentBoard.MyBoard[i, j].PlayerPiece;
                        oneBlank = 1;
                        flushCounter = 1;
                        isFlushWithBlankSquare = false;
                    }

                    if (flushCounter == 3 && isFlushWithBlankSquare == false)
                    {
                        if (j < i_CurrentBoard.Colmuns - 1)
                        {
                            if (IsCellOccupied(i_CurrentBoard, i, j + 1) == false)
                            {
                                i_ColumnToDrop = j + 1;
                                return true;
                            }
                            else if (IsCellOccupied(i_CurrentBoard, i, j - flushCounter) == false)
                            {
                                i_ColumnToDrop = j - flushCounter;
                                return true;
                            }
                        }
                        else if ((j - flushCounter) >= 0 && IsCellOccupied(i_CurrentBoard, i, j - flushCounter) == false)
                        {
                            i_ColumnToDrop = j - flushCounter;
                            return true;
                        }

                        flushCounter = 1;
                    }

                    if (isFlushWithBlankSquare == true && flushCounter == 3)
                    {
                        i_ColumnToDrop = savedColumnToDrop;
                        return true;
                    }
                }

                flushCounter = 1;
                oneBlank = 1;
            }

            i_ColumnToDrop = -1;
            return false;
        }

        /*
        check if there is a 3-pieces Diagonally flush in the current round.
        it check's for right & left diagonally 3-pieces-flushs.        
        */
        private bool AiThreeDiagonallyFlush(Board i_CurrentBoard, out int i_ColumnToDrop)
        {
            if (AiThreeRightDiagonallyFlush(i_CurrentBoard, out i_ColumnToDrop) == true)
            {
                return true;
            }
            else if (AiThreeLeftDiagonallyFlush(i_CurrentBoard, out i_ColumnToDrop) == true)
            {
                return true;
            }

            return false;
        }

        /*
        check if there is a 3-pieces right Diagonally flush in the current round.
        */
        private bool AiThreeRightDiagonallyFlush(Board i_CurrentBoard, out int i_ColumnToDrop)
        {
            byte flushCounter = 1;
            int j = 0;
            int i = i_CurrentBoard.Rows - 1;
            Piece currentPiece = new Piece(Piece.ePlayerPiece.NoPiece); 
            byte indexForFirstColumnCheck = 1;
            bool isFlushWithBlankSquare = false;
            int savedColumnToDrop = 0;
            byte oneBlank = 1;
            for (byte k = 0; k <= i_CurrentBoard.Colmuns - 4; k++)
            {
                i = i_CurrentBoard.Rows - 1;
                currentPiece.PlayerPiece = i_CurrentBoard.MyBoard[i, k].PlayerPiece;
                oneBlank = 1;
                isFlushWithBlankSquare = false;
            CheckForDiagonallInFirstColumn: // check for three Right Diagonally Flush in all of the first column
                for (j = k; j < i_CurrentBoard.Colmuns - 1 && i > 0; j++, i--)
                {
                    if (currentPiece.PlayerPiece == Piece.ePlayerPiece.NoPiece && oneBlank == 1)
                    {
                        if (i_CurrentBoard.MyBoard[i - 1, j + 1].PlayerPiece == Piece.ePlayerPiece.NoPiece)
                        {
                            oneBlank = 1;
                            isFlushWithBlankSquare = false;
                            flushCounter = 1;
                            break;
                        }

                        currentPiece.PlayerPiece = i_CurrentBoard.MyBoard[i - 1, j + 1].PlayerPiece;
                        savedColumnToDrop = j;
                        isFlushWithBlankSquare = true;
                        oneBlank = 0;
                        continue;
                    }
                    else if (currentPiece.PlayerPiece == i_CurrentBoard.MyBoard[i - 1, j + 1].PlayerPiece)
                    {
                        flushCounter++;
                    }
                    else if (i_CurrentBoard.MyBoard[i - 1, j + 1].PlayerPiece == Piece.ePlayerPiece.NoPiece && oneBlank == 1)
                    {
                        savedColumnToDrop = j + 1;
                        isFlushWithBlankSquare = true;
                        oneBlank = 0;
                        continue;
                    }
                    else
                    {
                        currentPiece.PlayerPiece = i_CurrentBoard.MyBoard[i - 1, j + 1].PlayerPiece;
                        flushCounter = 1;
                        isFlushWithBlankSquare = false;
                        oneBlank = 1;
                    }

                    if (isFlushWithBlankSquare == true && flushCounter >= 3)
                    {
                        if (IsCellOccupied(i_CurrentBoard, i + 1, savedColumnToDrop) == false)
                        {
                            i_ColumnToDrop = savedColumnToDrop;
                            return true;
                        }
                    }
                    else if (i - 2 >= 0 && j < i_CurrentBoard.Colmuns - 2 && isFlushWithBlankSquare == false && flushCounter == 3)
                    {
                        if (IsCellOccupied(i_CurrentBoard, i - 2, j + 2) == false)
                        {
                            i_ColumnToDrop = j + 2;
                            return true;
                        }
                    }
                }

                if (k == 0)
                {
                    if ((i_CurrentBoard.Rows - 1 - indexForFirstColumnCheck) >= 3 && i_CurrentBoard.MyBoard[i_CurrentBoard.Rows - 1 - indexForFirstColumnCheck, k].PlayerPiece != Piece.ePlayerPiece.NoPiece)
                    {
                        i = i_CurrentBoard.Rows - 1 - indexForFirstColumnCheck;
                        currentPiece.PlayerPiece = i_CurrentBoard.MyBoard[i_CurrentBoard.Rows - 1 - indexForFirstColumnCheck, k].PlayerPiece;
                        indexForFirstColumnCheck++;
                        isFlushWithBlankSquare = false;
                        goto CheckForDiagonallInFirstColumn;
                    }
                }
            }

            i_ColumnToDrop = -1;
            return false;
        }

        /*
        check if there is a 3-pieces left Diagonally flush in the current round.
        */
        private bool AiThreeLeftDiagonallyFlush(Board i_CurrentBoard, out int i_ColumnToDrop)
        {
            byte flushCounter = 1;
            int j = 3;
            int i = i_CurrentBoard.Rows - 1;
            Piece currentPiece = new Piece(Piece.ePlayerPiece.NoPiece); 
            byte indexForLastColumnCheck = 1;
            bool isFlushWithBlankSquare = false;
            int savedColumnToDrop = 0;
            byte oneBlank = 1;
            for (byte k = 3; k < i_CurrentBoard.Colmuns; k++)
            {
                i = i_CurrentBoard.Rows - 1;
                currentPiece.PlayerPiece = i_CurrentBoard.MyBoard[i, k].PlayerPiece;
                oneBlank = 1;
                flushCounter = 1;
                isFlushWithBlankSquare = false;
            CheckForDiagonallInLastColumn: // check for three left Diagonally Flush in all of the last column
                for (j = k; j > 0 && i > 0; j--, i--)
                {
                    if (currentPiece.PlayerPiece == Piece.ePlayerPiece.NoPiece && oneBlank == 1)
                    {
                        if (i_CurrentBoard.MyBoard[i - 1, j - 1].PlayerPiece == Piece.ePlayerPiece.NoPiece)
                        {
                            oneBlank = 1;
                            isFlushWithBlankSquare = false;
                            flushCounter = 1;
                            break;
                        }

                        currentPiece.PlayerPiece = i_CurrentBoard.MyBoard[i - 1, j - 1].PlayerPiece;
                        savedColumnToDrop = j;
                        isFlushWithBlankSquare = true;
                        oneBlank = 0;
                        continue;
                    }
                    else if (currentPiece.PlayerPiece == i_CurrentBoard.MyBoard[i - 1, j - 1].PlayerPiece)
                    {
                        flushCounter++;
                    }
                    else if (i_CurrentBoard.MyBoard[i - 1, j - 1].PlayerPiece == Piece.ePlayerPiece.NoPiece && oneBlank == 1)
                    {
                        savedColumnToDrop = j - 1;
                        isFlushWithBlankSquare = true;
                        oneBlank = 0;
                        continue;
                    }
                    else
                    {
                        currentPiece.PlayerPiece = i_CurrentBoard.MyBoard[i - 1, j - 1].PlayerPiece;
                        flushCounter = 1;
                        isFlushWithBlankSquare = false;
                        oneBlank = 1;
                    }

                    if (isFlushWithBlankSquare == true && flushCounter >= 3)
                    {
                        if (IsCellOccupied(i_CurrentBoard, i, savedColumnToDrop) == false)
                        {
                            i_ColumnToDrop = savedColumnToDrop;
                            return true;
                        }
                    }
                    else if (i - 2 >= 0 && j - 2 >= 0 && isFlushWithBlankSquare == false && flushCounter == 3)
                    {
                        if (IsCellOccupied(i_CurrentBoard, i - 2, j - 2) == false)
                        {
                            i_ColumnToDrop = j - 2;
                            return true;
                        }
                        else if (i + 2 < i_CurrentBoard.Rows && j + 2 < i_CurrentBoard.Colmuns && IsCellOccupied(i_CurrentBoard, i + 2, j + 2) == false)
                        {
                            i_ColumnToDrop = j + 2;
                            return true;
                        }
                    }
                }

                if (k == i_CurrentBoard.Colmuns - 1)
                {
                    if ((i_CurrentBoard.Rows - 1 - indexForLastColumnCheck) >= 3 && i_CurrentBoard.MyBoard[i_CurrentBoard.Rows - 1 - indexForLastColumnCheck, k].PlayerPiece != Piece.ePlayerPiece.NoPiece)
                    {
                        i = i_CurrentBoard.Rows - 1 - indexForLastColumnCheck;
                        currentPiece.PlayerPiece = i_CurrentBoard.MyBoard[i_CurrentBoard.Rows - 1 - indexForLastColumnCheck, k].PlayerPiece;
                        indexForLastColumnCheck++;
                        isFlushWithBlankSquare = false;
                        oneBlank = 1;
                        flushCounter = 1;
                        goto CheckForDiagonallInLastColumn;
                    }
                }
            }

            i_ColumnToDrop = -1;
            return false;
        }

        /*
        insert's a piece to a random cell after check if it is not occupied.
        */
        public int RandomInsert(Board i_CurrentBoard, Player i_CurrentPlayer, out int i_RowDroped)
        {
            int randomColumnToDrop = 0;
            if (this.IsCellOccupied(i_CurrentBoard, i_CurrentBoard.Rows - 1, (i_CurrentBoard.Colmuns - 1) / 2) == false)
            {
                randomColumnToDrop = (i_CurrentBoard.Colmuns - 1) / 2;
            }
            else
            {
                do
                {
                    randomColumnToDrop = m_Random.Next(0, i_CurrentBoard.Colmuns);
                }
                while (this.IsColumnFull(i_CurrentBoard, i_CurrentPlayer, randomColumnToDrop));
            }

            this.InsertToBoard(i_CurrentBoard, randomColumnToDrop, i_CurrentPlayer, out i_RowDroped);
            return randomColumnToDrop;
        }

        /*
        check if there is a 3-pieces-flush in the current round in order to win.
        the 3-pieces-flush checked can be vertically, horizontally or diagonally.
        it uses a feigned insert: 
            if there is a flush- the piece actually insert's the board.
            if there isn't a flush- the piece delete's from the board.
        */
        public bool AiCheckForFlushWinning(Board i_CurrentBoard, Player i_Player, out string i_WinningType)
        {
            int rowDroped = 0;
            for (int i = 0; i < i_CurrentBoard.Colmuns; i++)
            {
                if (InsertToBoard(i_CurrentBoard, i, i_Player, out rowDroped) == true)
                {
                    if (Decision(i_CurrentBoard, i_Player, rowDroped, i, out i_WinningType) == true)
                    {
                        return true;
                    }
                    else
                    {
                        DeleteFromBoard(i_CurrentBoard, rowDroped, i);
                        if (DeleteText != null)
                        {
                            DeleteText.Invoke(rowDroped, i);
                        }
                    }
                }
            }

            i_WinningType = string.Empty;
            return false;
        }

        /*
        delete's a piece from the board and puts blank space instead.
        */
        private void DeleteFromBoard(Board i_CurrentBoard, int i_Row, int i_Column)
        {
            i_CurrentBoard.MyBoard[i_Row, i_Column].PlayerPiece = Piece.ePlayerPiece.NoPiece;
        }

        public void PlayGameTurn(int i_SelectedCoulumnToDrop, ref string i_StatusMessege)
        {
               int cellRowSquareDroped = 0;
               int columnSquareDroped = 0;
               bool isPlayerOneWon = false;

               if (isPlayerOneTurn)
               {
                   m_CurrentPlayer = r_PlayerOne;
                   isPlayerOneTurn = false;
               }
               else
               {
                   m_CurrentPlayer = r_PlayerTwo;
                   isPlayerOneTurn = true;
               }

               this.InsertToBoard(m_MyBoard, i_SelectedCoulumnToDrop, m_CurrentPlayer, out cellRowSquareDroped);
               if (this.IsColumnFull(m_MyBoard, m_CurrentPlayer, i_SelectedCoulumnToDrop))
                    {
                        if (EnableButton != null)
                        {
                            EnableButton.Invoke(i_SelectedCoulumnToDrop);
                        }
                    }          
            
                      if (this.Decision(m_MyBoard, m_CurrentPlayer, cellRowSquareDroped, i_SelectedCoulumnToDrop, out winningType) == true)
                         {
                            isPlayerOneWon = true;
                            if (m_CurrentPlayer == r_PlayerOne)
                            {
                                i_StatusMessege = r_PlayerOne.PlayerName + " Won " + winningType;
                                isPlayerOneTurn = true;
                            }
                            else
                            {
                                i_StatusMessege = r_PlayerTwo.PlayerName + " Won " + winningType; 
                                isPlayerOneTurn = false;
                            }
                         }
                    else if (this.IsAllBoardFull(m_MyBoard) == true)
                        {
                            i_StatusMessege = "Tie!!";
                        }

                      if (!r_PlayerTwo.IsMan && !isPlayerOneWon)
                      {
                          m_CurrentPlayer = r_PlayerTwo;
                          if (this.AiCheckForFlushWinning(m_MyBoard, m_CurrentPlayer, out winningType) == true)
                          {
                              i_StatusMessege = "Computer Won " + winningType;
                              isPlayerOneTurn = false;
                          }
                          else if (this.AiCheckForFlushBlocking(m_MyBoard, out columnSquareDroped) == true)
                          {
                              if (this.InsertToBoard(m_MyBoard, columnSquareDroped, m_CurrentPlayer, out cellRowSquareDroped))
                              {
                                  isPlayerOneTurn = true;
                              }
                          }
                          else
                          {
                              columnSquareDroped = this.RandomInsert(m_MyBoard, m_CurrentPlayer, out cellRowSquareDroped);
                              isPlayerOneTurn = true;
                          }
    
                          if (this.Decision(m_MyBoard, m_CurrentPlayer, cellRowSquareDroped, columnSquareDroped, out winningType) == true)
                          {
                              if (m_CurrentPlayer == r_PlayerOne)
                              {
                                  i_StatusMessege = r_PlayerOne.PlayerName + " Won " + winningType;
                                  isPlayerOneTurn = true;
                              }
                              else
                              {
                                  i_StatusMessege = r_PlayerTwo.PlayerName + " Won " + winningType;
                                  isPlayerOneTurn = false;
                              }
                          }

                          if (this.IsColumnFull(m_MyBoard, m_CurrentPlayer, columnSquareDroped))
                          {
                              if (EnableButton != null)
                              {
                                  EnableButton.Invoke(columnSquareDroped);
                              }
                          }         
                      }
                      
                       if (this.IsAllBoardFull(m_MyBoard) == true)
                      {
                          i_StatusMessege = "Tie!!";
                      }
                }
        }
    }
