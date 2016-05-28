using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Adform.AdServing.AhoCorasickTree.Sandbox.V10
{
    public class AhoCorasickTree
    {
        private byte[] _data;
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

                CreateArray();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe char getKey(int* currentNodePtr, int ind)
        {
            return *(char*) ((byte*) currentNodePtr + sizeof(int) + sizeof(int) + sizeof(bool) +
                             ind*(sizeof(char) + sizeof(int)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe int* getNext(byte* b, int* currentNodePtr, int ind)
        {
            var n = *((byte*) currentNodePtr + sizeof(int) + sizeof(int) + sizeof(bool) +
                      ind*(sizeof(char) + sizeof(int)) + sizeof(char));
            return (int*) (b + n);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe bool GetIsWord(byte* b, int* currentNodePtr, int ind)
        {
            var n = *((byte*) currentNodePtr + sizeof(int) + sizeof(int) + sizeof(bool) +
                      ind*(sizeof(char) + sizeof(int)) + sizeof(char));

            return *(bool*) (b + n + sizeof(int) + sizeof(int));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe int* GetFailure(byte* b, int* currentNodePtr)
        {
            return (int*) (b + *((byte*) currentNodePtr + sizeof(int)));
        }


        public unsafe bool Contains(string text)
        {
            fixed (byte* b = _data)
            fixed (char* p = text)
            {
                char c1, c2, c3, c4;
                long len = text.Length * 2;
                long* lptr = (long*) p;
                lptr--;
                long l;

                int* currentNodePtr = (int*) b;
                int size;
                int ind;
                char key;

                for (int i = 0; i < len; i += 8)
                {
                    lptr++;
                    l = *lptr;
                    c1 = (char) (l & 0xffff);
                    c2 = (char) (l >> 16);
                    c3 = (char) (l >> 32);
                    c4 = (char) (l >> 48);
                    


                    while (true)
                    {
                        size = *currentNodePtr;
                        if (size == 0)
                        {
                            currentNodePtr = GetFailure(b, currentNodePtr);
                            if (currentNodePtr == b) break;
                        }

                        ind = c1 & (size - 1);
                        key = getKey(currentNodePtr, ind);
                        if (key == c1)
                        {
                            if (GetIsWord(b, currentNodePtr, ind))
                            {
                                return true;
                            }
                            currentNodePtr = getNext(b, currentNodePtr, ind);
                        }
                        else
                        {
                            currentNodePtr = GetFailure(b, currentNodePtr);
                            if (currentNodePtr == b) break;
                        }

                        size = *currentNodePtr;
                        if (size == 0)
                        {
                            currentNodePtr = GetFailure(b, currentNodePtr);
                            if (currentNodePtr == b) break;
                        }

                        ind = c2 & (size - 1);
                        key = getKey(currentNodePtr, ind);
                        if (key == c2)
                        {
                            if (GetIsWord(b, currentNodePtr, ind))
                            {
                                return true;
                            }
                            currentNodePtr = getNext(b, currentNodePtr, ind);
                        }
                        else
                        {
                            currentNodePtr = GetFailure(b, currentNodePtr);
                            if (currentNodePtr == b) break;
                        }

                        size = *currentNodePtr;
                        if (size == 0)
                        {
                            currentNodePtr = GetFailure(b, currentNodePtr);
                            if (currentNodePtr == b) break;
                        }

                        ind = c3 & (size - 1);
                        key = getKey(currentNodePtr, ind);
                        if (key == c3)
                        {
                            if (GetIsWord(b, currentNodePtr, ind))
                            {
                                return true;
                            }
                            currentNodePtr = getNext(b, currentNodePtr, ind);
                        }
                        else
                        {
                            currentNodePtr = GetFailure(b, currentNodePtr);
                            if (currentNodePtr == b) break;
                        }

                        size = *currentNodePtr;
                        if (size == 0)
                        {
                            currentNodePtr = GetFailure(b, currentNodePtr);
                            if (currentNodePtr == b) break;
                        }

                        ind = c4 & (size - 1);
                        key = getKey(currentNodePtr, ind);
                        if (key == c4)
                        {
                            if (GetIsWord(b, currentNodePtr, ind))
                            {
                                return true;
                            }
                            currentNodePtr = getNext(b, currentNodePtr, ind);
                            break;
                        }
                        else
                        {
                            currentNodePtr = GetFailure(b, currentNodePtr);
                            if (currentNodePtr == b) break;
                        }
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

        private unsafe void CreateArray()
        {
            //var size = GetArraySize();
            var data = new List<byte>();

            var currentPtr = 0;

            //fixed (byte* bytePtr = bytes)
            {
                //var charPtr = (char*) bytePtr;
                //var intPtr = (int*) bytePtr;
                //var boolPtr = (bool*) bytePtr;

                var queue = new Queue<AhoCorasickTreeNode>();
                queue.Enqueue(Root);
                while (queue.Count > 0)
                {
                    var currentNode = queue.Dequeue();

                    data.AddRange(BitConverter.GetBytes(currentNode._entries.Length));
                    currentPtr += sizeof(int);
                    data.AddRange(BitConverter.GetBytes(currentNode.Failure.Offset));
                    currentPtr += sizeof(int);
                    data.AddRange(BitConverter.GetBytes(currentNode.IsWord));
                    currentPtr += sizeof(bool);

                    if (currentNode._entries.Length > 0)
                    {
                        foreach (var entry in currentNode._entries.Where(x => x.Key != 0))
                        {
                            queue.Enqueue(entry.Value);

                            //charPtr[currentPtr] = entry.Key;
                            data.AddRange(BitConverter.GetBytes(entry.Key));
                            currentPtr += sizeof(char);
                            data.AddRange(BitConverter.GetBytes(entry.Value.Offset));
                            //nodePtr[currentPtr] = entry.Value.Offset;
                            currentPtr += sizeof(int);
                        }

                    }
                }
            }

            _data = data.ToArray();
        }

        private int GetArraySize()
        {
            var roots = 0;
            var elements = 0;

            var queue = new Queue<AhoCorasickTreeNode>();
            queue.Enqueue(Root);
            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();
                currentNode.Offset = calcOffset(roots, elements);
                roots++;
                foreach (var entry in currentNode._entries)
                {
                    queue.Enqueue(entry.Value);
                    elements++;
                }
            }


            return calcOffset(roots, elements)*2;
        }

        private int calcOffset(int roots, int childs)
        {
            return roots*(sizeof(int) + sizeof(int) + sizeof(bool)) + childs*(sizeof(char) + sizeof(int));
        }
    }
}