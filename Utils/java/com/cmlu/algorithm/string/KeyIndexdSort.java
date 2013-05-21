package com.cmlu.algorithm.string;

import java.util.Arrays;

/**
 * Key indexed counting uses 8N + 3R + 1 array access to sort N items whose keys are integers between 0 and R-1
 * @author Administrator
 *
 */
public class KeyIndexdSort {

    private Item[] items;
    private int[] count;
    
    public KeyIndexdSort(Item[] items,int R){
	this.items = items;
	//之所以加1是因为count[i+1]来表示key i出现的次数，使count[0] = 0
	count = new int[R+1];
    }
    
    public void sort(){
	//第一步计算每个key出现的频率
	for(int i=0;i<items.length;i++){
	    count[items[i].key+1]++;
	}
	//计算每个key在排序数组的其实位置
	for(int i=0;i<count.length-1;i++){
	    count[i+1] += count[i];
	}
	//将数据放到辅助数组
	Item[] aux = new Item[items.length];
	for(int i=0;i<items.length;i++){
	    aux[count[items[i].key]++] = items[i];
	}
	//复制回原数组
	for(int i=0;i<items.length;i++){
	    items[i] = aux[i];
	}
    }
}

class Item{
    public int key;
    //别的与之联系的实际的值
    public String actualValue;
}