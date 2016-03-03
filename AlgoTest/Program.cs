using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRAPHical_Learner;

namespace AlgoTest
{
    class Program : Connector
    {
        List<int>[] graph = new List<int>[1024];
        bool[] used = new bool[1024];
        int[] level = new int[1024];
        int n;

        void input()
        {
            String line = Console.ReadLine();

            String[] parts = line.Split(' ');
            n = int.Parse(parts[0]);
            int m = int.Parse(parts[1]);

            for (int i = 0; i < n; i++) graph[i] = new List<int>();
            SetVertices(n);

            for(int i = 0; i < m; i++)
            {
                line = Console.ReadLine();
                parts = line.Split(' ');
                int src = int.Parse(parts[0]);
                int dst = int.Parse(parts[1]);
                addEdge(src, dst);
            }
        }

        void addEdge(int src, int dest)
        {
            graph[src].Add(dest);
            graph[dest].Add(src);
            AddEdge(src, dest);
        }

        void bfs()
        {
            Queue<int> q = new Queue<int>();
            q.Enqueue(0);
            used[0] = true;
            Pause();

            while(q.Count > 0)
            {
                int current = q.Dequeue();
                
                List<int> vc = graph[current];

                for(int i = 0; i < vc.Count; i++)
                {
                    int cand = vc[i];
                    if (used[cand]) continue;
                    used[cand] = true;
                    level[cand] = level[current] + 1;
                    q.Enqueue(cand);
                    Pause();
                    Console.WriteLine("step...");
                }
            }
        }

        public Program()
        {
            SetupGui();
            input();
            Console.WriteLine("Starting UI...");
            StartGui();
            bfs();
        }

        public override object GetVertexProperty(int vertexId, int propertyId)
        {
            if (propertyId == propUsed) return used[vertexId];
            else if (propertyId == propLevel) return level[vertexId];
            return null;
        }

        public override object GetEdgeProperty(int edgeIdx, int propertyId)
        {
            return base.GetEdgeProperty(edgeIdx, propertyId);
        }

        int propUsed, propLevel;

        protected override void PreRun()
        {
            propUsed = RegisterProperty("използван");
            propLevel = RegisterProperty("ниво");
            AddPropertyToVertices(propUsed, false);
            AddPropertyToVertices(propLevel, 0);
        }

        static void Main(string[] args)
        {
            new Program();
        }
    }
}
