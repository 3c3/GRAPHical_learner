using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace GRAPHical_Learner
{
    public partial class MainUI
    {
        private RenderWindow window;
        private static Graph activeGraph;

        private RenderFrame renderFrame = new RenderFrame();

        List<Circle> circles = new List<Circle>();
        Gui gui;
        Font font1;

        public MainUI()
        {
            start();
        }

        void MakeGraph()
        {
            activeGraph = new Graph();
            activeGraph.vertices.Add(new Vertex(0, 0));
            /*activeGraph.vertices.Add(new Vertex(0, 0));
            activeGraph.vertices.Add(new Vertex(0, 100));
            activeGraph.vertices.Add(new Vertex(1, 2));

            activeGraph.vertices.Add(new Vertex(200, 20));
            activeGraph.vertices.Add(new Vertex(200, 10));
            activeGraph.vertices.Add(new Vertex(200, 0));
            activeGraph.vertices.Add(new Vertex(200, -50));

            activeGraph.AddEdge(0, 1);
            activeGraph.AddEdge(0, 2);
            //activeGraph.AddEdge(1, 2);
            activeGraph.AddEdge(0, 3);

            activeGraph.AddEdge(4, 5);
            activeGraph.AddEdge(4, 6);
            activeGraph.AddEdge(4, 7);
            activeGraph.AddEdge(0, 4);*/
        }

        void genCircles()
        {
            Random r = new Random();
            for(int i = 0; i < 10; i++)
            {
                Circle c = new Circle((float)r.NextDouble() * 1024.0f - 512.0f, (float)r.NextDouble() * 600.0f - 300.0f, 20);
                circles.Add(c);
            }

            Circle origin = new Circle(-512.0f, -300.0f, 20, Color.Green);
            circles.Add(origin);
            Circle origin2 = new Circle(512.0f, 300.0f, 20, Color.Yellow);
            circles.Add(origin2);

            Circle center = new Circle(0, 0, 20, Color.Cyan);
            circles.Add(center);
        }

        UiVerticalMenu menu;
        IForceSimulator fs;

        private void start()
        {
            ContextSettings cs = new ContextSettings();
            cs.AntialiasingLevel = 8;

            window = new RenderWindow(new VideoMode(renderFrame.width, renderFrame.height), "GRAPHical Learner", Styles.Titlebar | Styles.Close, cs);
            window.Closed += window_Closed;
            window.MouseWheelMoved += window_MouseWheelMoved;
            window.KeyPressed += window_KeyPressed;
            window.MouseButtonPressed += window_MouseButtonPressed;
            window.MouseButtonReleased += window_MouseButtonReleased;

            window.SetVerticalSyncEnabled(true);

            font1 = new Font("Ubuntu-R.ttf");

            //genCircles();
            MakeGraph();

            fs = new ForceSimulatorMkII(2.5f, 0.17f);
            fs.SetGraph(activeGraph);

            renderFrame.calcZoom();

            InitializeGui();

            loop();
        }

        bool physics = false;

        int cnt = 0;

        void loop()
        {
            while(window.IsOpen)
            {
                window.DispatchEvents();
                
                processMouse();

                cnt++;

                if (physics && cnt >= 0)
                {
                    cnt = 0;
                    fs.SimulateStep();
                }

                draw();
            }
        }

        Vector2i lastMousePos = new Vector2i();
        IMovable currentObject;
        
        Vertex getCircleAt(Vector2f pos)
        {
            if (activeGraph == null) return null;
            foreach(Vertex v in activeGraph.vertices)
            {
                if (v.circle.isInside(pos)) return v;
            }
            return null;
        }

        void processMouse()
        {
            dbgLabel3.SetText(currentObject != null ? currentObject.GetType().ToString() : "no IMovable");

            Vector2i mousePos = Mouse.GetPosition(window);

            float dx = mousePos.X - lastMousePos.X;
            float dy = mousePos.Y - lastMousePos.Y;

            if (lmbDown)
            {
                if (currentObject != null)
                {
                    if (currentObject is UiComponent)
                    {
                        currentObject.MoveX(dx);
                        currentObject.MoveY(dy);
                    }
                    else
                    {
                        dy *= -1;

                        dx /= renderFrame.zoom;
                        dy /= renderFrame.zoom;

                        currentObject.MoveX(dx);
                        currentObject.MoveY(dy);
                    }
                } 
                if (currentObject == null) currentObject = gui.lastChildMoused;
                if (currentObject == null) currentObject = getCircleAt(toGlobalCoords(mousePos));
                if (currentObject == null) currentObject = renderFrame;
            }
            else  currentObject = null;

            gui.ProcessMousePosition(mousePos);
            lastMousePos = mousePos;
        }

        void drawAxes()
        {
            SFML.Graphics.Vertex[] vertLine = { new SFML.Graphics.Vertex(new Vector2f(512, 0), new Color(255, 255, 255, 127)), 
                                               new SFML.Graphics.Vertex(new Vector2f(512, 600), new Color(255, 255, 255, 127))};

            SFML.Graphics.Vertex[] horrLine = { new SFML.Graphics.Vertex(new Vector2f(0, 300), new Color(255, 255, 255, 127)), 
                                               new SFML.Graphics.Vertex(new Vector2f(1024, 300), new Color(255, 255, 255, 127))};

            window.Draw(vertLine, PrimitiveType.Lines);
            window.Draw(horrLine, PrimitiveType.Lines);
        }

        void draw()
        {
            window.Clear(Color.Black);

            /*List<Drawable> draws = new List<Drawable>();
            foreach (Circle c in circles)
            {
                List<Drawable> elements = c.getDrawables(renderFrame);
                foreach (Drawable d in elements) draws.Add(d);
            }

            draws.ForEach(d => window.Draw(d));*/
            if (activeGraph != null) activeGraph.DrawSelf(window, renderFrame);

            drawAxes();

            dbgLabel1.SetText(gui.lastChildMoused != null ? gui.lastChildMoused.GetType().ToString() : "null");
            dbgLabel2.SetText(gui.MousedComponent != null ? gui.MousedComponent.GetType().ToString() : "null");

            gui.Draw(window);

            /*Vector2i mousePos = Mouse.GetPosition(window);

            RectangleShape rs = new RectangleShape(new Vector2f(5, 5));
            rs.FillColor = new Color(255, 0, 0, 150);
            rs.Position = new Vector2f(mousePos.X - 2, mousePos.Y - 2);*/

            //window.Draw(rs);

            window.Display();
        }

        Vector2f toGlobalCoords(Vector2i screenCoords)
        {
            Vector2f result = new Vector2f(screenCoords.X, screenCoords.Y);

            // от екранни към нетрансформирани глобални
            result.X -= renderFrame.width / 2.0f;
            result.Y = renderFrame.height / 2.0f - result.Y;

            // връщаме мащаба
            result.X /= renderFrame.zoom;
            result.Y /= renderFrame.zoom;

            // връщаме транслацията
            result.X += renderFrame.xCenter;
            result.Y += renderFrame.yCenter;

            return result;
        }

        void window_Closed(object sender, EventArgs e)
        {
            window.Close();
        }
    }
}
