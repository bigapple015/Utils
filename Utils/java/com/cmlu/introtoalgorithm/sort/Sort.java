package com.cmlu.introtoalgorithm.sort;

import com.cmlu.lang.StdOut;

public class Sort {
	/**
	 * 插入排序 非降序
	 * 
	 * @param array
	 */
	public static void InsertionSort(int[] array) {
		if (array == null || array.length <= 1) {
			return;
		}
		// 具体规则就像摸牌，手中的牌是排好序的
		// 不再创建新的数组
		int temp;
		int length = array.length;
		for (int i = 1; i < length; i++) {
			temp = array[i];
			// 第二个循环
			int j;
			for (j = i - 1; j > -1 && array[j] > temp; j--) {
				// 大牌向后移
				array[j + 1] = array[j];
			}
			array[j + 1] = temp;
		}
	}

	/**
	 * 选择排序（具体策略每次找出最小值，将其排到前面）
	 * 
	 * @param array
	 */
	public static void SelectionSort(int[] array) {
		// 不作处理
		if (array == null || array.length <= 1) {
			return;
		}

		for (int i = 0; i < array.length; i++) {
			// 从第二个元素开始
			int min = array[i];
			// 最小项所在的索引
			int minIndex = i;
			for (int j = i + 1; j < array.length; j++) {
				if (min > array[j]) {
					min = array[j];
					minIndex = j;
				}
			}

			// 交换i和j索引位置上的数据
			int temp = array[i];
			array[i] = array[minIndex];
			array[minIndex] = temp;
		}
	}

	/**
	 * 冒泡排序  每次交换两个反序的位置
	 * @param array
	 */
	public static void BubbleSort(int[]array){
		if(array == null || array.length <=1){
			return;
		}
		
		int temp;
		for(int i=0;i<array.length;i++){
			for(int j=array.length-1;j>i;j--){
				if(array[j]<array[j-1]){
					//交换两个反序的位置，小的提前
					temp = array[j];
					array[j] = array[j-1];
					array[j-1] = temp;
				}
			}
		}
	}
	
	
	// 合并排序************************************************************************************
	/**
	 * 合并排序，采用分治法
	 * 
	 * @param
	 */
	public static void MergeSort(int[] array) {
		if (array == null || array.length <= 1) {
			return;
		}

		MergeSort(array, 0, array.length - 1);
	}

	/**
	 * 合并排序 传入的数组不能是null，但长度可以是0
	 * 
	 * @param array
	 *            待排序数组
	 * @param beginIndex
	 *            数组开始索引 0
	 * @param endIndex
	 *            数组结束索引 即array.Length -1
	 */
	public static void MergeSort(int[] array, int beginIndex, int endIndex) {
		if (beginIndex < endIndex) {
			int mid = (beginIndex + endIndex) / 2;
			// 分治法
			MergeSort(array, beginIndex, mid);
			MergeSort(array, mid + 1, endIndex);
			// 合并两个已经排好序的数组，这两个数组至少长度为1的
			Merge(array, beginIndex, mid, endIndex);
		}
	}

	/**
	 * 合并两个已经排好序的数组，这两个数组至少长度为1的
	 * 
	 * @param array
	 * @param beginIndex
	 * @param mid
	 * @param endIndex
	 */
	public static void Merge(int[] array, int beginIndex, int mid, int endIndex) {
		int[] arrayLeft = new int[mid - beginIndex + 1];
		int[] arrayRight = new int[endIndex - mid];
		// 左边的数组备份
		for (int i = 0; i < arrayLeft.length; i++) {
			arrayLeft[i] = array[beginIndex + i];
		}
		// 右边的数组
		for (int i = 0; i < arrayRight.length; i++) {
			arrayRight[i] = array[mid + 1 + i];
		}
		// 左边的数组，右边的数组
		int left = 0;
		int right = 0;
		for (int i = beginIndex; i <= endIndex; i++) {

			// 左边的数组已经结束
			if (left >= arrayLeft.length && right <= arrayRight.length - 1) {
				array[i] = arrayRight[right];
				right++;
				continue;
			}

			// 右边的数组已经结束
			if (right >= arrayRight.length && left <= arrayLeft.length - 1) {
				array[i] = arrayLeft[left];
				left++;
				continue;
			}

			if (arrayLeft[left] < arrayRight[right]) {
				array[i] = arrayLeft[left];
				left++;
			} else {
				array[i] = arrayRight[right];
				right++;
			}
		}
	}

	// 合并排序************************************************************************************

	/**
	 * test client
	 * 
	 * @param args
	 */
	public static void main(String[] args) {
		int[] array = { 1, 2, 3, 4, 5, 6, -10, -11, 34, -10 ,123,-89};

		BubbleSort(array);
		// SelectionSort(array);
		for (int i : array) {
			StdOut.print(i + " ");
		}
		
		StdOut.println();
		StdOut.println(Search.BinarySearch(array, 34));
	}

}
