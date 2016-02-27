using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    public partial class MainUI
    {
        Random random = new Random();

        private void badButton_ComponentClicked(UiComponent sender, Object arg)
        {
            Console.WriteLine("Why did you click me?");
        }

        void BtnAddVertex(UiComponent sender, Object arg)
        {

            float x = (float)random.NextDouble() * 1600.0f - 800.0f;
            float y = (float)random.NextDouble() * 1400.0f - 700.0f;

            activeGraph.vertices.Add(new Vertex(x, y));

            if (activeGraph.vertices.Count < 2) return;

            int n = random.Next(3);
            for (int i = 0; i < n; i++)
            {
                int e_choice = random.Next(activeGraph.vertices.Count - 2);
                activeGraph.AddEdge(activeGraph.vertices.Count - 1, e_choice);
            }
        }

        void BtnPhysDisable(UiComponent sender, Object arg)
        {
            physics = false;
        }

        void BtnPhysEnable(UiComponent sender, Object arg)
        {
            //fs.SetForce(0.0f);
            physics = true;
            Console.WriteLine("Physics enabled.");
        }

        private int idxBtn = 1;
        void menu_FileOpen(UiComponent sender, Object arg)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                IOHandler.ReadGraphFile(ofd.FileName, ref activeGraph);
                fs.SetForce(100);
                physics = true;
            }
        }

        void menu_newclick(UiComponent sender, Object arg)
        {
            Console.WriteLine("You clicked a new button." + arg.ToString());
        }

        void menu_Generate(UiComponent sender, Object arg)
        {
            Vertex.ResetCounter();
            Graph g = new Graph();
            int n = random.Next(100);
            int m = random.Next(100);
            for (int i = 0; i < n; i++) g.vertices.Add(new Vertex((float)random.NextDouble() * 1000 - 500.0f, (float)random.NextDouble() * 600 - 300.0f));
            for (int i = 0; i < m; i++) g.AddEdge(random.Next(n), random.Next(n));

            fs.SetForce(50);

            activeGraph = g;
            fs.SetGraph(g);
        }

        void menu_Shuffle(UiComponent sender, Object arg)
        {
            foreach(Vertex v in activeGraph.vertices)
            {
                v.x = (float)(random.NextDouble() * 1000 - 500);
                v.y = (float)(random.NextDouble() * 600 - 300);
            }
        }

        void menu_FileSave(UiComponent sender, Object arg)
        {
            System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
            if(sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                IOHandler.WriteGraphFile(sfd.FileName, activeGraph);
            }
        }

        void menu_Clear(UiComponent sender, Object arg)
        {
            activeGraph.edges.Clear();
            activeGraph.vertices.Clear();
            Vertex.ResetCounter();
        }

        void rmbMenu_AddVertex(UiComponent sender, Object arg)
        {
            Vector2f pos = toGlobalCoords(Mouse.GetPosition(window));
            activeGraph.vertices.Add(new Vertex(pos.X, pos.Y));
            rmbMenu.visible = false;
        }

        void rmbMenu_Deselect(UiComponent sender, Object arg)
        {
            lastClickedVertex.selected = false;
            lastClickedVertex = null;
            rmbMenu.visible = false;
        }

        private bool lmbDown = false;

        void window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left) lmbDown = false;
        }

        Vertex lastClickedVertex = null;

        void window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            Vector2i mousePos = Mouse.GetPosition(window);
            Vector2f pos = toGlobalCoords(Mouse.GetPosition(window));

            Vertex v = getCircleAt(pos);
            if (e.Button == Mouse.Button.Left)
            {
                rmbMenu.visible = false;
                if (activeVertexMenu != null)
                {
                    activeVertexMenu.Remove();
                    activeVertexMenu = null;
                }
                if(!gui.ProcessMouseClick(mousePos))
                {                  
                    if(v!=null)
                    {
                        if (lastClickedVertex != null && lastClickedVertex != v)
                        {
                            activeGraph.AddEdge(lastClickedVertex, v);
                            lastClickedVertex.selected = false;
                            lastClickedVertex = null;
                        }
                        else
                        {
                            lastClickedVertex = v;
                            lastClickedVertex.selected = true;
                        }
                    }

                }

                if (!lmbDown)
                {
                    lastMousePos = Mouse.GetPosition(window);
                    lmbDown = true;
                }

            }
            else if(e.Button == Mouse.Button.Right)
            {
                if(v!=null)
                {
                    if(activeVertexMenu!=null)activeVertexMenu.Remove();
                    activeVertexMenu = new UiVertexMenu(v, activeGraph, mousePos.X, mousePos.Y);
                    gui.Add(activeVertexMenu);
                }
                else
                {
                    rmbMenu.X = mousePos.X;
                    rmbMenu.Y = mousePos.Y;
                    rmbMenu.visible = true;
                }
            }
        }

        UiVertexMenu activeVertexMenu;

        void window_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Left) renderFrame.xCenter -= 20.0f;
            if (e.Code == Keyboard.Key.Right) renderFrame.xCenter += 20.0f;
            if (e.Code == Keyboard.Key.Up) renderFrame.yCenter += 20.0f;
            if (e.Code == Keyboard.Key.Down) renderFrame.yCenter -= 20.0f;
        }

        void window_MouseWheelMoved(object sender, MouseWheelEventArgs e)
        {
            renderFrame.scale += ((float)e.Delta) / 10.0f;
            renderFrame.calcZoom();
            if (currentObject != null)
            {
                if (currentObject is Circle)
                {
                    Circle circle = currentObject as Circle;
                    circle.center = toGlobalCoords(Mouse.GetPosition(window));
                }
            }
        }
    }
}
