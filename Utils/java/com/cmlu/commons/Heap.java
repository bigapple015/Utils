package com.cmlu.commons;

import java.nio.channels.Pipe.SinkChannel;
import java.util.concurrent.Exchanger;

import com.cmlu.lang.StdIn;
import com.cmlu.lang.StdOut;

/**
 * 堆排序
 * @author Administrator
 *
 */
public class Heap {

    /**
     * 堆排序，每次找到最大的，并删除它，并把它放到已经空出来的位置
     * @param pq
     */
    public static void sort(Comparable[] pq){
	int N = pq.length;
	//g构建堆序
	for(int k=N/2;k>=1;k--){
	    sink(pq,k,N);
	}
	
	//sortdown
	while(N > 1){
	    //将优先队列中的堆头和空出来的位置交换
	    exch(pq,1,N--);
	    //重新构建堆序
	    sink(pq,1,N);
	}
	
    }
    
    /**
     * 重建堆序
     * @param pq
     * @param k
     * @param N
     */
    private static void sink(Comparable[] pq,int k,int N){
	while(2*k<=N){
	    int j = 2*k;
	    //找出两个孩子中最大的
	    if(j<N&& less(pq,j,j+1)) j++;
	    //如果父节点大于等于子节点，结束循环
	    if(!less(pq,k,j)) break;
	    
	    //将小的子节点下浮
	    exch(pq,k,j);
	    k = j;
	}
    }
    
    
    
    /***********************************************************************
     * Helper functions for comparisons and swaps.
     * Indices are "off-by-one" to support 1-based indexing.
     **********************************************************************/
     private static boolean less(Comparable[] pq, int i, int j) {
         return pq[i-1].compareTo(pq[j-1]) < 0;
     }

     private static void exch(Object[] pq, int i, int j) {
         Object swap = pq[i-1];
         pq[i-1] = pq[j-1];
         pq[j-1] = swap;
     }

     // is v < w ?
     private static boolean less(Comparable v, Comparable w) {
         return (v.compareTo(w) < 0);
     }
         

    /***********************************************************************
     *  Check if array is sorted - useful for debugging
     ***********************************************************************/
     private static boolean isSorted(Comparable[] a) {
         for (int i = 1; i < a.length; i++)
             if (less(a[i], a[i-1])) return false;
         return true;
     }


     // print array to standard output
     private static void show(Comparable[] a) {
         for (int i = 0; i < a.length; i++) {
             StdOut.println(a[i]);
         }
     }

     // Read strings from standard input, sort them, and print.
     public static void main(String[] args) {
         String[] a = StdIn.readStrings();
         Heap.sort(a);
         show(a);
     }
    
    
    
    
    
    
}
