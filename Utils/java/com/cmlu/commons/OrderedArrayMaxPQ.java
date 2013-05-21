package com.cmlu.commons;

import com.cmlu.lang.StdOut;

public class OrderedArrayMaxPQ<Key extends Comparable<Key>> {
    
    /**
     * 保存底层的数组
     */
    private Key[] pq;
    
    /**
     * 优先队列中的元素数目
     */
    private int N;

    /**
     * 构造函数
     */
    public OrderedArrayMaxPQ(int capacity){
	pq = (Key[])(new Comparable[capacity]);
	N = 0;
    }
    
    /**
     * 判断优先队列是否为空
     */
    public boolean isEmpty(){
	return N == 0;
    }
    
    /**
     * 优先队列中元素的数目
     * @return
     */
    public int size(){
	return N;
    }
    
    /**
     * 删除最大值
     * @return
     */
    public Key delMax(){
	return pq[--N];
    }
    
    /**
     * 插入元素
     * @param key
     */
    public void insert(Key key){
	int i = N - 1;
	while(i>=0 && less(key,pq[i])){
	    pq[i+1] = pq[i];
	    i--;
	}
	
	pq[i+1] = key;
	N++;
    }
    
    /**
     * v 是否小于 w
     * @param v
     * @param w
     * @return
     */
    private boolean less(Key v,Key w){
	return v.compareTo(w) < 0;
    }
    
    /**
     * 交换
     * @param i
     * @param j
     */
    public void exch(int i,int j){
	Key swap = pq[i];
	pq[i] = pq[j];
	pq[j] = swap;
    }
    
    /***********************************************************************
     * Test routine.
     **********************************************************************/
     public static void main(String[] args) {
         OrderedArrayMaxPQ<String> pq = new OrderedArrayMaxPQ<String>(10);
         pq.insert("this");
         pq.insert("is");
         pq.insert("a");
         pq.insert("test");
         while (!pq.isEmpty())
             StdOut.println(pq.delMax());
     }
}
