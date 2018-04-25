using Agent.Cocohub.Entity;
using Agent.Cocohub.Log;
using System;
using System.IO;
using Xunit;

namespace UnitTest
{
    public class LogResolverTest
    {
        [Fact]
        public void Test()
        {
            string log = File.ReadAllText("input.txt");
            LogEntity entity;
            Assert.True(LogResolver.TryResolve(log, out entity));
        }
    }
}
