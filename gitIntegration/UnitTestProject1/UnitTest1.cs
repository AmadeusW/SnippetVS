using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using gitIntegration;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var integration = new GitIntegration();
            integration.CreateGist(@"This is my test

gist....");
        }
    }
}
