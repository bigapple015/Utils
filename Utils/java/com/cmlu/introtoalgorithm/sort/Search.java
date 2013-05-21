package com.cmlu.introtoalgorithm.sort;

public class Search {

	//二分查找
	/**
	 * 数组中查找某个指定的元素，这个数组必须是排好序的
	 * @param array
	 * @param key
	 * @return  key所在的索引位置  -1如果找不到
	 */
	public static int BinarySearch(int[] array,int key){
		if(array == null || array.length==0){
			return -1;
		}
		
		return BinarySearch(array, 0,array.length-1 ,key);
	}
	
	/**
	 * 二分查找
	 * @param array
	 * @param beginIndex 数组索引开始位置
	 * @param endIndex  查找索引结束位置
	 * @param key
	 * @return
	 */
	public static int BinarySearch(int[] array,int beginIndex, int endIndex,int key){
		if(array == null || array.length==0 || endIndex <beginIndex){
			return -1;
		}
		int mid = (beginIndex+endIndex) /2;
		if(array[mid] == key){
			return mid;
		}
		if(array[mid] < key){
			return BinarySearch(array, mid+1,endIndex,key);
		}
		else{
			return BinarySearch(array, beginIndex, mid-1, key);
		}
	}
}
