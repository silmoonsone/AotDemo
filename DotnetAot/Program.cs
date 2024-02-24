using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Program
{
    public class Program
    {
        [UnmanagedCallersOnly(EntryPoint = "Add")]
        public static int Add(int a, int b)
        {
            return a + b;
        }
        [UnmanagedCallersOnly(EntryPoint = "strInOut")]
        public unsafe static char* strInOut(char* data, int len)
        {
            string str = new string((sbyte*)data, 0, len, Encoding.UTF8);
            Console.WriteLine("c# strInOut input string: " + str);
            return data;
        }
        [UnmanagedCallersOnly(EntryPoint = "strOut")]
        public unsafe static byte* strOut()
        {
            var str = "当前是在C# AOT中产生的，DLL直接输出String。";
            var strData = Encoding.UTF8.GetBytes(str);
            var newStr = Encoding.UTF8.GetString(strData);

            Console.WriteLine("c# strOut string: " + newStr);

            fixed (byte* p = strData)
            {
                return p;
            }
        }

        [UnmanagedCallersOnly(EntryPoint = "test")]
        public unsafe static char* test()
        {
            string str = "Test: 你好世界。";
            var strData = Encoding.UTF8.GetBytes(str);
            fixed (byte* p = strData)
            {
                return (char*)p;
            }
        }
    }
}