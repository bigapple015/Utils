package com.cmlu.unimportant;

/**
 * 固定大小的栈
 * @author Administrator
 *
 */
public class FixedCapacityStack<T> {
	private T[] a;
	private int N;
	public FixedCapacityStack(int capaticy){
		a = (T[])new Object[capaticy];
	}
	
	public boolean isEmpty(){return N==0;}
	
	public int size(){return N;}
	
	public void push(T item){
		if(N==a.length) resize(2*a.length);
		a[N++] = item;
	}
	public T pop(){
		if(N>0 && N <=a.length /4) resize(a.length/2);
		return a[--N];
	}
	
	private void resize(int max){
		T[] temp = (T[]) new Object[max];
		for(int i=0;i<N;i++){
			temp[i] = a[i];
		}
		a= temp;
	}
}
