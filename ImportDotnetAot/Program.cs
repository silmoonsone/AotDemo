using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Program
{
    public unsafe partial class Program
    {
        const string DllPath = @"..\..\..\..\DotnetAot\bin\Debug\net9.0\publish\win-x64\DotnetAot.dll";
        //const string DllPath2 = @"..\..\..\..\x64\Release\CppDll.dll";

        [LibraryImport(DllPath, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal static partial int Add(int a, int b);

        //[DllImport(DllPath2)]
        //public static extern int Add2(int a, int b);

        [DllImport(DllPath)]
        public static extern IntPtr strOut();

        [DllImport(DllPath)]
        public static extern IntPtr strInOut(byte* data, int len);

        [DllImport(DllPath)]
        public static extern IntPtr jsonIn(string json);


        public static void Main()
        {
            var result = Add(1, 2);
            Console.WriteLine(result);

            //result = Add2(1, 2);
            //Console.WriteLine(result);

            var result1 = strOut();
            string ret1 = Marshal.PtrToStringUTF8(result1);
            Console.WriteLine(ret1);


            var data = Encoding.UTF8.GetBytes("hello你好哈哈哈哈000");

            {
                byte* p = stackalloc byte[data.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    p[i] = data[i];
                }
                var result2 = strInOut(p, data.Length);
                string ret2 = Marshal.PtrToStringUTF8(result2);
                Console.WriteLine(ret2);
            }

            fixed (byte* p = data)
            {
                var result2 = strInOut(p, data.Length);
                string ret2 = Marshal.PtrToStringUTF8(result2);
                Console.WriteLine(ret2);
            }
        }
    }
}