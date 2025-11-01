using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Program
{
    /// <summary>
    /// 导入 AOT 编译 DLL 的测试程序
    /// </summary>
    public unsafe partial class Program
    {
        #region DLL 路径配置

        private const string DllPath = @"..\..\..\..\DotnetAot\bin\Debug\net9.0\publish\win-x64\DotnetAot.dll";

        #endregion

        #region DLL 函数导入声明

        // 基础数学运算函数
        [LibraryImport(DllPath, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal static partial int add(int a, int b);

        // 字符串函数
        [DllImport(DllPath)]
        public static extern IntPtr strOut();

        [DllImport(DllPath)]
        public static extern IntPtr strInOut(byte* data, int len);

        // 回调相关函数
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int CallbackDelegate(int a, int b);

        [DllImport(DllPath)]
        public static extern int callBack(IntPtr callback, int a, int b);

        [DllImport(DllPath)]
        public static extern int callBackStatic(IntPtr callback, int a, int b);

        #endregion


        #region 主程序入口

        public static void Main()
        {
            Console.WriteLine("==========================================");
            Console.WriteLine("  AOT DLL 调用测试程序");
            Console.WriteLine("==========================================\n");

            RunAllTests();
        }

        #endregion

        #region 测试方法

        /// <summary>
        /// 运行所有测试
        /// </summary>
        private static void RunAllTests()
        {
            TestAddFunction();
            TestCallback();
            TestStrOut();
            TestStrInOut();
        }

        /// <summary>
        /// 测试 add 函数
        /// </summary>
        private static void TestAddFunction()
        {
            Console.WriteLine("========== 测试 add 函数 ==========");

            int a = 15;
            int b = 27;
            int expected = a + b;
            int result = add(a, b);

            Console.WriteLine($"输入: add({a}, {b})");
            Console.WriteLine($"期望结果: {expected}");
            Console.WriteLine($"实际结果: {result}");
            Console.WriteLine($"测试结果: {(result == expected ? "√ 通过" : "× 失败")}\n");
        }

        /// <summary>
        /// 测试回调功能
        /// </summary>
        private static void TestCallback()
        {
            Console.WriteLine("========== 测试回调功能 ==========");

            var testClassInstance = new TestClass();

            // 测试实例方法回调 TestMethodB
            CallbackDelegate callbackWrapper = testClassInstance.TestMethod;
            IntPtr callbackBPtr = Marshal.GetFunctionPointerForDelegate(callbackWrapper);
            int resultB = callBack(callbackBPtr, 10, 20);
            Console.WriteLine($"callBackB(TestMethodB): {resultB} {(resultB == 30 ? "√" : "×")}\n");

            // 测试静态方法回调 TestMethodC
            CallbackDelegate callbackStaticWrapper = TestClass.TestMethodStatic;
            IntPtr callbackCPtr = Marshal.GetFunctionPointerForDelegate(callbackStaticWrapper);
            int resultC = callBackStatic(callbackCPtr, 30, 40);
            Console.WriteLine($"callBackC(TestMethodC): {resultC} {(resultC == 70 ? "√" : "×")}\n");
        }

        /// <summary>
        /// 测试 strOut 函数
        /// </summary>
        private static void TestStrOut()
        {
            Console.WriteLine("========== 测试 strOut 函数 ==========");

            try
            {
                IntPtr result = strOut();
                string output = Marshal.PtrToStringUTF8(result) ?? "";
                Console.WriteLine($"输出字符串: {output}");
                Console.WriteLine("√ 测试完成\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"× 测试失败: {ex.Message}\n");
            }
        }

        /// <summary>
        /// 测试 strInOut 函数
        /// </summary>
        private static void TestStrInOut()
        {
            Console.WriteLine("========== 测试 strInOut 函数 ==========");

            try
            {
                string testData = "Hello world, 你好世界，0x0。";
                byte[] data = Encoding.UTF8.GetBytes(testData);

                // 方式 1: 使用 stackalloc
                Console.WriteLine("--- 方式 1: 使用 stackalloc ---");
                byte* p0 = stackalloc byte[data.Length];
                Marshal.Copy(data, 0, (IntPtr)p0, data.Length);
                IntPtr result0 = strInOut(p0, data.Length);
                string output0 = Marshal.PtrToStringUTF8(result0) ?? "";
                Console.WriteLine($"输入: {testData}");
                Console.WriteLine($"输出: {output0}");

                // 方式 2: 使用 fixed
                Console.WriteLine("\n--- 方式 2: 使用 fixed ---");
                fixed (byte* p1 = data)
                {
                    IntPtr result1 = strInOut(p1, data.Length);
                    string output1 = Marshal.PtrToStringUTF8(result1) ?? "";
                    Console.WriteLine($"输入: {testData}");
                    Console.WriteLine($"输出: {output1}");
                }

                Console.WriteLine("√ 测试完成\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"× 测试失败: {ex.Message}\n");
            }
        }

        #endregion

        #region 测试辅助类

        /// <summary>
        /// 用于回调测试的测试类
        /// </summary>
        public class TestClass
        {

            public int TestMethod(int a, int b)
            {
                Console.WriteLine($"TestClass: TestMethodB 被调用，参数: a={a}, b={b}");
                return a + b;
            }

            public static int TestMethodStatic(int a, int b)
            {
                Console.WriteLine($"TestClass: TestMethodC (静态) 被调用，参数: a={a}, b={b}");
                return a + b;
            }
        }

        #endregion
    }
}