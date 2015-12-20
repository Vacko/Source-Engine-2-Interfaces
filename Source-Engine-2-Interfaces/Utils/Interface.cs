using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Get_Interfaces.Models;

namespace Get_Interfaces.Utils
{
    public static class Interface
    {
        #region Static Methods

        public static void GetInterface(Process process, string processModule)
        {
            ProcessModule module = null;

            Console.WriteLine(process.Modules.Count);

            foreach (ProcessModule proc in process.Modules)
                if (proc.ModuleName == processModule)
                    module = proc;

            IntPtr loadModule = MemoryApi.LoadLibraryEx(module.FileName, IntPtr.Zero,
                MemoryApi.LoadLibraryFlags.DontResolveDllReferences);

            IntPtr createInterface = MemoryApi.GetProcAddress(loadModule, "CreateInterface");
            IntPtr mInterface = createInterface;

            mInterface = IntPtr.Add(mInterface, 5);
            mInterface = IntPtr.Add(mInterface, Marshal.ReadInt32(mInterface) + 4);
            mInterface = IntPtr.Add(mInterface, 6);
            mInterface = new IntPtr(Marshal.ReadInt32(mInterface) - loadModule.ToInt32());

            IntPtr moduleInterface =
                Memory.ReadIntPtr(process, IntPtr.Add(mInterface, module.BaseAddress.ToInt32()));

            while (moduleInterface != IntPtr.Zero)
            {
                string interfaceName = Memory.ReadString(process,
                    Memory.ReadIntPtr(process, IntPtr.Add(moduleInterface, 0x04)));

                IntPtr interfacePointer = Memory.ReadIntPtr(process,
                    IntPtr.Add(Memory.ReadIntPtr(process, moduleInterface), 1));

                if (interfaceName != string.Empty)
                    Console.WriteLine($"{interfaceName}[0x{interfacePointer.ToString("X")}]");

                moduleInterface = Memory.ReadIntPtr(process, IntPtr.Add(moduleInterface, 0x08));
            }
        }

        #endregion
    }
}