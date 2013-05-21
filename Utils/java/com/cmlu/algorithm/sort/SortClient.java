package com.cmlu.algorithm.sort;

import com.cmlu.lang.In;
import com.cmlu.lang.StdIn;
import com.cmlu.lang.StdOut;

public class SortClient {
    
    /**
     * 每次找出最小的，排在前面
     * @param array
     */
    public static void selectSort(Comparable[] array){
	int N = array.length;
	for(int i=0;i<N;i++){
	    int min = i;
	    for(int j=i+1;j<N;j++){
		if(less(array[j], array[min]))
		    min = j;
	    }
	    exch(array, i, min);
	}
    }
    
    /**
     * 就像摸牌一样，手上的牌是排好序的
     * @param a
     */
    public static void insertionSort(Comparable[] a){
	int N = a.length;
	for(int i=1;i<N;i++){
	    for(int j=i;j>0 && less(a[j], a[j-1]);j--){
		exch(a, j, j-1);
	    }
	}
    }
    
    /**
     * 实际上，底层利用的是插入排序，这样做是为了提高排序的效率
     * 但具体的提高取决于排序前数组已经排好序的程序。
     * 这是因为插入排序是通过交换两个相邻位置的元素来实现的。
     * @param a
     */
    public static void shellSort(Comparable[] a){
	int N = a.length;
	//h是数组的增长序列间隔
	int h = 1;
	while(h < N/3) h = 3*h+1;
	while (h >= 1) {
	    for(int i= h;i<N;i++){
		for(int j=i;j>=h && less(a[j],a[j-h]);j-=h){
		    exch(a, j, j-h);
		}
	    }
	    h /=3 ;
	}
    }
    
    
    
    private static boolean less(Comparable v,Comparable w){
	return v.compareTo(w) < 0;
    }
    
    private static void exch(Comparable[] a,int i,int j){
	Comparable t = a[i];
	a[i] = a[j];
	a[j] = t;
    }
    
    private static void show(Comparable[] a){
	for(int i=0;i<a.length;i++){
	    StdOut.print(a[i]+" ");
	}
	StdOut.println();
    }
    
    public static boolean isSorted(Comparable[] a){
	for(int i=1;i<a.length;i++){
	    if(less(a[i], a[i-1])) return false;
	}
	
	return true;
    }
    
    public static void main(String[] args){
	String[] a = StdIn.readStrings();
	selectSort(a);
	assert isSorted(a);
	show(a);
    }
    
    
    
}
