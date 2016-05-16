using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    public partial class MainUI
    {
        UiLabel dbgLabel1; // ползват се при нужда от debug на интерфейса или 
        UiLabel dbgLabel2; // други неща
        UiLabel dbgLabel3;

        internal UiLabel edgeInputLabel;

        /// <summary>
        /// Менюто, появяващо се при дясно кликане
        /// </summary>
        UiVerticalMenu rmbMenu;
        UiVerticalMenu fileMenu, arrangeMenu;

        UiVertexMenu activeVerticalMenu;

        /// <summary>
        /// Горното меню
        /// </summary>
        UiHorizontalMenu menu;

        /// <summary>
        /// Панела със свойствата на върхове и ребра
        /// </summary>
        UiPropertyPanel propertyPanel;

        /// <summary>
        /// Долния панел
        /// </summary>
        UiHorizontalMenu algoControlMenu;

        /// <summary>
        /// Бутони, на които трябва да им се сменя текста
        /// </summary>
        UiButton physBtn, edgeBtn, directivityBtn, weightBtn;
        
        /// <summary>
        /// Слага всичките неща в Gui-то
        /// </summary>
        public void InitializeGui()
        {
            gui = new Gui(window);

            fileMenu = new UiVerticalMenu(true);
            fileMenu.AddItem("Зареди", menu_FileOpen);
            fileMenu.AddItem("Запази", menu_FileSave);

            fileMenu.X = 0;
            fileMenu.Y = 20;

            fileMenu.visible = false;

            gui.Add(fileMenu);

            arrangeMenu = new UiVerticalMenu(true);
            arrangeMenu.AddItem("В кръг", menu_Circle);
            arrangeMenu.AddItem("Центрирай", BtnCenterGraph);
            arrangeMenu.AddItem("Разбъракно", menu_Shuffle);

            arrangeMenu.X = 50;
            arrangeMenu.Y = 20;

            arrangeMenu.visible = false;

            gui.Add(arrangeMenu);

            menu = new UiHorizontalMenu(1024);

            menu.AddItem("Файл", menu_FileClicked);
            menu.AddItem("Подредба", menu_ArrangeClicked);
            edgeBtn = menu.AddItem("Добави ребра(включи)", BtnAddEdgeToggle);
            physBtn = menu.AddItem("Физика(включи)", BtnPhysToggle);
            menu.AddItem("Създай граф", menu_Generate);
            menu.AddItem("Изчисти", menu_Clear);
            directivityBtn = menu.AddItem("Насочен(не)", menu_Directivity);
            weightBtn = menu.AddItem("Претеглен(не)", menu_Weight);

            gui.Add(menu);

            #region Debug текстове

            dbgLabel1 = new UiLabel("Все още няма нужда от мен за дебъг!", GraphicScheme.font1);
            dbgLabel1.visible = false;
            dbgLabel1.Y = 25;
            dbgLabel1.X = 5;

            dbgLabel2 = new UiLabel("Все още няма нужда от мен за дебъг!", GraphicScheme.font1);
            dbgLabel2.visible = false;
            dbgLabel2.Y = 45;
            dbgLabel2.X = 5;

            dbgLabel3 = new UiLabel("Все още няма нужда от мен за дебъг!", GraphicScheme.font1);
            dbgLabel3.visible = false;
            dbgLabel3.Y = 65;
            dbgLabel3.X = 5;

            gui.Add(dbgLabel1);
            gui.Add(dbgLabel2);
            gui.Add(dbgLabel3);

            #endregion

            edgeInputLabel = new UiLabel("Въведете тегло на ребро(приключване с Enter или кликване)", GraphicScheme.font1);
            edgeInputLabel.X = 340;
            edgeInputLabel.Y = 50;
            edgeInputLabel.visible = false;
            gui.Add(edgeInputLabel);

            rmbMenu = new UiVerticalMenu();
            rmbMenu.visible = false;
            rmbMenu.AddItem("Добави връх", rmbMenu_AddVertex);
            rmbMenu.AddItem("Демаркирай", rmbMenu_Deselect);
            
            gui.Add(rmbMenu);
            //menu.movable = true;

            propertyPanel = new UiPropertyPanel();
            propertyPanel.X = 0;
            propertyPanel.Y = 500;

            gui.Add(propertyPanel);

            if (connector != null)
            { // менюто за контрол на алгоритъм се показва само когато има такъв
                algoControlMenu = new UiHorizontalMenu(200);

                algoControlMenu.X = 412;
                algoControlMenu.Y = 575;

                algoControlMenu.AddItem("Стъпка", BtnSingleStep);
                algoControlMenu.AddItem("Автоматично", MenuBtnPlay);
                algoControlMenu.AddItem("Пауза", MenuBtnPause);

                gui.Add(algoControlMenu);
            }

            
        }

    }
}
