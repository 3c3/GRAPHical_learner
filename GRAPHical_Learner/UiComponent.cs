using System;
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

    public class UiComponent : IDrawable, IMovable
    {
        public IntRect box; // правоъгълните граници на компонента
        public bool visible = false;
        public bool movable = false;
        public List<UiComponent> children;

        public UiComponent parent;
        
        public UiComponent()
        {
            box = new IntRect();
        }

        private UiComponent currentMoused;

        /// <summary>
        /// Обработва позицията на мишката в компонента. Предава на децата, ако е необходимо
        /// </summary>
        public bool ProcessMousePosition(Vector2i mousePos)
        {
            Vector2i localPos = new Vector2i(mousePos.X - box.Left, mousePos.Y - box.Top);
            bool result = checkMouseEvents(mousePos);

            if (!result) return false;

            result = false;
            //children.ForEach(c => c.isMouseIn(localPos));
            if(children!= null) foreach (UiComponent uic in children)
            {
                if(uic.ProcessMousePosition(localPos))
                {
                    currentMoused = uic;
                    //Console.Write(String.Format("<{0}> ", GetType()));
                    //Console.WriteLine("currentMoused is " + currentMoused.GetType().ToString());
                    result = true;
                }
            }
            if(!result) currentMoused = null;
            return true;
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

            if (!isPointInside(localPos)) return;

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

        private bool mouseIn;

        public Vector2i toLocalCoords(Vector2i pos)
        {
            return new Vector2i(pos.X - box.Left, pos.Y - box.Top);
        }

        /// <summary>
        /// Проверява дали трябва да се изтрелват event-и
        /// </summary>
        /// <param name="mousePos">Позицията на мишката спрямо елемнта</param>
        /// <returns></returns>
        public bool checkMouseEvents(Vector2i mousePos)
        {
            Vector2i localPos;
            localPos = new Vector2i(mousePos.X - box.Left, mousePos.Y - box.Top);

            bool old = mouseIn;
            if (localPos.X < 0) mouseIn = false;
            else if (localPos.X > box.Width) mouseIn = false;
            else if (localPos.Y < 0) mouseIn = false;
            else if (localPos.Y > box.Height) mouseIn = false;
            else mouseIn = true;

            if(old ^ mouseIn)
            {
                if (mouseIn) onMouseEnter();
                else onMouseLeave();
            }

            return mouseIn;
        }

        public bool isPointInside(Vector2i pos)
        {
            if (pos.X < 0) return false;
            else if (pos.X > box.Width) return false;
            else if (pos.Y < 0) return false;
            else if (pos.Y > box.Height) return false;
            else return true;
        }

        protected virtual void onMouseEnter()
        { }

        protected virtual void onMouseLeave() 
        {
            if (children != null) children.ForEach(u => u.onMouseLeave());
        }

        public void callMouseLeave()
        {
            onMouseLeave();
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

        public virtual List<Drawable> getDrawables(RenderFrame rf)
        {
            return new List<Drawable>();
        }

        public void moveX(float dx)
        {
            if (movable) box.Left += (int)dx;
        }

        public void moveY(float dy)
        {
            if (movable) box.Top += (int)dy;
        }
    }
}
