using System;
using System.Diagnostics;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Framework;
using NUnit.Framework;

namespace Jatech.NUnit.Test
{
    public abstract class BuildEngineTest
    {
#pragma warning disable 0618

        protected BuildEngineTest()
        {
            Verbosity = LoggerVerbosity.Normal;
        }

        protected Engine Engine { get; private set; }

        protected string DefaultTargets { get; set; }
        protected LoggerVerbosity Verbosity { get; set; }

        [SetUp]
        public virtual void SetUp()
        {
            //Console.WriteLine("Engine SetUp");
            Engine = new Engine();
            Engine.DefaultToolsVersion = "4.0";

            WriteEngineVersions();
            RegisterEngineLoggers();

            /*
            project = engine.CreateNewProject();
            project.DefaultTargets = "Build";
            var buildTarget = project.Targets.AddNewTarget("Build");
            */
        }

        protected virtual void RegisterEngineLoggers()
        {
            var logger = new ConsoleLogger();
            logger.Verbosity = Verbosity;
            Engine.RegisterLogger(logger);
        }

        [Conditional("DEBUG")]
        protected void WriteEngineVersions()
        {
            Debug.WriteLine("Engine.Version={0}", Engine.Version);
            Debug.WriteLine("Engine.DefaultToolsVersion={0}", Engine.DefaultToolsVersion);
        }

        [TearDown]
        public virtual void TearDown()
        {
            //Console.WriteLine("Engine TearDown");
            Engine.UnloadAllProjects();
            Engine.UnregisterAllLoggers();
            Engine.Shutdown();
            Engine = null;
        }

        protected void AssertProjectBuild(Project project, bool expectedResult = true)
        {           
            Assert.AreEqual(expectedResult, project.Build(DefaultTargets), "MSBuild failed.");
        }

#pragma warning restore 0618
    }
}