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
            Console.WriteLine($"Add result is {result}");

            //result = Add2(1, 2);
            //Console.WriteLine($"Add2 result is {result}");

            var result1 = strOut();
            string ret1 = Marshal.PtrToStringUTF8(result1);
            Console.WriteLine(ret1);

            Console.WriteLine("================");

            var data = Encoding.UTF8.GetBytes("hello你好哈哈哈哈000");

            {
                byte* p = stackalloc byte[data.Length];
                //for (int i = 0; i < data.Length; i++)
                //{
                //    p[i] = data[i];
                //}
                //data.AsSpan().CopyTo(new Span<byte>(p, data.Length));
                Marshal.Copy(data, 0, (IntPtr)p, data.Length);
                var result2 = strInOut(p, data.Length);
                string ret2 = Marshal.PtrToStringUTF8(result2);
                Console.WriteLine(ret2);
            }
            Console.WriteLine("================");

            fixed (byte* p = data)
            {
                var result2 = strInOut(p, data.Length);
                string ret2 = Marshal.PtrToStringUTF8(result2);
                Console.WriteLine(ret2);
            }
        }
    }
}