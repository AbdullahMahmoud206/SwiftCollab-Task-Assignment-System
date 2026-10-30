using System;
using System.Collections.Generic;

public class Node
{
    public int Value { get; set; }
    public Node Left { get; set; } = null!;
    public Node Right { get; set; } = null!;

    // Added Height property to avoid repeatedly calculating
    // subtree heights during balancing operations.
    // This improves performance from repeated O(n) height calculations
    // to O(1) height retrieval.
    public int Height { get; set; }

    public Node(int value)
    {
        Value = value;
        Height = 1;
    }
}

public class AvlTree
{
    // Encapsulated Root property to prevent external modification
    // of the tree structure.
    public Node Root { get; private set; } = null!;

    #region Public Methods

    public void Insert(int value)
    {
        // LLM suggestion:
        // Use AVL insertion instead of standard BST insertion
        // to maintain tree balance after every insertion.
        Root = Insert(Root, value);
    }

    public bool Search(int value)
    {
        // Added efficient search functionality.
        // Uses BST ordering to eliminate half of the remaining
        // nodes at each comparison.
        Node current = Root;

        while (current != null)
        {
            if (value == current.Value)
                return true;

            current = value < current.Value ? current.Left : current.Right;
        }

        return false;
    }

    public IEnumerable<int> InOrderTraversal()
    {
        return InOrderTraversal(Root);
    }

    #endregion

    #region AVL Insert

    private Node Insert(Node node, int value)
    {
        if (node == null)
            return new Node(value);

        if (value < node.Value)
        {
            node.Left = Insert(node.Left, value);
        }
        else if (value > node.Value)
        {
            node.Right = Insert(node.Right, value);
        }
        else
        {
            // Ignore duplicates
            return node;
        }

        // Optimization:
        // Update cached height instead of recalculating
        // the entire subtree height repeatedly.

        UpdateHeight(node);

        int balance = GetBalance(node);

        // Left Left
        if (balance > 1 && value < node.Left.Value)
            return RotateRight(node);

        // Right Right
        if (balance < -1 && value > node.Right.Value)
            return RotateLeft(node);

        // Left Right
        if (balance > 1 && value > node.Left.Value)
        {
            node.Left = RotateLeft(node.Left);
            return RotateRight(node);
        }

        // Right Left
        if (balance < -1 && value < node.Right.Value)
        {
            node.Right = RotateRight(node.Right);
            return RotateLeft(node);
        }

        return node;
    }

    #endregion

    #region AVL Helpers

    private int Height(Node node)
    {
        return node?.Height ?? 0;
    }

    private void UpdateHeight(Node node)
    {
        node.Height = Math.Max(
            Height(node.Left),
            Height(node.Right))
            + 1;
    }

    private int GetBalance(Node node)
    {
        // Returns balance factor:
        // Positive => left-heavy
        // Negative => right-heavy
        // Zero => perfectly balanced
        return node == null
            ? 0
            : Height(node.Left) - Height(node.Right);
    }

    private Node RotateRight(Node y)
    {
        // AVL rotation used to restore balance
        // when a left-heavy subtree is detected.
        Node x = y.Left;
        Node t2 = x.Right;

        x.Right = y;
        y.Left = t2;

        UpdateHeight(y);
        UpdateHeight(x);

        return x;
    }

    private Node RotateLeft(Node x)
    {

        Node y = x.Right;
        Node t2 = y.Left;

        y.Left = x;
        x.Right = t2;

        UpdateHeight(x);
        UpdateHeight(y);

        return y;
    }

    #endregion

    #region Traversal

    private IEnumerable<int> InOrderTraversal(Node node)
    {
        // Changed from direct Console.Write output
        // to IEnumerable<int> for better reusability,
        // testability, and separation of concerns.
        if (node == null)
            yield break;

        foreach (var value in InOrderTraversal(node.Left))
            yield return value;

        yield return node.Value;

        foreach (var value in InOrderTraversal(node.Right))
            yield return value;
    }

    #endregion
}