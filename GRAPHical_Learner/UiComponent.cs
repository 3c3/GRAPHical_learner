﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GRAPHical_Learner
{
    public delegate void ComponentClickedHandler();

    public class UiComponent : IMovable
    {
        public IntRect box; // правоъгълните граници на компонента
        public bool visible = false; // при true компонента се рисува
        public bool movable = false; // при true компонента може да се влачи с мишката
        public List<UiComponent> children;

        public UiComponent parent;
        
        public UiComponent()
        {
            box = new IntRect();
        }

        private UiComponent lastMoused;

        /// <summary>
        /// Обработва позицията на мишката в компонента. Предава на децата, ако е необходимо
        /// </summary>
        public bool ProcessMousePosition(Vector2i mousePos)
        {
            Vector2i localPos = new Vector2i(mousePos.X - box.Left, mousePos.Y - box.Top); // превръща координатите в локални
            if (!CheckMouseEvents(mousePos)) return false;// проверява за влизане/излизане, връща дали мишката е вътрe

            if(children!= null) foreach (UiComponent uic in children)
            { // предава позицията на мишката на децата
                if(uic.ProcessMousePosition(localPos))
                { // мишката е върху текущия под-компонент; уведомяваме предния такъв(ако има), че мишката е излязла от него
                    if (lastMoused != null) if (lastMoused != uic) lastMoused.CallMouseLeave(); 
                    lastMoused = uic;
                    return true;
                }
            }
            lastMoused = null;
            return true;
        }

        public bool mouseIn;

        /// <summary>
        /// Проверява дали трябва да се изтрелват event-и
        /// </summary>
        /// <param name="mousePos">Позицията на мишката спрямо елемнта</param>
        /// <returns></returns>
        public bool CheckMouseEvents(Vector2i mousePos)
        {
            Vector2i localPos = new Vector2i(mousePos.X - box.Left, mousePos.Y - box.Top);

            bool oldState = mouseIn;
            mouseIn = IsPointInside(localPos);

            if (oldState ^ mouseIn)
            {
                if (mouseIn) OnMouseEnter();
                else OnMouseLeave();
            }

            return mouseIn;
        }
        
        /// <summary>
        /// Извиква event-а за кликване. Ползва се само за класове, които нямат родители
        /// </summary>
        public void MouseClick(Vector2i mousePos)
        {
            Vector2i localPos;
            localPos = new Vector2i(mousePos.X, mousePos.Y);

            localPos.X -= box.Left;
            localPos.Y -= box.Top;

            if (!IsPointInside(localPos)) return;

            if (ComponentClicked != null) ComponentClicked();
            if (children != null)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    UiComponent uic = children[i];
                    uic.MouseClick(localPos);
                }
            }
        }

        public bool IsPointInside(Vector2i pos)
        {
            if (pos.X < 0) return false;
            else if (pos.X > box.Width) return false;
            else if (pos.Y < 0) return false;
            else if (pos.Y > box.Height) return false;
            else return true;
        }

        protected virtual void OnMouseEnter()
        { }

        protected virtual void OnMouseLeave() 
        {
            if (children != null) children.ForEach(u => u.CallMouseLeave());
        }

        public void CallMouseLeave()
        {
            mouseIn = false;
            OnMouseLeave();
        }

        public event ComponentClickedHandler ComponentClicked;

        public virtual void AddChild(UiComponent uic)
        {
            children.Add(uic);
            uic.parent = this;
        }

        public void Remove()
        {
            if(Gui.activeGui != null)
            {
                Gui.activeGui.Remove(this);
            }
        }

        public virtual List<Drawable> GetUiDrawables()
        {
            return new List<Drawable>();
        }

        public void MoveX(float dx)
        {
            if (movable) box.Left += (int)dx;
        }

        public void MoveY(float dy)
        {
            if (movable) box.Top += (int)dy;
        }
    }
}
