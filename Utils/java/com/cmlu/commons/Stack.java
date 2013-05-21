package com.cmlu.commons;

import java.awt.Checkbox;
import java.util.Iterator;
import java.util.ListIterator;
import java.util.NoSuchElementException;

import com.cmlu.lang.StdIn;
import com.cmlu.lang.StdOut;

/**
 * 用链表的方式实现堆栈
 * @author Administrator
 *
 */
public class Stack<Item> implements Iterable<Item> {
	//the size of the stack
	private int N;
	//栈顶
	private Node first;
	
	private class Node{
		private Item item;
		private Node next;
	}
	
	/**
	 * instructor
	 */
	public Stack(){
		first = null;
		N = 0;
		assert check();
	}

	/**
	 * Is the stack Empty
	 * @return
	 */
	public boolean isEmpty(){
		return first == null;
	}
	
	/**
	 * return the number of items in the stack
	 * @return
	 */
	public int size(){
		return N;
	}
	
	/**
	 * Add the item to the stack
	 * @param item
	 */
	public void push(Item item){
		Node oldfirst = first;
		first = new Node();
		first.item = item;
		first.next = oldfirst;
		N++;
		assert check();
	}
	
	public Item pop(){
		if(isEmpty()) throw new RuntimeException("stack underflow");
		Item item = first.item;
		first = first.next;
		N--;
		//一致性检查
		assert check();
		return item;
	}
	
	
	public Item peek(){
		if(isEmpty()) throw new RuntimeException("Stack underflow");
		return first.item;
	}
	
	@Override
	public String toString(){
		StringBuilder sb = new StringBuilder();
		for(Item item:this){
			sb.append(item + " ");
		}
		return sb.toString();
	}
	
	@Override
	public Iterator<Item> iterator() {
		// TODO Auto-generated method stub
		return new ListIterator();
	}
	
	
	private class ListIterator implements Iterator<Item>{
		private Node current = first;
		public boolean hasNext(){return current != null;}
		public void remove(){
			throw new UnsupportedOperationException();
		}
		
		public Item next(){
			if(!hasNext()) throw new NoSuchElementException();
			Item item = current.item;
			current = current.next;
			return item;
		}
		
	}
	
	// check internal invariants
    private boolean check() {
        if (N == 0) {
            if (first != null) return false;
        }
        else if (N == 1) {
            if (first == null)      return false;
            if (first.next != null) return false;
        }
        else {
            if (first.next == null) return false;
        }

        // check internal consistency of instance variable N
        int numberOfNodes = 0;
        for (Node x = first; x != null; x = x.next) {
            numberOfNodes++;
        }
        if (numberOfNodes != N) return false;

        return true;
    } 

    //test client
    public static void main(String[] args){
    	Stack<String> stack = new Stack<String>();
    	while (!StdIn.isEmpty()) {
			String item = StdIn.readString();
			if(!item.equals("-")) stack.push(item);
			else if (!stack.isEmpty()) StdOut.print(stack.pop() + " ");
		}
    	StdOut.println("("+stack.size()+" left on stack)");
    }
    
}
