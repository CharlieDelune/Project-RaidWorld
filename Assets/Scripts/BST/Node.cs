class Node<T> where T : INodable
{
    public Node<T> LeftNode { get; set; }
    public Node<T> RightNode { get; set; }
    public T node { get; set; }
}