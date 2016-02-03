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

            goodButton = new UiButton("Кликни ме", GraphicScheme.font1, 100, 25);
            goodButton.box.Left = 10;
            goodButton.box.Top = 5;
            goodButton.ComponentClicked += goodButton_ComponentClicked;

            UiButton badButton = new UiButton("Не ме кликай", GraphicScheme.font1, 100, 25);
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

            dbgLabel1 = new UiLabel("<not set>", GraphicScheme.font1);
            dbgLabel1.box.Top = 25;
            dbgLabel1.box.Left = 5;

            gui.Add(dbgLabel1);

            //menu.movable = true;
        }

        UiLabel dbgLabel1;
        UiButton goodButton;
    }
}
