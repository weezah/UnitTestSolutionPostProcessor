// Author:
//       weezah <ts_matt@hotmail.com>
//
// Copyright (c) 2015 weezah
using UnityEditor;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;

public class UnitTestSolutionPostProcessor : AssetPostprocessor
{
	// solution file name
	private const string SOLUTION_FILENAME = "MyUnityProject.sln";
	
	// unit test project folder
	private const string UNIT_TEST_PRJ_PATH = "UnitTest";
	
	// unit test project file
	private const string UNIT_TEST_PRJ_FILENAME = "UnitTest.csproj";
	
	// unit test project name (as will appear in the solution)
	private const string UNIT_TEST_PRJ_NAME = "UnitTests";


	private const string MSBUILD_SCHEMA = "http://schemas.microsoft.com/developer/msbuild/2003"; // ms build schema needed for XmlNamespaceManager
	private const string PRJ_TEMPLATE = "Project(\"{0}\") = \"{1}\", \"{2}\", \"{3}\"\r\nEndProject\r\n"; //slnGuid, prjName, prjFullPath, prjGuid
	private const string BUILD_CONFIG_LINE = "GlobalSection(ProjectConfigurationPlatforms) = postSolution";
	private const string BUILD_CONFIG_TEMPLATE = "\r\n\t\t{0}.Debug|Any CPU.ActiveCfg = Debug|Any CPU\r\n\t\t{0}.Debug|Any CPU.Build.0 = Debug|Any CPU"; //prjGuid

	private static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		string slnFileContent;
		string utGuid;
		string slnGuid;
		string currentDir = Directory.GetCurrentDirectory ();
		string slnPath = Path.Combine (currentDir, SOLUTION_FILENAME);
		string utFullPath = Path.Combine (UNIT_TEST_PRJ_PATH, UNIT_TEST_PRJ_FILENAME);

		try {

			// check if project is already in the sln
			slnFileContent = File.ReadAllText (slnPath);	
			Regex prjRegex = new Regex (UNIT_TEST_PRJ_FILENAME);
			if (prjRegex.IsMatch (slnFileContent))
				return;

			// get unit test project guid
			XmlDocument unitTestPrj = new XmlDocument ();
			unitTestPrj.LoadXml (File.ReadAllText (Path.Combine (currentDir, utFullPath)));
			XmlNamespaceManager nsmgr = new XmlNamespaceManager (unitTestPrj.NameTable);
			nsmgr.AddNamespace ("ms", MSBUILD_SCHEMA);
			utGuid = unitTestPrj.SelectSingleNode ("//ms:ProjectGuid", nsmgr).InnerText;

			// get slnGuid
			Regex slnGuidRegex = new Regex ("Project[(]\"(.*)\"[)]");
			var m = slnGuidRegex.Match (slnFileContent);
			slnGuid = m.Groups [1].ToString ();

			// inject project
			int idx = slnFileContent.IndexOf ("Global");
			slnFileContent = slnFileContent.Insert (idx, string.Format (PRJ_TEMPLATE, slnGuid, UNIT_TEST_PRJ_NAME, utFullPath, utGuid));

			// inject build config
			idx = slnFileContent.IndexOf (BUILD_CONFIG_LINE) + BUILD_CONFIG_LINE.Length;
			slnFileContent = slnFileContent.Insert (idx, string.Format (BUILD_CONFIG_TEMPLATE, utGuid));

			File.WriteAllText (slnPath, slnFileContent);

		} catch (Exception ex) {
			UnityEngine.Debug.LogErrorFormat ("UnitTestSolutionPostProcessor: {0}\n{1}", ex.Message, ex.StackTrace);
		}
	}
}
