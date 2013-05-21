package com.cmlu.commons;

import java.util.Stack;

import javax.script.ScriptEngineManager;
import javax.script.ScriptException;

import com.cmlu.lang.StdIn;
import com.cmlu.lang.StdOut;

/**
 * Dijkstra’s Two-Stack Algorithm for Expression Evaluation
 * 具体描述如下：
 * 1、push操作数放到操作数栈
 * 2、push操作符放到操作数栈中
 * 3、忽略左括号
 * 4、当遇到右括号，弹出一个操作符，弹出操作符需要的操作数，将计算结果放入操作数栈中
 * 当最后一个右括号被处理后只有一个值在操作数栈中即最终结果
 * @author Administrator
 *
For simplicity, this code assumes that the expression
is fully parenthesized, with numbers and characters
separated by whitespace.
 */
public class Evaluate {

	public static void main(String[] args) {
		Stack<String> ops = new Stack<String>();
		Stack<Double> vals = new Stack<Double>();

		while (!StdIn.isEmpty()) {
			String s = StdIn.readString();
			if(s.equals("(")){}
			else if (s.equals("+")){ops.push(s);}
			else if (s.equals("-")){ops.push(s);}
			else if(s.equals("*")) {ops.push(s);}
			else if(s.equals("/")) {ops.push(s);}
			else if(s.equals("sqrt")){ops.push(s);}
			else if(s.equals(")")){
				String op = ops.pop();
				double v = vals.pop();
				if(op.equals("+")) v = vals.pop()+v;
				if(op.equals("-")) v = vals.pop()-v;
				if(op.equals("*")) v = vals.pop()*v;
				if(op.equals("/")) v = vals.pop()/v;
				if(op.equals("sqrt")) v = Math.sqrt(v);
				vals.push(v);
			}
			else{
				vals.push(Double.parseDouble(s));
			}
		}
		
		//打印最终结果
		StdOut.println(vals.pop());
	}
	
	
	/**
	 * 计算表达式的值
	 * @param expr
	 * @return
	 */
	public static Object eval(String expr){
		  ScriptEngineManager sem = new ScriptEngineManager();
		  Object result = null;
		  try {
		    result = sem.getEngineByName("javascript").eval(expr);
		   //System.out.println(result.getClass());
		   //System.out.println(result);
		  } catch (Exception e) {
		  }
		  return result;
	}
}
