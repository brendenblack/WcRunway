using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WcRunway.Cli.Features.Generate;
using Xunit;

namespace WcRunway.Cli.Tests.Features.Generate.GenerateHandlerTests
{
    public class CreateOutputDirectory_Should : IClassFixture<GenerateHandlerFixture>
    {
        private readonly GenerateHandlerFixture fixture;
        private readonly GenerateHandler sut;

        public CreateOutputDirectory_Should(GenerateHandlerFixture fixture)
        {
            this.fixture = fixture;
            this.sut = fixture.Handler;
        }
        
        [Fact]
        public void UseCurrentDirectoryWhenNoneSpecified()
        {
            var expectedPath = Path.Combine(Environment.CurrentDirectory, "Test");

            this.sut.CreateOutputDirectory("", "Test");

            Directory.Exists(expectedPath).ShouldBe(true);

            Directory.Delete(expectedPath);
        }

        [Theory]
        [InlineData("XKWF:\\")]
        [InlineData("??!:<>")]
        public void ThrowWhenDirectoryInvalid(string invalidPath)
        {
            Should.Throw<IOException>(() => this.sut.CreateOutputDirectory(invalidPath, "Test"));
        }


        // Reluctant to write a test that isn't portable by specifying a directory to use
        //[Fact]
        //public void UseSpecifiedDirectoryWhenSpecified()
        //{

        //}
    }
}
