using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace B16_Ex05
{
    public partial class FormGameBoard : Form
    {
        private Management m_Managment = null;
        private List<Button> m_PlayerSelectionButtonList = null;
        private Button[,] m_ButtonsPresentionBoard = null;

        public FormGameBoard(Management i_Management)
        {
            m_Managment = i_Management;
            m_PlayerSelectionButtonList = new List<Button>();
            m_ButtonsPresentionBoard = new Button[m_Managment.MyBoard.Rows, m_Managment.MyBoard.Colmuns];
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            InitializeComponent();
            InitializeGameBoard();
            buildBoardButtons();
        }

        private void InitializeGameBoard()
        {
            labelPlayerOneScore.Text = m_Managment.MyBoard.PlayerOne.PlayerName + ": " + m_Managment.MyBoard.PlayerOne.PlayerScore;
            labelPlayerTwoScore.Text = m_Managment.MyBoard.PlayerTwo.PlayerName + ": " + m_Managment.MyBoard.PlayerTwo.PlayerScore;
            m_Managment.CellBoardTextChanged += new BoardTextChanged(ButtonPresentionTextChanged);
            m_Managment.DeleteText += new DeleteTextFromBoard(ButtonPresentionDeleteText);
            m_Managment.EnableButton += new EnableButtonWhenColumnIsFull(changeButtonToEnable);
        }

        private void buildBoardButtons()
        {
            int left = 0;
            int top = 0;
            int i = 0;
            int j = 0;
            int widthClientSize = 0;
            int heightClientSize = 0;
            for (; i < m_Managment.MyBoard.Colmuns; i++)
            {
                Button button = new Button();
                button.Text = string.Format("{0}", i + 1);
                button.Name = "button" + i + 1;
                button.Size = new System.Drawing.Size(40, 25);
                button.Location = new System.Drawing.Point(10 + left, 20);
                button.Click += new System.EventHandler(this.playerSelectionButtonClicked_Click);
                this.Controls.Add(button);
                m_PlayerSelectionButtonList.Add(button);
                left += 50; 
            }

            widthClientSize = m_PlayerSelectionButtonList[i - 1].Right + 10;
            left = 0;
            for (i = 0; i < m_Managment.MyBoard.Rows; i++)
            {
                for (j = 0; j < m_Managment.MyBoard.Colmuns; j++)
                {
                    Button buttonToAdd = new Button();
                    buttonToAdd.Size = new System.Drawing.Size(40, 40);
                    buttonToAdd.Name = "buttonPresention" + i + j;
                    buttonToAdd.Location = new System.Drawing.Point(10 + left, 50 + top);
                    left += 50;
                    buttonToAdd.Text = string.Empty;
                    this.Controls.Add(buttonToAdd);
                    m_ButtonsPresentionBoard[i, j] = buttonToAdd;
                }

                left = 0;
                top += 50;
            }

            labelPlayerOneScore.Top = m_ButtonsPresentionBoard[m_Managment.MyBoard.Rows - 1, 0].Bottom + 10;
            labelPlayerOneScore.Left = m_ButtonsPresentionBoard[m_Managment.MyBoard.Rows - 1, (m_Managment.MyBoard.Colmuns - 1) / 2].Left;
            labelPlayerTwoScore.Top = labelPlayerOneScore.Top;
            labelPlayerTwoScore.Left = labelPlayerOneScore.Right + 20;
            heightClientSize = labelPlayerOneScore.Bottom + 10;
            this.ClientSize = new Size(widthClientSize, heightClientSize);
        }

        private void playerSelectionButtonClicked_Click(object sender, EventArgs e)
        {
            string statusMessage = string.Empty;
            int selectedColumn = int.Parse((sender as Button).Text);
            m_Managment.PlayGameTurn(selectedColumn - 1, ref statusMessage);

            if (statusMessage != string.Empty)
            {
                showMessageBoxWhenGameEnds(statusMessage);
            }
        }

        public void ButtonPresentionTextChanged(Piece i_Piece, int i_Rows, int i_Columns)
        {
            if (m_Managment.MyBoard.MyBoard[i_Rows, i_Columns].PlayerPiece == Piece.ePlayerPiece.PlayerOne)
            {
                m_ButtonsPresentionBoard[i_Rows, i_Columns].Text = "X";
            }
            else if (m_Managment.MyBoard.MyBoard[i_Rows, i_Columns].PlayerPiece == Piece.ePlayerPiece.PlayerTwo)
            {
                m_ButtonsPresentionBoard[i_Rows, i_Columns].Text = "O";
            }
            else
            {
                m_ButtonsPresentionBoard[i_Rows, i_Columns].Text = string.Empty;
            }
        }

        public void ButtonPresentionDeleteText(int i_Rows, int i_Columns)
        {
            m_Managment.MyBoard.MyBoard[i_Rows, i_Columns].PlayerPiece = Piece.ePlayerPiece.NoPiece;
            m_ButtonsPresentionBoard[i_Rows, i_Columns].Text = string.Empty;
        }

        private void boardReset()
        {
            for (int i = 0; i < m_Managment.MyBoard.Rows; i++)
            {
                for (int j = 0; j < m_Managment.MyBoard.Colmuns; j++)
                {
                    m_ButtonsPresentionBoard[i, j].Text = string.Empty;
                    if (i == 0)
                    {
                        m_PlayerSelectionButtonList[j].Enabled = true;
                    }
                }
            }

            Board currentBoard = new Board(m_Managment.MyBoard.Rows, m_Managment.MyBoard.Colmuns, m_Managment.MyBoard.PlayerOne, m_Managment.MyBoard.PlayerTwo);
            m_Managment = new Management(currentBoard);
            InitializeGameBoard();
        }

        private void showMessageBoxWhenGameEnds(string i_Msg)
        {
            string title = string.Empty;

            if (string.Compare(i_Msg, "Tie!!") == 0)
            {
                title = "A Tie!";
            }
            else
            {
                title = "A Win!";
            }

                if (MessageBox.Show(i_Msg + System.Environment.NewLine + "Another Round?", title, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    boardReset();
                }
            else
            {
                this.Close();
            }
        }

        private void changeButtonToEnable(int i_Column)
        {
            if (m_PlayerSelectionButtonList[i_Column].Enabled == true)
            {
                m_PlayerSelectionButtonList[i_Column].Enabled = false;
            }
        }
    }
}
