package com.cmlu.algorithm.search;

import com.cmlu.commons.Queue;
import com.cmlu.lang.StdIn;
import com.cmlu.lang.StdOut;

public class SequentialSearchST<Key, Value> {

    //元素的个数
    private int N;
    //第一个结点
    private Node first;
    
    //内部类
    private class Node{
	private Key key;
	private Value value;
	private Node next;
	
	public Node(Key key,Value value,Node next){
	    this.key = key;
	    this.value = value;
	    this.next = next;
	}
    }
    
    /**
     * 返回元素的数目
     * @return
     */
    public int size(){
	return N;
    }
    
    /**
     * 是否为空
     * @return
     */
    public boolean isEmpty(){
	return N == 0;
    }
    
    
    public boolean contains(Key key){
	return get(key) != null;
    }
    
    /**
     * 获取结点，如果找不到返回null
     * @param key
     * @return
     */
    public Value get(Key key){
	for(Node x=first;x!=null;x=x.next){
	    if(key.equals(x.key)) return x.value;
	}
	
	return null;
    }
    
    /**
     * 如果已经包含该建，则更新值；否则插入到结点开头
     * 如果值为null，则删除该键
     * @param key
     * @param val
     */
    public void put(Key key,Value val){
	if(val == null) {
	    delete(key);
	    return;
	}
	
	for(Node x=first;x!=null;x=x.next){
	    if(key.equals(x.key)){
		x.value = val;
		return;
	    }
	}
	
	first = new Node(key, val, first);
	N++;
	
    }
    
    
    public void delete(Key key){
	first = delete(first,key);
    }
    
    private Node delete(Node x,Key key){
	if(x == null) return null;
	
	if(key.equals(x.key)){
	    N--;
	    return x.next;
	}
	
	x.next = delete(x.next, key);
	return x;
    }
    
 // return all keys as an Iterable
    public Iterable<Key> keys()  {
        Queue<Key> queue = new Queue<Key>();
        for (Node x = first; x != null; x = x.next)
            queue.enqueue(x.key);
        return queue;
    }
    
    
    /***********************************************************************
     * Test client
     **********************************************************************/
     public static void main(String[] args) {
         SequentialSearchST<String, Integer> st = new SequentialSearchST<String, Integer>();
         for (int i = 0; !StdIn.isEmpty(); i++) {
             String key = StdIn.readString();
             st.put(key, i);
         }
         for (String s : st.keys())
             StdOut.println(s + " " + st.get(s));
     }
    
}
