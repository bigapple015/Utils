package com.cmlu.algorithm.sort;

/**
 * 合并两个已经排好序的子数组
 * @author Administrator
 *
 */
public class Merge {
    /**
     * 合并两个排序的子数组
     * @param a
     * @param aux
     * @param lo
     * @param mid
     * @param hi
     */
    public static void merge(Comparable[] a,Comparable[] aux,int lo,int mid,int hi){
	assert isSorted(a,lo,mid);
	assert isSorted(a,mid+1,hi);
	//备份一份数据，这意味着合并排序需要额外的排序空间
	for(int k=lo;k<=hi;k++){
	    aux[k] = a[k];
	}
	//将数据复制为a[]
	int i = lo,j = mid+1;
	for(int k=lo;k<=hi;k++){
	    if(i>mid) a[k] = aux[j++];//左边的数组已经全处理完了
	    else if(j>hi) a[k] = aux[i++];//右边的数组已经全处理
	    else if(less(aux[j],aux[i])) a[k] = aux[j++];
	    else a[k] = aux[i++];
	}
	
	//验证数据排好序了
	assert isSorted(a,lo,hi);
    }
    
    /**
     * 合并排序
     * @param a
     * @param aux
     * @param lo
     * @param hi
     */
    private static void mergeSort(Comparable[] a,Comparable[] aux,int lo,int hi){
	if(hi <= lo) return;
	int mid = lo + (hi - lo)/2;
	mergeSort(a, aux, lo, mid);
	mergeSort(a, aux, mid+1, hi);
	merge(a, aux, lo, mid, hi);
    }
    
    /**
     * 合并排序
     * @param a
     */
    public static void mergeSort(Comparable[] a){
	//合并排序实际上需要长度为N的额外空间
	Comparable[] aux = new Comparable[a.length];
	mergeSort(a, aux, 0, a.length-1);
	assert isSorted(a);
    }
    
 // is v < w ?
    private static boolean less(Comparable v, Comparable w) {
        return (v.compareTo(w) < 0);
    }
        
    // exchange a[i] and a[j]
    private static void exch(Object[] a, int i, int j) {
        Object swap = a[i];
        a[i] = a[j];
        a[j] = swap;
    }


   /***********************************************************************
    *  Check if array is sorted - useful for debugging
    ***********************************************************************/
    private static boolean isSorted(Comparable[] a) {
        return isSorted(a, 0, a.length - 1);
    }

    private static boolean isSorted(Comparable[] a, int lo, int hi) {
        for (int i = lo + 1; i <= hi; i++)
            if (less(a[i], a[i-1])) return false;
        return true;
    }

    
}
