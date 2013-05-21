package io;

import java.io.*;

public class BufferedInputFile {
	/**
	 * ¶ÁÈ¡ÎÄ¼þ
	 * @param filename
	 * @return
	 * @throws IOException
	 */
	public static String read(String filename) throws IOException{
		BufferedReader buffReader = null;
		try{
			buffReader = new BufferedReader(new FileReader(filename));
			String str = "";
			StringBuffer sb = new StringBuffer();
			while((str = buffReader.readLine()) != null){
				sb.append(str + "\n");
			}
			return sb.toString();
		}finally{
			buffReader.close();
		}
	}
	
	public static void main(String[] args) throws IOException{
		System.out.println(read("D:\\Readme.txt"));
	}
}
