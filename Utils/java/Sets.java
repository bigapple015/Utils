package com.lcm;

import java.util.*;

public class Sets {
	public static <T> Set<T> union(Set<T> a,Set<T> b){
		Set<T> result = new HashSet<T>(a);
		result.addAll(b);
		return result;
	}
	
	public static <T> Set<T> intersection(Set<T> a,Set<T> b){
		Set<T> result = new HashSet<T>(a);
		result.retainAll(b);
		return result;
	}
	/**
	 * superset-subset
	 * @param superset
	 * @param subset
	 * @return
	 */
	public static <T> Set<T> difference(Set<T> superset,Set<T> subset){
		Set<T> result = new HashSet<T>(superset);
		result.removeAll(subset);
		return result;
	}
	
	/**
	 * a²¢b - a¡Éb
	 * @param a
	 * @param b
	 * @return
	 */
	public static <T> Set<T> complemet(Set<T> a,Set<T>b){
		return difference(union(a,b),intersection(a,b));
	}
	
}
