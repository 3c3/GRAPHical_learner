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
    /// <summary>
    /// Евент за кликване
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="arg"></param>
    public delegate void ComponentClickedHandler(UiComponent sender, Object arg);

    /// <summary>
    /// Основен клас за компоненти на интерфейса
    /// </summary>
    public class UiComponent : IMovable
    {
        static int count = 0;

        public readonly int id;

        protected IntRect box; // правоъгълните граници на компонента

        public int X
        {
            get { return box.Left; }
            set { box.Left = value; OnComponentMoved(); }
        }

        public int Y
        {
            get { return box.Top; }
            set { box.Top = value; OnComponentMoved(); }
        }

        public int Width
        {
            get { return box.Width; }
            set { box.Width = value; OnComponentResized(); }
        }

        public int Height
        {
            get { return box.Height; }
            set { box.Height = value; OnComponentResized(); }
        }

        public bool visible = false; // при true компонента се рисува
        public bool movable = false; // при true компонента може да се влачи с мишката
        public List<UiComponent> children; // всеки компонент си има деца, така се образува дървовидна йерархия

        public Gui gui; // елементите имат референция към съдържащия ги интерфейс, за да могат да се самоизтрият, и някои други неща
        public UiComponent parent;
        
        public UiComponent()
        {
            id = count++;
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

            gui.lastChildMoused = this;

            if(children!= null) foreach (UiComponent uic in children)
            { // предава позицията на мишката на децата
                if(uic.ProcessMousePosition(localPos))
                { // мишката е върху текущия под-компонент; уведомяваме предния такъв(ако има), че мишката е излязла от него
                    if (lastMoused != null) if (lastMoused != uic) lastMoused.CallMouseLeave(); 
                    lastMoused = uic;
                    FinalizeLastChildMoused();
                    return true;
                }
            }
            lastMoused = null;
            gui.lastChildMoused = this;
            return true;
        }

        /// <summary>
        /// Определя дали дадения елемент ще е последен при посочване с мишката
        /// </summary>
        protected virtual void FinalizeLastChildMoused()
        {
            return;
        }

        private bool mouseIn;

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

        protected virtual void fireClickEvent()
        {
            if (ComponentClicked != null) ComponentClicked(this, id);
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

            fireClickEvent();
            if (children != null)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    UiComponent uic = children[i];
                    uic.MouseClick(localPos);
                }
            }

            if (autoClose) visible = false;
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

        protected bool autoClose = false;

        protected virtual void OnMouseLeave() 
        {
            if (autoClose) visible = false;
            if (children != null) children.ForEach(u => u.CallMouseLeave());
        }

        protected virtual void OnComponentResized()
        { }

        protected virtual void OnComponentMoved()
        {
        }

        public void CallMouseLeave()
        {
            mouseIn = false;
            OnMouseLeave();
        }

        public event ComponentClickedHandler ComponentClicked;

        /// <summary>
        /// Добавя компонент-дете
        /// </summary>
        /// <param name="uic"></param>
        public virtual void AddChild(UiComponent uic)
        {
            uic.parent = this;
            uic.gui = gui;
            uic.UpdateChildrenGuiReference();
            children.Add(uic);
        }

        public void Remove()
        {
            if(gui != null)
            {
                gui.Remove(this);
            }
        }

        public void UpdateChildrenGuiReference()
        {
            if (parent != null) gui = parent.gui;
            if (children != null) children.ForEach(c => c.UpdateChildrenGuiReference());
        }

        public virtual void Draw(int relX, int relY)
        {
            return;
        }

        /// <summary>
        /// Предава движението нагоре
        /// </summary>
        /// <param name="dx"></param>
        private void ForceMoveX(float dx)
        {
            if (parent == null) box.Left += (int)dx;
            else parent.ForceMoveX(dx);
        }

        /// <summary>
        /// Предава движението нагоре
        /// </summary>
        /// <param name="dy"></param>
        private void ForceMoveY(float dy)
        {
            if (parent == null) box.Top += (int)dy;
            else parent.ForceMoveY(dy);
        }

        public void MoveX(float dx)
        {
            if (movable) ForceMoveX(dx);
        }

        public void MoveY(float dy)
        {
            if (movable) ForceMoveY(dy);
        }
    }
}
