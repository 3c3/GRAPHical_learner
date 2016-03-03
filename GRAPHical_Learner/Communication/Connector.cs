using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    public delegate void AlgorithmSuspendedHandler();

    /// <summary>
    /// Клас за свръзка между алгоритъм и графична среда
    /// </summary>
    public class Connector
    {
        private volatile Graph graph = new Graph();
        private int nVertices;
        private volatile MainUI ui;

        /// <summary>
        /// Извиква се, когато алгоритъмът удари Pause()
        /// </summary>
        public event AlgorithmSuspendedHandler AlgorithmSuspended;

        public Graph GraphInstance
        {
            get { return graph; }
        }

        /// <summary>
        /// Задава броя на върхове. Трябва да се извика, преди да се добавят ребра
        /// </summary>
        /// <param name="n">Броя ребра</param>
        protected void SetVertices(int n)
        {
            nVertices = n;
            graph.vertices.Clear();
            Vertex.ResetCounter();
            
            for (int i = 0; i < n; i++) graph.vertices.Add(new Vertex());
        }

        /// <summary>
        /// Добавя ребро
        /// </summary>
        /// <param name="idx1">Източник</param>
        /// <param name="idx2">Дестинация</param>
        /// <returns>Id на реброто</returns>
        protected int AddEdge(int idx1, int idx2)
        {
            return graph.AddEdge(idx1, idx2).id;
        }

        /// <summary>
        /// Създава свойство
        /// </summary>
        /// <param name="name">Името на свойството</param>
        /// <returns>Id на свойството</returns>
        protected int RegisterProperty(string name)
        {
            return Property.GetPropertyId(name);
        }

        /// <summary>
        /// Overrride - задължително. Връща стойността на свойство
        /// </summary>
        /// <param name="vertexId">Номер на връх</param>
        /// <param name="propertyId">Id на свойството</param>
        /// <returns>Стойност</returns>
        public virtual Object GetVertexProperty(int vertexId, int propertyId)
        {
            return null;
        }

        /// <summary>
        /// Overrride - задължително. Връща стойността на свойство
        /// </summary>
        /// <param name="edgeIdx">Номер на ребро</param>
        /// <param name="propertyId">Id на свойството</param>
        /// <returns>Стойността</returns>
        public virtual Object GetEdgeProperty(int edgeIdx, int propertyId)
        {
            return null;
        }

        /// <summary>
        /// Задава(ако е необходимо - създава) стойност на свойството за връх
        /// </summary>
        /// <param name="vertexId">Номер на връх</param>
        /// <param name="propertyId">Id на свойството</param>
        /// <param name="value">Стойност</param>
        protected void SetVertexProperty(int vertexId, int propertyId, Object value)
        {
            graph.vertices[vertexId].SetProperty(propertyId, value);
        }

        /// <summary>
        /// Добавя свойство към всички върхове
        /// </summary>
        /// <param name="propertyId">Id на свойството(от RegisterProperty)</param>
        /// <param name="defaultValue">първоначалната стойност</param>
        protected void AddPropertyToVertices(int propertyId, Object defaultValue)
        {
            foreach(Vertex v in graph.vertices)
            {
                v.SetProperty(propertyId, defaultValue);
            }
        }

        /// <summary>
        /// Задава(ако е необходимо - създава) стойност на свойството за ребро
        /// </summary>
        /// <param name="edgeId">Номер на ребро</param>
        /// <param name="propertyId">Id на свойството</param>
        /// <param name="value">Стойност</param>
        protected void SetEdgeProperty(int edgeId, int propertyId, Object value)
        {
            graph.edges[edgeId].SetProperty(propertyId, value);
        }

        volatile Thread algoThread;

        /// <summary>
        /// Продължава алгоритъма
        /// </summary>
        internal void Resume()
        {
            if (algoThread != null)
            {
                algoThread.Resume();
                algoThread = null;
            }
        }

        /// <summary>
        /// Слага алгоритъма на пауза - може да бъде продължен от визуалната среда
        /// </summary>
        protected void Pause()
        {
            algoThread = Thread.CurrentThread;
            if (AlgorithmSuspended != null) AlgorithmSuspended();
            algoThread.Suspend();
        }

        /// <summary>
        /// Извиква се преди запалването на графичната среда. Полезно за настройки
        /// </summary>
        protected virtual void PreRun()
        {

        }

        /// <summary>
        /// Thread функцията
        /// </summary>
        void LaunchUi()
        {
            ui = new MainUI(this);
        }

        /// <summary>
        /// Зарежда шрифта. Получават се бъгове, ако не се извика
        /// </summary>
        protected void SetupGui()
        {
            GraphicScheme.LoadFont();
        }

        /// <summary>
        /// Пуска графичната среда
        /// </summary>
        protected void StartGui()
        {
            //GraphicScheme.LoadFont();
            PreRun();            
            Thread uiThread = new Thread(new ThreadStart(LaunchUi));
            uiThread.SetApartmentState(ApartmentState.STA);
            uiThread.Start();
        }
    }
}
