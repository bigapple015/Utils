package com.cmlu.commons;

import java.nio.channels.Pipe.SinkChannel;
import java.util.Comparator;
import java.util.Iterator;
import java.util.NoSuchElementException;
import java.util.concurrent.Exchanger;

import javax.management.RuntimeErrorException;

import com.cmlu.lang.StdIn;
import com.cmlu.lang.StdOut;

public class MaxPQ<Key> implements Iterable<Key> {

    /**
     * 底层存储
     */
    private Key[] pq;
    
    /**
     * 优先队列中存储的元素个数
     */
    private int N;
    
    /**
     * 可选的比较
     */
    private Comparator<Key> comparator;
    
    /**
     * 构造函数
     * @param capacity
     */
    public MaxPQ(int capacity){
	pq = (Key[]) new Object[capacity+1];
	N = 0;
    }
    
    /**
     * 构造函数
     */
    public MaxPQ(){
	this(1);
    }
    
    /**
     * 构造函数
     */
    public MaxPQ(int initCapacity,Comparator<Key> comparator){
	this.comparator = comparator;
	pq = (Key[]) new Object[initCapacity+1];
	N = 0;
    }
    
    /**
     * 构造函数
     * @param comparator
     */
    public MaxPQ(Comparator<Key> comparator){
	this(1,comparator);
    }
    
    
    public MaxPQ(Key[] keys){
	N = keys.length;
	pq = (Key[])new Object[keys.length + 1];
	for(int i=0;i<N;i++){
	    pq[i+1] = keys[i];
	}
	
	for(int k= N/2;k>=1;k--){
	    sink(k);
	}
	
	assert isMaxHeap();
    }
    
    
    /**
     * 判断优先队列是否为空
     * @return
     */
    public boolean isEmpty(){
	return N == 0;
    }
    
    /**
     * 返回优先队列中元素的个数
     * @return
     */
    public int size(){
	return N;
    }
    
    /**
     * 返回优先队列中的最大值
     * @return
     */
    public Key max(){
	if(isEmpty()){
	    throw new RuntimeException("Priority queue underflow");
	}
	
	return pq[1];
    }
    
    /**
     * 增加完全平衡堆的数量
     * @param capacity
     */
    private void resize(int capacity){
	assert capacity > N;
	Key[] temp = (Key[])new Object[capacity];
	for(int i=1;i<=N;i++){
	    temp[i] = pq[i];
	}
	pq = temp;
    }
    
    /**
     * 插入一个优先队列
     * @param x
     */
    public void insert(Key x){
	if(N >= pq.length-1){
	    resize(2*pq.length);
	}
	
	pq[++N] = x;
	swim(N);
	assert isMaxHeap();
    }
    
    /**
     * 删除最大值，并返回
     * @return
     */
    public Key delMax(){
	if(N == 0){
	    throw new RuntimeException("Priority quue underflow");
	}
	
	Key max = pq[1];
	exch(1,N--);
	sink(1);
	//帮助垃圾回收
	pq[N+1] = null;
	if((N>0) && (N==(pq.length-1)/4)){
	    resize(pq.length/2);
	}
	
	assert isMaxHeap();
	return max;
    }
    
    /**
     * 重排heap，将最小的一个上调
     * @param k
     */
    private void swim(int k){
	while(k>1 && less(k/2,k)){
	    exch(k,k/2);
	    k = k/2;
	}
    }
    
    /**
     * 找到子节点中大的一个，如果它比子节点小，则交换
     * @param k
     */
    private void sink(int k){
	while(2*k<=N){
	    int j = 2*k;
	    if(j<N && less(j,j+1)) j++;
	    if(!less(k,j)) break;
	    exch(k,j);
	    k = j;
	}
    }
    
    /**
     * 如果i < j索引所在的元素，则返回true
     * @param i
     * @param j
     * @return
     */
    private boolean less(int i,int j){
	if(comparator == null){
	    return ((Comparable<Key>)pq[i]).compareTo(pq[j]) <0;
	}
	else{
	    return comparator.compare(pq[i], pq[j]) < 0;
	}
    }
    
    /**
     * 交换两个索引位置的元素
     * @param i
     * @param j
     * @return
     */
    private void exch(int i,int j){
	Key swap = pq[i];
	pq[i] = pq[j];
	pq[j] = swap;
    }
    
    
 // is pq[1..N] a max heap?
    private boolean isMaxHeap() {
        return isMaxHeap(1);
    }

    // is subtree of pq[1..N] rooted at k a max heap?
    private boolean isMaxHeap(int k) {
        if (k > N) return true;
        int left = 2*k, right = 2*k + 1;
        if (left  <= N && less(k, left))  return false;
        if (right <= N && less(k, right)) return false;
        return isMaxHeap(left) && isMaxHeap(right);
    }


   /***********************************************************************
    * Iterator
    **********************************************************************/

   /**
     * Return an iterator that iterates over all of the keys on the priority queue
     * in descending order.
     * <p>
     * The iterator doesn't implement <tt>remove()</tt> since it's optional.
     */
    public Iterator<Key> iterator() { return new HeapIterator(); }

    private class HeapIterator implements Iterator<Key> {

        // create a new pq
        private MaxPQ<Key> copy;

        // add all items to copy of heap
        // takes linear time since already in heap order so no keys move
        public HeapIterator() {
            if (comparator == null) copy = new MaxPQ<Key>(size());
            else                    copy = new MaxPQ<Key>(size(), comparator);
            for (int i = 1; i <= N; i++)
                copy.insert(pq[i]);
        }

        public boolean hasNext()  { return !copy.isEmpty();                     }
        public void remove()      { throw new UnsupportedOperationException();  }

        public Key next() {
            if (!hasNext()) throw new NoSuchElementException();
            return copy.delMax();
        }
    }

   /**
     * A test client.
     */
    public static void main(String[] args) {
        MaxPQ<String> pq = new MaxPQ<String>();
        while (!StdIn.isEmpty()) {
            String item = StdIn.readString();
            if (!item.equals("-")) pq.insert(item);
            else if (!pq.isEmpty()) StdOut.print(pq.delMax() + " ");
        }
        StdOut.println("(" + pq.size() + " left on pq)");
    }



}
