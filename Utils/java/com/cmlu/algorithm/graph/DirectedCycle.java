package com.cmlu.algorithm.graph;

import java.awt.Checkbox;

import com.cmlu.commons.Stack;
import com.cmlu.lang.In;
import com.cmlu.lang.StdOut;

public class DirectedCycle {

    //marked[v] = has vertex v been marked
    private boolean[] marked;
    
    /**
     * edgeTp[v] = previous vertex on path to v
     */
    private int[] edgeTo;
    
    /**
     * onStack[v] = is vertex on the stack
     */
    private boolean[] onStack;
    
    /**
     * directed cycle if null, then no such cycle
     */
    private Stack<Integer> cycle;
    
    public DirectedCycle(Digraph G){
	marked = new boolean[G.V()];
	onStack = new boolean[G.V()];
	edgeTo = new int[G.V()];
	for(int v=0;v<G.V();v++){
	    if(!marked[v]) dfs(G,v);
	}
	//判断是否是无环的
	assert check(G);
    }
    
 // check that algorithm computes either the topological order or finds a directed cycle
    private void dfs(Digraph G, int v) {
	onStack[v] = true;
	marked[v] = true;
	for(int w:G.adj(v)){
	    //short circuit if directed cycle found
	    if(cycle != null) return;
	    else if(!marked[w]){
		edgeTo[w] = v;
		dfs(G, w);
	    }
	    //trace back directed cycle
	    else if (onStack[w]){
		cycle = new Stack<Integer>();
		for(int x=v;x!=w;x=edgeTo[x]){
		    cycle.push(x);
		}
		cycle.push(w);
		cycle.push(v);
	    }
	}
	onStack[v] = false;
    }
    public boolean hasCycle()        { return cycle != null;   }
    public Iterable<Integer> cycle() { return cycle;           }


    // certify that digraph is either acyclic or has a directed cycle
    private boolean check(Digraph G) {

        if (hasCycle()) {
            // verify cycle
            int first = -1, last = -1;
            for (int v : cycle()) {
                if (first == -1) first = v;
                last = v;
            }
            if (first != last) {
                System.err.printf("cycle begins with %d and ends with %d\n", first, last);
                return false;
            }
        }


        return true;
    }

    public static void main(String[] args) {
        In in = new In(args[0]);
        Digraph G = new Digraph(in);

        DirectedCycle finder = new DirectedCycle(G);
        if (finder.hasCycle()) {
            StdOut.print("Cycle: ");
            for (int v : finder.cycle()) {
                StdOut.print(v + " ");
            }
            StdOut.println();
        }

        else {
            StdOut.println("No cycle");
        }
    }
    
}
