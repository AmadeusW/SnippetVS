using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnippetVS;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GistTest()
        {
            var integration = new GitIntegration();
            integration.CreateGist(@"This is my test

gist....");
        }
    }
}
