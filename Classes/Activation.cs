using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace B16_Ex05
{
     public class Activation
    {
        public static void Run()
        {
            FormGameSettings formGameSettings = new FormGameSettings();
            formGameSettings.ShowDialog();
            if (formGameSettings.DialogResult == DialogResult.OK)
            {
                FormGameBoard formGameBoard = new FormGameBoard(new Management(new Board(formGameSettings.Rows, formGameSettings.Cols, new Player(false, Piece.ePlayerPiece.PlayerOne, formGameSettings.PlayerOneName), new Player(formGameSettings.IsMan, Piece.ePlayerPiece.PlayerTwo, formGameSettings.PlayerTwoName))));
                formGameBoard.ShowDialog();
            }
        }
        }
    }
