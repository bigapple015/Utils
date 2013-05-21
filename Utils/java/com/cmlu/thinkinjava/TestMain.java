package com.cmlu.thinkinjava;

public class TestMain {
	
	class Contents{
		private int i = 11;
		public int value(){
			return i;
		}
	}
	
	class Destination{
		private String label;
		public Destination(String whereTo) {
			// TODO Auto-generated constructor stub
			label = whereTo;
		}
		String readLabel(){
			return label;
		}
	}
	
	public void ship(String dest){
		Contents c = new Contents();
		Destination d = new Destination(dest);
		System.out.println(d.readLabel());
	}
	
	//≤‚ ‘static”Ôæ‰øÈ	
	public static void main(String []args){
		TestMain main = new TestMain();
		main.ship("hello world");
	}
}
