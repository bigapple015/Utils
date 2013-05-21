package com.cmlu.lang;

/**
 * 数据计算
 * @author Administrator
 *
 */
public final class StdStats {
	//私有函数
	private StdStats(){}
	
	/**
	 * 返回数组的最大值，-infinity如果没有该值
	 * @param a
	 * @return
	 */
	public static double max(double[] a){
		double max = Double.NEGATIVE_INFINITY;
		for(int i=0;i<a.length;i++){
			if(a[i] > max) max = a[i];
		}
		return max;
	}
	
	/**
	 * return max in subarray a[lo,hi]
	 * @param a
	 * @param lo
	 * @param hi
	 * @return
	 */
	public static double max(double[]a,int lo,int hi){
		if(lo < 0 || hi>=a.length || lo>hi){
			throw new RuntimeException("subarray out of dounds");
		}
		
		double max = Double.NEGATIVE_INFINITY;
		for(int i=lo;i<=hi;i++){
			if(a[i]>max) max = a[i];
		}
		return max;
	}
	
	/**
	 * return Integer.MIN_VALUE if no such value
	 */
	public static int max(int[] a){
		int max = Integer.MIN_VALUE;
		for(int i=0;i<a.length;i++){
			if(a[i]>max) max = a[i];
		}
		return max;
	}
	
	/**
	 * +infinity if no such value
	 * @param a
	 * @return
	 */
	public static double min(double[] a){
		double min = Double.POSITIVE_INFINITY;
		for(int i=0;i<a.length;i++){
			if(a[i]<min) min = a[i];
		}
		return min;
	}
	
	/**
	 * 返回 min value in subarray a[lo,hi]  +infinity 如果没有该值
	 */
	public static double min(double[] a,int lo,int hi){
		if(lo<0|| hi>=a.length|lo>hi){
			throw new RuntimeException("subarray out if bounds");
		}
		
		double min = Double.POSITIVE_INFINITY;
		for(int i=lo;i<=hi;i++){
			if(a[i] < min) min = a[i];
		}
		return min;
	}
	
	/*
	 * Integer.MAX_VALUE if no such value
	 */
	public static int min(int[] a){
		int min = Integer.MAX_VALUE;
		for(int i=0;i<a.length;i++){
			if(a[i]<min) min = a[i];
		}
		return min;
	}
	
	/**
	 * 求平均值 ，NAN if no such value
	 */
	public static double mean(double[] a){
		if(a == null || a.length == 0){
			return Double.NaN;
		}
		double sum = sum(a);
		return sum/a.length;
	}
	
	/**
	 * NAN if no such value
	 */
	public static double mean(double[]a,int lo,int hi){
		if(lo < 0 || hi>a.length||lo>hi){
			throw new RuntimeException("subarray out of bounds");
		}
		double sum = sum(a,lo,hi);
		return sum/(hi - lo );
	}
	
	/**
	 * NAN if no such value
	 * @param a
	 * @return
	 */
	public static double mean(int[] a){
		if(a==null || a.length == 0){
			return Double.NaN;
		}
		double sum = 0.0;
		for(int i=0;i<a.length;i++){
			sum += a[i];
		}
		return sum/a.length;
	}
	
	/**
     * Return sample variance of array, NaN if no such value.
     */
    public static double var(double[] a) {
        if (a.length == 0) return Double.NaN;
        double avg = mean(a);
        double sum = 0.0;
        for (int i = 0; i < a.length; i++) {
            sum += (a[i] - avg) * (a[i] - avg);
        }
        return sum / (a.length - 1);
    }

   /**
     * Return sample variance of subarray a[lo..hi], NaN if no such value.
     */
    public static double var(double[] a, int lo, int hi) {
        int length = hi - lo + 1;
        if (lo < 0 || hi >= a.length || lo > hi)
            throw new RuntimeException("Subarray indices out of bounds");
        if (length == 0) return Double.NaN;
        double avg = mean(a, lo, hi);
        double sum = 0.0;
        for (int i = lo; i <= hi; i++) {
            sum += (a[i] - avg) * (a[i] - avg);
        }
        return sum / (length - 1);
    }

