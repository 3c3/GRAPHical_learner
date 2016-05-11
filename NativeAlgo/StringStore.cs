using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace NativeAlgo
{
    public class StringStore
    {
        Dictionary<IntPtr, string> strings = new Dictionary<IntPtr,string>();
        IntPtr hProcess, fpLen;

        int nStored = 0;

        public StringStore(IntPtr processHandle, IntPtr lengthFunctionPointer)
        {
            hProcess = processHandle;
            fpLen = lengthFunctionPointer;
        }

        public string GetString(IntPtr szPtr)
        {
            if (strings.ContainsKey(szPtr)) return strings[szPtr];

            int len = GetSzLength(szPtr);
            if(len==-1)
            {
                strings[szPtr] = "!error!";
                return strings[szPtr];
            }

            byte[] localBuff = new byte[len];
            int nRead;

            NativeMethods.ReadProcessMemory(hProcess, szPtr, localBuff, len, out nRead);
            string readString = Encoding.ASCII.GetString(localBuff, 0, len);
            strings[szPtr] = readString;

            nStored++;
            //Console.WriteLine("Stored strings: {0}", nStored);

            return readString;
        }

        private int GetSzLength(IntPtr szPtr)
        {
            int threadId;
            IntPtr hThread = NativeMethods.CreateRemoteThread(hProcess, IntPtr.Zero, 0, fpLen, szPtr, 0, out threadId);

            if (hThread.Equals(0)) return -1;

            NativeMethods.WaitForSingleObject(hThread, 0xFFFFFFFF);

            int result = -1;
            NativeMethods.GetExitCodeThread(hThread, out result);
            return result;
        }
    }
}
