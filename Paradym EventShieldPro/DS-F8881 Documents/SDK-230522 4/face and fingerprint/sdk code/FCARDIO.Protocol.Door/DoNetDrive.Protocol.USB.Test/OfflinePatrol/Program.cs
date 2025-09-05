using System;
using System.Reflection;
using System.Windows.Forms;

namespace DoNetDrive.Protocol.USB.OfflinePatrol.Test
{
    public class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }

        private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string sPath = AppDomain.CurrentDomain.BaseDirectory;
            string sDllName = args.Name.Split(',')[0];
            sPath = System.IO.Path.Combine(sPath, "DLL", $"{sDllName}.dll");
            if (System.IO.File.Exists(sPath))
            {
                return Assembly.LoadFrom(sPath);
            }
            else
                return null;

        }
    }
}
