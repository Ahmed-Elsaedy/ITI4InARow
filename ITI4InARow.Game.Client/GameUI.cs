using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.PowerPacks;

namespace ITI4InARow.Game.UI
{
    public partial class GameUI : UserControl
    {
        int[] columns;
        bool playerTurn;
        Color player1Color;
        Color player2Color;
        bool isGameRunning;
        int playersMovesCount;
        Color defaultOvalColor;
        List<int> NorthBanned;
        List<int> SouthBanned;

        internal enum CheckPosition { NORTH = (-1), SOUTH = (1), EAST = (-6), WEST = 6, NORTH_EAST = (-6 - 1), NORTH_WEST = (6 - 1), SOUTH_EAST = (-6 + 1), SOUTH_WEST = (6 + 1) };

        public GameUI()
        {
            InitializeComponent();
            InitGame();
            //OvalShape ovalShape3 = new OvalShape(shapeContainer1);
            //ovalShape3.Size = new Size(90, 90);
            //ovalShape3.Location = new Point(150, 90);

            //ovalShape3.FillStyle = FillStyle.Solid;
            //ovalShape3.FillColor = Color.Azure;
            //shapeContainer1.ForeColor = Color.Green;
            NorthBanned = new List<int> { 1, 2, 3, 7, 8, 9, 13, 14, 15, 19, 20, 21, 25, 26, 27, 31, 32, 33, 37, 38, 39 };
            SouthBanned = new List<int> { 4, 5, 6, 10, 11, 12, 16, 17, 18, 22, 23, 24, 28, 29, 30, 34, 35, 36, 40, 41, 42 };


        }

        private void InitGame()
        {
            columns = new int[] { 6, 12, 18, 24, 30, 36, 42 };
            playerTurn = true;
            player1Color = Color.Purple;
            player2Color = Color.SpringGreen;
            isGameRunning = true;
            playersMovesCount = 0;
            defaultOvalColor = Color.White;
        }
        private void ovalShape2_Click(object sender, EventArgs e)
        {
            //ovalShape1.FillColor = Color.Aqua;

            //MessageBox.Show(ovalShape1.Tag.ToString());
            //if (shapeContainer1.Shapes.Contains((Shape)sender))
            //{
            //    MessageBox.Show(((Shape)sender).ToString());
            //}


        }

        private void ovalShape41_MouseClick(object sender, MouseEventArgs e)
        {
            //ovalShape41.FillColor = Color.Blue;
        }

        private void GameUI_Load(object sender, EventArgs e)
        {
            /*
            for (int i = 0; i < 42; i++)
            {
                ((OvalShape)shapeContainer1.Shapes.get_Item(i)).Tag = i + 1;
                ((OvalShape)shapeContainer1.Shapes.get_Item(i)).MouseClick += GameUI_MouseClick;
                //((OvalShape)shapeContainer1.Shapes.get_Item(i)).MouseEnter += GameUI_MouseEnter;
              // ((OvalShape)shapeContainer1.Shapes.get_Item(i)).MouseLeave += GameUI_Leave;

             }*/
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
            ((OvalShape)sender).BorderColor = Color.Black;
            ((OvalShape)sender).BorderWidth -= 5;
        }

        private void GameUI_MouseEnter(object sender, EventArgs e)
        {
            ((OvalShape)sender).BorderWidth += 5;
            if (playerTurn)
            {
                ((OvalShape)sender).BorderColor = player1Color;

            }
            else
            {
                ((OvalShape)sender).BorderColor = player2Color;
            }
        }

        private void GameUI_MouseClick(object sender, MouseEventArgs e)
        {
            GameLogic(int.Parse(((OvalShape)sender).Tag.ToString()));
            // MessageBox.Show(((OvalShape)sender).Tag.ToString());
        }



