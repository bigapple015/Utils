package com.cmlu.algorithm.search;

import javax.swing.text.html.MinimalHTMLWriter;

import com.cmlu.commons.Queue;
import com.cmlu.lang.StdIn;
import com.cmlu.lang.StdOut;

public class BST<Key extends Comparable<Key>, Value> {

    /**
     * 二叉搜索树的根
     */
    public Node root;
    
    /**
     * 内部结点类
     * @author Administrator
     *
     */
    public class Node{
	private Key key;
	private Value val;
	private Node left,right;
	/**
	 * 该子树的所有结点的数目，包含根节点
	 */
	private int N;
	
	
	public Node(Key key,Value val,int N){
	    this.key = key;
	    this.val = val;
	    this.N = N;
	}
	
    }
    
    /**
     * 容器是否为空
     * @return
     */
    public boolean isEmpty(){
	return size() == 0;
    }
    
    /**
     * 返回二叉搜索树的元素的个数
     * @return
     */
    public int size(){
	return size(root);
    }
    
    /**
     * 结点x所在的子树的结点的个数
     * @param x
     * @return
     */
    private int size(Node x){
	if(x == null) return 0;
	else return x.N;
    }
    
    /**
     * 是否包含指定的键
     */
    public boolean contains(Key key){
	return get(key) != null;
    }
    
    /**
     * 返回键连接到的值，如果没有键则返回null
     * @param key
     * @return
     */
    public Value get(Key key){
	return get(root,key);
    }
    
    /**
     * 从指定的子树中获取指定的键
     */
    public Value get(Node x,Key key){
	if(x == null) return null;
	int cmp = key.compareTo(x.key);
	if(cmp < 0) return get(x.left,key);
	else if(cmp>0) return get(x.right,key);
	else return x.val;
    }
    
    /**
     * 插入一个键值对，如果已经存在键，则更新值，如果值为null，则删除键
     * @param key
     * @param val
     */
    public void put(Key key,Value val){
	if(val == null){
	    delete(key);
	    return;
	}
	
	root = put(root, key, val);
	assert check();
    }
    
    
    private Node put(Node x,Key key,Value val){
	//插入了第一个结点
	if(x == null) return new Node(key, val, 1);
	int cmp = key.compareTo(x.key);
	if(cmp < 0) x.left = put(x.left, key, val);
	else if(cmp>0) x.right = put(x.right, key, val);
	else x.val = val;
	
	x.N = 1 + size(x.left) + size(x.right);
	return x;
    }
    
    /**
     * 删除最小值
     */
    public void deleteMin(){
	if(isEmpty()) throw new RuntimeException("Symbol table underflow");
	
	root = deleteMin(root);
	assert check();
    }
    
    
    private Node deleteMin(Node x){
	if(x.left == null) return x.right;
	x.left = deleteMin(x.left);
	x.N = size(x.left) +size(x.right)+1;
	return x;
    }
    
    
    public void deleteMax(){
	if(isEmpty()){
	    throw new RuntimeException("Symbol table underflow");
	}
	
	root = deleteMax(root);
	assert check();
    }
    
    public Node deleteMax(Node x){
	if(x.right == null) return x.left;
	x.right = deleteMax(x.right);
	x.N = size(x.left) + size(x.right) + 1;
	return x;
    }
    
    public void delete(Key key){
	root = delete(root,key);
	assert check();
    }
    
    
    private Node delete(Node x,Key key){
	if( x == null) return null;
	int cmp = key.compareTo(x.key);
	if(cmp < 0) x.left = delete(x.left, key);
	else if(cmp >0) x.right = delete(x.right, key);
	else{
	    if(x.right == null) return x.left;
	    if(x.left == null) return x.right;
	    Node t = x;
	    x = min(t.right);
	    x.right = deleteMin(t.right);
	    x.left = t.left;
	   }
	
	x.N = size(x.left) +size(x.right)+1;
	return x;
    }
    
    
    /***********************************************************************
     *  Min, max, floor, and ceiling
     ***********************************************************************/
     public Key min() {
         if (isEmpty()) return null;
         return min(root).key;
     } 

     private Node min(Node x) { 
         if (x.left == null) return x; 
         else                return min(x.left); 
     } 

     public Key max() {
         if (isEmpty()) return null;
         return max(root).key;
     } 

     private Node max(Node x) { 
         if (x.right == null) return x; 
         else                 return max(x.right); 
     } 
     
     
     public Key floor(Key key) {
	        Node x = floor(root, key);
	        if (x == null) return null;
	        else return x.key;
	    } 

	    private Node floor(Node x, Key key) {
	        if (x == null) return null;
	        int cmp = key.compareTo(x.key);
	        if (cmp == 0) return x;
	        if (cmp <  0) return floor(x.left, key);
	        Node t = floor(x.right, key); 
	        if (t != null) return t;
	        else return x; 
	    } 

	    public Key ceiling(Key key) {
	        Node x = ceiling(root, key);
	        if (x == null) return null;
	        else return x.key;
	    }

	    private Node ceiling(Node x, Key key) {
	        if (x == null) return null;
	        int cmp = key.compareTo(x.key);
	        if (cmp == 0) return x;
	        if (cmp < 0) { 
	            Node t = ceiling(x.left, key); 
	            if (t != null) return t;
	            else return x; 
	        } 
	        return ceiling(x.right, key); 
	    } 

