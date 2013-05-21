package com.cmlu.algorithm.graph;

import com.cmlu.commons.Bag;
import com.cmlu.lang.In;
import com.cmlu.lang.StdOut;

/**
// * 有向图的深度优先搜索算法
 * @author Administrator
 *
 */
public class DirectedDFS {
    /**
     * marked[v] = true if v is reachable
     */
    private boolean[] marked;
    
    public DirectedDFS(Digraph G,int s){
	marked = new boolean[G.V()];
	dfs(G,s);
    }
    
    /**
     * 多源可访问性
     */
    public DirectedDFS(Digraph G,Iterable<Integer> sources){
	marked = new boolean[G.V()];
	for(int v:sources){
	    dfs(G,v);
	}
    }
    
    private void dfs(Digraph G,int v){
	marked[v] = true;
	for(int w:G.adj(v)){
	    if(!marked[w]){
		dfs(G, w);
	    }
	}
    }
    
 // is there a directed path from the source (or sources) to v?
    public boolean marked(int v) {
        return marked[v];
    }

    // test client
    public static void main(String[] args) {

        // read in digraph from command-line argument
        In in = new In(args[0]);
        Digraph G = new Digraph(in);

        // read in sources from command-line arguments
        Bag<Integer> sources = new Bag<Integer>();
        for (int i = 1; i < args.length; i++) {
            int s = Integer.parseInt(args[i]);
            sources.add(s);
        }

        // multiple-source reachability
        DirectedDFS dfs = new DirectedDFS(G, sources);

        // print out vertices reachable from sources
        for (int v = 0; v < G.V(); v++) {
            if (dfs.marked(v)) StdOut.print(v + " ");
        }
        StdOut.println();
    }
    
    
}
