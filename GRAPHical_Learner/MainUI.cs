using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using System.Timers;

namespace GRAPHical_Learner
{
    public partial class MainUI
    {
        private RenderWindow window;
        private static Graph activeGraph;

        private Connector connector;

        private RenderFrame renderFrame = new RenderFrame();

        List<Circle> circles = new List<Circle>();
        Gui gui;
        Font font1;

        Timer algoTimer;
        bool timerEnabled = false;
        bool addEdgeEnabled = true;

        public MainUI(Connector con)
        {
            connector = con;
            activeGraph = connector.GraphInstance;
            algoTimer = new Timer(150);
            algoTimer.Elapsed += algoTimer_Elapsed;
            connector.AlgorithmSuspended += connector_AlgorithmSuspended;
            Start();
        }

        void algoTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            connector.Resume();
            algoTimer.Stop();
        }

        void connector_AlgorithmSuspended()
        {
            if(timerEnabled) algoTimer.Start();
        }

        public MainUI()
        {
            activeGraph = new Graph();
            Start();
        }

        void MakeGraph()
        {
            Property.GetPropertyId("име");
            Property.GetPropertyId("цена");
            Property.GetPropertyId("поток");
            Property.GetPropertyId("обратен поток");

            activeGraph = new Graph();
            
            Vertex v = new Vertex(0, 0);
            v.SetProperty(0, "Варна");

            Vertex v1 = new Vertex(100, 0);
            v1.SetProperty(0, "София");
            v1.SetProperty(1, 512.25f);

            Vertex v2 = new Vertex(100, 0);
            v2.SetProperty(0, "Шумен");
            v2.SetProperty(2, 112.125f);

            Vertex v3 = new Vertex(100, 0);
            v3.SetProperty(0, "Стара Загора");
            v3.SetProperty(3, 12.725f);

            activeGraph.AddVertex(v);
            activeGraph.AddVertex(v1);
            activeGraph.AddVertex(v2);
            activeGraph.AddVertex(v3);

        }

        IForceSimulator fs;

        private void Start()
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
           // MakeGraph();

            //fs = new ForceSimulatorMkI(200, 40, 150, 0.98f, 0);
            fs = new ForceSimulatorMKIIB(1.5, 0.8);
            fs.SetGraph(activeGraph);
            fs.SimulatorStopped += fs_SimulatorStopped;

            renderFrame.calcZoom();

            InitializeGui();

            Loop();
        }

        

        bool physics = false;

        int cnt = 0;

        void Loop()
        {
            while(window.IsOpen)
            {
                window.DispatchEvents();

                if (connector != null) GetProperties();

                ProcessMouse();

                cnt++;

                if (physics && cnt >= 0)
                {
                    cnt = 0;
                    fs.SimulateStep();
                }

                Draw();
            }
        }

        void GetProperties()
        {
            foreach(Vertex v in activeGraph.vertices)
            {
                foreach(Property p in v.properties)
                {
                    p.Value = connector.GetVertexProperty(v.id, p.id);
                }
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

        void ProcessMouse()
        {
            dbgLabel3.Text = currentObject != null ? currentObject.GetType().ToString() : "no IMovable";

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
                if (currentObject == null) currentObject = getCircleAt(ToGlobalCoords(mousePos));
                if (currentObject == null) currentObject = renderFrame;
            }
            else  currentObject = null;

            Vertex moused = getCircleAt(ToGlobalCoords(mousePos));
            propertyPanel.Holder = moused;

            gui.ProcessMousePosition(mousePos);
            lastMousePos = mousePos;
        }

        void DrawAxes()
        {
            SFML.Graphics.Vertex[] vertLine = { new SFML.Graphics.Vertex(new Vector2f(512, 0), new Color(255, 255, 255, 127)), 
                                               new SFML.Graphics.Vertex(new Vector2f(512, 600), new Color(255, 255, 255, 127))};

            SFML.Graphics.Vertex[] horrLine = { new SFML.Graphics.Vertex(new Vector2f(0, 300), new Color(255, 255, 255, 127)), 
                                               new SFML.Graphics.Vertex(new Vector2f(1024, 300), new Color(255, 255, 255, 127))};

            window.Draw(vertLine, PrimitiveType.Lines);
            window.Draw(horrLine, PrimitiveType.Lines);
        }

        void Draw()
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

            //drawAxes();

            //dbgLabel1.Text = gui.lastChildMoused != null ? gui.lastChildMoused.GetType().ToString() : "null";
            //dbgLabel2.Тext = gui.MousedComponent != null ? gui.MousedComponent.GetType().ToString() : "null";

            gui.Draw(window);

            /*Vector2i mousePos = Mouse.GetPosition(window);

            RectangleShape rs = new RectangleShape(new Vector2f(5, 5));
            rs.FillColor = new Color(255, 0, 0, 150);
            rs.Position = new Vector2f(mousePos.X - 2, mousePos.Y - 2);*/

            //window.Draw(rs);

            window.Display();
        }

        Vector2f ToGlobalCoords(Vector2i screenCoords)
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
