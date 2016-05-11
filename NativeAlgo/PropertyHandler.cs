using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using GRAPHical_Learner;

namespace NativeAlgo
{
    public class PropertyHandler
    {
        private static readonly byte TYPE_BOOL = 0;
        private static readonly byte TYPE_MARKED = 1;
        private static readonly byte TYPE_VISIBLE = 2;
        private static readonly byte TYPE_INT = 3;
        private static readonly byte TYPE_LONG = 4;
        private static readonly byte TYPE_FLOAT = 5;
        private static readonly byte TYPE_DOUBLE = 6;
        private static readonly byte TYPE_RGB24 = 7;
        private static readonly byte TYPE_STRING = 8;

        NativeConnector connector;
        IntPtr primaryBuffer, secondaryBuffer;
        IntPtr vertexBuffer, edgeBuffer;

        BinaryReader primaryReader, secondaryReader;
        int nProperties;
        int nRead;

        BinaryReader vertexReader, edgeReader;

        byte[] propertyDataType = new byte[256];

        Dictionary<byte, int> idMap = new Dictionary<byte, int>();

        public PropertyHandler(NativeConnector nc, IntPtr pBuffer, IntPtr sBuffer, IntPtr vBuffer, IntPtr eBuffer)
        {
            connector = nc;
            primaryBuffer = pBuffer;
            secondaryBuffer = sBuffer;
            vertexBuffer = vBuffer;
            edgeBuffer = eBuffer;
        }

        public void ReadInitialData()
        {
            ReadPrimaryBuffer();
            ReadSecondaryBuffer();
            for (int i = 0; i < nProperties; i++) ReadProperty();
        }

        public void ReadChanges()
        {
            byte[] localBuff = new byte[4];

            int vertexBuffLen = 0;
            long time;

            NativeMethods.QueryPerformanceCounter(out time);

            bool result = NativeMethods.ReadProcessMemory(connector.processHandle, vertexBuffer, localBuff, 4, out nRead);
            //vertexBuffLen = GetLenght(vertexBuffer);
            vertexBuffLen = localBuff[0] | localBuff[1] << 8 | localBuff[2] << 16 | localBuff[3] << 24;

            Console.WriteLine("Time of read: {0}", time);

            NativeMethods.ReadProcessMemory(connector.processHandle, edgeBuffer, localBuff, 4, out nRead);
            int edgeBuffLen = localBuff[0] | localBuff[1] << 8 | localBuff[2] << 16 | localBuff[3] << 24;

            Console.WriteLine("VertexBuffLen: {0}", vertexBuffLen);

            if(vertexBuffLen > 0)
            {
                localBuff = new byte[vertexBuffLen];
                NativeMethods.ReadProcessMemory(connector.processHandle, vertexBuffer + 4, localBuff, vertexBuffLen, out nRead);
                vertexReader = new BinaryReader(new MemoryStream(localBuff));
                ReadVertexBuffer();
            }
            if(edgeBuffLen > 0)
            {
                localBuff = new byte[edgeBuffLen];
                NativeMethods.ReadProcessMemory(connector.processHandle, edgeBuffer + 4, localBuff, edgeBuffLen, out nRead);
                edgeReader = new BinaryReader(new MemoryStream(localBuff));
                ReadEdgeBuffer();
            }
            Console.WriteLine("------------");
        }

        public void ReadVertexBuffer()
        {
            int read = 0;
            while(vertexReader.BaseStream.Position != vertexReader.BaseStream.Length)
            {
                try
                {
                    int vertexId = vertexReader.ReadInt32();
                    Console.WriteLine("Vertex id: {0}", vertexId);
                    byte propertyId = vertexReader.ReadByte();
                    Console.WriteLine("Property id: {0}", propertyId);
                    object value = ReadValue(propertyDataType[propertyId], vertexReader);
                    Console.WriteLine("Value: {0}", value);
                    connector.SetVertexProperty(vertexId, propertyId, value);
                    read++;
                }
                catch(Exception e)
                {
                    Console.WriteLine("Reading vertex buffer failed, read: {0}", read);
                    Console.WriteLine(e.Message);
                    return;
                }
            }
        }

