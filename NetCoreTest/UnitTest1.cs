using System;
using System.IO;
using System.Reflection;
using Xunit;
using Xunit.Extensions;
using Microsoft.Extensions.DependencyModel;


namespace NetCoreTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            FileStream stream = File.Create(@"D:\Hello.txt");
            var type = typeof(Stream);
            Assert.IsAssignableFrom(type, stream);
        }


        public void SessionCheck()
        {

        }
    }
}
