using UnityEngine;
using System.Collections.Generic;
using System.Text;

namespace ExNovo
{
    /// <summary>
    /// Represents one node in the tree of actions. Contains data needed for visually representing the action, and data needed to run the action
    /// </summary>
    public class ExNovoActionTreeNode
    {
        public string ActionName { get; private set; }
        public Color Color { get; private set; }
        public string MethodCallText { get; private set; }
        private List<ExNovoActionTreeNode> Children;
        public ExNovoActionTreeNode Parent { get; set; }

        public bool IsRoot => Parent == null;
        public bool IsLeaf => Children == null || Children.Count < 1;

        public bool HasMethodToCall => MethodCallText != null;

        public ExNovoActionTreeNode(string name, Color color, string methodCallText, ExNovoActionTreeNode parent)
        {
            this.ActionName = name;
            this.Color = color;
            this.MethodCallText = methodCallText;
            this.Parent = parent;
            Children = new List<ExNovoActionTreeNode>();

            if (parent != null)
            {
                parent.AddChildNode(this);
            }
        }

        private void AddChildNode(ExNovoActionTreeNode childNode)
        {
            Children.Add(childNode);
        }


        /// <summary>
        /// Returns a child node if it exists, or null. Child is identified with integer 1, 2 or 3
        /// </summary>
        /// <param name="number">The branch number of the child. Valid values: 1, 2, 3</param>
        /// <returns>The child node, or null</returns>
        public ExNovoActionTreeNode Child(int number)
        {
            if (number < 1 || number > 3)
            {
                throw new System.ArgumentOutOfRangeException(nameof(number), number, "Child number must be 1, 2 or 3");
            }
            if (!HasChild(number))
            {
                return null;
            }
            return Children[number - 1];
        }

        public bool HasChild(int childNumber)
        {
            return Children.Count >= childNumber;
        }

        /// <summary>
        /// Recusively prints the tree to the console
        /// </summary>
        public void DEBUG_print_tree()
        {
            Debug.Log(this.ToString());
            foreach (ExNovoActionTreeNode node in Children)
            {
                node.DEBUG_print_tree();
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Node: ").Append(ActionName)
                .Append(". Parent: ").Append(Parent == null ? "None" : Parent.ActionName)
                .Append(". Children: ").Append(Children.Count);
            return stringBuilder.ToString();
        }
    }
}
