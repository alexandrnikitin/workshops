using System.Linq;
using NUnit.Framework;

namespace Adform.AdServing.AhoCorasickTree.Tests
{
    using AhoCorasickTreeNode = Adform.AdServing.AhoCorasickTree.Sandbox.V4.AhoCorasickTreeNode;

    [TestFixture]
    public class AhoCorasickTreeNodeTest
    {
        [Test]
        public void AddResult_Twice_OnlyGetsAddedOnce()
        {
            // arrange
            var node = new AhoCorasickTreeNode();

            // act
            node.AddResults(new[] {"test", "test"});

            // assert
            Assert.AreEqual(1, node.Results.Count());
            CollectionAssert.AllItemsAreUnique(node.Results);
        }

        [Test]
        public void AddTransition_Twice_AddedTransitionsCanBeRetrievedAsEnumerable()
        {
            // arrange
            var node = new AhoCorasickTreeNode();

            // act
            node.AddTransition('a');
            node.AddTransition('b');

            // assert
            Assert.AreEqual(2, node.Transitions.Count());
        }
    }
}