MoreComplexDataStructures
-------------------------

MoreComplexDataStructures is a class library containing a collection of data structures more complex than those found in the standard .NET framework.

#### Contents
**WeightBalancedTree** - An implementation of a [weight-balanced tree](https://en.wikipedia.org/wiki/Weight-balanced_tree).  Although self-balancing is not yet implemented, the tree maintains counts of the nodes in each node's subtrees.  This allows methods GetCountGreaterThan(T item) and GetCountLessThan(T item) (which return the number of items greater than and less than a specified item) to return with order O(log(n)) time complexity.  The class also provides methods to perform pre, post, and in-order depth-first search, and breadth-first search.

**MinHeap** - A tree-based implementation of a [min heap](https://en.wikipedia.org/wiki/Heap_(data_structure)).  Insert() and ExtractMin() methods return with order O(log(n)) time complexity.  Also provides a method to traverse the nodes of the underlying tree via a breadth-first search.

**MaxHeap** - A tree-based implementation of a [max heap](https://en.wikipedia.org/wiki/Heap_(data_structure)).  Insert() and ExtractMax() methods return with order O(log(n)) time complexity.  Also provides a method to traverse the nodes of the underlying tree via a breadth-first search.

**LongIntegerStatusStorer** - Stores a true/false status for a complete set of long (Int64) integers.  Uses an underlying tree holding ranges of integers to store the statuses.  Also provides a method TraverseTree() to traverse the ranges stored in the tree via a breadth-first search.

**ListRandomizer** - Randomizes a List or Array using the [Fisher/Yates/Knuth algorithm](https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle) (O(n) time complexity).

#### Future Enhancements
- Implement self-balancing in the WeightBalancedTree class.
- Refactor methods Insert() and ExtractMax()/ExtractMin() on the MaxHeap/MinHeap classes into the HeapBase class.
- Abstract use of IComparable<T>.CompareTo() in heap classes to make code easier to read.
- Add a trie.

#### Release History

<table>
  <tr>
    <td><b>Version</b></td>
    <td><b>Changes</b></td>
  </tr>
  <tr>
    <td valign="top">1.1.0.0</td>
    <td>
      Added MinHeap, MaxHeap, LongIntegerStatusStorer, and ListRandomizer.<br />
      Allowed the WeightBalancedTree 'Depth' property to return without an exception if called after an item has been removed from the tree.<br />
      Added additional constructor to WeightBalancedTree to accept an IEnumerable<T> object, whose contents are added to the tree.<br />
    </td>
  </tr>
  <tr>
    <td valign="top">1.0.0.0</td>
    <td>
      Initial version containing WeightBalancedTree.
    </td>
  </tr>
</table>