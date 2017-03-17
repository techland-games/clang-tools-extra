This directory contains a VSPackage project to generate a Visual Studio extension
for clang-tidy.

Build prerequisites are:
- Visual Studio 2015 Professional
- Visual Studio 2015 SDK

The extension is built using CMake by setting BUILD_CLANG_TIDY_VS_PLUGIN=ON
when configuring a Clang build, and building the clang_tidy_vsix target.

The CMake build will copy clang-tidy.exe and LICENSE.TXT into the ClangTidy/
directory so they can be bundled with the plug-in, as well as creating
ClangTidy/source.extension.vsixmanifest. Once the plug-in has been built with
CMake once, it can be built manually from the ClangTidy.sln solution in Visual
Studio.

===========
 Debugging
===========

Open ClangFormat.sln in Visual Studio, then:

- Make sure the "Debug" target is selected
- Open the ClangTidy project properties
- Select the Debug tab
- Set "Start external program:" to where your devenv.exe is installed. Typically
  it's "C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe"
- Set "Command line arguments" to: /rootsuffix Exp
- You can now set breakpoints if you like
- Press F5 to build and run with debugger

If all goes well, a new instance of Visual Studio will be launched in a special
mode where it uses the experimental hive instead of the normal configuration hive.
By default, when you build a VSIX project in Visual Studio, it auto-registers the
extension in the experimental hive, allowing you to test it. In the new Visual Studio
instance, open or create a C++ solution, and you should now see the Clang Format
entries in the Tool menu. You can test it out, and any breakpoints you set will be
hit where you can debug as usual.
