package com.cmlu.lang;

import java.io.BufferedInputStream;
import java.io.File;
import java.io.InputStream;
import java.net.Socket;
import java.net.URL;
import java.net.URLConnection;
import java.util.Locale;
import java.util.Scanner;

/**
 * 从stdin,socket、文件或者url中读取数据
 * @author Administrator
 *
 */
public final class In {
	
	/**
	 * 可以从正则表达式来解析基本类型和字符串的简单文本扫描器，分割符默认是空白
	 */
	private Scanner scanner;
	
	/**
	 * 使用的编码方式， 如UTF-8
	 */
	private String charsetName = "ISO-8859-1";
	
	/**
	 * 标志特定的地理、政治和文化区域
	 */
	private Locale usLocale = new Locale("en", "US");
	//private Locale chLocale = Locale.CHINESE;
	
	/**
	 * 默认的构造函数，默认输入源是标准流
	 */
	public In(){
		//charsetName标志字节转换为字符的方式
		scanner = new Scanner(new BufferedInputStream(System.in),charsetName);
		//将此扫描器设置为指定的语言环境
		scanner.useLocale(usLocale);
	}
	
	/**
	 * 从socket中获取输入流
	 * @param socket
	 * @throws Exception 
	 */
	public In(Socket socket) throws Exception{
		if(socket == null){
			throw new Exception("input parameter can not be null");
		}
		try{
			InputStream is = socket.getInputStream();
			scanner = new Scanner(new BufferedInputStream(is),charsetName);
			//将此扫描器设置为指定的环境
			scanner.useLocale(usLocale);
		}
		catch (Exception e) {
			// TODO: handle exception
			System.err.println("Could not open socket,exception is "+e.toString());
		}
	}
	
	/**
	 * 从url中获取输入流
	 * @param url
	 */
	public In(URL url){
		try{
			URLConnection site = url.openConnection();
			InputStream is = site.getInputStream();
			scanner = new Scanner(new BufferedInputStream(is),charsetName);
			scanner.useLocale(usLocale);
		}
		catch (Exception e) {
			// TODO: handle exception
			System.err.println("exception :"+e.toString());
		}
	}
	
	/**
	 * 从文件中获取输入流
	 * @param file
	 */
	public In(File file){
		try{
			scanner = new Scanner(file,charsetName);
			scanner.useLocale(usLocale);
		}
		catch (Exception e) {
			// TODO: handle exception
			System.err.println("exception:"+file);
		}
	}
	
	/**
	 * 从文件或者网页页面中获取输入
	 * @param s
	 */
	public In(String s){
		try{
			File file = new File(s);
			if(file.exists()){
				scanner = new Scanner(file,charsetName);
				scanner.useLocale(usLocale);
				return;
			}
			
			//将其作为url解析
			URL url = getClass().getResource(s);
			if(url == null){
				url = new URL(s);
			}
			
			URLConnection site = url.openConnection();
			InputStream is = site.getInputStream();
			scanner = new Scanner(new BufferedInputStream(is),charsetName);
			scanner.useLocale(usLocale);
		}
		catch(Exception e){
			System.out.println("Exception:"+e.toString());
		}
		
	}

	/**
	 * 输入流是否存在
	 * @return
	 */
	public boolean exists(){
		return scanner != null;
	}
	
	/**
	 * 数据流是否为空
	 * @return
	 */
	public boolean isEmpty(){
		return !scanner.hasNext();
	}
	
	/**
	 * 是否有下一行
	 * @return
	 */
	public boolean hasNextLine(){
		return scanner.hasNextLine();
	}
	
	/**
	 * 从数据流中读取下一行
	 * @return
	 */
	public String readLine(){
		String line;
		try{
			line = scanner.nextLine();
		}
		catch (Exception e) {
			// TODO: handle exception
			line = null;
		}
		return line;
	}
	
	
	/**
	 * 从数据流中读取下一个字符
	 * @return
	 */
	public char readChar(){
		String s = scanner.findWithinHorizon("(?s).", 1);
		return s.charAt(0);
	}
	
	/**
	 * 读取所有内容
	 * @return
	 */
	public String readAll(){
		if(!scanner.hasNextLine()){
			return null;
		}
		
		return scanner.useDelimiter("\\A").next();
	}
	
	/**
	 * 从输入流中读取下一个字符串
	 * @return
	 */
	public String readString(){
		return scanner.next();
	}
	
	/**
	 * 从下一个输入流中读取整数
	 * @return
	 */
	public int readInt(){
		return scanner.nextInt();
	}
	
	/**
	 * 从输入流中读取下一个double
	 * @return
	 */
	public double readDouble(){
		return scanner.nextDouble();
	}
	
	/**
	 * 从输入流中读取下一个浮点数
	 * @return
	 */
	public float readFloat(){
		return scanner.nextFloat();
	}
	
	/**
	 * 读取下一个长整数
	 * @return
	 */
	public long readLong(){
		return scanner.nextLong();
	}
	
	/**
	 * 读取下一个字节
	 * @return
	 */
	public byte readByte(){
		return scanner.nextByte();
	}
	
	/**
	 * 从输入流中读取一行，如果是 true或者1，则返回true，如果是false或者0则返回false
	 * @return
	 */
	public boolean readBoolean(){
		String s = readString();
		if(s.equalsIgnoreCase("true")){
			return true;
		}
		if(s.equalsIgnoreCase("false")){
			return false;
		}
		if(s.equalsIgnoreCase("1")){
			return true;
		}
		if(s.equalsIgnoreCase("0")){
			return false;
		}
		throw new java.util.InputMismatchException();
	}

	/**
	 * read ints from file
	 * @param filename
	 * @return
	 */
	public static int[] readInts(String filename){
		In in = new In(filename);
		String[] fields = in.readAll().trim().split("\\s+");
		int[] vals = new int[fields.length];
		for(int i=0;i<fields.length;i++){
			vals[i] = Integer.parseInt(fields[i]);
		}
		return vals;
	}
	
	/**
	 * 从文件中读取double
	 * @param filename
	 * @return
	 */
	public static double[] readDoubles(String filename){
		In in = new In(filename);
		String[] fields = in.readAll().trim().split("\\s+");
		double[] vals = new double[fields.length];
		for(int i=0;i<fields.length;i++){
			vals[i] = Double.parseDouble(fields[i]);
		}
		return vals;
	}

	/**
	 * 从文件中读取字符串
	 * @param filename
	 * @return
	 */
	public static String[] readStrings(String filename){
		In in = new In(filename);
		String[] fields = in.readAll().trim().split("\\s+");
		return fields;
	}


	/**
	 * 关闭输入流
	 */
	public void close(){
		if(scanner != null){
			scanner.close();
		}
	}
	
	/**
	 * 测试客户端
	 * @param args
	 */
	public static void main(String[] args){
		In in;
		String urlName = "http://introcs.cs.princeton.edu/stdlib/InTest.txt";
		System.out.println("readAll() from URL "+urlName);
		System.out.println("-----------------------------------------------------");
		try{
			in = new In(urlName);
			System.out.println(in.readAll());
		}
		catch(Exception ex){
			System.out.println(ex);
		}
		
		System.out.println();
		
	}
	
	
	
	
}
