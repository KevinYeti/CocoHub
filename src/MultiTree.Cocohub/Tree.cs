using System;

namespace MultiTree.Cocohub
{
    public class Tree
    {
        public string Id { get; set; }

        public Node Root { get; set; }

        public Tree()
        {
            Root = new Node(0) { Parent = null };
        }

        public bool TryLink(string span, long time, string name, string parms)
        {
            var spans = span.Split(new string[] {"."}, StringSplitOptions.RemoveEmptyEntries);
            if (string.IsNullOrEmpty(span) || spans.Length == 0 || spans.Length == 1)
                return false;
            else if (spans.Length == 2)
            {
                //0.1 or 0.2
                Root.Name = name;
                Root.Time = time;
                Root.Params = parms;

                return true;
            }
            else
            {
                //0.1[.n.n.n.n]
                Node nParent = Root;
                for (int i = 2; i < spans.Length; i++)
                {
                    int index = Convert.ToInt32(spans[i]);
                    var node = nParent.Children.Find(n => n.Index == index);
                    if (node == null)
                    {
                        //found nothing, need create first
                        node = new Node(index);
                        nParent.Children.Add(node);
                    }

                    if (i == spans.Length - 1)
                    {
                        //last one, the exact one we want
                        node.Name = name;
                        node.Time = time;
                        node.Params = parms;

                        return true;
                    }
                    else
                    {
                        nParent = node;
                        continue;
                    }
                }
            }

            return false;
        }
    }
}
