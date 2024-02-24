using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Program
{
    public unsafe partial class Program
    {
        [LibraryImport("AOTDemo.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal static partial int Add(int a, int b);


        [DllImport(@"D:\repos\AOTDemo\AOTDemo\bin\Release\net8.0\win-x64\publish\AOTDemo.dll")]
        public static extern IntPtr strOut();

        [DllImport(@"D:\repos\AOTDemo\AOTDemo\bin\Release\net8.0\win-x64\publish\AOTDemo.dll")]
        public static extern IntPtr strInOut(byte* data, int len);


        public static void Main()
        {
            //var result = Add(1, 2);
            //Console.WriteLine(result);

            var result1 = strOut();
            string ret1 = Marshal.PtrToStringUTF8(result1);
            Console.WriteLine(ret1);


            var data = Encoding.UTF8.GetBytes("hello你好哈哈哈哈000");
            fixed (byte* p = data)
            {
                var result2 = strInOut(p, data.Length);
                string ret2 = Marshal.PtrToStringUTF8(result2);
                Console.WriteLine(ret2);
            }

        }
    }
}