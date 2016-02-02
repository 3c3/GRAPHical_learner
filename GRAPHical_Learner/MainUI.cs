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
    public class MainUI
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

            genCircles();
            renderFrame.calcZoom();

            gui = new Gui();
            Gui.activeGui = gui;

            UiPanel testPanel = new UiPanel(ColorScheme.uiBackgroundColor, 120, 350);
            testPanel.movable = true;
            testPanel.box.Left = 300;
            testPanel.children = new List<UiComponent>();

            UiButton goodButton = new UiButton("Кликни ме", font1, 100, 25);
            goodButton.box.Left = 10;
            goodButton.box.Top = 5;
            goodButton.ComponentClicked += goodButton_ComponentClicked;

            UiButton badButton = new UiButton("Не ме кликай", font1, 100, 25);
            badButton.box.Left = 10;
            badButton.box.Top = 35;
            badButton.ComponentClicked += badButton_ComponentClicked;

            testPanel.AddChild(goodButton);
            testPanel.AddChild(badButton);

            gui.Add(testPanel);

            menu = new UiVerticalMenu();

            menu.AddItem("Add", menu_add);
            menu.AddItem("Close", menu_close);
            menu.AddItem("Remove last button", menu_remove);

            menu.box.Left = 300;
            menu.box.Top = 100;
            gui.Add(menu);

            menu.movable = true;
            loop();
        }

        private void badButton_ComponentClicked()
        {
            Console.WriteLine("Why did you click me?");
        }

        void goodButton_ComponentClicked()
        {
            Console.WriteLine("You clicked me!");
        }

        private int idxBtn = 1;
        void menu_add()
        {
            String text = "Button " + Convert.ToString(idxBtn, 2);
            idxBtn++;
            menu.AddItem(text, menu_newclick);
        }

        void menu_newclick()
        {
            Console.WriteLine("You clicked a new button.");
        }
        
        void menu_remove()
        {
            if (menu.children.Count > 3)
            {
                menu.RemoveLast();
            }
        }

        void menu_close()
        {
            menu.Remove();
        }

        private bool lmbDown = false;

        void window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left) lmbDown = false;
        }

        void window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                Vector2i mousePos = Mouse.GetPosition(window);
                gui.processMouseClick(mousePos);

                if(!lmbDown)
                {
                    lastMousePos = Mouse.GetPosition(window);
                    lmbDown = true;
                }
                
            }
        }

        void window_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Left) renderFrame.xCenter -= 20.0f;
            if (e.Code == Keyboard.Key.Right) renderFrame.xCenter += 20.0f;
            if (e.Code == Keyboard.Key.Up) renderFrame.yCenter += 20.0f;
            if (e.Code == Keyboard.Key.Down) renderFrame.yCenter -= 20.0f;
        }

        void window_MouseWheelMoved(object sender, MouseWheelEventArgs e)
        {
            renderFrame.scale += ((float)e.Delta)/10.0f;
            renderFrame.calcZoom();
        }

        void loop()
        {
            while(window.IsOpen)
            {
                window.DispatchEvents();
                processMouse();
                draw();
            }
        }

        Vector2i lastMousePos = new Vector2i();
        IMovable currentObject;
        IMovable currentGuiObject;
        
        Circle getCircleAt(Vector2f pos)
        {
            foreach(Circle c in circles)
            {
                if (c.isInside(pos)) return c;
            }
            return null;
        }

        void processMouse()
        {
            Vector2i mousePos = Mouse.GetPosition(window);
            bool inGui = gui.processMouse(mousePos);

            //Console.WriteLine("Mouse:" + lmbDown);
            if (lmbDown)
            {
                float dx = mousePos.X - lastMousePos.X;
                float dy = mousePos.Y - lastMousePos.Y;

                if (currentObject == null && currentGuiObject != null)
                {
                    currentGuiObject.moveX(dx);
                    currentGuiObject.moveY(dy);
                }
                else if (currentObject == null && inGui)
                {
                    UiComponent uic = gui.lastMoused;
                    if (uic != null && uic is IMovable)
                    {
                        currentGuiObject = uic as IMovable;
                        currentGuiObject.moveX(dx);
                        currentGuiObject.moveY(dy);
                    }
                }
                else
                {
                    dy *= -1;

                    dx /= renderFrame.zoom;
                    dy /= renderFrame.zoom;

                    if (currentObject != null) // вече има обект
                    {
                        currentObject.moveX(dx);
                        currentObject.moveY(dy);
                        lastMousePos = mousePos;
                        return;
                    }

                    // търсим обект, който може да е посочен
                    currentObject = getCircleAt(toGlobalCoords(mousePos));
                    if (currentObject == null) currentObject = renderFrame; // няма такъв -> местим рамката

                    currentObject.moveX(dx);
                    currentObject.moveY(dy);
                }
                lastMousePos = mousePos;
            }
            else
            {
                currentObject = null;
                currentGuiObject = null;
            }
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

            List<Drawable> draws = new List<Drawable>();
            foreach (Circle c in circles)
            {
                List<Drawable> elements = c.getDrawables(renderFrame);
                foreach (Drawable d in elements) draws.Add(d);
            }

            draws.ForEach(d => window.Draw(d));

            Text scaleText = new Text(String.Format("Zoom: {0}\nCenter X: {1}\nCenter Y:{2}", renderFrame.zoom, renderFrame.xCenter, renderFrame.yCenter), font1, 13);
            scaleText.Position = new Vector2f(20, 20);
            scaleText.Color = Color.Red;

            Vector2f coords = toGlobalCoords(Mouse.GetPosition(window));
            Text coordText = new Text(String.Format("X: {0}\nY: {1}", coords.X, coords.Y), font1, 13);
            coordText.Position = new Vector2f(900, 20);
            coordText.Color = Color.Red;

            Vector2i scoords = Mouse.GetPosition(window);
            Text scoordText = new Text(String.Format("X: {0}\nY: {1}", scoords.X, scoords.Y), font1, 13);
            scoordText.Position = new Vector2f(800, 20);
            scoordText.Color = Color.Red;

            window.Draw(scaleText);
            window.Draw(coordText);
            window.Draw(scoordText);

            drawAxes();

            List<Drawable> uiList = gui.getDrawables(renderFrame);

            uiList.ForEach(d => window.Draw(d));

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
