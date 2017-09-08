using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetEvent.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            ScatterGather<string, string>.Subscribe(msg => "hello");
            ScatterGather<string, string>.Subscribe(msg => "you");
            var res = await ScatterGather<string, string>.Request("hi", TimeSpan.FromSeconds(1));
            Assert.AreEqual(2, res.Length);
            Assert.IsTrue(res.Any(s => s == "hello"));
            Assert.IsTrue(res.Any(s => s == "you"));
        }
    }
}
