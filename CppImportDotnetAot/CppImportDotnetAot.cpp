#include <iostream>
#include "windows.h"
#include <string.h>
#include <string>
#include <codecvt>
#include <filesystem>
#include <iostream>
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

	char libPath[] = R"(..\DotnetAot\bin\Debug\net8.0\publish\win-x64\DotnetAot.dll)";

	FILE* file;
	fopen_s(&file, libPath, "rb");
	if (file == NULL)
	{
		char fullPath[MAX_PATH];
		DWORD len = GetFullPathNameA(libPath, MAX_PATH, fullPath, NULL);
		cout << "fullPath: " << fullPath << endl;
		cout << "the dll not exists." << endl;
		return 0;
	}
	fclose(file);




	HMODULE hmod = ::LoadLibraryA(libPath);
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
		size_t len = strlen(inputStr);

		int nRet = Add(1, 2);
		void* rStrInOut = strInOut(inputStr, len);
		void* rStrOut = strOut();
		void* rTest = test();

		string pStr_StrInOut = utf8_to_gbk((char*)rStrInOut).c_str();
		string pStr_StrOut = utf8_to_gbk((char*)rStrOut).c_str();
		string pStr_Test = utf8_to_gbk((char*)rTest).c_str();

		printf("1: Add%d\r\n", nRet);
		printf("2: %s\r\n", pStr_StrInOut.c_str());
		printf("3: %s\r\n", pStr_StrOut.c_str());
		printf("4: %s\r\n", pStr_Test.c_str());

		std::cout << "data" << std::endl;
	}
}
