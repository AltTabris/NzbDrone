﻿using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using NzbDrone.Core.Test.Framework;
using NzbDrone.Test.Common;

namespace NzbDrone.Core.Test
{
    [TestFixture]
    
    public class FluentTest : CoreTest
    {
        [TestCase(null, "def", "def")]
        [TestCase("", "def", "def")]
        [TestCase("", 1, "1")]
        [TestCase(null, "", "")]
        [TestCase("actual", "def", "actual")]
        public void WithDefault_success(string actual, object defaultValue, string result)
        {
            actual.WithDefault(defaultValue).Should().Be(result);
        }

        [Test]
        public void WithDefault_Fail()
        {
            Assert.Throws<ArgumentNullException>(() => "test".WithDefault(null));

            ExceptionVerification.IgnoreWarns();
        }

        [Test]
        public void ToBestDateTime_Yesterday()
        {
            var dateTime = DateTime.Today.AddDays(-1);

            dateTime.ToBestDateString().Should().Be("Yesterday");
        }

        [Test]
        public void ToBestDateTime_Today()
        {
            var dateTime = DateTime.Today;

            dateTime.ToBestDateString().Should().Be("Today");
        }

        [Test]
        public void ToBestDateTime_Tomorrow()
        {
            var dateTime = DateTime.Today.AddDays(1);

            dateTime.ToBestDateString().Should().Be("Tomorrow");
        }

        [Test]
        public void ToBestDateTime_DayOfWeek()
        {
            for (int i = 2; i < 7; i++)
            {
                var dateTime = DateTime.Today.AddDays(i);

                Console.WriteLine(dateTime.DayOfWeek);
                dateTime.ToBestDateString().Should().Be(dateTime.DayOfWeek.ToString());
            }
        }

        [Test]
        public void ToBestDateTime_Over_One_Week()
        {
            var dateTime = DateTime.Today.AddDays(8);

            Console.WriteLine(dateTime.DayOfWeek);
            dateTime.ToBestDateString().Should().Be(dateTime.ToShortDateString());
        }

        [Test]
        public void ToBestDateTime_Before_Yesterday()
        {
            var dateTime = DateTime.Today.AddDays(-2);

            Console.WriteLine(dateTime.DayOfWeek);
            dateTime.ToBestDateString().Should().Be(dateTime.ToShortDateString());
        }

        [Test]
        public void ParentUriString_should_return_self_if_already_parent()
        {
            
            var url = "http://www.nzbdrone.com";
            var uri = new Uri(url);

            
            var result = uri.ParentUriString();

            //Resolve
            result.Should().Be(url);
        }

        [Test]
        public void ParentUriString_should_return_parent_url_when_path_is_passed()
        {
            
            var url = "http://www.nzbdrone.com/test/";
            var uri = new Uri(url);

            
            var result = uri.ParentUriString();

            //Resolve
            result.Should().Be("http://www.nzbdrone.com");
        }

        [Test]
        public void ParentUriString_should_return_parent_url_when_multiple_paths_are_passed()
        {
            
            var url = "http://www.nzbdrone.com/test/test2";
            var uri = new Uri(url);

            
            var result = uri.ParentUriString();

            //Resolve
            result.Should().Be("http://www.nzbdrone.com");
        }

        [Test]
        public void ParentUriString_should_return_parent_url_when_url_with_query_string_is_passed()
        {
            
            var url = "http://www.nzbdrone.com/test.aspx?test=10";
            var uri = new Uri(url);

            
            var result = uri.ParentUriString();

            //Resolve
            result.Should().Be("http://www.nzbdrone.com");
        }

        [Test]
        public void ParentUriString_should_return_parent_url_when_url_with_path_and_query_strings_is_passed()
        {
            
            var url = "http://www.nzbdrone.com/tester/test.aspx?test=10";
            var uri = new Uri(url);

            
            var result = uri.ParentUriString();

            //Resolve
            result.Should().Be("http://www.nzbdrone.com");
        }

        [Test]
        public void ParentUriString_should_return_parent_url_when_url_with_query_strings_is_passed()
        {
            
            var url = "http://www.nzbdrone.com/test.aspx?test=10&test2=5";
            var uri = new Uri(url);

            
            var result = uri.ParentUriString();

            //Resolve
            result.Should().Be("http://www.nzbdrone.com");
        }

        [Test]
        public void MaxOrDefault_should_return_zero_when_collection_is_empty()
        {
            


            
            var result = (new List<int>()).MaxOrDefault();

            //Resolve
            result.Should().Be(0);
        }

        [Test]
        public void MaxOrDefault_should_return_max_when_collection_is_not_empty()
        {
            
            var list = new List<int> { 6, 4, 5, 3, 8, 10 };

            
            var result = list.MaxOrDefault();

            //Resolve
            result.Should().Be(10);
        }

        [Test]
        public void MaxOrDefault_should_return_zero_when_collection_is_null()
        {
            
            List<int> list = null;

            
            var result = list.MaxOrDefault();

            //Resolve
            result.Should().Be(0);
        }

        [Test]
        public void Truncate_should_truncate_strings_to_max_specified_number_of_bytes()
        {
            
            var str = ReadAllText("Files", "LongOverview.txt");

            
            var resultString = str.Truncate(1000);

            //Resolve
            var result = new UTF8Encoding().GetBytes(resultString);
            result.Length.Should().BeLessOrEqualTo(1000);
        }

        [Test]
        public void Truncate_should_not_truncate_string_shorter_than_max_bytes()
        {
            
            var str = "Hello World";

            
            var resultString = str.Truncate(1000);

            //Resolve
            var result = new UTF8Encoding().GetBytes(resultString);
            result.Length.Should().Be(11);
        }

        [Test]
        public void MinOrDefault_should_return_zero_when_collection_is_empty()
        {
            


            
            var result = (new List<int>()).MinOrDefault();

            //Resolve
            result.Should().Be(0);
        }

        [Test]
        public void MinOrDefault_should_return_min_when_collection_is_not_empty()
        {
            
            var list = new List<int> { 6, 4, 5, 3, 8, 10 };

            
            var result = list.MinOrDefault();

            //Resolve
            result.Should().Be(3);
        }

        [Test]
        public void MinOrDefault_should_return_zero_when_collection_is_null()
        {
            
            List<int> list = null;

            
            var result = list.MinOrDefault();

            //Resolve
            result.Should().Be(0);
        }


        [TestCase(100,100,100)]
        [TestCase(110,100,100)]
        [TestCase(199,100,100)]
        [TestCase(1000,100,1000)]
        [TestCase(0,100,0)]
        public void round_to_level(long number, int level, int result)
        {
            number.Round(level).Should().Be(result);
        }
    }
}
