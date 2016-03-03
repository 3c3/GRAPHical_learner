using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace GRAPHical_Learner
{
    public abstract class IOHandler
    {
        public static void WriteGraphFile(string filename, Graph graph)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(String.Format("{0} {1}", graph.vertices.Count, graph.edges.Count));

            int offset = graph.vertices.Last().id - graph.vertices.Count + 1;

            foreach (Edge e in graph.edges) builder.AppendLine(String.Format("{0} {1}", e.source.id - offset, e.destination.id - offset));

            File.WriteAllText(filename, builder.ToString());
        }

        public static void ReadGraphFile(string filename, ref Graph graph)
        {
            String[] lines = File.ReadAllLines(filename);

            int lineIdx = 0;
            try
            {
                String[] parts = lines[0].Split(' ');
                int n = int.Parse(parts[0]);
                int m = int.Parse(parts[1]);

                Vertex.ResetCounter();
                for (int i = 0; i < n; i++) graph.vertices.Add(new Vertex());

                lineIdx++;
                for(int i = 0; i < m; i++)
                {
                    parts = lines[lineIdx].Split(' ');
                    int src = int.Parse(parts[0]);
                    int dest = int.Parse(parts[1]);

                    Edge e = graph.AddEdge(src, dest);

                    if(parts.Length > 2)
                    {
                        int val = 0;
                        try
                        {
                            val = int.Parse(parts[2]);
                            int propId = Property.GetPropertyId("тегло");
                            Property.edgeWeighId = propId;
                            e.SetProperty(propId, int.Parse(parts[2]));
                        }
                        catch(Exception exc)
                        {
                            Console.WriteLine(String.Format("Line {0}: third part was NaN: {1}", lineIdx, parts[2]));
                        }
                        
                    }

                    lineIdx++;
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(String.Format("An exception occured on line {0}: {1}", lineIdx, e.Message), e.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                graph = new Graph();
                Vertex.ResetCounter();
                return;
            }
            graph.ArrangeInCircle();
        }
    }
}
