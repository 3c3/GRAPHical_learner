﻿using System;
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

        /// <summary>
        /// Менюто, появяващо се при дясно кликане
        /// </summary>
        UiVerticalMenu rmbMenu;
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
        UiButton physBtn, edgeBtn;
        
        /// <summary>
        /// Слага всичките неща в Gui-то
        /// </summary>
        public void InitializeGui()
        {
            gui = new Gui(window);

            menu = new UiHorizontalMenu(1024);

            menu.AddItem("Зареди", menu_FileOpen);
            menu.AddItem("Запази", menu_FileSave);
            menu.AddItem("Случаен", menu_Generate);
            menu.AddItem("Разбъркай", menu_Shuffle);
            menu.AddItem("В кръг", menu_Circle);
            menu.AddItem("Изчисти", menu_Clear);
            edgeBtn = menu.AddItem("Добавяне на ребра: включено", BtnAddEdgeToggle);

            gui.Add(menu);

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

            UiVerticalMenu vmenu = new UiVerticalMenu();
            physBtn = vmenu.AddItem("Включи физика", BtnPhysToggle);
            vmenu.AddItem("Центрирай", BtnCenterGraph);
            
            vmenu.X = 0;
            vmenu.Y = 20;

            gui.Add(vmenu);

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
