# SwiftCollab Task Assignment System
## Binary Tree Optimization and Enhancement

### Overview

The original implementation used a basic Binary Search Tree (BST) to store and retrieve tasks based on priority. While functional, the implementation lacked search capabilities, balancing mechanisms, and performance optimizations. As the number of tasks increased, the tree could become unbalanced, leading to degraded performance and slower task retrieval.

To address these limitations, the BST was refactored into a self-balancing AVL Tree that maintains efficient operations regardless of insertion order.

---

## Objectives

The following improvements were implemented:

- Ensure tree balance after insertions.
- Add efficient task search functionality.
- Reduce redundant recursive operations.
- Improve overall performance and scalability.
- Improve code maintainability and encapsulation.

---

## Improvements Implemented

### 1. Tree Balancing (AVL Tree)

#### Problem

The original BST could become heavily skewed when values were inserted in sorted order.

Example:

```text
10
 \
  20
   \
    30
     \
      40
```

This structure behaves like a linked list and degrades search performance from O(log n) to O(n).

#### Solution

The tree was converted to an AVL Tree that automatically maintains balance using rotations:

- Left Rotation
- Right Rotation
- Left-Right Rotation
- Right-Left Rotation

#### Benefit

```text
Insert  : O(log n)
Search  : O(log n)
Traversal : O(n)
```

Performance remains consistent even with large datasets.

---

### 2. Search Functionality

#### Problem

The original implementation did not provide a search operation.

#### Solution

An iterative search method was added:

```csharp
public bool Search(int value)
```

#### Benefit

- Fast task retrieval.
- Utilizes BST ordering.
- Avoids unnecessary traversal.

Time Complexity:

```text
O(log n)
```

for balanced trees.

---

### 3. Recursive Optimization

#### Problem

Tree operations often require height calculations. Computing height recursively every time causes unnecessary overhead.

#### Solution

A cached Height property was added to each node.

```csharp
public int Height { get; set; }
```

Height is updated only when modifications occur.

#### Benefit

- Eliminates repeated subtree traversals.
- Improves balancing efficiency.
- Reduces overall computational cost.

---

### 4. Memory and Performance Enhancements

Several additional optimizations were applied:

#### Encapsulation

Changed:

```csharp
public Node Root;
```

To:

```csharp
public Node Root { get; private set; }
```

This prevents accidental external modification.

---

#### Iterative Search

Search was implemented iteratively instead of recursively.

Benefits:

- Reduced stack usage.
- Improved performance.
- Eliminates recursion overhead.

---

#### Duplicate Handling

Duplicate values are ignored:

```csharp
else
{
    return node;
}
```

This prevents unnecessary node creation and excessive tree growth.

---

#### Traversal Improvements

Changed from:

```csharp
Console.Write(...)
```

to:

```csharp
IEnumerable<int>
```

Benefits:

- Better testability.
- Better separation of concerns.
- More reusable API.

---

## Complexity Comparison

| Operation | Original BST | AVL Tree |
|------------|-------------|----------|
| Insert | O(n) worst case | O(log n) |
| Search | O(n) worst case | O(log n) |
| Traversal | O(n) | O(n) |
| Space Complexity | O(n) | O(n) |

---

## Annotated Changes Summary

| Change | Purpose |
|----------|---------|
| Added AVL Rotations | Maintain balanced tree structure |
| Added Search Method | Efficient task retrieval |
| Cached Node Heights | Avoid repeated height calculations |
| Iterative Search | Reduce recursion overhead |
| Encapsulation | Protect tree integrity |
| Duplicate Prevention | Avoid unnecessary growth |
| IEnumerable Traversal | Improve reusability and maintainability |

---

## Reflection

### How did the LLM assist in refining the code?

The LLM analyzed the original Binary Search Tree implementation and identified several scalability and performance shortcomings. It suggested introducing an AVL Tree structure, implementing an efficient search method, caching node heights, improving encapsulation, and enhancing traversal design. These recommendations guided the refactoring process and helped produce a more scalable and maintainable solution.

### Were any LLM-generated suggestions inaccurate or unnecessary?

Most suggestions were useful and aligned with the assignment requirements. Some recommendations, such as implementing deletion functionality and creating a fully generic tree (`BinarySearchTree<T>`), were not strictly required by the assignment. While beneficial for production environments, they were considered optional enhancements rather than essential requirements.

### What were the most impactful improvements implemented?

The most significant improvement was replacing the unbalanced Binary Search Tree with an AVL Tree. This guarantees logarithmic search and insertion performance regardless of insertion order. Additionally, adding search functionality directly addressed a major missing feature, while height caching greatly improved balancing efficiency by eliminating unnecessary recursive height calculations.

---

## Conclusion

The original Binary Search Tree successfully stored task priorities but did not scale efficiently. By introducing AVL balancing, efficient searching, height caching, improved encapsulation, and optimized traversal patterns, the new implementation delivers reliable O(log n) insertion and retrieval performance, making it significantly more suitable for SwiftCollab's growing task assignment system.