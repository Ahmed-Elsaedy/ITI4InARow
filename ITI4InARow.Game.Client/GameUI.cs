using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ITI4InARow.Module.Core;
using Microsoft.VisualBasic.PowerPacks;

namespace ITI4InARow.Game.UI
{
    public partial class GameUI : UserControl
    {
        int[] columns;
        public Color player1Color;
        public Color player2Color;
        public bool isGameRunning;
        public bool isSpectatorMode;
        int playersMovesCount;
        Color defaultOvalColor;

        public GameUI()
        {
            InitializeComponent();
            InitGame();
        }

        private void InitGame()
        {
            columns = new int[] { 6, 12, 18, 24, 30, 36, 42 };
            playersMovesCount = 0;
            defaultOvalColor = Color.White;
            
        }
        private void GameUI_Load(object sender, EventArgs e)
        {
           

            int index = 1;
            foreach (OvalShape oval in shapeContainer1.Shapes)
            {
                oval.Tag = index;
                index++;
                oval.MouseClick += GameUI_MouseClick;
                oval.MouseEnter += GameUI_MouseEnter;
                oval.MouseLeave += GameUI_Leave;
            }
        }
        private void GameUI_Leave(object sender, EventArgs e)
        {
            if (!isSpectatorMode)
            {
                ((OvalShape)sender).BorderColor = Color.Black;
                ((OvalShape)sender).BorderWidth -= 5; 
            }
        }

        private void GameUI_MouseEnter(object sender, EventArgs e)
        {
            if (!isSpectatorMode)
            {
                ((OvalShape)sender).BorderWidth += 5;
                ((OvalShape)sender).BorderColor = player1Color; 
            }
        }

        private void GameUI_MouseClick(object sender, MouseEventArgs e)
        {
            GameLogic(int.Parse(((OvalShape)sender).Tag.ToString()),player1Color);
        }
        public void GameLogic(int TokenPosition , Color TokenColor)
        {
            if (!isSpectatorMode)
            {
                if (isGameRunning)
                {

                    lblplayerturn2.BackColor = Color.White;
                    lblplayerturn.BackColor = Color.Transparent;
                    playersMovesCount++;
                    if (TokenPosition >= 1 && TokenPosition <= 6)
                    {
                        if (columns[0] > 0)
                        {
                            ((OvalShape)shapeContainer1.Shapes.get_Item(columns[0] - 1)).FillColor = TokenColor;
                            ((OvalShape)shapeContainer1.Shapes.get_Item(columns[0] - 1)).Enabled = false;
                            PlayerAction?.Invoke(this, ((OvalShape)shapeContainer1.Shapes.get_Item(columns[0] - 1)));
                            columns[0]--;
                        }
                    }
                    else if (TokenPosition >= 7 && TokenPosition <= 12)
                    {
                        if (columns[1] > 6)
                        {
                            ((OvalShape)shapeContainer1.Shapes.get_Item(columns[1] - 1)).FillColor = TokenColor;
                            ((OvalShape)shapeContainer1.Shapes.get_Item(columns[1] - 1)).Enabled = false;
                            PlayerAction?.Invoke(this, ((OvalShape)shapeContainer1.Shapes.get_Item(columns[1] - 1)));
                            columns[1]--;
                        }
                    }
                    else if (TokenPosition >= 13 && TokenPosition <= 18)
                    {
                        if (columns[2] > 12)
                        {
                            ((OvalShape)shapeContainer1.Shapes.get_Item(columns[2] - 1)).FillColor = TokenColor;
                            ((OvalShape)shapeContainer1.Shapes.get_Item(columns[2] - 1)).Enabled = false;
                            PlayerAction?.Invoke(this, ((OvalShape)shapeContainer1.Shapes.get_Item(columns[2] - 1)));
                            columns[2]--;
                        }
                    }
                    else if (TokenPosition >= 19 && TokenPosition <= 24)
                    {
                        if (columns[3] > 18)
                        {
                            ((OvalShape)shapeContainer1.Shapes.get_Item(columns[3] - 1)).FillColor = TokenColor;
                            ((OvalShape)shapeContainer1.Shapes.get_Item(columns[3] - 1)).Enabled = false;
                            PlayerAction?.Invoke(this, ((OvalShape)shapeContainer1.Shapes.get_Item(columns[3] - 1)));
                            columns[3]--;
                        }
                    }
                    else if (TokenPosition >= 25 && TokenPosition <= 30)
                    {
                        if (columns[4] > 24)
                        {
                            ((OvalShape)shapeContainer1.Shapes.get_Item(columns[4] - 1)).FillColor = TokenColor;
                            ((OvalShape)shapeContainer1.Shapes.get_Item(columns[4] - 1)).Enabled = false;
                            PlayerAction?.Invoke(this, ((OvalShape)shapeContainer1.Shapes.get_Item(columns[4] - 1)));
                            columns[4]--;
                        }
                    }
                    else if (TokenPosition >= 31 && TokenPosition <= 36)
                    {
                        if (columns[5] > 30)
                        {
                            ((OvalShape)shapeContainer1.Shapes.get_Item(columns[5] - 1)).FillColor = TokenColor;
                            ((OvalShape)shapeContainer1.Shapes.get_Item(columns[5] - 1)).Enabled = false;
                            PlayerAction?.Invoke(this, ((OvalShape)shapeContainer1.Shapes.get_Item(columns[5] - 1)));
                            columns[5]--;
                        }
                    }
                    else if (TokenPosition >= 37 && TokenPosition <= 42)
                    {
                        if (columns[6] > 36)
                        {
                            ((OvalShape)shapeContainer1.Shapes.get_Item(columns[6] - 1)).FillColor = TokenColor;
                            ((OvalShape)shapeContainer1.Shapes.get_Item(columns[6] - 1)).Enabled = false;
                            PlayerAction?.Invoke(this, ((OvalShape)shapeContainer1.Shapes.get_Item(columns[6] - 1)));
                            columns[6]--;
                        }
                    }
                }
                isGameRunning = false;
                
            }
        }

