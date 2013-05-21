package io;

import java.util.regex.*;
import java.io.*;
import java.util.*;

public class Directory {
	/**
	 * 返回指定目录下的文件，并不递归子目录下的文件
	 * 这里的文件包括目录
	 */
	public static File[] local(File dir,final String regex){
		return dir.listFiles(new FilenameFilter() {
			private Pattern pattern = Pattern.compile(regex);
			@Override
			public boolean accept(File dir, String name) {
				// TODO Auto-generated method stub
				return pattern.matcher(new File(name).getName()).matches();
			}
		});
	}
	
	/**
	 * 返回指定目录下的文件，并不递归子目录下的文件
	 * 这里的文件包括目录
	 */
	public static File[] local(String path,final String regex){
		return local(new File(path), regex);
	}
	
	/**
	 * 目录下的文件和目录信息
	 * @author Administrator
	 *
	 */
	public static class TreeInfo implements Iterable<File>{

		public List<File> files = new ArrayList<File>();
		public List<File> dirs = new ArrayList<File>();
		@Override
		public Iterator<File> iterator() {
			// TODO Auto-generated method stub
			return files.iterator();
		}
		
		public void addAll(TreeInfo other){
			files.addAll(other.files);
			dirs.addAll(other.dirs);
		}
	}
	
	public static TreeInfo walk(String start, String regex){
		return recurseDirs(new File(start),regex);
	}
	
	public static TreeInfo walk(File start,String regex){
		return recurseDirs(start,regex);
	}
	
	/**
	 * 返回递归目录
	 * @param start
	 * @return
	 */
	public static TreeInfo walk(File start){
		return recurseDirs(start,".*");
	}
	
	public static TreeInfo walk(String start){
		return recurseDirs(new File(start),".*");
	}
	
	public static TreeInfo recurseDirs(File startDir,String regex){
		TreeInfo resultInfo = new TreeInfo();
		for(File item:startDir.listFiles()){
			if(item.isDirectory()){
				resultInfo.dirs.add(item);
				resultInfo.addAll(recurseDirs(item, regex));
			}
			else{
				if(item.getName().matches(regex)){
					resultInfo.files.add(item);
				}
			}
		}
		return resultInfo;
	}
	
}