        public void ReadEdgeBuffer()
        {
            int read = 0;
            while (edgeReader.BaseStream.Position != edgeReader.BaseStream.Length)
            {
                try
                {
                    int edgeId = edgeReader.ReadInt32();
                    byte propertyId = edgeReader.ReadByte();
                    object value = ReadValue(propertyDataType[propertyId], edgeReader);
                    connector.SetEdgeProperty(edgeId, propertyId, value);
                    read++;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Reading edge buffer failed, read: {0}", read);
                    return;
                }
            }
        }

        private void ReadProperty()
        {
            byte type = secondaryReader.ReadByte();
            string name = connector.stringStore.GetString((IntPtr)secondaryReader.ReadInt32());

            byte remoteId = primaryReader.ReadByte();
            int localId = idMap[remoteId] = connector.RegisterProperty(name);
            
            byte dataType = (byte)(type & 0x7f);

            propertyDataType[remoteId] = dataType;

            if (dataType == TYPE_MARKED)
            {
                Property.vertexColorId = localId;
            }

            object val = ReadValue(dataType, primaryReader);
            if ((type & 0x80) == 0) connector.AddPropertyToVertices(localId, val);
            else connector.AddPropertyToEdges(localId, val);
        }

        private object ReadValue(byte dataType, BinaryReader reader)
        {
            if (dataType < TYPE_VISIBLE) return reader.ReadBoolean();

            switch(dataType)
            {
                case 3:
                    return reader.ReadInt32();
                case 4:
                    return reader.ReadInt64();
                case 5:
                    return reader.ReadSingle();
                case 6:
                    return reader.ReadDouble();
                case 7:
                    Console.Write("NYI! Color!");
                    break;
                case 8:
                    return connector.stringStore.GetString((IntPtr)reader.ReadInt32());
            }

            return -137;
        }

        private void ReadPrimaryBuffer()
        {
            byte[] localBuff = new byte[8];
            NativeMethods.ReadProcessMemory(connector.processHandle, primaryBuffer, localBuff, 8, out nRead);

            int lenght = localBuff[0] | localBuff[1] << 8 | localBuff[2] << 16 | localBuff[3] << 24 - 8;
            nProperties = localBuff[4] | localBuff[5] << 8 | localBuff[6] << 16 | localBuff[7] << 24;

            localBuff = new byte[lenght];
            NativeMethods.ReadProcessMemory(connector.processHandle, primaryBuffer + 8, localBuff, lenght, out nRead);

            primaryReader = new BinaryReader(new MemoryStream(localBuff));
        }

        private void ReadSecondaryBuffer()
        {
            byte[] localBuff = new byte[4];
            NativeMethods.ReadProcessMemory(connector.processHandle, secondaryBuffer, localBuff, 4, out nRead);

            int n = localBuff[0] | localBuff[1] << 8 | localBuff[2] << 16 | localBuff[3] << 24;
            if(n!=nProperties)
            {
                Console.WriteLine("Warning! Secondary buffer has different number of properties - {0} as opposed to {1}!", n, nProperties);
            }

            int lenght = 5 * n;
            localBuff = new byte[lenght];
            NativeMethods.ReadProcessMemory(connector.processHandle, secondaryBuffer + 4, localBuff, lenght, out nRead);
            secondaryReader = new BinaryReader(new MemoryStream(localBuff));
        }

        private int GetLenght(IntPtr buff)
        {
            int threadId;
            IntPtr hThread = NativeMethods.CreateRemoteThread(connector.processHandle, IntPtr.Zero, 0, connector.startupBuffer[5], buff, 0, out threadId);

            if (hThread.Equals(0)) return -1;

            NativeMethods.WaitForSingleObject(hThread, 0xFFFFFFFF);

            int result = -1;
            NativeMethods.GetExitCodeThread(hThread, out result);
            return result;
        }
    }
}
