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
    public partial class ModalDialog : Form
    {
        public event ApplyEventHandler Apply;
        
        public ModalDialog()
        {
            InitializeComponent();
        }

        #region Color Stuff  // Not Working, not used.


        Color bgm_color;
        Color grid_color;
        private void bgm_btn_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = bgm_color;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                bgm_color = dlg.Color;
                bgm_btn.BackColor = bgm_color;

            }
        }

        private void grid_btn_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = grid_color;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                grid_color = dlg.Color;
                grid_btn.BackColor = grid_color;

            }
        }

        public Color GetbgmCol()
        {
            return bgm_btn.BackColor;
        }

        public Color GetgridCol()
        {
            return grid_color;
        }
        #endregion Color Stuff


        #region Numeric Settings
        public int Generations
        {
            get
            {
                return (int)generator.Value;


            }
            set
            {
                generator.Value = value;
            }
        }
        public int GridHeight
        {
            get
            {
                return (int)g_height.Value;
            }
            set
            {
                g_height.Value = value;
            }
        }
        public int GridWidth
        {
            get
            {
                return (int)g_width.Value;
            }
            set
            {
                g_width.Value = value;
            }
        }
        #endregion Numeric Settings


        //Apply
        private void buttonApply_Click(object sender, EventArgs e)
        {
            Apply?.Invoke(this, new ApplyEventArgs((int)generator.Value, (int)GridHeight, (int)GridWidth));
        }
        //Delegate
        public delegate void ApplyEventHandler(object sender, ApplyEventArgs e);

        //Event Arguements
        public class ApplyEventArgs : EventArgs
        {
            public int GridHeight { get; set; }
            public int GridWidth { get; set; }
            public int Generations { get; set; }

            public ApplyEventArgs(int Generations, int GridHeight, int GridWidth)
            {
                this.Generations = Generations;
                this.GridHeight = GridHeight;
                this.GridWidth = GridWidth;
            }
        }
    }
}
