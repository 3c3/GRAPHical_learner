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

        void menu_FileClicked(UiComponent sender, Object arg)
        {
            fileMenu.visible = true;
        }

        void menu_ArrangeClicked(UiComponent sender, Object arg)
        {
            arrangeMenu.visible = true;
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

        void BtnAddEdgeToggle(UiComponent sender, Object arg)
        {
            if (addEdgeEnabled) DisableEdgeAdding();
            else EnableEdgeAdding();
        }

        void BtnPhysToggle(UiComponent sender, Object arg)
        {
            if (physics) DisablePhysics();
            else EnablePhysics();
        }

        void MenuBtnPlay(UiComponent sender, Object arg)
        {
            timerEnabled = true;
            if (connector != null) connector.Resume();
        }

        void MenuBtnPause(UiComponent sender, Object arg)
        {
            timerEnabled = false;
        }

        void menu_Circle(UiComponent sender, Object arg)
        {
            activeGraph.ArrangeInCircle();
        }

        void BtnCenterGraph(UiComponent sender, Object arg)
        {
            CenterGraph();
        }

        void BtnSingleStep(UiComponent sender, Object arg)
        {
            if (connector != null) connector.Resume();
        }

        private int idxBtn = 1;
        void menu_FileOpen(UiComponent sender, Object arg)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Graph g = new Graph();
                IOHandler.ReadGraphFile(ofd.FileName, ref g);
                ChangeGraph(g);
                DisableEdgeAdding();
                DisablePhysics();
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
            int n = random.Next(50);
            int m = random.Next(100);
            for (int i = 0; i < n; i++)
            {
                Vertex v = new Vertex((float)random.NextDouble() * 1000 - 500.0f, (float)random.NextDouble() * 600 - 300.0f);
                g.AddVertex(v);
                if (i == 0) continue;

                int connections = random.Next(1, (int)Math.Min(i, 3));
                for(int j = 0; j < connections; j++)
                {
                    int vo = random.Next(0, i);

                    bool contains = false;
                    foreach(Edge e in v.edges)
                    {
                        if(e.source.id == vo || e.destination.id == vo)
                        {
                            contains = true;
                            break;
                        }
                    }
                    if (contains) j--;
                    else g.AddEdge(v.id, vo);
                }
            }

            

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
            ClearAll();
        }

        void menu_Directivity(UiComponent sender, Object arg)
        {
            if (activeGraph.Directed) DisableDirected();
            else EnableDirected();
        }

        void menu_Weight(UiComponent sender, Object arg)
        {
            if (weighted) DisableWeight();
            else EnableWeight();
        }

        void rmbMenu_AddVertex(UiComponent sender, Object arg)
        {
            Vector2f pos = ToGlobalCoords(Mouse.GetPosition(window));
            activeGraph.vertices.Add(new Vertex(pos.X, pos.Y));
            rmbMenu.visible = false;
        }

        void rmbMenu_Deselect(UiComponent sender, Object arg)
        {
            if(lastClickedVertex!=null)
            {
                lastClickedVertex.selected = false;
                lastClickedVertex = null;
            }
            
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
            Vector2f pos = ToGlobalCoords(Mouse.GetPosition(window));

            Vertex v = GetVertexAt(pos);
            Edge edge = GetEdgeAt(pos);
            if (e.Button == Mouse.Button.Left && inputEdge != null) FinishWeight();
            else if (e.Button == Mouse.Button.Left && inputEdge == null)
            {
                rmbMenu.visible = false;
                if (activeVertexMenu != null)
                {
                    activeVertexMenu.Remove();
                    activeVertexMenu = null;
                }
                if(activeEdgeMenu != null)
                {
                    activeEdgeMenu.Remove();
                    activeEdgeMenu = null;
                }
                if(!gui.ProcessMouseClick(mousePos) && addEdgeEnabled)
                {                  
                    if(v!=null)
                    {
                        if (lastClickedVertex != null && lastClickedVertex != v)
                        {
                            Edge newEdge = activeGraph.AddEdge(lastClickedVertex, v);
                            lastClickedVertex.selected = false;
                            lastClickedVertex = null;

                            if(weighted)
                            {
                                inputEdge = newEdge;
                                newEdge.SetProperty(Property.EdgeWeightId, "");
                                inputWeight = newEdge.GetProperty(Property.EdgeWeightId);
                                //Console.WriteLine("Enter edge weight");
                                edgeInputLabel.visible = true;
                            }
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
                if(inputEdge!=null)
                {
                    activeGraph.RemoveEdge(inputEdge);
                    inputWeight = null;
                    inputEdge = null;
                    edgeInputLabel.visible = false;
                }
                else if(v!=null)
                {
                    if(activeVertexMenu!=null)activeVertexMenu.Remove();
                    activeVertexMenu = new UiVertexMenu(v, activeGraph, mousePos.X, mousePos.Y);
                    gui.Add(activeVertexMenu);
                }
                else if(edge!=null)
                {
                    if (activeEdgeMenu != null) activeEdgeMenu.Remove();
                    activeEdgeMenu = new UiEdgeMenu(edge, activeGraph, this, mousePos.X, mousePos.Y);
                    gui.Add(activeEdgeMenu);
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
        UiEdgeMenu activeEdgeMenu;

        void window_KeyPressed(object sender, KeyEventArgs e)
        {
            if (inputWeight == null) return;
            String val = (String)inputWeight.Value;

            if(e.Code == Keyboard.Key.Return)
            {
                FinishWeight();
            }
            if(e.Code == Keyboard.Key.BackSpace)
            {
                if (val.Length > 0) inputWeight.Value = val.Remove(val.Length - 1);
                return;
            }


            char c = 'a';
            if (e.Code >= Keyboard.Key.Num0 && e.Code <= Keyboard.Key.Num9) c = (char)('0' + (e.Code - Keyboard.Key.Num0));
            else if (e.Code >= Keyboard.Key.Numpad0 && e.Code <= Keyboard.Key.Numpad9) c = (char)('0' + (e.Code - Keyboard.Key.Numpad0));
            else if (e.Code == Keyboard.Key.Period || e.Code == Keyboard.Key.Comma) c = ',';

            //Console.WriteLine("val: {0}, c: {1}", val, c);

            if(c!='a') val += c;
            if(inputWeight!=null)inputWeight.Value = val;
            //Console.WriteLine(val);
        }

        void window_MouseWheelMoved(object sender, MouseWheelEventArgs e)
        {
            renderFrame.scale += ((float)e.Delta) / 10.0f;
            renderFrame.CalcZoom();
            if (currentObject != null)
            {
                if (currentObject is Circle)
                {
                    Circle circle = currentObject as Circle;
                    circle.center = ToGlobalCoords(Mouse.GetPosition(window));
                }
            }
        }

        void fs_SimulatorStopped()
        {
            //Console.WriteLine("Auto-disabling physics.");
            DisablePhysics();
            CenterGraph();
        }
    }
}
