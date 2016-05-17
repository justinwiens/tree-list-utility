using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeListUtility
{

    /// <summary>
    ///     Utility class providing methods to convert lists of hierarchal information into and out of tree-like structures.
    /// </summary>
    public static class TreeListUtility
    {
        
        #region Extension Methods (Tree Construction)

        /// <summary>
        ///     Converts an array of ITreeNode objects into a list of hierarchical trees.
        ///     The returned list will will contain the top level "root" nodes with each
        ///     node having a populated set of child nodes (if applicable).
        /// </summary>
        /// <typeparam name="T">Data type for the tree structure</typeparam>
        /// <param name="flatNodeList">Array of ITreeNode objects to convert to trees</param>
        /// <returns>List of top level root nodes (trees) populated with nested child nodes</returns>
        public static List<T> ConvertToTrees<T>(this IEnumerable<T> flatNodeList)
            where T : class, ITreeNode<T>
        {

            //convert input generic type to the stronger list type
            flatNodeList = flatNodeList.ToList();


            //build a dictionary to contain our nodes and populate it
            Dictionary<Int32, T> allNodes = new Dictionary<Int32, T>();

            foreach (T node in flatNodeList)
            {
                allNodes.Add(node.ID, node);
                node.Children = new List<T>();
            }


            List<T> rootTreeNodes = new List<T>();


            /* iterate through all nodes.  if a node is a top-level parent then
             * add it to our list of root nodes, otherwise add it as a child of
             * its respective parent */
            foreach (T node in flatNodeList)
            {
                if (node.Parent == null)
                {
                    rootTreeNodes.Add(node);
                }
                else
                {

                    if (allNodes.ContainsKey(node.Parent.ID))
                    {
                        node.Parent = allNodes[node.Parent.ID];

                        node.Parent.Children.Add(node);
                    }
                }
            }

            return rootTreeNodes;

        } //ConvertToTrees


        /// <summary>
        ///     Converts an array of hierarchical trees into a flat list of nodes.
        ///     Preserves parent / child relationships between nodes.
        /// </summary>
        /// <typeparam name="T">Data type for the tree structure</typeparam>
        /// <param name="trees">List of hierarchical trees to flatten</param>
        /// <returns>Flat list of all tree nodes</returns>
        public static List<T> FlattenTrees<T>(this IEnumerable<T> treeLists)
            where T : class, ITreeNode<T>
        {
            List<T> flatNodeList = new List<T>();

            foreach (T rootNode in treeLists)
            {
                foreach (T node in GetChildren(rootNode, true))
                {
                    flatNodeList.Add(node);
                }
            }

            return flatNodeList;

        } //FlattenTrees

        #endregion


        #region Iterators

        /// <summary>
        ///     Find all connected child nodes beneath the provided node
        /// </summary>
        /// <param name="startNode">ITreeNode to find children of</param>
        /// <param name="returnStartNode">True if startNode be included in return results</param>
        public static IEnumerable<T> GetChildren<T>(this T startNode, Boolean returnStartNode = true)
            where T : class, ITreeNode<T>
        {
            if (returnStartNode)
            {
                yield return startNode;
            }

            foreach (T child in startNode.Children)
            {
                foreach (T grandChild in GetChildren(child, true))
                {
                    yield return grandChild;
                }
            }

        } //GetChildren


        /// <summary>
        ///     Find all connected parent nodes above the provided node
        /// </summary>
        /// <param name="startNode">ITreeNode to find parents for</param>
        /// <param name="returnStartNode">True if startNode be included in return results</param>
        public static IEnumerable<T> GetParents<T>(this T startNode, Boolean returnStartNode = true)
            where T : class, ITreeNode<T>
        {
            T currentNode = startNode;

            Int32 i = 0;

            while (currentNode != null)
            {
                if ((i == 0 && returnStartNode) || i > 0)
                {
                    yield return currentNode;
                }

                currentNode = currentNode.Parent;
                i++;
            }

        } //GetParents


        /// <summary>
        ///     Find all connected sibling nodes (underneath the same parent) beside the provided node 
        /// </summary>
        /// <param name="startNode">ITreeNode to find siblings of</param>
        /// <param name="returnStartNode">True if startNode be included in return results</param>
        public static IEnumerable<T> GetSiblings<T>(this T startNode, Boolean returnStartNode = true)
            where T : class, ITreeNode<T>
        {
            if (returnStartNode)
            {
                yield return startNode;
            }

            foreach (T sibling in startNode.Parent.Children)
            {
                if (sibling.ID != startNode.ID)
                {
                    yield return sibling;
                }
            }

        } //GetSiblings

        #endregion


        #region Searching Functionality

        /// <summary>
        ///     Find a node inside of a given array of hierarchial trees
        /// </summary>
        /// <typeparam name="T">Data type for the tree structure</typeparam>
        /// <param name="trees">Set of hierarchical trees to search</param>
        /// <param name="nodeID">Unique identifier of node to locate in trees</param>
        /// <returns>Node matching provided unique identifier</returns>
        public static T FindNodeByID<T>(this IEnumerable<T> trees, Int32 nodeID)
            where T : class, ITreeNode<T>
        {

            foreach (T root in trees)
            {
                foreach (T child in GetChildren(root, true))
                {
                    if (child.ID == nodeID)
                    {
                        return child;
                    }
                }
            }

            return null;

        } //FindNodeById

        #endregion


        #region Properties

        /// <summary>
        ///     Gets the depth of a node in a tree
        /// </summary>
        public static int GetDepth<T>(this T node)
            where T : class, ITreeNode<T>
        {
            Int32 depth = 0;

            while (node.Parent != null)
            {
                depth++;
                node = node.Parent;
            }

            return depth;

        } //GetDepth

        #endregion

    }

}
