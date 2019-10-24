using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ExNovo
{
    public class ExNovoActionTreeNode
    {
        public string ActionName { get; private set; }
        public Color Color { get; private set; }
        public string CommandText { get; private set; }
        private List<ExNovoActionTreeNode> Children;
        public ExNovoActionTreeNode Parent { get; set; }

        public bool IsRoot => Parent == null;
        public bool IsLeaf => Children == null || Children.Count < 1;

        public bool HasCommandToRun => CommandText != null;

        public ExNovoActionTreeNode(string name, Color color, string command, ExNovoActionTreeNode parent)
        {
            this.ActionName = name;
            this.Color = color;
            this.CommandText = command;
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


        public ExNovoActionTreeNode Child(int number)
        {
            if (number < 1 || number > 3)
            {
                throw new System.ArgumentOutOfRangeException(nameof(number), number, "Child number must be 1, 2 or 3");
            }
            if (!HasChild(number))
            {
                throw new System.ArgumentOutOfRangeException(nameof(number), number, "Child does not exist");
            }
            return Children[number - 1];
        }

        public bool HasChild(int childNumber)
        {
            return Children.Count >= childNumber;
        }

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
