using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B16_Ex05
{
    public class Board
    {
        private readonly Player r_PlayerOne = null;

        public Player PlayerOne
        {
            get { return r_PlayerOne; }
        }

        private readonly Player r_PlayerTwo = null;

        public Player PlayerTwo
        {
            get 
            { 
                return r_PlayerTwo;
            }
        } 

        private readonly int m_Colmuns;

        private readonly int m_Rows;

        private Piece[,] m_MyBoard = null;

        public Piece[,] MyBoard
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

        public int Rows
        {
            get
            {
                return m_Rows;
            }
        }

        public int Colmuns
        {
            get
            {
                return m_Colmuns;
            }
        }
        ////constructor
        public Board(int i_Rows, int i_Columns, Player i_PlayerOne, Player i_PlayerTwo)
        {
            m_Rows = i_Rows;
            m_Colmuns = i_Columns;
            m_MyBoard = new Piece[m_Rows, m_Colmuns];
            r_PlayerOne = i_PlayerOne;
            r_PlayerTwo = i_PlayerTwo;
            buildEmptyBoard();
        }

        /*
        build's empty board according to the rows & columns sizes.
        */
        private void buildEmptyBoard()
        {
            for (int i = 0; i < m_Rows; i++)
            {
                for (int j = 0; j < m_Colmuns; j++)
                {
                    m_MyBoard[i, j] = new Piece(Piece.ePlayerPiece.NoPiece);
                }
            }
        }
    }
}
