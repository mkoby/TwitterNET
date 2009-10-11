using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TwitterNET;

namespace TwitterNET_Tests
{
    [TestFixture]
    public class TwitterNET_Mentions_Tests
    {
        private Twitter twitter = null;

        [TestFixtureSetUp]
        public void TestFixture_Setup()
        {
        }

        [TestFixtureTearDown]
        public void TestFixture_TearDown()
        { }

        [SetUp]
        public void Test_Setup()
        {
            twitter = new Twitter("apitest4769", "testaccount");
        }

        [TearDown]
        public void Test_TearDown()
        {
            twitter = null;
        }

        [Test]
        public void Get_Metions_NoArgs_Test()
        {
            IList<StatusMessage> mentions = twitter.GetMetions(new StatusRequestOptions());

            Assert.IsNotEmpty((ICollection)mentions);
        }

        [Test]
        public void Get_Mentions_WithCount_Test()
        {
            StatusRequestOptions statusRequestOptions = new StatusRequestOptions();
			statusRequestOptions.Add(StatusRequestOptionNames.Count, 1);
			
            IList<StatusMessage> mentions = twitter.GetMetions(statusRequestOptions);

            Assert.GreaterOrEqual(1, mentions.Count);
        }
    }
}
