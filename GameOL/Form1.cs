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
            if (gen_state == 0)
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

        #region Toroid Generator // Not Used
        private void NextWrapAroundGeneration()
        {
            bool[,] scratchpad = new bool[gridWidth, gridHeight];

            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    int neighbor = GetWNeighbourCount(x, y);
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
        #endregion Toroid Generator
        
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
        //Start
        private void startButton_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }
        //Stop
        private void stopButton_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }
        //Next
        private void nextButton_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            NextGeneration();
        }
        //New
        private void newToolStripButton_Click(object sender, EventArgs e)
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
        #endregion ToolStr_Btns

        #region ContextMen_Btns
        //ContextStart
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }
        //ContextStop
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }
        //ContextNext
        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            NextGeneration();
        }
        //ContextColor - Cell Color
        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = cellColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                cellColor = dlg.Color;
                Properties.Settings.Default.cellColor = dlg.Color;
            }
            graphicsPanel1.Invalidate();
        }

        #endregion ContextMen_Btns

        #region MenuStr_Btns

        //BackGround Color
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = graphicsPanel1.BackColor;
            
            if (DialogResult.OK == dlg.ShowDialog())
            {
                graphicsPanel1.BackColor = dlg.Color;
                Properties.Settings.Default.graphicsPanel1 = dlg.Color;
            }
            graphicsPanel1.Invalidate();
        }

        //Cell Color
        private void colorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = cellColor;
            
            if (DialogResult.OK == dlg.ShowDialog())
            {
                cellColor = dlg.Color;
                Properties.Settings.Default.cellColor = dlg.Color;

            }
            graphicsPanel1.Invalidate();

        }

        //Grid Color
        private void generationsSpeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = gridColor;
            
            if (DialogResult.OK == dlg.ShowDialog())
            {
                gridColor = dlg.Color;
                Properties.Settings.Default.gridColor = dlg.Color;
            }
            graphicsPanel1.Invalidate();
        }

        //MenuNew
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
        
        //MenuOptions(View Only)
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModalDialog dlg = new ModalDialog();

            dlg.Generations = timer.Interval;
            dlg.GridHeight = gridHeight;
            dlg.GridWidth = gridWidth;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                timer.Interval = dlg.Generations;
                gridHeight = dlg.GridHeight;
                gridWidth = dlg.GridWidth;
                Properties.Settings.Default.gridHeight = dlg.GridHeight;
                Properties.Settings.Default.gridWidth = dlg.GridWidth;
            }
            graphicsPanel1.Invalidate();
        }

        #endregion MenuStr_Btns

        #endregion Buttons

        #region Functions

        #region Finite Functions
        //Neighbour Counter(finite)
        private int GetNeighbourCount(int x, int y)
        {
            return IsAlive(x - 1, y - 1) + IsAlive(x, y - 1) + IsAlive(x + 1, y - 1)
                 + IsAlive(x - 1, y) + IsAlive(x + 1, y)
                 + IsAlive(x - 1, y + 1) + IsAlive(x, y + 1) + IsAlive(x + 1, y + 1);
        }
        //Cell Alive(finite)
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
        //Cell InBounds(finite)
        private bool InBounds(int x, int y)
        {
            return x > -1 && y > -1 && x < gridWidth && y < gridHeight;
        }
        #endregion Finite Functions
        
        #region Wrap Functions // Not working
        //Neighbour Counter(wrap)
        private int GetWNeighbourCount(int x, int y)
        {
            return IsWAlive(x - 1, y - 1) + IsWAlive(x, y - 1) + IsWAlive(x + 1, y - 1)
                 + IsWAlive(x - 1, y) + IsWAlive(x + 1, y)
                 + IsWAlive(x - 1, y + 1) + IsWAlive(x, y + 1) + IsWAlive(x + 1, y + 1);
        }
        //Cell isAlive(wrap)
        private int IsWAlive(int x, int y)
        {
            if (InBounds(x, y))
            {
                if (universe[x, y])
                    return 1;
                else
                    return 0;
            }
            else if (OutBounds(x, y))
            {
                if (x < -1)
                {
                    x = gridWidth - 1;
                    return 1;
                }
                else if (y < -1)
                {
                    y = gridHeight - 1;
                    return 1;
                }
                else if (x > gridWidth)
                {
                    x = 0;
                    return 1;
                }
                else if (y > gridHeight)
                {
                    y = 0;
                    return 1;
                }
            }

            return 0;
        }
        //Cell OutBounds(wrap)
        private bool OutBounds(int x, int y)
        {
            return x < -1 && y < -1 && x > gridWidth && y > gridHeight;
        }









        #endregion Wrap Functions

        #endregion Functions

    }
}
