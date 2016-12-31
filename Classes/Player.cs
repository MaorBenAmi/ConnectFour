using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B16_Ex05
{
    public class Player
    {
        private readonly bool r_IsMan;

        public bool IsMan
        {
            ////properties
            get
            {
                return r_IsMan;
            }
        }

        private int m_PlayerScore;

        public int PlayerScore
        {
            get
            {
                return m_PlayerScore;
            }

            set
            {
                m_PlayerScore = value;
            }
        }

        private Piece.ePlayerPiece m_Piece;

        public Piece.ePlayerPiece Piece
        {
            get
            {
                return m_Piece;
            }

            set
            {
                m_Piece = value;
            }
        }

        private readonly string m_PlayerName;

        public string PlayerName
        {
            get
            {
                return m_PlayerName;
            }
        }
        ////constructor
        public Player(bool i_IsMan, Piece.ePlayerPiece i_Piece, string i_PlayerName)
        {
            r_IsMan = i_IsMan;
            m_Piece = i_Piece;
            m_PlayerScore = 0;
            m_PlayerName = i_PlayerName;
        }
    }
}
