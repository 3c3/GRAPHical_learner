using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRAPHical_Learner;

namespace AlgoTest
{
    /// <summary>
    /// Примерна програма за ползване със приложението
    /// </summary>
    class Program : Connector
    {
        List<int>[] graph = new List<int>[1024]; // граф чрез списък на съседи
        bool[] used = new bool[1024]; // пази дали върха е обходен
        int[] level = new int[1024]; // нивото във BFS дървото
        int[] pred = new int[1024];
        int n;

        void input()
        { // вход от клавиатурата
            String line = Console.ReadLine();

            String[] parts = line.Split(' ');
            n = int.Parse(parts[0]);
            int m = int.Parse(parts[1]);

            for (int i = 0; i < n; i++) graph[i] = new List<int>();
            SetVertices(n); // задава броя върхове на визуалния граф
            //SetDirected(true);

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
            AddEdge(src, dest); // Добавя ребро във визуалния граф
        }

        void bfs()
        {
            Queue<int> q = new Queue<int>();
            q.Enqueue(0);
            used[0] = true;
            Pause(); // прави пауза в началото

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
                    pred[cand] = current;
                    q.Enqueue(cand);
                    Pause(); // и на всеки обходен връх
                    Console.WriteLine("step...");
                }
            }
        }

        public Program()
        {
            SetupGui(); // задължително тука
            input();
            Console.WriteLine("Starting UI...");
            StartGui(); // пуска визуалната среда преди алгоритъма
            bfs();
        }

        /// <summary>
        /// Връща стойностите на зададените свойства
        /// </summary>
        /// <param name="vertexId"></param>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public override object GetVertexProperty(int vertexId, int propertyId)
        {
            if (propertyId == propUsed) return used[vertexId];
            else if (propertyId == propLevel) return level[vertexId];
            else if (propertyId == propPred) return pred[vertexId];
            return null;
        }

        int propUsed, propLevel; // id-та на свойствата за обходеност и ниво
        int propPred;

        /// <summary>
        /// Извиква се автоматично преди да се пусне визуалната среда
        /// Полезно за настройчици
        /// </summary>
        protected override void PreRun()
        {
            propUsed = RegisterProperty("използван"); // регистрира свойства
            propLevel = RegisterProperty("ниво");
            propPred = RegisterProperty("предшественик");
            AddPropertyToVertices(propUsed, false); // разпространява ги на всички върове
            AddPropertyToVertices(propLevel, 0);
            AddPropertyToVertices(propPred, 0);
        }

        static void Main(string[] args)
        {
            new Program(); // необходима е инстанция на Connector
        }
    }
}
