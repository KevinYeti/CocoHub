using System;
using System.Collections.Generic;

namespace MultiTree.Cocohub
{
    public class Node : IComparable<Node>
    {
        public int Index { get; set; }

        public long Time { get; set; }

        public Node Parent { get; set; }

        public List<Node> Children { get; set; }

        public string Name { get; set; }

        public string Params { get; set; }

        public Node(int index)
        {
            Index = index;
        }

        public string Span => (Parent == null ? "0" : Parent.Span) + "." + Index.ToString();

        public int CompareTo(Node other)
        {
            if (other == null)
                return 1;
            else
                return this.Index.CompareTo(other.Index);
        }
    }
}
