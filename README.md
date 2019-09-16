MoreComplexDataStructures
-------------------------

MoreComplexDataStructures is a class library containing a collection of data structures (plus related utility classes) more complex than those found in the standard .NET framework.

### Contents
**WeightBalancedTree** - An implementation of a [weight-balanced tree](https://en.wikipedia.org/wiki/Weight-balanced_tree).  The tree maintains counts of the nodes in each node's subtrees, and implements self-balancing by performing standard rotations and splay tree [zig-zag operations](https://en.wikipedia.org/wiki/Splay_tree#Splaying).  The balancing algorithm maintains an overall height typically within a factor of 1.2 of the optimal (1-based) height for the number of nodes stored.  The class implements several methods to traverse based on item value comparison (e.g. GetNextLessThan(), GetNextGreaterThan(), GetAllLessThan(), GetAllGreaterThan()), and executes GetCountLessThan() and GetCountGreaterThan() in O(log(n)) time (since these values are stored and maintained at each node).  The class also provides methods to perform pre, post, and in-order depth-first search, breadth-first search, and to return a random node item.

**MinHeap** - A tree-based implementation of a [min heap](https://en.wikipedia.org/wiki/Heap_(data_structure)).  Insert() and ExtractMin() methods return with order O(log(n)) time complexity.  Also provides a method to traverse the nodes of the underlying tree via a breadth-first search.

**MaxHeap** - A tree-based implementation of a [max heap](https://en.wikipedia.org/wiki/Heap_(data_structure)).  Insert() and ExtractMax() methods return with order O(log(n)) time complexity.  Also provides a method to traverse the nodes of the underlying tree via a breadth-first search.

**LongIntegerStatusStorer** - Stores a true/false status for a complete set of long (Int64) integers.  Uses an underlying tree holding ranges of integers to store the statuses.  Also provides a method TraverseTree() to traverse the ranges stored in the tree via a breadth-first search.  Designed to be more memory efficient than an equivalent boolean array when large sets of the Int64 key values are contiguous (and hence can be 'condensed' into a range), and to support ranges larger than Int32.MaxValue.

**ListRandomizer** - Randomizes a List or Array using the [Fisher/Yates/Knuth algorithm](https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle) (O(n) time complexity).

**WeightedRandomGenerator** - Returns items randomly based on configured weightings.  The underlying implementation uses a tree, so the Generate() method returns with order O(log(n)) time complexity (where n is the number of weightings defined).  The items and weightings can be defined as follows...

    WeightedRandomGenerator<Char> weightedRandomGenerator = new WeightedRandomGenerator<Char>();
    List<Tuple<Char, Int64>> weightings = new List<Tuple<Char, Int64>>()
    {
        new Tuple<Char, Int64>('a', 1), 
        new Tuple<Char, Int64>('b', 2),
        new Tuple<Char, Int64>('c', 3),
        new Tuple<Char, Int64>('d', 4)
    };a
    weightedRandomGenerator.SetWeightings(weightings);

...then calling the Generate() method 100,000 times would result in a distribution similar to the following...

    a : 10022
    b : 19694
    c : 30456
    d : 39828

**Trie** - An implementation of a [trie / prefix tree](https://en.wikipedia.org/wiki/Trie).  Nodes of the trie maintain a count of the number of sequences in each subtree, hence the GetCountOfSequencesWithPrefix() method returns a count of sequences with the specified prefix with order O(n) time complexity (where n is the number of elements in the prefix sequence).

**FrequencyTable** - A simple (Dictionary-based) frequency table, which stores the frequency of occurrence of objects.

**BinarySearchTreeBalancedInserter** - Inserts a set of items into a binary search tree, ensuring that the tree is balanced, and depth is minimized (although this class is now somewhat redundant since balancing has been implemented in WeightBalancedTree).

**CharacterTrie** - Effectively a Trie&lt;Char&gt;, but with special implementations of the Insert(), Delete(), and Contains() methods which accept String parameters (and avoid the overhead of having to call String.ToCharArray() as is required with a Trie&lt;Char&gt;).

**LRUCache** - A simple implementation of a [least-recently-used](https://en.wikipedia.org/wiki/Cache_replacement_policies#Least_recently_used_(LRU)) [cache](https://en.wikipedia.org/wiki/Cache_(computing)) using an underlying Dictionary and LinkedList.  By default the cache stores a fixed number of items (defined by constructor parameter 'itemLimit'), but it optionally allows overriding the routine to check whether the cache is full (e.g. to decide instead based on total memory usage, etc...).

**UnflaggedNumberGenerator** - Allows 'flagging' (i.e. setting true or false) for each number in a consecutive range (where the range length is <= Int64.MaxValue), and then provides methods to identify which numbers have not been flagged (GetLowestUnflaggedNumbers(), GetHighestUnflaggedNumbers(), etc...).  The underlying implementation uses a tree of integer ranges, so the class is most efficient when large, contiguous sections of the overall range are either flagged or unflagged.

**UniqueRandomGenerator** - Generates unique Int64 random numbers within a given range (where the range length is <= Int64.MaxValue). The underlying implementation uses a balanced tree of integer ranges, so the Generate() method returns with order O(log(n)) time complexity.

Note - Another (more simple) way to generate a range of unique random numbers is to initialize the numbers sequentially in an array, and then randomize the array using the Fisher/Yates/Knuth algorithm (as the ListRandomizer class does).  The issue with this method for very large ranges is that the memory usage is also large (since every number in the range must be preallocated in memory).  Also the range cannot be larger than Int32.MaxValue.  I built this class with the goal of generating larger ranges with far more efficient memory usage (the class starts with a single node item representing the full range, and then ’splits’ these node items and pushes them down the tree as the Generate() method is called… hence consecutive numbers are ‘condensed’ into a single range class).  After testing I found that during the process of generating all numbers in the range, at some point, the number of nodes in the tree will become ~n/4 (where n is the range size).  Unfortunately this makes the class less memory efficient than the Fisher/Yates/Knuth array method, as each node consumes more than 4 times as much memory as an Int64 (references to parent and child nodes, counts of child nodes, counts of ranges of child nodes, object overhead, etc…).  In any case building the class was an interesting exercise, and someone may find a use for it.

**PriorityQueue** - An implementation of a [double-ended](https://en.wikipedia.org/wiki/Double-ended_priority_queue) [priority queue](https://en.wikipedia.org/wiki/Priority_queue).  As the underlying structure is a balanced tree, most methods return with order O(log(n)) time complexity.  The class allows dequeuing of specific items aside from the minimum and maximum, and exposes several methods to inspect the contents of the queue.

Note - The priority of enqueued items is set and stored as a double.  Whilst NaN is not permitted (attempting to enqueue an item with NaN priority will throw an exception), it is possible to enqueue items with Double.PositiveInfinity or Double.NegativeInfinity priority.  Doing so will affect the behaviour of the EnqueueAsMax() and EnqueueAsMin() methods (e.g. calling EnqueueAsMax() when the maximum priority is already Double.PositiveInfinity will result in the new item also being enqueued with Double.PositiveInfinity priority... the same as the current maximum, not greater). Properties 'MaxPriority' and 'MinPriority' are provided to retrieve the maximum and minimum priorities if this behaviour needs to be predicted, or avoided.

### Future Enhancements
- Enhance any methods which return an IEnumerable to throw an InvalidOperationException if the object structure is changed while enumerating.
- Remove inefficiency of converting List&lt;Char&gt; to String in CharacterTrie.GetAllStringsWithPrefix().
- Remove redundant / unused conditional branches in WeightBalancedTree.GetRandomItem() method (e.g. 'if (currentNode.LeftChildNode == null)')
- Refactor to remove inefficiency of traversing to the start node twice in GetAllLessThan(T item) and GetAllGreaterThan(T item) in the WeightBalancedTree class.
- Refactor methods Insert() and ExtractMax()/ExtractMin() on the MaxHeap/MinHeap classes into the HeapBase class.
- Abstract use of IComparable&lt;T&gt;.CompareTo() in heap classes to make code easier to read.
- Consider adding a linked list implementation which supports Contains(T item) ( O(1) ), by additionally storing list data in a HashSet.
- Consider adding a [skip list](https://en.wikipedia.org/wiki/Skip_list).
- Consider building an IBinarySearchTree implementation using struct rather than class nodes (for potential reduced memory usage).
- Consider adding array-based heaps

### Release History

<table>
  <tr>
    <td><b>Version</b></td>
    <td><b>Changes</b></td>
  </tr>
  <tr>
    <td valign="top">1.8.0.0</td>
    <td>
      Corrected bug in PriorityQueue class where methods EnqueueAsMax() and EnqueueAsMin() would not always result in items being enqueued as maximum or minimum priority.  Now uses class DoubleBinaryIncrementer to ensure that the new priority generated is the binary successor or predecessor to the existing maximum or minimum.<br />
      Added MaxPriority and MinPriority properties to the PriorityQueue class.
    </td>
  </tr>
  <tr>
    <td valign="top">1.7.0.0</td>
    <td>
      Optimized the WeightBalancedTree balancing algorithm to include both standard rotations and splay tree zig-zag operations.
    </td>
  </tr>
  <tr>
    <td valign="top">1.6.0.0</td>
    <td>
      Added PriorityQueue.<br />
      Added an additional FrequencyTable constructor to allow pre-population with a collection of items and corresponding counts. 
    </td>
  </tr>
  <tr>
    <td valign="top">1.5.0.0</td>
    <td>
      Converted to .NET Standard.<br />
      Added LRUCache, CharacterTrie, UniqueRandomGenerator, UnflaggedNumberGenerator classes.<br />
      Implemented automatic balancing in WeightBalancedTree.<br />
      Added Min and Max properties to WeightBalancedTree.<br />
      Added method Get() to WeightBalancedTree to return a specified node item (useful when the node items are container classes which hold additional data to that used in the IComparable<T> implementation... i.e. allowing the tree to be used as a treemap).<br />
      Trie.GetAllSequencesWithPrefix() now returns IEnumerable&lt;List&lt;T&gt;&gt;.<br />
      Added an additional Trie constructor which returns the root node via an 'out' parameter (to allow custom traversals).<br />
      Added method FrequencyTable.Clear().<br />
      Refactored WeightedRandomGenerator to remove Dictionary member 'weightingToItemMap' and instead store data in ItemAndWeighting&lt;T&gt; as each tree node item.<br />
      Removed TreeBasedListRandomizer class.<br />
    </td>
  </tr>
  <tr>
    <td valign="top">1.4.0.0</td>
    <td>
      Added GetAllLessThan(T item) and GetAllGreaterThan(T item) methods to WeightBalancedTree.<br />
    </td>
  </tr>
  <tr>
    <td valign="top">1.3.0.0</td>
    <td>
      Added Trie, FrequencyTable, and BinarySearchTreeBalancedInserter.<br />
      WeightBalancedTree.GetRandomItem() method now returns random items with even distribution.<br />
      WeightBalancedTree now implements interface IBinarySearchTree&lt;T&gt;.<br />
    </td>
  </tr>
  <tr>
    <td valign="top">1.2.0.0</td>
    <td>
      Added WeightedRandomGenerator.<br />
    </td>
  </tr>
  <tr>
    <td valign="top">1.1.0.0</td>
    <td>
      Added MinHeap, MaxHeap, LongIntegerStatusStorer, and ListRandomizer.<br />
      Allowed the WeightBalancedTree 'Depth' property to return without an exception if called after an item has been removed from the tree.<br />
      Added additional constructor to WeightBalancedTree to accept an IEnumerable&lt;T&gt; object, whose contents are added to the tree.<br />
    </td>
  </tr>
  <tr>
    <td valign="top">1.0.0.0</td>
    <td>
      Initial version containing WeightBalancedTree.
    </td>
  </tr>
</table>