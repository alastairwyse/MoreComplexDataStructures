MoreComplexDataStructures
-------------------------

MoreComplexDataStructures is a class library containing a collection of data structures more complex than those found in the standard .NET framework.  The project currently contains an implementation of a [weight-balanced tree] (https://en.wikipedia.org/wiki/Weight-balanced_tree).  Although self-balancing is not yet implemented, the tree maintains counts of the nodes in each node's subtrees.  This allows methods GetCountGreaterThan(T item) and GetCountLessThan(T item) (which return the number of items greater than and less than a specified item) to return with order O(log(n)) time complexity.  The WeightBalancedTree class also provides methods to perform pre, post, and in-order depth-first search, and breadth-first search.

#### Future Enhancements
- Implement self-balancing in the WeightBalancedTree class.

#### Release History

<table>
  <tr>
    <td><b>Version</b></td>
    <td><b>Changes</b></td>
  </tr>
  <tr>
    <td valign="top">1.0.0.0</td>
    <td>
      Initial version containing WeightBalancedTree.
    </td>
  </tr>
</table>