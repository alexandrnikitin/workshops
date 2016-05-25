using System.Linq;
using NUnit.Framework;

namespace Adform.AdServing.AhoCorasickTree.Tests
{
    using AhoCorasickTree = Adform.AdServing.AhoCorasickTree.Sandbox.V4.AhoCorasickTree;

    [TestFixture]
    public class AhoCorasickTreeTest
    {
        [Test]
        public void Constructor_InputIsNull_ShouldHaveOnlyParentNode()
        {
            // act
            var tree = new AhoCorasickTree(null);

            // assert
            CollectionAssert.IsEmpty(tree.Root.Transitions);
        }

        [Test]
        public void Constructor_InputIsEmpty_ShouldHaveOnlyParentNode()
        {
            // act
            var tree = new AhoCorasickTree(new string[] {});

            // assert
            CollectionAssert.IsEmpty(tree.Root.Transitions);
        }

        [Test]
        public void Constructor_TreeSameLetters_HasOneTransition()
        {
            // act
            var tree = new AhoCorasickTree(new[] {"a", "a", "a"});

            // assert
            Assert.AreEqual(1, tree.Root.Transitions.Count());
        }

        [Test]
        public void Constructor_FourPatterns_CreatesCorrectTransitionTree()
        {
            // act
            var tree = new AhoCorasickTree(new[] {"a", "ab", "ba", "abc"});

            // assert
            Assert.AreEqual(2, tree.Root.Transitions.Count());

            var a = tree.Root.GetTransition('a');
            Assert.IsNotNull(a);
            var ab = a.GetTransition('b');
            Assert.IsNotNull(ab);
            var abc = ab.GetTransition('c');
            Assert.IsNotNull(abc);

            var b = tree.Root.GetTransition('b');
            Assert.IsNotNull(b);
            var ba = b.GetTransition('a');
            Assert.IsNotNull(ba);
        }

        [TestCase("d", ExpectedResult = false)]
        [TestCase("bce", ExpectedResult = false)]
        [TestCase("bcd", ExpectedResult = true)]
        [TestCase("abcd", ExpectedResult = true)]
        public bool Contains(string input)
        {
            // arrange
            var tree = new AhoCorasickTree(new[] {"ab", "abc", "bcd"});

            // act
            return tree.Contains(input);
        }

        [TestCase("d", ExpectedResult = false)]
        [TestCase("dbcd", ExpectedResult = false)]
        [TestCase("abcd", ExpectedResult = true)]
        public bool ContainsThatStart(string input)
        {
            // arrange
            var tree = new AhoCorasickTree(new[] {"ab", "abc", "bcd"});

            // act
            return tree.ContainsThatStart(input);
        }

        [TestCase("abc", ExpectedResult = 2)]
        [TestCase("abcd", ExpectedResult = 3)]
        [TestCase("d", ExpectedResult = 0)]
        public int FindAll(string input)
        {
            // arrange
            var tree = new AhoCorasickTree(new[] {"ab", "abc", "bcd"});

            // act
            return tree.FindAll(input).Count();
        }
    }
}