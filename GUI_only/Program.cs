using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRAPHical_Learner;

namespace GUI_only
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        { // само се обяснява
            GraphicScheme.LoadFont();
            MainUI ui = new MainUI();
        }
    }
}
