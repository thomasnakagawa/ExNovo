using System;
using UnityEngine;
using SimpleJSON;

namespace ExNovo
{
    public static class ExNovoActionTreeJSONReader
    {
        [System.Serializable]
        public class ActionTreeJSONNode
        {
            public string name;
            public string color;
            public string command;
            public ActionTreeJSONNode next1;
            public ActionTreeJSONNode next2;
            public ActionTreeJSONNode next3;

            public static ActionTreeJSONNode CreateFromJSON(string jsonString)
            {
                return JsonUtility.FromJson<ActionTreeJSONNode>(jsonString);
            }
        }

        /// <summary>
        /// Reads the json string and converts it to a tree of ExNovoActionTreeNode
        /// </summary>
        /// <param name="jsonString">String containing the entire json action tree</param>
        /// <returns>The root to the ExNovoActionTreeNode tree</returns>
        public static ExNovoActionTreeNode ReadTreeFromJSON(string jsonString) 
        {
            // read the content as json nodes
            //ActionTreeJSONNode jsonTree = ActionTreeJSONNode.CreateFromJSON(jsonString);
            var jsonTree = JSON.Parse(jsonString);

            // create the root xnv node
            ExNovoActionTreeNode rootXNVNode = BuildXNVNode(jsonTree, null);

            // create xnv nodes for the entire tree
            CreateChildNodes(jsonTree, rootXNVNode);

            return rootXNVNode;
        }

        /// <summary>
        /// Recursive method for building the action tree
        /// </summary>
        /// <param name="jsonNode">JSON representation of the current node</param>
        /// <param name="xnvNode">ExNovoActionTreeNode representation of the parent of the current node</param>
        private static void CreateChildNodes(JSONNode jsonNode, ExNovoActionTreeNode xnvNode)
        {
            foreach (JSONNode jsonChild in new JSONNode[] { jsonNode["next1"], jsonNode["next2"], jsonNode["next3"] } )
            {
                if (jsonChild != null)
                {
                    var childXNVNode = BuildXNVNode(jsonChild, xnvNode);
                    CreateChildNodes(jsonChild, childXNVNode);
                }
            }
        }

        private static ExNovoActionTreeNode BuildXNVNode(JSONNode jsonNode, ExNovoActionTreeNode parentXNVNode)
        {
            Color nodeColor = parentXNVNode != null ? parentXNVNode.Color : Color.white; // set default color to the parent color in case this node doesnt have it's own color defined
            if (jsonNode["color"] != null)
            {
                ColorUtility.TryParseHtmlString(jsonNode["color"], out nodeColor);
            }
            return new ExNovoActionTreeNode(jsonNode["name"], nodeColor, jsonNode["methodCall"], parentXNVNode);
        }
    }
}
