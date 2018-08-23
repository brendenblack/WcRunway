//using Shouldly;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Text;
//using WcRunway.Cli.Features.Generate;
//using Xunit;

//namespace WcRunway.Cli.Tests.Features.Generate.GenerateHandlerTests
//{
//    public class WriteCohort_Should : IClassFixture<GenerateHandlerFixture>, IDisposable
//    {
//        private readonly GenerateHandlerFixture fixture;
//        private readonly GenerateHandler sut;
//        private string testCsvFile = Path.Combine(Environment.CurrentDirectory, "Test.csv");

//        public WriteCohort_Should(GenerateHandlerFixture fixture)
//        {
//            this.fixture = fixture;
//            this.sut = fixture.Handler;
//        }

//        [Fact]
//        public void CreateCohortFile()
//        {
//            File.Exists(testCsvFile).ShouldBe(false);
//            var cohort = new List<int> { 12345, 09877, 23173, 08124, 809124 };

//            this.sut.WriteCohort(testCsvFile, cohort);

//            File.Exists(testCsvFile).ShouldBe(true);
//        }

//        [Fact]
//        public void WriteCohortToFile()
//        {
//            var cohort = new List<int> { 12345, 09877, 23173, 08124, 809124 };

//            this.sut.WriteCohort(testCsvFile, cohort);
//            var result = new List<int>();
//            using (TextReader reader = File.OpenText(testCsvFile))
//            {
//                string userId;
//                while ((userId = reader.ReadLine()) != null)
//                {
//                    result.Add(Int32.Parse(userId));
//                }
//            }

//            result.Count.ShouldBe(cohort.Count);
//        }

//        public void Dispose()
//        {
//            if (File.Exists(testCsvFile))
//            {
//                File.Delete(testCsvFile);
//            }
//        }
//    }
//}
