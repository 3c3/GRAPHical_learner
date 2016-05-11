using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using GRAPHical_Learner;

namespace NativeAlgo
{
    public class NativeConnector : Connector
    {
        static readonly int createThread = 0x0002;
        static readonly int vmOperation = 0x0008;
        static readonly int vmRead = 0x0010;

        internal IntPtr processHandle; // handle към native процеса, който изпълнява алгоритъма за графа
        IntPtr threadHandle; // handle за нишката на алгоритъма

        internal IntPtr vertexBuffer; // буфера с променени стойности на върхове
        internal IntPtr edgeBuffer; // буфера с променени стойности на ребра

        ThreadHandler th;
        PropertyHandler propertyHandler;
        internal StringStore stringStore;

        public NativeConnector()
        {
            string cmd = Environment.CommandLine;
            string[] parts = cmd.Split(';'); // очакваме формат [id на процес];[пойнтър към началната структура]

            pollProperties = false;

            if(parts.Length == 2)
            {
                try
                {
                    int pid = int.Parse(parts[0]); // четем id на процеса
                    int startupPtr = Convert.ToInt32(parts[1], 16); // и адреса на пъровначалната структура
                    ReadProcess(pid, (IntPtr)startupPtr); // чете първоначалната информация
                    SetupGui(); // подготвя графичния интерфейс
                    StartGui();
                    NativeMethods.ResumeThread(threadHandle); // пуска нишката на алгоритъма
                }
                catch(Exception e)
                {
                    /// При некоректни данни излиза

                    MessageBox.Show(e.Message, "Constructor: " + e.GetType().ToString());

                    Environment.Exit(0);
                }                
            }
        }

        internal IntPtr[] startupBuffer = new IntPtr[6]; // пазим -те пойнтъра

        /// <summary>
        /// Чете първоначалните данни за процеса
        /// </summary>
        /// <param name="pid">id на процеса</param>
        /// <param name="startupInfo">указател към стартовата структура(в address space-а на процеса)</param>
        void ReadProcess(int pid, IntPtr startupInfo)
        {
            processHandle = NativeMethods.OpenProcess(createThread | vmOperation | vmRead, false, pid); // отваряме процеса с достатъчен достъп

            byte[] tempBuff = new byte[28];
            int nRead;
            bool result = NativeMethods.ReadProcessMemory(processHandle, startupInfo, tempBuff, 28, out nRead); // взимаме данните

            if(!result)
            {
                int errCode = Marshal.GetLastWin32Error();
                MessageBox.Show(errCode.ToString(), "Error when reading memory!");
                Environment.Exit(0);
            }

            int threadId = tempBuff[3] << 24 | tempBuff[2] << 16 | tempBuff[1] << 8 | tempBuff[0]; // първите 4 байта са id на треда на алгоритъма

            // Прехвъърляме данните от буфера към масива с пойнтъри
            Buffer.BlockCopy(tempBuff, 4, startupBuffer, 0, 24);

            // Отваряме треда с правомощия за пускане
            threadHandle = NativeMethods.OpenThread(0x0002, true, threadId);
            
            th = new ThreadHandler(processHandle, threadHandle, startupBuffer[4]);
            th.ThreadSuspended += th_ThreadSuspended;

            ReadGraphData(startupBuffer[0]);

            stringStore = new StringStore(processHandle, startupBuffer[5]);

            ReadPropertyData(startupBuffer[1]);
        }

        /// <summary>
        /// Извиква се, когато нишката на алгоритъма е суспендирана(на пауза)
        /// </summary>
        /// <param name="hThread">Handle към нишката, към момента не се ползва</param>
        void th_ThreadSuspended(IntPtr hThread)
        {
            try
            {
                propertyHandler.ReadChanges();
            }
            catch(Exception e)
            {
                Console.WriteLine("NPE! {0}", propertyHandler == null ? "null" : "inside propertyHandler");
            }
            
            RaiseSuspendedEvent();
        }

        /// <summary>
        /// Продължава нишката в процеса на алгоритъма
        /// </summary>
        protected override void Resume()
        {
            th.Resume();
        }

        /// <summary>
        /// Чете информацията за свойствата
        /// </summary>
        /// <param name="ptr">указател към буфера с данните за свойства</param>
        void ReadPropertyData(IntPtr ptr)
        {
            byte[] initBuff = new byte[8];
            int nRead;

            bool result = NativeMethods.ReadProcessMemory(processHandle, ptr, initBuff, 8, out nRead);
            
            int pb = initBuff[0] | initBuff[1] << 8 | initBuff[2] << 16 | initBuff[3] << 24;
            int sb = initBuff[4] | initBuff[5] << 8 | initBuff[6] << 16 | initBuff[7] << 24;

            propertyHandler = new PropertyHandler(this, (IntPtr)pb, (IntPtr) sb, startupBuffer[2], startupBuffer[3]);
            propertyHandler.ReadInitialData();
        }

        /// <summary>
        /// Чете данните за графа и го създава
        /// </summary>
        /// <param name="graphDataPtr">указател към буфера</param>
        void ReadGraphData(IntPtr graphDataPtr)
        {
            GraphicScheme.LoadFont();

            byte[] gdBuff = new byte[9]; // информация за графа: [брой върхове] [брой ребра] [насочен]

            int nRead;

            bool result = NativeMethods.ReadProcessMemory(processHandle, graphDataPtr, gdBuff, 9, out nRead);

            int[] count = new int[2];
            Buffer.BlockCopy(gdBuff, 0, count, 0, 8);

            //MessageBox.Show(String.Format("Vertices: {0}, Edges: {1}", count[0], count[1]));

            graph = new Graph(gdBuff[8]!=0);
            graph.vertices = new List<Vertex>(count[0]);


            Vertex.ResetCounter(); // better safe than sorry
            Edge.ResetCounter();

            for(int i = 0; i < count[0]; i++)
            {
                graph.AddVertex(new Vertex());
            }

            int[] edges = new int[count[1] * 2];

            byte[] edgeBuff = new byte[count[1] * 8];
            result = NativeMethods.ReadProcessMemory(processHandle, graphDataPtr + 9, edgeBuff, count[1] * 8, out nRead);

            Buffer.BlockCopy(edgeBuff, 0, edges, 0, count[1] * 8);

            for(int i = 0; i < count[1]; i++)
            {
                graph.AddEdge(edges[2 * i], edges[2 * i + 1]);
            }
        }

        static void Main(string[] args)
        {
            new NativeConnector();
        }

        static internal void Halt(string msg)
        {
            Console.WriteLine("Press any key to: {0}", msg);
            Console.ReadKey();
        }
    }
}
