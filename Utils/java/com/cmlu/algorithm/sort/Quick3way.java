package com.cmlu.algorithm.sort;

import java.util.concurrent.Exchanger;

import com.cmlu.lang.StdIn;
import com.cmlu.lang.StdOut;

/**
 * 快速排序的一个变种
 * 它的分割是 小于分割点元素集       等于分割点元素集     大于分割点元素集
 * @author Administrator
 *
 */
public class Quick3way {
    //quicksort the array a[] using 3-way partitioning
    public static void sort(Comparable[] a){
	sort(a,0,a.length-1);
	assert isSorted(a);
    }
    
    /**
     * a[i] less than v: exchange a[lt] with a[i] and increment both lt and i
■ a[i] greater than v: exchange a[i] with a[gt] and decrement gt
■ a[i] equal to v: increment i
     * @param a
     * @param lo
     * @param hi
     */
    private static void sort(Comparable[] a,int lo,int hi){
	if(hi <= lo) return;
	int lt = lo,gt=hi;
	int i = lo;
	Comparable v = a[lo];
	while(i <= gt){
	    int cmp = a[i].compareTo(v);
	    if(cmp < 0) exch(a,lt++,i++);
	    else if(cmp>0) exch(a,i,gt--);
	    else i++;
	}
	
	//递归排序
	sort(a,lo,lt-1);
	sort(a,gt+1,hi);
	assert isSorted(a,lo,hi);
    }
    
    
    private static boolean less(Comparable v,Comparable w){
	return (v.compareTo(w) <0);
    }
    
    public static boolean eq(Comparable v,Comparable w){
	return (v.compareTo(w) == 0);
    }
    
    private static void exch(Object[] a,int i,int j){
	Object swap = a[i];
	a[i] = a[j];
	a[j] = swap;
    }
    
    private static boolean isSorted(Comparable[] a){
	return isSorted(a,0,a.length-1);
    }
    
    private static boolean isSorted(Comparable[] a,int lo,int hi){
	for(int i=lo+1;i<=hi;i++){
	    if(less(a[i], a[i-1])) return false;
	}
	
	return true;
    }
    
    
    private static void show(Comparable[] a){
	for(int i=0;i<a.length;i++){
	    StdOut.println(a[i]);
	}
    }
    
    
    public static void main(String[] args){
	String[] a = StdIn.readStrings();
	Quick3way.sort(a);
	show(a);
    }
    
}
