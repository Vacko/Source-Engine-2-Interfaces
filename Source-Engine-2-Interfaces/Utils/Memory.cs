using System;
using System.Diagnostics;
using System.Text;
using Get_Interfaces.Models;

namespace Get_Interfaces.Utils
{
    public static class Memory
    {
        #region Static Methods

        private static int ReadInt(Process proc, IntPtr offset)
        {
            byte[] buf = new byte[4];
            MemoryApi.ReadProcessMemory(proc.Handle, offset, buf, 4, out int _);
            return BitConverter.ToInt32(buf, 0);
        }

        public static IntPtr ReadIntPtr(Process proc, IntPtr offset) => new IntPtr(ReadInt(proc, offset));

        public static string ReadString(Process proc, IntPtr offset)
        {
            byte[] buf = new byte[128];
            MemoryApi.ReadProcessMemory(proc.Handle, offset, buf, 128, out int _);

            int endIndex = 0;

            for (int i = 0; i < buf.Length; i++)
            {
                if (buf[i] != '\0') continue;
                endIndex = i;
                break;
            }

            return Encoding.ASCII.GetString(buf, 0, endIndex);
        }

        #endregion
    }
}