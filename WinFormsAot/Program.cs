using System.Runtime.InteropServices;

namespace WinFormsApp1
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
        [UnmanagedCallersOnly(EntryPoint = "Add")]
        public static int Add(int a, int b)
        {
            return a + b;
        }
    }
}