using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;

namespace NativeAlgo
{
    public delegate void ThreadSuspendedHandler(IntPtr hThread);

    public class ThreadHandler
    {
        IntPtr processHandle; // процеса в който е треда
        IntPtr threadHandle; // хендъл към треда
        IntPtr pControlByte; // адреса на байта, който казва дали треда е спрян

        public event ThreadSuspendedHandler ThreadSuspended;

        Thread checkerThread;
        bool wasSuspended = false;

        public ThreadHandler(IntPtr hProcess, IntPtr hThread, IntPtr pByte)
        {
            processHandle = hProcess;
            threadHandle = hThread;
            pControlByte = pByte;

            checkerThread = new Thread(new ThreadStart(CheckLoop));
            checkerThread.Start();
        }

        bool shouldStop = false;

        void CheckLoop()
        {
            while(true)
            {
                if (IsSuspended() && wasSuspended == false)
                {
                    //Thread.Sleep(60);
                    Console.WriteLine("Algorithm on pause");
                    wasSuspended = true;
                    if (ThreadSuspended != null) ThreadSuspended(threadHandle);                    
                }
                if (shouldStop) return;
                Thread.Sleep(5);
            }
            
        }

        public void Resume()
        {
            wasSuspended = false;
            int result = NativeMethods.ResumeThread(threadHandle);
        }

        byte[] localBuff = new byte[1];

        bool IsSuspended()
        {
            int nRead;
            bool result = NativeMethods.ReadProcessMemory(processHandle, pControlByte, localBuff, 1, out nRead);
            if(!result)
            {
                Console.WriteLine(String.Format("Error while reading control byte: {0}", Marshal.GetLastWin32Error()));
                shouldStop = true;
                return false;
            }

            return localBuff[0] != 0;
        }

    }
}
