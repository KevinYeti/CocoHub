using System;

namespace MultiTree.Cocohub
{
    public class Tree
    {
        public string Id { get; set; }

        public Node Root { get; set; }

        public Tree()
        {
            Root = new Node(1) { Parent = null };
        }

        public bool TryPeek(string span, out Node node)
        {
            node = null;
            var spans = span.Split(new string[] {"."}, StringSplitOptions.RemoveEmptyEntries);
            if (string.IsNullOrEmpty(span) || spans.Length == 0 || spans.Length == 1)
                return false;
            else if (spans.Length == 2)
            {
                if (spans[1] != "1")
                    return false;
                else
                {
                    //0.1
                    node = Root;
                    return true;  
                }              
            }
            else
            {
                //0.1....
                Node nParent = Root;
                for (int i = 2; i < spans.Length; i++)
                {
                    
                }
            }

            return false;
        }
    }
}
