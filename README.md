# UnitTestSolutionPostProcessor
Adds a UnitTest project to the Unity generated solution

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
 
