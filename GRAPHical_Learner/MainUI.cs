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
    /// <summary>
    /// Основният интерфейс на програмата
    /// </summary>
    public partial class MainUI
    {
        private RenderWindow window;
        private static Graph activeGraph;

        private bool weighted = false;
        internal Property inputWeight;
        internal Edge inputEdge;

        private Connector connector;

        private RenderFrame renderFrame = new RenderFrame(); // рамката, използвана за чертане

        List<Circle> circles = new List<Circle>();
        Gui gui;

        Timer algoTimer;
        bool timerEnabled = false;
        bool addEdgeEnabled = false;

        public MainUI(Connector con)
        {
            connector = con;
            activeGraph = connector.GraphInstance;
            activeGraph.ArrangeInCircle();

            //DumpProperties();

            algoTimer = new Timer(150); // при създаване с конектор(т.е. алгоритъм), се създава и таймера за автоматичен
            algoTimer.Elapsed += algoTimer_Elapsed; // ход
            connector.AlgorithmSuspended += connector_AlgorithmSuspended;
            Start();
        }

        void DumpProperties()
        {
            foreach(Vertex v in activeGraph.vertices)
            {
                Console.WriteLine("Vertex " + v.id.ToString());
                foreach(Property p in v.properties)
                {
                    Console.WriteLine(String.Format("Name: {0}, Value: {1}", p.Name, p.Value != null ? p.Value : "null"));
                }
                Console.WriteLine();
            }
        }

        void algoTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            connector.Resume();
            algoTimer.Stop();
        }

        /// <summary>
        /// Извиква се, когато алгоритъма удари Pause()
        /// </summary>
        void connector_AlgorithmSuspended()
        {
            if(timerEnabled) algoTimer.Start();
        }

        public MainUI()
        {
            activeGraph = new Graph(false);
            Start();
        }

        /// <summary>
        /// Създава първоначален граф
        /// </summary>
        void MakeGraph()
        {
            // ИЗВЪН ЕКСПЛОАТАЦИЯ
            //Property.GetPropertyId("цвят")
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

            Edge e1 = activeGraph.AddEdge(0, 3);
            e1.SetProperty(2, 200);

        }

        IForceSimulator fs;

        private void Start()
        {
            //SFML неща
            ContextSettings cs = new ContextSettings();
            cs.AntialiasingLevel = 8;

            window = new RenderWindow(new VideoMode(renderFrame.width, renderFrame.height), "GRAPHical Learner", Styles.Titlebar | Styles.Close, cs);
            window.Closed += window_Closed;
            window.MouseWheelMoved += window_MouseWheelMoved;
            window.KeyPressed += window_KeyPressed;
            window.MouseButtonPressed += window_MouseButtonPressed;
            window.MouseButtonReleased += window_MouseButtonReleased;

            window.SetVerticalSyncEnabled(true);

            //Алгоритъм за подреждане
            fs = new ForceSimulatorMKIIB(1.5, 0.8);
            fs.SetGraph(activeGraph);
            fs.SimulatorStopped += fs_SimulatorStopped;

            //Изчислява се приближението на рамката
            renderFrame.CalcZoom();

            //Пуска се GUI-то
            InitializeGui();
            if (connector != null) EnablePhysics(); // ако е вързан алгоритъм, графа се подрежда автоматично
            Loop();
        }
 
        // показва дали физиката(алгоритъма за подреждане) трябва да работи
        bool physics = false;

        int cnt = 0;

        /// <summary>
        /// Стандартен цикъл 
        /// </summary>
        void Loop()
        {
            while(window.IsOpen)
            {
                window.DispatchEvents();

                if (connector != null && connector.pollProperties) GetProperties();

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

        /// <summary>
        /// Обновява всички свойства
        /// </summary>
        void GetProperties()
        {
            foreach(Vertex v in activeGraph.vertices)
            {
                foreach(Property p in v.properties)
                {
                    object val = connector.GetVertexProperty(v.id, p.id);
                    if (val == null) continue;
                    p.Value = val;
                }
            }
        }

        Vector2i lastMousePos = new Vector2i();
        IMovable currentObject;
        
        /// <summary>
        /// Намира върха на дадената позиция
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        Vertex GetVertexAt(Vector2f pos)
        {
            if (activeGraph == null) return null;
            foreach(Vertex v in activeGraph.vertices)
            {
                if (v.circle.IsInside(pos)) return v;
            }
            return null;
        }

        Edge GetEdgeAt(Vector2f pos)
        {
            if (activeGraph == null) return null;
            foreach (Edge e in activeGraph.edges)
            {
                if (e.HitCheck(pos)) return e;
            }
            return null;
        }

        /// <summary>
        /// Обработва позицията на мишката
        /// </summary>
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
                if (currentObject == null) currentObject = GetVertexAt(ToGlobalCoords(mousePos));
                if (currentObject == null) currentObject = renderFrame;
            }
            else  currentObject = null;

            Vertex moused = GetVertexAt(ToGlobalCoords(mousePos));
            propertyPanel.Holder = moused;

            Edge e = GetEdgeAt(ToGlobalCoords(mousePos));
            if (moused == null) propertyPanel.Holder = e;

            gui.ProcessMousePosition(mousePos);
            lastMousePos = mousePos;
        }

        /// <summary>
        /// Рисува координатни оси, не се използва
        /// </summary>
        void DrawAxes()
        {
            SFML.Graphics.Vertex[] vertLine = { new SFML.Graphics.Vertex(new Vector2f(512, 0), new Color(255, 255, 255, 127)), 
                                               new SFML.Graphics.Vertex(new Vector2f(512, 600), new Color(255, 255, 255, 127))};

            SFML.Graphics.Vertex[] horrLine = { new SFML.Graphics.Vertex(new Vector2f(0, 300), new Color(255, 255, 255, 127)), 
                                               new SFML.Graphics.Vertex(new Vector2f(1024, 300), new Color(255, 255, 255, 127))};

            window.Draw(vertLine, PrimitiveType.Lines);
            window.Draw(horrLine, PrimitiveType.Lines);
        }

        Color grey = new Color(33, 33, 33);

        /// <summary>
        /// Рисува всичко
        /// </summary>
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

        /// <summary>
        /// Превръща от локални в глобални координати
        /// </summary>
        /// <param name="screenCoords"></param>
        /// <returns></returns>
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
