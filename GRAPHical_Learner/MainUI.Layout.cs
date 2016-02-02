using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    public partial class MainUI
    {
        /// <summary>
        /// Слага всичките неща в Gui-то
        /// </summary>
        public void InitializeGui()
        {
            gui = new Gui();
            Gui.activeGui = gui;

            UiPanel testPanel = new UiPanel(GraphicScheme.uiBackgroundColor, 120, 350);
            testPanel.movable = true;
            testPanel.box.Left = 300;
            testPanel.children = new List<UiComponent>();

            UiButton goodButton = new UiButton("Кликни ме", font1, 100, 25);
            goodButton.box.Left = 10;
            goodButton.box.Top = 5;
            goodButton.ComponentClicked += goodButton_ComponentClicked;

            UiButton badButton = new UiButton("Не ме кликай", font1, 100, 25);
            badButton.box.Left = 10;
            badButton.box.Top = 35;
            badButton.ComponentClicked += badButton_ComponentClicked;

            testPanel.AddChild(goodButton);
            testPanel.AddChild(badButton);

            gui.Add(testPanel);

            menu = new UiHorizontalMenu(1024);

            menu.AddItem("Add", menu_add);
            menu.AddItem("Close", menu_close);
            menu.AddItem("Remove last button", menu_remove);

            gui.Add(menu);

            //menu.movable = true;
        }
    }
}
