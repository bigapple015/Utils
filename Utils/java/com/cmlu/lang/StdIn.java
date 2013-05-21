package com.cmlu.lang;

import java.io.BufferedInputStream;
import java.util.Locale;
import java.util.Scanner;

/**
 * 从标准流中读取字符串和数据
 * @author Administrator
 *
 */
public final class StdIn {

	/**
	 * 指定编码方式
	 */
	private static String charsetName="UTF-8";
	
	/**
	 * 设置特定的地理、政治和文化地区
	 */
	private static Locale usLocale = new Locale("en","US");
	
	/**
	 * 数据扫描器对象
	 */
	private static Scanner scanner = new Scanner(new BufferedInputStream(System.in),charsetName);
	
	/**
	 * 静态初始化
	 */
	static{
		scanner.useLocale(usLocale);
	}
	
	/**
	 * 私有构造函数      单例
	 */
	private StdIn(){
	}
	
	/**
	 * 判断输入流中是否只有空白符
	 * @return
	 */
	public static boolean isEmpty(){
		return !scanner.hasNext();
	}
	
	/**
	 * 从标准输入流中读取下一行
	 * @return
	 */
	public static String readString(){
		return scanner.next();
	}
	
	/**
	 * 从标准输入流中读取下一个整数
	 * @return
	 */
	public static int readInt(){
		return scanner.nextInt();
	}
	
	/**
	 * 从标准输入中返回下一个double
	 * @return
	 */
	public static double readDouble(){
		return scanner.nextDouble();
	}
	
	/**
	 * 从标准输入中读取下一个浮点数
	 * @return
	 */
	public static float readFloat(){
		return scanner.nextFloat();
	}
	
	/**
	 * 从标准输入流中读取下一个短整型
	 * @return
	 */
	public static short readShort(){
		return scanner.nextShort();
	}
	
	/**
	 * 从标准输入中读取长整数
	 * @return
	 */
	public static long readLong(){
		return scanner.nextLong();
	}
	
	/**
	 * 从标准输入中读取下一个字节
	 * @return
	 */
	public static byte readByte(){
		return scanner.nextByte();
	}
	
	/**
	 * 从标准流中读取数据，并将其转换为boolean类型，true或1转换为true，false或0转换为false
	 * @return
	 */
	public static boolean readBoolean(){
		String s = readString();
		
		if(s.equalsIgnoreCase("true") || s.equalsIgnoreCase("1")){
			return true;
		}
		if(s.equalsIgnoreCase("false") || s.equalsIgnoreCase("0")){
			return false;
		}
		throw new java.util.InputMismatchException();
	}
	
	/**
	 * 判断标准流中是否有下一行
	 * @return
	 */
	public static boolean hasNextLine(){
		return scanner.hasNextLine();
	}
	
	/**
	 * 从输入流中读取下一行
	 * @return
	 */
	public static String readLine(){
		return scanner.nextLine();
	}
	
	/**
	 * 从输入流中读取下一个字符
	 * @return
	 */
	public static char readChar(){
		String s = scanner.findWithinHorizon("(?s).", 1);
		return s.charAt(0);
	}
	
	/**
	 * 从标准输入中获取其他数据
	 * @return
	 */
	public static String readAll(){
		if(!scanner.hasNextLine()){
			return null;
		}
		
		return scanner.useDelimiter("\\A").next();
	}
	
	/**
	 * 从输入流中读取int数组
	 * @return
	 */
	public static int[] readInts(){
		String[] fields = readAll().trim().split("\\s+");
		int[] vals = new int[fields.length];
		for(int i=0;i<fields.length;i++){
			vals[i] = Integer.parseInt(fields[i]);
		}
		return vals;
	}
	
	public static double[] readDoubles(){
		String[] fields = readAll().trim().split("\\s+");
		double[] vals = new double[fields.length];
		for(int i=0;i<fields.length;i++){
			vals[i] = Double.parseDouble(fields[i]);
		}
		return vals;
	}
	
	/**
	 * 从输入流中读取所有的字符串
	 * @return
	 */
	public static String[] readStrings(){
		String[] fields =readAll().trim().split("\\s+");
		return fields;
	}
	
	/**
	 * 单元测试
	 * @param args
	 */
	public static void main(String[] args){
		System.out.println("Type a string:");
		String string = StdIn.readString();
		System.out.println("Your string was: "+string);
		System.out.println();
		
		System.out.println("Type an int: ");
		int a = StdIn.readInt();
		System.out.println("Your int was: "+a);
		System.out.println();
		
		System.out.println("Type a boolean:");
		boolean b = StdIn.readBoolean();
		System.out.println("Your boolean was: "+ b);
		System.out.println();
		
		System.out.println("Type a double: ");
		double d = StdIn.readDouble();
		System.out.println("your double was: "+d);
		System.out.println();
	}
	
}
