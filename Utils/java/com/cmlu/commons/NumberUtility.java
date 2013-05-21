package com.cmlu.commons;

import org.omg.PortableServer.ID_ASSIGNMENT_POLICY_ID;

/**
 * 处理数字操作
 * @author Administrator
 */
public class NumberUtility {
	
	/**
	 * 计算两个非负整数的最大公约数，如果a=0,则返回b， 否则gcd(a,b) = gcd(b,a mod b)
	 * @param a
	 * @param b
	 * @return
	 * @throws Exception 
	 */
	public static int gcd(int a,int b) throws Exception{
		if(a < 0 || b < 0){
			throw new Exception("input parameter must be not less than 0");
		}
		
		if(a == 0){
			return b;
		}
		
		if(b == 0){
			return a;
		}
		
		return gcd(b, a % b);
	}
	
	/**
	 * 二分查找
	 * @param key 要查找的键
	 * @param a 已经排好序的数组， 非降序
	 * @return
	 * @throws Exception 
	 */
	public static int binarySearch(int key,int[] a) throws Exception{
		
		//这个抛出异常
		if(a == null || a.length == 0){
			throw new Exception("a can not be null or zero length");
		}
		
		int lo = 0;
		int hi = a.length - 1;
		
		while(lo <= hi){
			int mid = lo + (hi-lo)/2;
			if(key < a[mid]){
				hi = mid -1;
			}
			else if(key > a[mid]){
				lo = mid +1;
			}
			else {
				return mid;
			}
		}
		return -1;
	}
	
	/**
	 * 二分查找 递归实现
	 * @param key
	 * @param a
	 * @param lo
	 * @param hi
	 * @return
	 */
	public static int binarySearch(int key,int[] a,int lo,int hi){
		if(lo > hi) return -1;
		int mid = lo + (hi-lo)/2;
		if(key < a[mid]) return binarySearch(key, a,lo,mid-1);
		else if(key > a[mid]) return binarySearch(key, a,mid+1,hi);
		else return mid;
	}
	
	/**
	 * 判断是否是质数
	 * @param n
	 * @return
	 */
	public static boolean isPrime(int n){
		if(n<2)return false;
		for(int i=2;i*i<=n;i++){
			if(n%i == 0){
				return false;
			}
		}
		return true;
	}

	/**
	 * 获取平均值
	 * @param array
	 * @return
	 * @throws Exception 
	 */
	public static double average(int[] array) throws Exception{
		if(array == null || array.length ==0){
			throw new Exception("input array must have some datas");
		}
		
		double sum = 0.0d;
		for(int i=0;i<array.length;i++){
			sum += array[0];
		}
		
		return sum/array.length;
	}

	/**
	 * 获取数组的最小值
	 * @param array
	 * @return
	 * @throws Exception 
	 */
	public static int min(int[] array) throws Exception{
		if(array == null || array.length == 0){
			throw new Exception("input array must have some datas");
		}
		
		int min = array[0];
		for(int i=1;i<array.length;i++){
			if(min > array[i]){
				min = array[i];
			}
		}
		
		return min;
	}
	
	/**
	 * 获取数组的最大值
	 * @param array
	 * @return
	 * @throws Exception
	 */
	public static int max(int[] array) throws Exception{
		if(array==null || array.length ==0){
			throw new Exception("input array must have some datas");
		}
		
		int max = array[0];
		for(int i=1;i<array.length;i++){
			if(max<array[i]){
				max = array[i];
			}
		}
		return max;
	}
	
	/**
	 * 求数组的和
	 * @param array
	 * @return
	 * @throws Exception
	 */
	public static long sum(int[] array) throws Exception{
		if(array == null || array.length == 0){
			throw new Exception("array cannot be empty");
		}
		
		long sum = 0;
		for(int i:array){
			sum += i;
		}
		return sum;
	}

	
}