	   /***********************************************************************
	    *  Rank and selection
	    ***********************************************************************/
	    public Key select(int k) {
	        if (k < 0 || k >= size())  return null;
	        Node x = select(root, k);
	        return x.key;
	    }

	    // Return key of rank k. 
	    private Node select(Node x, int k) {
	        if (x == null) return null; 
	        int t = size(x.left); 
	        if      (t > k) return select(x.left,  k); 
	        else if (t < k) return select(x.right, k-t-1); 
	        else            return x; 
	    } 

	    public int rank(Key key) {
	        return rank(key, root);
	    } 

	    // Number of keys in the subtree less than x.key. 
	    private int rank(Key key, Node x) {
	        if (x == null) return 0; 
	        int cmp = key.compareTo(x.key); 
	        if      (cmp < 0) return rank(key, x.left); 
	        else if (cmp > 0) return 1 + size(x.left) + rank(key, x.right); 
	        else              return size(x.left); 
	    } 

	   /***********************************************************************
	    *  Range count and range search.
	    ***********************************************************************/
	    public Iterable<Key> keys() {
	        return keys(min(), max());
	    }

	    public Iterable<Key> keys(Key lo, Key hi) {
	        Queue<Key> queue = new Queue<Key>();
	        keys(root, queue, lo, hi);
	        return queue;
	    } 

	    private void keys(Node x, Queue<Key> queue, Key lo, Key hi) { 
	        if (x == null) return; 
	        int cmplo = lo.compareTo(x.key); 
	        int cmphi = hi.compareTo(x.key); 
	        if (cmplo < 0) keys(x.left, queue, lo, hi); 
	        if (cmplo <= 0 && cmphi >= 0) queue.enqueue(x.key); 
	        if (cmphi > 0) keys(x.right, queue, lo, hi); 
	    } 

	    public int size(Key lo, Key hi) {
	        if (lo.compareTo(hi) > 0) return 0;
	        if (contains(hi)) return rank(hi) - rank(lo) + 1;
	        else              return rank(hi) - rank(lo);
	    }


	    // height of this BST (one-node tree has height 0)
	    public int height() { return height(root); }
	    private int height(Node x) {
	        if (x == null) return -1;
	        return 1 + Math.max(height(x.left), height(x.right));
	    }


	    // level order traversal
	    public Iterable<Key> levelOrder() {
	        Queue<Key> keys = new Queue<Key>();
	        Queue<Node> queue = new Queue<Node>();
	        queue.enqueue(root);
	        while (!queue.isEmpty()) {
	            Node x = queue.dequeue();
	            if (x == null) continue;
	            keys.enqueue(x.key);
	            queue.enqueue(x.left);
	            queue.enqueue(x.right);
	        }
	        return keys;
	    }

	  /*************************************************************************
	    *  Check integrity of BST data structure
	    *************************************************************************/
	    private boolean check() {
	        if (!isBST())            StdOut.println("Not in symmetric order");
	        if (!isSizeConsistent()) StdOut.println("Subtree counts not consistent");
	        if (!isRankConsistent()) StdOut.println("Ranks not consistent");
	        return isBST() && isSizeConsistent() && isRankConsistent();
	    }

	    // does this binary tree satisfy symmetric order?
	    // Note: this test also ensures that data structure is a binary tree since order is strict
	    private boolean isBST() {
	        return isBST(root, null, null);
	    }

	    // is the tree rooted at x a BST with all keys strictly between min and max
	    // (if min or max is null, treat as empty constraint)
	    // Credit: Bob Dondero's elegant solution
	    private boolean isBST(Node x, Key min, Key max) {
	        if (x == null) return true;
	        if (min != null && x.key.compareTo(min) <= 0) return false;
	        if (max != null && x.key.compareTo(max) >= 0) return false;
	        return isBST(x.left, min, x.key) && isBST(x.right, x.key, max);
	    } 

	    // are the size fields correct?
	    private boolean isSizeConsistent() { return isSizeConsistent(root); }
	    private boolean isSizeConsistent(Node x) {
	        if (x == null) return true;
	        if (x.N != size(x.left) + size(x.right) + 1) return false;
	        return isSizeConsistent(x.left) && isSizeConsistent(x.right);
	    } 

	    // check that ranks are consistent
	    private boolean isRankConsistent() {
	        for (int i = 0; i < size(); i++)
	            if (i != rank(select(i))) return false;
	        for (Key key : keys())
	            if (key.compareTo(select(rank(key))) != 0) return false;
	        return true;
	    }


	   /*****************************************************************************
	    *  Test client
	    *****************************************************************************/
	    public static void main(String[] args) { 
	        BST<String, Integer> st = new BST<String, Integer>();
	        for (int i = 0; !StdIn.isEmpty(); i++) {
	            String key = StdIn.readString();
	            st.put(key, i);
	        }

	        for (String s : st.levelOrder())
	            StdOut.println(s + " " + st.get(s));

	        StdOut.println();

	        for (String s : st.keys())
	            StdOut.println(s + " " + st.get(s));
	    }
     
     
    
    
}
