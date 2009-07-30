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
            IList<StatusMessage> mentions = twitter.GetMetions(new RequestOptions());

            Assert.IsNotEmpty((ICollection)mentions);
        }

        [Test]
        public void Get_Mentions_WithCount()
        {
            RequestOptions requestOptions = new RequestOptions();
			requestOptions.Add(RequestOptionNames.Count, 1);
			
            IList<StatusMessage> mentions = twitter.GetMetions(requestOptions);

            Assert.GreaterOrEqual(1, mentions.Count);
        }
    }
}
