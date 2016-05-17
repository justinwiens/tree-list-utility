# tree-list-utility
Helper utility for managing and traversing hierarchical data models.  Not intended to be feature complete, but to cover the basics.
Implement in your project and extend to your heart's content.

Tested in .NET 3.5+, but should be compatible all the way back to .NET 2.0.

###Usage
For entities that have a hierarchical / parent-child relationship to other entities in your model, implement the
provided *ITreeNode* interface.

The *TreeListUtility* static class provided exposes basic functionality for objects implementing the *ITreeNode* interface, including 
conversion of a flat list of objects into a nested parent/child collection, iteration through parent and child objects, and node
location.

###Example
Setting up your entity as a tree node:
```csharp
public class Item 
  : ITreeNode<Item>
{
  //database id or other unique identifier
  public int ID { get; set; }
  public string Name { get; set; }
  
  public Item Parent { get; set; }
  public IList<Item> Children { get; set; }
}
```

Working with tree nodes
```csharp
var items = ItemRepository.GetAll(); 

//arranges all items as a nested parent/child collection
var itemsTree = items.ConvertToTrees(); 

//navigate to a specific node in the collection with ID of 10
var node = itemsTree.FindNodeByID(10); 

//get all nodes with the same parent as the current node, not including the current node itself
var siblings = node.GetSiblings(false); 
```
