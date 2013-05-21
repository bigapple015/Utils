package com.cmlu.commons;

import java.util.concurrent.Exchanger;

import com.cmlu.lang.StdOut;

/**
 * 优先队列，每次弹出最大值
 * @author Administrator
 *
 * @param <Key>
 */
public class UnorderedArrayMaxPQ<Key extends Comparable<Key>> {

    /**
     * 数据内部存储
     */
    private Key[] pq;
    
    /**
     * 元素的数目
     */
    private int N;
    
    /**
     * 构造函数
     * @param capacity
     */
    public UnorderedArrayMaxPQ(int capacity){
	pq = (Key[]) new Comparable[capacity];
	N = 0;
    }
    
    /**
     * 判断优先队列是否为空
     * @return
     */
    public boolean isEmpty(){
	return N == 0;
    }
    
    /**
     * 元素的数目
     * @return
     */
    public int size(){
	return N;
    }
    
    /**
     * 插入元素
     * @param x
     */
    public void insert(Key x){
	pq[N++] = x;
    }
    
    /**
     * 删除最大值
     * @return
     */
    public Key delMax(){
	int max = 0;
	for(int i=1;i<N;i++){
	    if(less(max,i))
		max = i;
	}
	exch(max,N-1);
	return pq[--N];
    }
    
    
    /**
     * 比较索引为i，j的元素的大小
     */
    private boolean less(int i,int j){
	return (pq[i].compareTo(pq[j]) < 0);
    }
    
    /**
     * 交换索引为i和j的元素
     * @param i
     * @param j
     */
    private void exch(int i,int j){
	Key swap = pq[i];
	pq[i] = pq[j];
	pq[j] = swap;
    }
    
    
    /***********************************************************************
     * Test routine.
     **********************************************************************/
     public static void main(String[] args) {
         UnorderedArrayMaxPQ<String> pq = new UnorderedArrayMaxPQ<String>(10);
         pq.insert("this");
         pq.insert("is");
         pq.insert("a");
         pq.insert("test");
         while (!pq.isEmpty()) 
             StdOut.println(pq.delMax());
     }

}
