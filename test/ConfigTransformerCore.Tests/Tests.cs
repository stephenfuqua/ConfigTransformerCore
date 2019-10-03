using System;
using System.IO;
using System.Reflection;
using CommandLine;
using ConfigTransformerCore.Tests.Properties;
using FakeItEasy;
using Microsoft.Web.XmlTransform;
using NUnit.Framework;
using Shouldly;

namespace ConfigTransformerCore.Tests
{
    [TestFixture]
    public class Tests
    {
        [TearDown]
        public void TearDown()
        {
            Directory.Delete(GetTestFileDirectory(), true);
        }

        [Test]
        public void Given_base_web_config_when_using_release_transform_then_file_should_be_transformed()
        {
            // Arrange
            var srcPath = CreateATestFile("Web.config", Resources.Web);
            var transformPath = CreateATestFile("Web.Release.config", Resources.Web_Release);
            var finalPath = GetTestFilePath("Web.final.config");
            var expected = Resources.Web_final;
            var arguments = CreateArguments(srcPath, transformPath, finalPath);

            // Act
            Program.Main(arguments);

            // Assert
            finalPath.ShouldSatisfyAllConditions(
                () => File.Exists(finalPath).ShouldBeTrue(),
                () => ReadFile(finalPath).ShouldBe(expected)
            );
        }

        [Test]
        public void Given_source_config_does_not_exist_when_using_release_transform_then_should_display_error_and_exit_with_code()
        {
            // Arrange
            var srcPath = CreateATestFile("Web.config", Resources.Web);
            var transformPath = CreateATestFile("Web.Release.config", Resources.Web_Release);
            var finalPath = GetTestFilePath("Web.final.config");
            var arguments = CreateArguments(srcPath, transformPath, finalPath);

            var logger = A.Fake<IXmlTransformationLogger>();
            var filesystem = A.Fake<IFilesystemAdapter>();
            Program.Logger = logger;
            Program.FilesystemAdapter = filesystem;

            A.CallTo(() => filesystem.FileExist(srcPath)).Returns(false);

            // Act
            Program.Main(arguments);

            // Assert
            finalPath.ShouldSatisfyAllConditions(
                () => File.Exists(finalPath).ShouldBeFalse(),
                () => A.CallTo(() => logger.LogError(A<string>._)).MustHaveHappened(),
                () => Environment.ExitCode.ShouldBe(-1)
            );
        }

        [Test]
        public void Given_source_config_exists_when_using_release_transform_that_does_not_exist_then_should_display_error_and_exit_with_code()
        {
            // Arrange
            var srcPath = CreateATestFile("Web.config", Resources.Web);
            var transformPath = CreateATestFile("Web.Release.config", Resources.Web_Release);
            var finalPath = GetTestFilePath("Web.final.config");
            var arguments = CreateArguments(srcPath, transformPath, finalPath);

            var logger = A.Fake<IXmlTransformationLogger>();
            var filesystem = A.Fake<IFilesystemAdapter>();
            Program.Logger = logger;
            Program.FilesystemAdapter = filesystem;

            A.CallTo(() => filesystem.FileExist(srcPath)).Returns(true);
            A.CallTo(() => filesystem.FileExist(transformPath)).Returns(false);

            // Act
            Program.Main(arguments);

            // Assert
            finalPath.ShouldSatisfyAllConditions(
                () => File.Exists(finalPath).ShouldBeFalse(),
                () => A.CallTo(() => logger.LogError(A<string>._)).MustHaveHappened(),
                () => Environment.ExitCode.ShouldBe(-1)
            );
        }

        [Test]
        public void Given_source_config_exists_when_using_bad_transform_then_display_errors_and_exit_with_code()
        {
            // Arrange
            var srcPath = CreateATestFile("WarningsAndErrors_source.xml", Resources.WarningsAndErrors_source);
            var transformPath = CreateATestFile("WarningsAndErrors_transform.xml", Resources.WarningsAndErrors_transform);
            var finalPath = GetTestFilePath("Web.final.config");
            var arguments = CreateArguments(srcPath, transformPath, finalPath);

            var logger = A.Fake<IXmlTransformationLogger>();
            var filesystem = A.Fake<IFilesystemAdapter>();
            Program.Logger = logger;
            Program.FilesystemAdapter = filesystem;

            A.CallTo(() => filesystem.FileExist(srcPath)).Returns(true);
            A.CallTo(() => filesystem.FileExist(transformPath)).Returns(true);

            // Act
            Program.Main(arguments);

            // Assert
            finalPath.ShouldSatisfyAllConditions(
                () => File.Exists(finalPath).ShouldBeFalse(),
                () => Environment.ExitCode.ShouldBe(-1)
                // Don't check logging - trust that the XDT tranformation has already been well tested in this regard.
            );

        }


        private string CreateATestFile(string filename, string contents)
        {
            // Copied from original Microsoft unit tests
            string file = GetTestFilePath(filename);
            File.WriteAllText(file, contents);
            return file;
        }

        private string GetTestFilePath(string filename)
        {
            // Copied from original Microsoft unit tests, and modified
            var folder = GetTestFileDirectory();
            Directory.CreateDirectory(folder);
            string file = Path.Combine(folder, filename);
            return file;
        }

        private string GetTestFileDirectory()
        {
            // Copied from original Microsoft unit tests, and modified
            Uri asm = new Uri(typeof(Tests).GetTypeInfo().Assembly.CodeBase, UriKind.Absolute);
            string dir = Path.GetDirectoryName(asm.LocalPath);
            string folder = Path.Combine(dir, "testfiles");
            return folder;
        }

        private string ReadFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        private string[] CreateArguments(string srcPath, string transformPath, string finalPath)
        {
            var options = new Options
            {
                DestinationFile = finalPath,
                SourceFile = srcPath,
                TransformFile = transformPath
            };
            var arguments = Parser.Default.FormatCommandLine(options, config => config.GroupSwitches = true).Split(" ");

            return arguments;
        }
    }
}