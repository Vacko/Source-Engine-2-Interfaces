using System;
using System.Diagnostics;
using System.Linq;
using Get_Interfaces.Utils;

namespace Get_Interfaces
{
    internal static class Program
    {
        #region Fields

        private static readonly string[] Modules =
            {"client.dll", "engine2.dll", "vstdlib.dll", "vgui2.dll", "gameui2.dll", "vguirendersurface.dll"};

        #endregion

        #region Static Methods

        private static void Main(string[] args)
        {
            Process processApp = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == "dota2");

            if (processApp != default(Process))
                foreach (string module in Modules)
                {
                    Console.WriteLine($"----------------------------");
                    Console.WriteLine($"-----Module {module}-----");
                    Console.WriteLine($"----------------------------");
                    Interface.GetInterface(processApp, module);
                }

            Console.WriteLine($"Press key to exit console.");
            Console.ReadLine();
        }

        #endregion
    }
}