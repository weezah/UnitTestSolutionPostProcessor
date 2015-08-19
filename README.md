# UnitTestSolutionPostProcessor
Adds a UnitTest project to the Unity generated solution every time unity imports assets.
Working with Unity and Unit tests can be boring as every time a new file is added through Unity the sln file is re-generated and the refereces to other projects are lost. This script is meant to solve this problem.

### Usage ###

Place the script on your Editor folder and then set the solution and project parameters

```csharp
	// your generated solution file name
	private const string SOLUTION_FILENAME = "MyUnityProject.sln";
	
	// unit test project folder
	private const string UNIT_TEST_PRJ_PATH = "UnitTest";
	
	// unit test project file
	private const string UNIT_TEST_PRJ_FILENAME = "UnitTest.csproj";
	
	// unit test project name (as will appear in the solution)
	private const string UNIT_TEST_PRJ_NAME = "UnitTests";
```
*In this example I have a Unity project called "MyUnityProject" and I've placed the Unit test project in a subfolder "UnitTest/UnitTest.csproj"*
 