        internal void HighlightedPlayer()=> lblplayerturn.BackColor = Color.White;
         

        internal void Apply_Other_Client_Action(int TokenPosition , Color otherPlayerColor)
        {
            if (!isGameRunning)
            {
                lblplayerturn.BackColor = Color.White;
                lblplayerturn2.BackColor = Color.Transparent;
                playersMovesCount++;

                if (TokenPosition >= 1 && TokenPosition <= 6)
                {
                    if (columns[0] > 0)
                    {
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[0] - 1)).FillColor = otherPlayerColor;
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[0] - 1)).Enabled = false;
                        columns[0]--;
                    }
                }
                else if (TokenPosition >= 7 && TokenPosition <= 12)
                {
                    if (columns[1] > 6)
                    {
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[1] - 1)).FillColor = otherPlayerColor;
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[1] - 1)).Enabled = false;
                        columns[1]--;
                    }
                }
                else if (TokenPosition >= 13 && TokenPosition <= 18)
                {
                    if (columns[2] > 12)
                    {
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[2] - 1)).FillColor = otherPlayerColor;
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[2] - 1)).Enabled = false;
                        columns[2]--;
                    }
                }
                else if (TokenPosition >= 19 && TokenPosition <= 24)
                {
                    if (columns[3] > 18)
                    {
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[3] - 1)).FillColor = otherPlayerColor;
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[3] - 1)).Enabled = false;
                        columns[3]--;
                    }
                }
                else if (TokenPosition >= 25 && TokenPosition <= 30)
                {
                    if (columns[4] > 24)
                    {
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[4] - 1)).FillColor = otherPlayerColor;
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[4] - 1)).Enabled = false;
                        columns[4]--;
                    }
                }
                else if (TokenPosition >= 31 && TokenPosition <= 36)
                {
                    if (columns[5] > 30)
                    {
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[5] - 1)).FillColor = otherPlayerColor;
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[5] - 1)).Enabled = false;
                        columns[5]--;
                    }

                }
                else if (TokenPosition >= 37 && TokenPosition <= 42)
                {
                    if (columns[6] > 36)
                    {
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[6] - 1)).FillColor = otherPlayerColor;
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[6] - 1)).Enabled = false;
                        columns[6]--;
                    }
                } 
            }
            isGameRunning = true;
            if (isSpectatorMode)
            {
                isGameRunning = false;
            }
        }

        public void fillcolorsforspectetorjoin(string[] viewSpectatorBoard)
        {
            isGameRunning = false;
            isSpectatorMode = true;
            lblplayerturn.Hide();
            lblplayerturn2.Hide();
            for (int i = 0; i < viewSpectatorBoard.Length; i++)
            {
                if (!viewSpectatorBoard[i].Equals("White"))
                {
                    ((OvalShape)shapeContainer1.Shapes.get_Item(i)).FillColor = Color.FromArgb(int.Parse(viewSpectatorBoard[i]));
                }
            }
            int index = 1;
            foreach (OvalShape oval in shapeContainer1.Shapes)
            {
                oval.Cursor = Cursors.No;
                oval.Tag = index;
                index++;
                oval.MouseClick -= GameUI_MouseClick;
                oval.MouseEnter -= GameUI_MouseEnter;
                oval.MouseLeave -= GameUI_Leave;
            }
        }

        bool GamePlan(OvalShape ovalClicked, ref int x, CheckPosition cp)
        {
            if (x < 4)
            {
                int TokenIndex = int.Parse(ovalClicked.Tag.ToString()) - 1;
                int leftTokenIndex = TokenIndex + (int)cp;
                if (leftTokenIndex >= 0 && leftTokenIndex < 42)
                {
                    if (ovalClicked.FillColor.Equals(((OvalShape)shapeContainer1.Shapes.get_Item(leftTokenIndex)).FillColor))
                    {
                        x++;
                        return GamePlan(((OvalShape)shapeContainer1.Shapes.get_Item(leftTokenIndex)), ref x, cp);
                    }
                    else
                    {
                        return false;
                    }
                }
                else return false;
            }
            return true;
        }
        private void GameUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult res = MessageBox.Show("you will lose the game !", "watch out!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (res == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }
        public void BoardReset()
        {
            InitGame();
            foreach (OvalShape oval in shapeContainer1.Shapes)
            {
                oval.Enabled = true;
                oval.FillColor = defaultOvalColor;
            }
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("you will lose the game !", "watch out!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (res == DialogResult.OK)
            {
                Visible = false;
            }
        }
        public event EventHandler<OvalShape> PlayerAction;
    }
}
