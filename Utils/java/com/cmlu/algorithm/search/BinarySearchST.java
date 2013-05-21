package com.cmlu.algorithm.search;

import java.awt.Checkbox;

import com.cmlu.commons.Queue;
import com.cmlu.lang.StdIn;
import com.cmlu.lang.StdOut;

public class BinarySearchST<Key extends Comparable<Key>, Value> {
    
    private static final int INIT_CAPACITY = 2;
    
    private Key[] keys;
    
    private Value[] vals;
    /**
     * 符号表中的元素的个数
     */
    private int N =0;
    
    
    /**
     * 构造函数
     */
    public BinarySearchST(){
	this(INIT_CAPACITY);
    }
    
    
    public BinarySearchST(int capacity){
	keys = (Key[])new Comparable[capacity];
	vals = (Value[])new Object[capacity];
    }
    
    
    private void resize(int capatity){
	//容量大于现有的数组个数
	assert capatity >= N;
	Key[] tempk = (Key[])new Comparable[capatity];
	Value[] tempv = (Value[]) new Object[capatity];
	
	for(int i=0;i<N;i++){
	    tempk[i] = keys[i];
	    tempv[i] = vals[i];
	}
	
	vals = tempv;
	keys = tempk;
    }
    
    
    public boolean contains(Key key){
	return get(key) != null;
    }
    
    public int size(){
	return N;
    }
    
    public boolean isEmpty(){
	return N == 0;
    }
    
    /**
     * 根据键获取值，如果没有对应的键，则返回null
     */
    public Value get(Key key){
	if(isEmpty()) return null;
	
	int i = rank(key);
	
	if(i<N && keys[i].compareTo(key) == 0){
	    return vals[i];
	}
	
	
	return null;
    }
    
    /**
     * 返回比给定键小的元素的个数(不包括等于)
     * @param key
     * @return
     */
    public int rank(Key key){
	int lo = 0,hi = N-1;
	while(lo<=hi){
	    int m = (hi-lo)/2+lo;
	    //二分查找
	    int cmp = key.compareTo(keys[m]);
	    if(cmp < 0) hi = m-1;
	    else if(cmp >0) lo = m+1;
	    else return m;
	}
	
	return lo;
    }
    
    /**
     * 插入到符号表一个值，如果已经有了该键，则更新值，如果值为null，则删除键
     * @param key
     * @param val
     */
    public void put(Key key,Value val){
	//值为null，则删除
	if(val == null){
	    delete(key);
	    return;
	}
	
	//已经存在，则更新值
	int i = rank(key);
	if(i<N && keys[i].compareTo(key) == 0){
	    vals[i] = val;
	    return;
	}
	
	//插入新的键值对
	if(N == keys.length){
	    resize(2*keys.length);
	}
	
	//保存符号表顺序
	for(int j=N;j>i;j--){
	    keys[i] = keys[i-1];
	    vals[i] = vals[i-1];
	}
	
	keys[i] = key;
	vals[i] = val;
	N++;
	assert check();
    }
    
    /**
     * 删除键
     * @param key
     */
    public void delete(Key key){
	if(isEmpty()) return;
	int i = rank(key);
	
	if(i==N || keys[i].compareTo(key)!=0){
	    return;
	}
	
	for(int j=i;j<N-1;j++){
	    keys[j] = keys[j+1];
	    vals[j] = vals[j+1];
	}
	
	N--;
	//帮助内存回收
	keys[N] = null;
	vals[N] = null;
	
	if(N>4&& N==keys.length/4){
	    resize(keys.length/2);
	}
	
	assert check();
	
    }
    
    
 // delete the minimum key and its associated value
    public void deleteMin() {
        if (isEmpty()) throw new RuntimeException("Symbol table underflow error");
        delete(min());
    }

    // delete the maximum key and its associated value
    public void deleteMax() {
        if (isEmpty()) throw new RuntimeException("Symbol table underflow error");
        delete(max());
    }


   /*****************************************************************************
    *  Ordered symbol table methods
    *****************************************************************************/
    public Key min() {
        if (isEmpty()) return null;
        return keys[0]; 
    }

    public Key max() {
        if (isEmpty()) return null;
        return keys[N-1];
    }
    
    /**
     * 获取第i大的元素，从0计数
     * @param k
     * @return
     */
    public Key select(int k){
	if(k<0||k>=N) return null;
	return keys[k];
    }
    
    
    public Key floor(Key key){
	int i= rank(key);
	if(i<N&&key.compareTo(keys[i]) ==0){
	    return keys[i];
	}
	
	if(i==0) return null;
	else return keys[i-1];
	
    }
    
    
    /**
     * 天花板函数
     */
    public Key ceiling(Key key){
	int i = rank(key);
	if(i== N) return null;
	else return keys[i];
    }
    
    /**
     * 包含在[lo, hi]之间的所有元素数目
     * @param lo
     * @param hi
     * @return
     */
    public int size(Key lo,Key hi){
	if(lo.compareTo(hi)>0) return 0;
	if(contains(hi)) return rank(hi)-rank(lo)+1;
	else return rank(hi) - rank(lo);
    }
    
    /**
     * 提供迭代方案
     * @return
     */
    public Iterable<Key> keys(){
	return keys(min(),max());
    }
    
    /**
     * 提供一个[lo,hi]之间的迭代
     * @param lo
     * @param hi
     * @return
     */
    public Iterable<Key> keys(Key lo,Key hi){
	Queue<Key> queue = new Queue<Key>();
	if(lo == null && hi == null) return queue;
	if (lo == null) throw new RuntimeException("lo is null in keys()");
        if (hi == null) throw new RuntimeException("hi is null in keys()");
        if (lo.compareTo(hi) > 0) return queue;
        //注rank[hi]最好提取出循环
        for(int i=rank(lo);i<rank(hi);i++){
            queue.enqueue(keys[i]);
        }
        
        if(contains(hi)) queue.enqueue(keys[rank(hi)]);
        
        return queue;
    }
    
    
    private boolean check() {
        return isSorted() && rankCheck();
    }


    
 // are the items in the array in ascending order?
    private boolean isSorted() {
        for (int i = 1; i < size(); i++)
            if (keys[i].compareTo(keys[i-1]) < 0) return false;
        return true;
    }

    // check that rank(select(i)) = i
    private boolean rankCheck() {
        for (int i = 0; i < size(); i++)
            if (i != rank(select(i))) return false;
        for (int i = 0; i < size(); i++)
            if (keys[i].compareTo(select(rank(keys[i]))) != 0) return false;
        return true;
    }


   /*****************************************************************************
    *  Test client
    *****************************************************************************/
    public static void main(String[] args) { 
        BinarySearchST<String, Integer> st = new BinarySearchST<String, Integer>();
        for (int i = 0; !StdIn.isEmpty(); i++) {
            String key = StdIn.readString();
            st.put(key, i);
        }
        for (String s : st.keys())
            StdOut.println(s + " " + st.get(s));
    }
    
    
    
    
}