        private void GameLogic(int TokenPosition)
        {

            Color TokenColor;
            OvalShape ovalClicked = new OvalShape();

            //mouse enter issue when it considrate fill
            // ((OvalShape)shapeContainer1.Shapes.get_Item(TokenPosition)).FillColor = defaultOvalColor;
            if (isGameRunning)
            {

                playersMovesCount++;
                if (playerTurn)
                {
                    TokenColor = player1Color;

                }
                else
                {
                    TokenColor = player2Color;
                }
                playerTurn = !playerTurn;
                if (TokenPosition >= 1 && TokenPosition <= 6)
                {
                    if (columns[0] > 0)
                    {
                        ovalClicked = ((OvalShape)shapeContainer1.Shapes.get_Item(columns[0] - 1));
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[0] - 1)).FillColor = TokenColor;
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[0] - 1)).Enabled = false;
                        columns[0]--;
                    }

                }
                else if (TokenPosition >= 7 && TokenPosition <= 12)
                {
                    if (columns[1] > 6)
                    {
                        ovalClicked = ((OvalShape)shapeContainer1.Shapes.get_Item(columns[1] - 1));
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[1] - 1)).FillColor = TokenColor;
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[1] - 1)).Enabled = false;
                        columns[1]--;
                    }

                }
                else if (TokenPosition >= 13 && TokenPosition <= 18)
                {
                    if (columns[2] > 12)
                    {
                        ovalClicked = ((OvalShape)shapeContainer1.Shapes.get_Item(columns[2] - 1));
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[2] - 1)).FillColor = TokenColor;
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[2] - 1)).Enabled = false;
                        // ((OvalShape)shapeContainer1.Shapes.get_Item(columns[2] - 1)).FillStyle = FillStyle.Cross;
                        columns[2]--;
                    }

                }
                else if (TokenPosition >= 19 && TokenPosition <= 24)
                {
                    if (columns[3] > 18)
                    {
                        ovalClicked = ((OvalShape)shapeContainer1.Shapes.get_Item(columns[3] - 1));
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[3] - 1)).FillColor = TokenColor;
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[3] - 1)).Enabled = false;
                        // ((OvalShape)shapeContainer1.Shapes.get_Item(columns[2] - 1)).FillStyle = FillStyle.Cross;
                        columns[3]--;
                    }

                }
                else if (TokenPosition >= 25 && TokenPosition <= 30)
                {
                    if (columns[4] > 24)
                    {
                        ovalClicked = ((OvalShape)shapeContainer1.Shapes.get_Item(columns[4] - 1));
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[4] - 1)).FillColor = TokenColor;
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[4] - 1)).Enabled = false;
                        // ((OvalShape)shapeContainer1.Shapes.get_Item(columns[2] - 1)).FillStyle = FillStyle.Cross;
                        columns[4]--;
                    }

                }
                else if (TokenPosition >= 31 && TokenPosition <= 36)
                {
                    if (columns[5] > 30)
                    {
                        ovalClicked = ((OvalShape)shapeContainer1.Shapes.get_Item(columns[5] - 1));
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[5] - 1)).FillColor = TokenColor;
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[5] - 1)).Enabled = false;
                        // ((OvalShape)shapeContainer1.Shapes.get_Item(columns[2] - 1)).FillStyle = FillStyle.Cross;
                        columns[5]--;
                    }

                }
                else if (TokenPosition >= 37 && TokenPosition <= 42)
                {
                    if (columns[6] > 36)
                    {
                        ovalClicked = ((OvalShape)shapeContainer1.Shapes.get_Item(columns[6] - 1));
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[6] - 1)).FillColor = TokenColor;
                        ((OvalShape)shapeContainer1.Shapes.get_Item(columns[6] - 1)).Enabled = false;
                        // ((OvalShape)shapeContainer1.Shapes.get_Item(columns[2] - 1)).FillStyle = FillStyle.Cross;
                        columns[6]--;
                    }

                }
                //((OvalShape)(shapeContainer1.Shapes.get_Item(TokenPosition - 1))).FillColor = defaultOvalColor;

                if (playersMovesCount == 42)
                {
                    isGameRunning = false;
                    MessageBox.Show("Game End");
                    return;
                }
                int x = 1;
                //MessageBox.Show(northBanned.IndexOf((int)ovalClicked.Tag).ToString());
                if (NorthBanned.IndexOf((int)ovalClicked.Tag) == -1)
                {
                    if (GamePlan(ovalClicked, ref x, CheckPosition.NORTH))
                    {
                        //isGameRunning = false;
                        MessageBox.Show(ovalClicked.FillColor.ToString() + " is win North");
                    }
                }
                if (SouthBanned.IndexOf((int)ovalClicked.Tag) == -1)
                {
                    if (GamePlan(ovalClicked, ref x, CheckPosition.SOUTH))
                    {
                        isGameRunning = false;
                        MessageBox.Show(ovalClicked.FillColor.ToString() + " is win south");
                        return;
                    }
                }
                /////////////////////////////////////////////
                x = 1;
                if (GamePlan(ovalClicked, ref x, CheckPosition.WEST))
                {
                    //isGameRunning = false;
                    MessageBox.Show(ovalClicked.FillColor.ToString() + " is win west");
                }
                if (GamePlan(ovalClicked, ref x, CheckPosition.EAST))
                {
                    isGameRunning = false;
                    MessageBox.Show(ovalClicked.FillColor.ToString() + " is win east");
                    return;
                }
                //////////////////////////////////////////////
                x = 1;
                if (NorthBanned.IndexOf((int)ovalClicked.Tag) == -1)
                {
                    if (GamePlan(ovalClicked, ref x, CheckPosition.NORTH_EAST))
                    {
                        //isGameRunning = false;
                        MessageBox.Show(ovalClicked.FillColor.ToString() + " is win north west");
                    }
                }
                if (SouthBanned.IndexOf((int)ovalClicked.Tag) == -1)
                {
                    if (GamePlan(ovalClicked, ref x, CheckPosition.SOUTH_WEST))
                    {
                        isGameRunning = false;
                        MessageBox.Show(ovalClicked.FillColor.ToString() + " is win south west");
                        return;
                    }
                }
                ////////////////////////////////////////////////
                x = 1;

                if (GamePlan(ovalClicked, ref x, CheckPosition.NORTH_WEST))
                {
                    //isGameRunning = false;
                    MessageBox.Show(ovalClicked.FillColor.ToString() + " is win north west");
                }

                if (GamePlan(ovalClicked, ref x, CheckPosition.SOUTH_EAST))
                {
                    isGameRunning = false;
                    MessageBox.Show(ovalClicked.FillColor.ToString() + " is win south east");
                    return;
                }
                /////////////////

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

        private void BtnReset_Click(object sender, EventArgs e)
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
                this.Visible = false;
            }
            
            
        }
    }
}
