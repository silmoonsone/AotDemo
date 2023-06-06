// ConsoleApplication1.cpp : 此文件包含 "main" 函数。程序执行将在此处开始并结束。
//

#include <iostream>
#include "windows.h"
#include <string.h>
#include <string>
#include <codecvt>
using namespace std;

typedef int (*pAdd)(int x, int y);
typedef void* (*pStrInOut)(const char*, int);
typedef void* (*pStrOut)();
typedef void* (*ptest)();

std::string utf8_to_gbk(const std::string& str)
{
	std::wstring_convert<std::codecvt_utf8<wchar_t> > conv;
	std::wstring tmp_wstr = conv.from_bytes(str);

	//GBK locale name in windows
	const char* GBK_LOCALE_NAME = ".936";
	std::wstring_convert<std::codecvt_byname<wchar_t, char, mbstate_t>> convert(new std::codecvt_byname<wchar_t, char, mbstate_t>(GBK_LOCALE_NAME));
	return convert.to_bytes(tmp_wstr);
}

int main()
{
	std::wstring_convert<std::codecvt_utf8<wchar_t>> convert;

	HMODULE hmod = ::LoadLibraryA(R"(D:\repos\AOTDemo\AOTDemo\bin\Release\net7.0\win-x64\publish\AOTDemo.dll)");
	if (hmod == NULL)
	{
		std::cout << "LoadLibrary failed" << std::endl;
		return 0;
	}
	else
	{
		pAdd Add = (pAdd)GetProcAddress(hmod, "Add");
		pStrInOut strInOut = (pStrInOut)GetProcAddress(hmod, "strInOut");
		pStrOut strOut = (pStrOut)GetProcAddress(hmod, "strOut");
		ptest test = (pStrOut)GetProcAddress(hmod, "test");

		const char* inputStr = u8"你好SILMOON。";
		int len = strlen(inputStr);

		int nRet = Add(1, 2);
		void* rStrInOut = strInOut(inputStr, len);
		void* rStrOut = strOut();
		void* rTest = test();

		std::string pStr_StrInOut = utf8_to_gbk((char*)rStrInOut).c_str();
		std::string pStr_StrOut = utf8_to_gbk((char*)rStrOut).c_str();
		std::string pStr_Test = utf8_to_gbk((char*)rTest).c_str();


		printf("1: %s\r\n", pStr_StrInOut.c_str());
		printf("2: %s\r\n", pStr_StrOut.c_str());
		printf("3: %s\r\n", pStr_Test.c_str());

		//char c[6];



		std::cout << "data" << std::endl;
	}
}

// 运行程序: Ctrl + F5 或调试 >“开始执行(不调试)”菜单
// 调试程序: F5 或调试 >“开始调试”菜单

// 入门使用技巧: 
//   1. 使用解决方案资源管理器窗口添加/管理文件
//   2. 使用团队资源管理器窗口连接到源代码管理
//   3. 使用输出窗口查看生成输出和其他消息
//   4. 使用错误列表窗口查看错误
//   5. 转到“项目”>“添加新项”以创建新的代码文件，或转到“项目”>“添加现有项”以将现有代码文件添加到项目
//   6. 将来，若要再次打开此项目，请转到“文件”>“打开”>“项目”并选择 .sln 文件
