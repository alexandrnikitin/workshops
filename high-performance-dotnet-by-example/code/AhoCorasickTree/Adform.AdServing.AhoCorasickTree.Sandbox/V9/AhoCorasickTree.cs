using System.Collections.Generic;

namespace Adform.AdServing.AhoCorasickTree.Sandbox.V9
{
    public class AhoCorasickTree
    {
        internal AhoCorasickTreeNode Root { get; set; }

        public AhoCorasickTree(IEnumerable<string> keywords)
        {
            Root = new AhoCorasickTreeNode();

            if (keywords != null)
            {
                foreach (var p in keywords)
                {
                    AddPatternToTree(p);
                }

                SetFailureNodes();
            }
        }

        public unsafe bool Contains(string text)
        {
            var currentNode = Root;

            fixed (char* p = text)
            {
                AhoCorasickTreeNode node;
                char c1, c2, c3, c4;
                int len = text.Length * 2;
                long* lptr = (long*)p;
                long l;

                int count = len / 8;
                len -= (len / 8) * 8;
                while (count > 0)
                {
                    l = *lptr;
                    c1 = (char)(l & 0xffff);
                    c2 = (char)(l >> 16);
                    c3 = (char)(l >> 32);
                    c4 = (char)(l >> 48);
                    lptr++;

                    C1:
                    node = currentNode.GetTransition(c1);
                    if (node != null)
                    {
                        if (node.IsWord) return true;

                        currentNode = node;
                    }
                    else
                    {
                        currentNode = currentNode.Failure;
                        if (currentNode != Root) goto C1;
                    }

                    C2:
                    node = currentNode.GetTransition(c2);
                    if (node != null)
                    {
                        if (node.IsWord) return true;
                        currentNode = node;
                    }
                    else
                    {
                        currentNode = currentNode.Failure;
                        if (currentNode != Root) goto C2;
                    }

                    C3:
                    node = currentNode.GetTransition(c3);
                    if (node != null)
                    {
                        if (node.IsWord) return true;
                        currentNode = node;
                    }
                    else
                    {
                        currentNode = currentNode.Failure;
                        if (currentNode != Root) goto C3;
                    }

                    C4:
                    node = currentNode.GetTransition(c4);
                    if (node != null)
                    {
                        if (node.IsWord) return true;
                        currentNode = node;
                    }
                    else
                    {
                        currentNode = currentNode.Failure;
                        if (currentNode != Root) goto C4;
                    }

                    count--;
                }

                var cptr = (char*)lptr;
                
                while (len > 0)
                {
                    c1 = *cptr;
                    cptr++;
                    len -= 2;

                    node = currentNode.GetTransition(c1);
                    if (node != null)
                    {
                        if (node.IsWord) return true;

                        currentNode = node;
                    }
                    else
                    {
                        currentNode = currentNode.Failure;
                    }
                }
            }

            return false;

        }

        public bool ContainsThatStart(string text)
        {
            return Contains(text, true);
        }

        private bool Contains(string text, bool onlyStarts)
        {
            var pointer = Root;

            for (var i = 0; i < text.Length; i++)
            {
                AhoCorasickTreeNode transition = null;
                while (transition == null)
                {
                    transition = pointer.GetTransition(text[i]);

                    if (pointer == Root)
                        break;

                    if (transition == null)
                        pointer = pointer.Failure;
                }

                if (transition != null)
                    pointer = transition;
                else if (onlyStarts)
                    return false;

                if (pointer.Results.Count > 0)
                    return true;
            }
            return false;
        }

        public IEnumerable<string> FindAll(string text)
        {
            var pointer = Root;

            foreach (var c in text)
            {
                var transition = GetTransition(c, ref pointer);

                if (transition != null)
                    pointer = transition;

                foreach (var result in pointer.Results)
                    yield return result;
            }
        }

        private AhoCorasickTreeNode GetTransition(char c, ref AhoCorasickTreeNode pointer)
        {
            AhoCorasickTreeNode transition = null;
            while (transition == null)
            {
                transition = pointer.GetTransition(c);

                if (pointer == Root)
                    break;

                if (transition == null)
                    pointer = pointer.Failure;
            }
            return transition;
        }

        private void SetFailureNodes()
        {
            var nodes = FailToRootNode();
            FailUsingBFS(nodes);
            Root.Failure = Root;
        }

        private void AddPatternToTree(string pattern)
        {
            var node = Root;
            foreach (var c in pattern)
            {
                node = node.GetTransition(c)
                       ?? node.AddTransition(c);
            }
            node.AddResult(pattern);

            node.IsWord = true;
        }

        private List<AhoCorasickTreeNode> FailToRootNode()
        {
            var nodes = new List<AhoCorasickTreeNode>();
            foreach (var node in Root.Transitions)
            {
                node.Failure = Root;
                nodes.AddRange(node.Transitions);
            }
            return nodes;
        }

        private void FailUsingBFS(List<AhoCorasickTreeNode> nodes)
        {
            while (nodes.Count != 0)
            {
                var newNodes = new List<AhoCorasickTreeNode>();
                foreach (var node in nodes)
                {
                    var failure = node.ParentFailure;
                    var value = node.Value;

                    while (failure != null && !failure.ContainsTransition(value))
                    {
                        failure = failure.Failure;
                    }

                    if (failure == null)
                    {
                        node.Failure = Root;
                    }
                    else
                    {
                        node.Failure = failure.GetTransition(value);
                        node.AddResults(node.Failure.Results);

                        if (!node.IsWord)
                        {
                            node.IsWord = failure.IsWord;
                        }

                    }

                    newNodes.AddRange(node.Transitions);
                }
                nodes = newNodes;
            }
        }
    }
}