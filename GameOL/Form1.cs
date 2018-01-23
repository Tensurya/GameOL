using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOL
{
    public partial class Form1 : Form
    {
        #region Variables
        //The universe size
        int gridHeight = 20;
        int gridWidth = 20;

        // The universe array
        bool[,] universe;

        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.Gray;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;

        // Generation state
        int gen_state = 0;

        #endregion Variables

        public Form1()
        {
            universe = new bool[gridWidth, gridHeight];

            InitializeComponent();


            universe[1, 1] = true;
            timer.Interval = 40;
            timer.Enabled = false;
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object Sender, EventArgs e)
        {
            if(gen_state == 0)
            {
                NextGeneration();
            }
        }

        #region Generations

        #region Finite Generator
        private void NextGeneration()
        {
            bool[,] scratchpad = new bool[gridWidth, gridHeight];

            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    int neighbor = GetNeighbourCount(x, y);
                    if (universe[x, y])
                    {
                        if (neighbor < 2 || neighbor > 3)
                        {
                            scratchpad[x, y] = false;
                        }
                        else if (neighbor == 2 || neighbor == 3)
                        {
                            scratchpad[x, y] = true;
                        }
                    }
                    else if (neighbor == 3)
                    {
                        scratchpad[x, y] = true;
                    }
                }
            }

            generations++;
            toolStripStatusLabel1Gener.Text = "Generations = " + generations.ToString();

            universe = scratchpad;
            graphicsPanel1.Invalidate();
        }
        #endregion Finite Generator


        #endregion Generations

        #region PaintWorld

        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            // The width and height of each cell in pixels
            float cellWidth = graphicsPanel1.ClientSize.Width / (float)universe.GetLength(0);
            float cellHeight = graphicsPanel1.ClientSize.Height / (float)universe.GetLength(1);

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);

            Brush cellBrush = new SolidBrush(cellColor);

            Font font = new Font("Arial", 8f);

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels
                    RectangleF cellRect = RectangleF.Empty;
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    // Fill the cell with a brush
                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                    }
                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);

                    e.Graphics.DrawString(GetNeighbourCount(x, y).ToString(), font, Brushes.Black, cellRect, stringFormat);
                }
            }

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
        }
        #endregion PaintWorld

        #region DrawCell
        private void graphicsPanel1_Click(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                float cellWidth = graphicsPanel1.ClientSize.Width / (float)universe.GetLength(0);
                float cellHeight = graphicsPanel1.ClientSize.Height / (float)universe.GetLength(1);


                if (cellWidth == 0 || cellHeight == 0)
                {
                    return;
                }
                float x = e.X / cellWidth;
                float y = e.Y / cellHeight;

                //Stops game from bugging out when clicked on the extreme right or bottom. 
                if (x >= gridWidth || y >= gridHeight)
                {
                    return;
                }
                universe[(int)x, (int)y] = !universe[(int)x, (int)y];
                graphicsPanel1.Invalidate();
            }
        }
        #endregion DrawCell

        #region Buttons

        #region ToolStr_Btns
        private void startButton_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            NextGeneration();
        }
        #endregion ToolStr_Btns

        #region ContextMen_Btns
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }

        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            NextGeneration();
        }
        #endregion ContextMen_Btns

        #region MenuStr_Btns
        private void newToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false;
                }
            }
            generations = 0;

            graphicsPanel1.Invalidate();
            toolStripStatusLabel1Gener.Text = "Generations = " + generations.ToString();
        }
        #endregion MenuStr_Btns

        #endregion Buttons

        #region Functions
        private int GetNeighbourCount(int x, int y)
        {

            return IsAlive(x - 1, y - 1) + IsAlive(x, y - 1) + IsAlive(x + 1, y - 1)
                 + IsAlive(x - 1, y) + IsAlive(x + 1, y)
                 + IsAlive(x - 1, y + 1) + IsAlive(x, y + 1) + IsAlive(x + 1, y + 1);
        }

        private int IsAlive(int x, int y)
        {
            if (InBounds(x, y))
            {
                if (universe[x, y])
                    return 1;
                else
                    return 0;
            }
            else
                return 0;
        }

        private bool InBounds(int x, int y)
        {
            return x > -1 && y > -1 && x < gridWidth && y < gridHeight;
        }

        #endregion Functions




        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModalDialog dlg = new ModalDialog();

            if(DialogResult.OK == dlg.ShowDialog())
            {

            }
        }
    }
}
