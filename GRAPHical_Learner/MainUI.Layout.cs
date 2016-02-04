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

            UiPanel testPanel = new UiPanel(GraphicScheme.uiBackgroundColor, 120, 350);
            testPanel.box.Left = 300;
            testPanel.box.Top = 200;
            testPanel.children = new List<UiComponent>();

            UiPanel dragPanel = new UiPanel(GraphicScheme.uiBackgroundColor, 100, 15);
            dragPanel.box.Left = 10;
            dragPanel.box.Top = 5;
            dragPanel.movable = true;
            
            goodButton = new UiButton("Кликни ме", GraphicScheme.font1, 100, 25);
            goodButton.box.Left = 10;
            goodButton.box.Top = 25;
            goodButton.ComponentClicked += goodButton_ComponentClicked;

            UiButton badButton = new UiButton("Не ме кликай", GraphicScheme.font1, 100, 25);
            badButton.box.Left = 10;
            badButton.box.Top = 55;
            badButton.ComponentClicked += badButton_ComponentClicked;

            testPanel.AddChild(dragPanel);
            testPanel.AddChild(goodButton);
            testPanel.AddChild(badButton);

            gui.Add(testPanel);

            menu = new UiHorizontalMenu(1024);

            menu.AddItem("Add", menu_add);
            menu.AddItem("Close", menu_close);
            menu.AddItem("Remove last button", menu_remove);

            gui.Add(menu);

            dbgLabel1 = new UiLabel("Все още няма нужда от мен за дебъг!", GraphicScheme.font1);
            dbgLabel1.box.Top = 25;
            dbgLabel1.box.Left = 5;

            dbgLabel2 = new UiLabel("Все още няма нужда от мен за дебъг!", GraphicScheme.font1);
            dbgLabel2.box.Top = 45;
            dbgLabel2.box.Left = 5;

            dbgLabel3 = new UiLabel("Все още няма нужда от мен за дебъг!", GraphicScheme.font1);
            dbgLabel3.box.Top = 65;
            dbgLabel3.box.Left = 5;

            gui.Add(dbgLabel1);
            gui.Add(dbgLabel2);
            gui.Add(dbgLabel3);

            //menu.movable = true;
        }

        UiLabel dbgLabel1;
        UiLabel dbgLabel2;
        UiLabel dbgLabel3;
        UiButton goodButton;
    }
}
