using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B16_Ex05
{
    public class Piece
    {
        private ePlayerPiece m_PlayerPiece;

        public enum ePlayerPiece
        {
            PlayerOne,
            PlayerTwo,
            NoPiece,
        }

        public Piece(ePlayerPiece i_PlayerPiece)
        {
            m_PlayerPiece = i_PlayerPiece;
        }

        public ePlayerPiece PlayerPiece
        {
            get 
            {
                return m_PlayerPiece;
            }

            set 
            {
                m_PlayerPiece = value;
            }
        }
    }
}
