using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCHA
{
	public class OlapDimensionsTree
	{
		public string name;
		public string value;
		public int weight;
        public OlapDimensionsTree parent;

        public List<OlapDimensionsTree> children; // todo: use linkedlist

		public OlapDimensionsTree()
			: this(string.Empty, string.Empty, null)
		{

		}

		public OlapDimensionsTree(string name, string value, OlapDimensionsTree parent)
		{
			this.name = name;
			this.value = value;
			this.children = new List<OlapDimensionsTree>();
			this.weight = 1;
            this.parent = parent;
		}

		public void CalculateWeight()
		{
			weight = 0;
			foreach (var c in children)
			{
				c.CalculateWeight();
				weight += c.weight;
			}
		}

		public bool IsLeave
		{
			get { return 0 == children.Count; }
		}

		public int Height
		{
			get { return 1 + (!IsLeave ? children.Select(c => c.Height).Max() : 0); }
		}

        public bool IsVirtualRoot
        {
            get { return name.IsNullOrEmpty(); }
        }

        public bool IsRoot
        {
            get { return null == parent; }
        }

        public delegate void LeavesIterationCallback(OlapDimensionsTree node, LinkedList<KeyValuePair<string, string>> filters);

		public int IterateLeaves(LeavesIterationCallback cb)
		{
			LinkedList<KeyValuePair<string, string>> filters = new LinkedList<KeyValuePair<string, string>>();
			return IterateLeaves(cb, filters);
		}

		public int IterateLeaves(LeavesIterationCallback cb, LinkedList<KeyValuePair<string, string>> filters)
		{
			int rv = 0;
			if (!name.IsEmpty())
				filters.AddLast(new LinkedListNode<KeyValuePair<string, string>>(new KeyValuePair<string,string>(name, QueryBuilder.DecorateString(value))));
			if (IsLeave)
			{
				cb(this, filters);
				++rv;
			}
			else
				foreach (OlapDimensionsTree node in children)
					rv += node.IterateLeaves(cb, filters);
			if (!name.IsEmpty())
				filters.RemoveLast();
			return rv;
		}

        public override string ToString()
        {
            if (IsRoot)
                return string.Empty;
            string self = string.Format("{0}={1}", name, value);
            if (parent.IsRoot)
                return self;
            return string.Format("{0}, {1}", parent.ToString(), self);
        }
    }
}
