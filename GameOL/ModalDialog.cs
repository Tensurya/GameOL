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

        Color bgm_color;
        Color grid_color;

        public ModalDialog()
        {
            InitializeComponent();
        }

        #region Color Stuff
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

        private void button1_Click(object sender, EventArgs e)
        {
            Apply?.Invoke(this, new ApplyEventArgs((int)generator.Value));
        }

        //Delegate
        public delegate void ApplyEventHandler(object sender, ApplyEventArgs e);

        //Event Arguements
        public class ApplyEventArgs : EventArgs
        {

            public int Generations { get; set; }

            public ApplyEventArgs(int Generations)
            {
                this.Generations = Generations;
                
            }
        }

      
    }
}