   /**
     * Return sample variance of array, NaN if no such value.
     */
    public static double var(int[] a) {
        if (a.length == 0) return Double.NaN;
        double avg = mean(a);
        double sum = 0.0;
        for (int i = 0; i < a.length; i++) {
            sum += (a[i] - avg) * (a[i] - avg);
        }
        return sum / (a.length - 1);
    }

   /**
     * Return population variance of array, NaN if no such value.
     */
    public static double varp(double[] a) {
        if (a.length == 0) return Double.NaN;
        double avg = mean(a);
        double sum = 0.0;
        for (int i = 0; i < a.length; i++) {
            sum += (a[i] - avg) * (a[i] - avg);
        }
        return sum / a.length;
    }

   /**
     * Return population variance of subarray a[lo..hi],  NaN if no such value.
     */
    public static double varp(double[] a, int lo, int hi) {
        int length = hi - lo + 1;
        if (lo < 0 || hi >= a.length || lo > hi)
            throw new RuntimeException("Subarray indices out of bounds");
        if (length == 0) return Double.NaN;
        double avg = mean(a, lo, hi);
        double sum = 0.0;
        for (int i = lo; i <= hi; i++) {
            sum += (a[i] - avg) * (a[i] - avg);
        }
        return sum / length;
    }


   /**
     * Return sample standard deviation of array, NaN if no such value.
     */
    public static double stddev(double[] a) {
        return Math.sqrt(var(a));
    }

   /**
     * Return sample standard deviation of subarray a[lo..hi], NaN if no such value.
     */
    public static double stddev(double[] a, int lo, int hi) {
        return Math.sqrt(var(a, lo, hi));
    }

   /**
     * Return sample standard deviation of array, NaN if no such value.
     */
    public static double stddev(int[] a) {
        return Math.sqrt(var(a));
    }

   /**
     * Return population standard deviation of array, NaN if no such value.
     */
    public static double stddevp(double[] a) {
        return Math.sqrt(varp(a));
    }

   /**
     * Return population standard deviation of subarray a[lo..hi], NaN if no such value.
     */
    public static double stddevp(double[] a, int lo, int hi) {
        return Math.sqrt(varp(a, lo, hi));
    }

   /**
     * Return sum of all values in array.
     */
    public static double sum(double[] a) {
        double sum = 0.0;
        for (int i = 0; i < a.length; i++) {
            sum += a[i];
        }
        return sum;
    }

   /**
     * Return sum of all values in subarray a[lo..hi].
     */
    public static double sum(double[] a, int lo, int hi) {
        if (lo < 0 || hi >= a.length || lo > hi)
            throw new RuntimeException("Subarray indices out of bounds");
        double sum = 0.0;
        for (int i = lo; i <= hi; i++) {
            sum += a[i];
        }
        return sum;
    }

   /**
     * Return sum of all values in array.
     */
    public static int sum(int[] a) {
        int sum = 0;
        for (int i = 0; i < a.length; i++) {
            sum += a[i];
        }
        return sum;
    }

   /**
     * Plot points (i, a[i]) to standard draw.
     */
    public static void plotPoints(double[] a) {
        int N = a.length;
        StdDraw.setXscale(0, N-1);
        StdDraw.setPenRadius(1.0 / (3.0 * N));
        for (int i = 0; i < N; i++) {
            StdDraw.point(i, a[i]);
        }
    }

   /**
     * Plot line segments connecting points (i, a[i]) to standard draw.
     */
    public static void plotLines(double[] a) {
        int N = a.length;
        StdDraw.setXscale(0, N-1);
        StdDraw.setPenRadius();
        for (int i = 1; i < N; i++) {
            StdDraw.line(i-1, a[i-1], i, a[i]);
        }
    }

   /**
     * Plot bars from (0, a[i]) to (i, a[i]) to standard draw.
     */
    public static void plotBars(double[] a) {
        int N = a.length;
        StdDraw.setXscale(0, N-1);
        for (int i = 0; i < N; i++) {
            StdDraw.filledRectangle(i, a[i]/2, .25, a[i]/2);
        }
    }	
	
	
}
