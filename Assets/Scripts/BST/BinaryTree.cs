using System;
using UnityEngine;

class BinaryTree<T> where T: INodable
{
    public Node<T> Root { get; set; }

    public Node<T> Add(T nodeToAdd)
    {
        return Add(this.Root, nodeToAdd);
    }
 
    private Node<T> Add(Node<T> node, T nodeToAdd)
    {
        if (this.Root == null || this.Root.node == null)
        {
            this.Root = new Node<T>();
            this.Root.node = nodeToAdd;
            node = this.Root;
        }
        else
        {
            if (node == null || node.node == null)
            {
                node = new Node<T>();
                node.node = nodeToAdd;
                return node;
            }
            if(nodeToAdd.nodeValue <= node.node.nodeValue)
            {
                node.LeftNode = Add(node.LeftNode, nodeToAdd);
            }
            else if (nodeToAdd.nodeValue > node.node.nodeValue)
            {
                node.RightNode = Add(node.RightNode, nodeToAdd);
            }
        }
        return node;
    }

    public bool Contains(T node)
    {
        if (Find(node) == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
 
    public T Find(T node)
    {
        return this.Find(node, this.Root);            
    }
 
    public void Remove(T node)
    {
        if (node != null) {
            Remove(this.Root, node);
        }
    }

    public T FindLowestNode()
    {
        return MinNode(Root);
    }
 
    private Node<T> Remove(Node<T> parent, T node)  
    {  
        if (parent == null)
        { 
            return null;
        }

        if(parent.node == null)
        {
            return null;
        }
  
        if (node.nodeValue <= parent.node.nodeValue && node.id != parent.node.id)
        {
            parent.LeftNode = Remove(parent.LeftNode, node);
        }
        else if (node.nodeValue > parent.node.nodeValue)
        {
            parent.RightNode = Remove(parent.RightNode, node); 
        }
        else
        {
            parent.node = MinNode(parent.RightNode);  

            parent.RightNode = Remove(parent.RightNode, parent.node);
        }

        if (parent.node == null)
        {
            return null;
        }
        
        return parent;  
    }  
 
    private int MinValue(Node<T> node)
    {
        int minv = node.node.nodeValue;
 
        while (node.LeftNode != null)
        {
            minv = node.LeftNode.node.nodeValue;
            node = node.LeftNode;
        }
 
        return minv;
    }

    private T MinNode(Node<T> startingNode)
    {
        if (startingNode == null)
        {
            return default(T);
        }
        Node<T> minN = startingNode;
 
        while (startingNode.LeftNode != null && startingNode.LeftNode.node != null)
        {
            minN = startingNode.LeftNode;
            startingNode = startingNode.LeftNode;
        }
 
        return minN.node;
    }
 
    private T Find(T node, Node<T> parent)
    {
        if (parent != null && parent.node != null)
        {
            if (node.id == parent.node.id) 
            {
                return parent.node;
            }
            if (node.nodeValue < parent.node.nodeValue)
            {
                return Find(node, parent.LeftNode);
            }
            else
            {
                return Find(node, parent.RightNode);
            }
        }
 
        return default(T);
    }

    public int GetTreeDepth()
    {
        return this.GetTreeDepth(this.Root);
    }
 
    private int GetTreeDepth(Node<T> parent)
    {
        return parent == null ? 0 : Mathf.Max(GetTreeDepth(parent.LeftNode), GetTreeDepth(parent.RightNode)) + 1;
    }

    public int Count()  
    {  
        return RecursiveCount(Root);
    }

    private int RecursiveCount(Node<T> startingNode)
    {
        if(startingNode == null || startingNode.node == null)
        {
            return 0;
        }
        else
        {
            return 1 + RecursiveCount(startingNode.LeftNode) + RecursiveCount(startingNode.RightNode);
        }

    }
}