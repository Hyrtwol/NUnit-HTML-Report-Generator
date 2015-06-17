using System.Diagnostics;
using System.IO;
using NUnit.Framework;

namespace Jatech.NUnit.Test
{
    [TestFixture]
    public class NUnitHtmlReportGeneratorTests : BuildEngineTest
    {
        [TestCase(@"TestData\ExampleResults.xml")]
        public void GenerateHtmlFromNUnitTestResult(string inputFileName)
        {
            var project = Engine.CreateNewProject();

            project.DefaultTargets = "Build";

            //project.AddNewUsingTaskFromAssemblyName<NUnitHtmlReportGenerator>();
            var taskType = typeof(NUnitHtmlReportGenerator);
            project.AddNewUsingTaskFromAssemblyName(taskType.Name, taskType.Assembly.FullName);
            Assert.IsNotNull(project.Targets);

            var buildTarget = project.Targets.AddNewTarget("Build");

            var task = buildTarget.AddNewTask("NUnitHtmlReportGenerator");
            task.SetParameterValue("InputFileName", inputFileName);
            task.SetParameterValue("OverwriteOutput", true.ToString());

            Debug.WriteLine(project.Xml);
            //Assert.IsTrue(project.Build(), "MSBuild failed.");
            AssertProjectBuild(project);

            Assert.IsTrue(File.Exists(@"TestData\ExampleResults.html"), "ExampleResults.html not found");
        }
    }
}