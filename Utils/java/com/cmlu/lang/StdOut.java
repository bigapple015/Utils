package com.cmlu.lang;

import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.util.Locale;

/**
 * 标准输出
 * @author Administrator
 *
 */
public class StdOut {
	/**
	 * 设置文件的编码方式
	 */
	private static final String UTF8 = "UTF-8";
	
	/**
	 * 设置特定的地理、政治和文化区域
	 */
	private static final Locale US_LOCALE = new Locale("en","US");
	
	/**
	 * 输出
	 */
	private static PrintWriter out;
	
	/**
	 * 静态初始化
	 */
	static{
		try{
			out = new PrintWriter(new OutputStreamWriter(System.out,UTF8),true);
		}
		catch(Exception ex){
			System.out.println(ex);
		}
	}
	
	/**
	 * 单件实例
	 */
	private StdOut(){}
	
	/**
	 * 关闭输出流
	 */
	public static void close(){
		if(out != null){
			out.close();
		}
	}
	
	/**
	 * 打印空行
	 */
	public static void println(){
		out.println();
	}
	
	/**
	 * 打印一个对象
	 * @param x
	 */
	public static void println(Object obj){
		out.println(obj);
	}
	
	/**
	 * 输出字符串
	 * @param s
	 */
	public static void println(String s){
		out.println(s);
	}
	
	/**
	 * 打印boolean值
	 * @param b
	 */
	public static void println(boolean b){
		out.println(b);
	}
	
	/**
	 * 把字符打印到控制台
	 * @param c
	 */
	public static void println(char c){
		out.println(c);
	}
	
	/**
	 * 打印浮点数
	 * @param d
	 */
	public static void println(double d){
		out.println(d);
	}
	
	/**
	 * 打印浮点值
	 * @param x
	 */
	public static void println(float f){
		out.println(f);
	}
	
	/**
	 * 打印整数值
	 * @param i
	 */
	public static void println(int i){
		out.println(i);
	}
	
	/**
	 * 打印长整数值
	 * @param l
	 */
	public static void println(long l){
		out.println(l);
	}
	
	/**
	 * 打印短整型
	 * @param s
	 */
	public static void println(short s){
		out.println(s);
	}
	
	/**
	 * 打印字节
	 * @param b
	 */
	public static  void println(byte b){
		out.println(b);
	}
	
	/**
	 * 打印输出流
	 */
	public static void print(){
		out.flush();
	}
	
	/**
	 * 打印对象
	 * @param obj
	 */
	public static void print(Object obj){
		out.print(obj);
		out.flush();
	}
	
	/**
	 * 打印字符串
	 * @param s
	 */
	public static void print(String s){
		out.print(s);
		out.flush();
	}
	
	/**
	 * 打印boolean值，
	 * @param b
	 */
	public static void print(boolean b){
		out.print(b);
		out.flush();
	}
	
	/**
	 * 打印字符
	 * @param ch
	 */
	public static void print(char ch){
		out.print(ch);
		out.flush();
	}
	
	/**
	 * 打印浮点数
	 * @param d
	 */
	public static void print(double d){
		out.print(d);
		out.flush();
	}
	
	/**
	 * 打印浮点数
	 * @param f
	 */
	public static void print(float f){
		out.print(f);
		out.flush();
	}
	
	/**
	 * 打印整数
	 * @param i
	 */
	public static void print(int i){
		out.print(i);
		out.flush();
	}
	
	/**
	 * 打印长整型
	 * @param l
	 */
	public static void print(long l){
		out.print(l);
		out.flush();
	}
	
	/**
	 * 打印短整型
	 * @param x
	 */
	public static void print(short x){
		out.print(x);
		out.flush();
	}
	
	/**
	 * 打印字节
	 * @param b
	 */
	public static void print(byte b){
		out.print(b);
		out.flush();
	}
	
	/**
	 * 格式化字符串
	 */
	public static void printf(String format,Object... args){
		out.printf(US_LOCALE, format,args);
		out.flush();
	}
	
	/**
	 * 指定地域信息格式化字符串
	 */
	public static void printf(Locale locale,String format,Object... args){
		out.printf(locale, format ,args);
		out.flush();
	}
	
	/**
	 * 单元测试
	 * @param args
	 */
	public static void main(String[] args){
		StdOut.println("test");
		StdOut.println(18);
		StdOut.println(true);
		StdOut.printf("%.6f\n", 1.0/7.0);
	}
	
}
