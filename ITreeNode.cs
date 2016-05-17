using System.Collections.Generic;

namespace TreeListUtility
{

    /// <summary>
    ///     An object implementing this interface is a node in a tree structure.
    /// </summary>
    /// <typeparam name="T">Data type for the tree structure.</typeparam>
    public interface ITreeNode<T>
        where T : class
    {

        int ID { get; }

        T Parent { get; set; }
        ICollection<T> Children { get; set; }

    } //ITreeNode

}
