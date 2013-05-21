package com.cmlu.algorithm.search;

import com.cmlu.lang.StdIn;
import com.cmlu.lang.StdOut;

/**
 * 统计各个键值出现的次数
 * @author Administrator
 *
 */
public class FrequencyCounter {

    
    public static void main(String[] args){
	/*//字典中不同的元素数和总的元素数
	int distinct = 0,words = 0;
	int minlen = Integer.parseInt(args[0]);
	ST<String, Integer> st = new ST<String,Integer>();
	
	//计算出现的频率
	while(!StdIn.isEmpty()){
	    String key = StdIn.readString();
	    //小于最小长度的删掉
	    if(key.length() < minlen) continue;
	    
	    words++;
	    if(st.contains(key)){
		st.put(key,st.get(key)+1);
	    }
	    else{
		st.put(key,1);
		distinct++;
	    }
	}
	
	//找到出现次数最大
	String max = "";
	st.put(max,0);
	for(String word:st.keys()){
	    if(st.get(word)>st.get(max)){
		max = word;
	    }
	}
	
	
	StdOut.println(max + " " + st.get(max));
        StdOut.println("distinct = " + distinct);
        StdOut.println("words  = " + words);*/
    }
    
    
}
