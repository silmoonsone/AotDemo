using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Program
{
    /// <summary>
    /// AOT 编译的 .NET 库，提供可被非托管代码调用的函数
    /// </summary>
    public class Program
    {
        #region 基础数学运算函数

        /// <summary>
        /// 简单的加法运算
        /// </summary>
        [UnmanagedCallersOnly(EntryPoint = "add")]
        public static int add(int a, int b)
        {
            return a + b;
        }

        #endregion

        #region 字符串输出函数

        /// <summary>
        /// 输出字符串（返回 UTF-8 编码的字节指针）
        /// </summary>
        [UnmanagedCallersOnly(EntryPoint = "strOut")]
        public unsafe static byte* strOut()
        {
            var str = "当前是在C# AOT中产生的，DLL直接输出String。";
            var strData = Encoding.UTF8.GetBytes(str);
            var newStr = Encoding.UTF8.GetString(strData);

            Console.WriteLine("DotnetAot: strOut newStr: " + newStr);

            fixed (byte* p = strData)
            {
                return p;
            }
        }

        #endregion

        #region 字符串输入输出函数

        /// <summary>
        /// 输入输出字符串（接收 UTF-8 编码的字符指针，返回相同指针）
        /// </summary>
        [UnmanagedCallersOnly(EntryPoint = "strInOut")]
        public unsafe static char* strInOut(char* data, int len)
        {
            string str = new string((sbyte*)data, 0, len, Encoding.UTF8);
            Console.WriteLine("DotnetAot: strInOut 接收到字符串: " + str);

            return data;
        }

        #endregion

        #region 回调函数

        /// <summary>
        /// 调用回调 B - 直接接受函数指针参数（用于调用实例方法 TestMethodB）
        /// </summary>
        [UnmanagedCallersOnly(EntryPoint = "callBack")]
        public unsafe static int callBack(delegate* unmanaged<int, int, int> callback, int a, int b)
        {
            if (callback != null)
            {
                int result = callback(a, b);
                Console.WriteLine($"DotnetAot: callBackB 调用成功，参数: a={a}, b={b}, 返回值={result}");
                return result;
            }
            Console.WriteLine("DotnetAot: callBackB 回调指针为空");
            return -1;
        }

        /// <summary>
        /// 调用回调 C - 直接接受函数指针参数（用于调用静态方法 TestMethodC）
        /// </summary>
        [UnmanagedCallersOnly(EntryPoint = "callBackStatic")]
        public unsafe static int callBackStatic(delegate* unmanaged<int, int, int> callback, int a, int b)
        {
            if (callback != null)
            {
                int result = callback(a, b);
                Console.WriteLine($"DotnetAot: callBackC 调用成功，参数: a={a}, b={b}, 返回值={result}");
                return result;
            }
            Console.WriteLine("DotnetAot: callBackC 回调指针为空");
            return -1;
        }

        #endregion
    }
}