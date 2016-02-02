using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace GRAPHical_Learner
{
    public partial class SfmlPanel : UserControl
    {
        public RenderWindow renderWindow;
        public SfmlPanel()
        {
            InitializeComponent();
            renderWindow = new RenderWindow(this.Handle);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }
    }
}
