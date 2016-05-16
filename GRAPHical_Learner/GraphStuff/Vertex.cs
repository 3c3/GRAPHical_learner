using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    /// <summary>
    /// Представлява връх във граф
    /// </summary>
    public class Vertex : PropertyHolder, IMovable
    {
        private static int idCounter = 0;
        public readonly int id;
        private ScalableLabel idLabel;

        private Property markedProperty, colorProperty, visibleProperty;

        public List<Edge> edges = new List<Edge>();

        public float x, y;
        public PVector velocity = new PVector(); // ползва се от алгоритмите за подреждане

        // основното кръгче и това за селекция
        public Circle circle, selectionCircle;

        public bool selected; // маркирано от потребителя

        private bool marked; // маркирано от алгоритъма
        public bool Marked
        {
            get { return marked; }
            set
            {
                if(value)
                {
                    circle.color = GraphicScheme.vertexMarked;
                    marked = true;
                }
                else
                {
                    circle.color = GraphicScheme.vertexNormal;
                    marked = false;
                }
            }
        }

        private bool visible;

        public bool Visible
        {
            get { return visible; }
        }

        public Vertex()
        {
            id = idCounter++;
            idLabel = new ScalableLabel(id.ToString());

            circle = new Circle(0, 0, 20);
            selectionCircle = new Circle(0, 0, 25, Color.Cyan);
            x = 0;
            y = 0;
        }

        public Vertex(float x, float y)
        {
            id = idCounter++;
            idLabel = new ScalableLabel(id.ToString());

            circle = new Circle(x, y, 20);
            selectionCircle = new Circle(0, 0, 25, Color.Cyan);
            this.x = x;
            this.y = y;
        }

        public override string GetName()
        {
            return String.Format("Връх {0}", id);
        }

        public override void SetProperty(int propertyId, object value)
        {
            //Console.WriteLine("setting property {0} on {1}", propertyId, id);
            base.SetProperty(propertyId, value);
            if(markedProperty == null && propertyId == Property.UsedId)
            {
                markedProperty = properties.Last();
                //Console.WriteLine(String.Format("Color property is set and it is {0}", colorProperty.Value.GetType()));
            }
            if (propertyId == Property.ColorId)
            {
                if(colorProperty == null) colorProperty = properties.Last();
                UpdateColor((Color3b)colorProperty.Value);
            }

            if (visibleProperty == null && propertyId == Property.VisibleId) visibleProperty = properties.Last();
        }

        Color3b currentColor;

        private void UpdateColor(Color3b c3b)
        {
            currentColor = c3b;
            circle.SetColor3b(c3b);
        }

        private bool IsVisible()
        {
            if (visibleProperty == null) return true;
            if (visibleProperty.Value == null) return true;
            if ((visibleProperty.Value is bool) == false) return true;

            return (bool)visibleProperty.Value;
        }

        private void CheckMarkedProperty()
        {
            if (markedProperty == null || markedProperty.Value == null) return;

            bool markedVal = false;
            try
            {
                markedVal = (bool)markedProperty.Value;
            }
            catch(Exception e)
            {
                Console.WriteLine(String.Format("Couldn't read marked! Exception: {0}", e.Message));
            }
            
            if (markedVal == marked) return; // вече е със същата маркираност
            if(markedVal)
            {
                circle.color = GraphicScheme.vertexMarked;
                marked = true;
            }
            else
            {
               circle.color = GraphicScheme.vertexNormal;
               marked = false;
            }
        }

        public void DrawSelf(RenderTarget window, RenderFrame rf)
        {
            visible = IsVisible();
            if (!visible) return;

            if(currentColor==null) CheckMarkedProperty();

            circle.center.X = x;
            circle.center.Y = y;

            selectionCircle.center.X = x;
            selectionCircle.center.Y = y;

            List<Drawable> ld = new List<Drawable>();
            if (selected) ld.AddRange(selectionCircle.getDrawables(rf));
            ld.AddRange(circle.getDrawables(rf));
            ld.ForEach(d => window.Draw(d));

            idLabel.DrawSelf(window, rf, x, y);
        }

        public void MoveX(float dx)
        {
            x += dx;
            velocity.x = 0;
            velocity.y = 0;
        }

        public void MoveY(float dy)
        {
            y += dy;
            velocity.x = 0;
            velocity.y = 0;
        }

        public static void ResetCounter()
        {
            idCounter = 0;
        }
    }
}
