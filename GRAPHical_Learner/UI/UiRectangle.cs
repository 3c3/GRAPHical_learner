using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;

namespace GRAPHical_Learner
{
    public class UiRectangle/* : Drawable*/
    {
        private SFML.Graphics.Vertex[] vertices = new SFML.Graphics.Vertex[4];
        private Color backgroundColor;

        private int x, y;
        private int width, height;

        #region Свойства и ъпдейтването им

        public int X
        {
            get { return x; }
            set { x = value; updateX(); }
        }

        public int Y
        {
            get { return y; }
            set { y = value; updateY(); }
        }

        public int Width
        {
            get { return width; }
            set { width = value; updateWidth(); }
        }

        public int Height
        {
            get { return height; }
            set { height = value; updateHeight(); }
        }

        public Color FillColor
        {
            get { return backgroundColor; }
            set { backgroundColor = FillColor; UpdateColor(); }
        }

        private void UpdateColor()
        {
            vertices[0].Color = backgroundColor;
            vertices[1].Color = backgroundColor;
            vertices[2].Color = backgroundColor;
            vertices[3].Color = backgroundColor;
        }

        private void updateX()
        {
            vertices[0].Position.X = x;
            vertices[1].Position.X = x + width;
            vertices[2].Position.X = x + width;
            vertices[3].Position.X = x;
        }

        private void updateY()
        {
            vertices[0].Position.Y = y;
            vertices[1].Position.Y = y;
            vertices[2].Position.Y = y + height;
            vertices[3].Position.Y = y + height;
        }

        private void updateWidth()
        {
            vertices[1].Position.X = x + width;
            vertices[2].Position.X = x + width;
        }

        private void updateHeight()
        {
            vertices[2].Position.Y = y + height;
            vertices[3].Position.Y = y + height;
        }

        #endregion

        public UiRectangle(Color color, int width, int height)
        {
            FillColor = color;
            Width = width;
            Height = height;
        }

       /* public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform.
            target.Draw(vertices, PrimitiveType.Quads, states);
        }*/
    }
}
