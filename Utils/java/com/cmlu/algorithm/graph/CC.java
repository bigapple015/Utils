package com.cmlu.algorithm.graph;

import com.cmlu.commons.Queue;
import com.cmlu.lang.In;
import com.cmlu.lang.StdOut;

/**
 * 连通性问题
 * @author Administrator
 *
 */
public class CC {
    /**
     * marked[v] = has vertex v been marked
     */
    private boolean[] marked;
    /**
     * id[v] = id of connected component containing v
     */
    private int[] id;
    
    /**
     * size[id] = number of vertices in given compent
     */
    private int[] size;
    
    /**
     * number of connected components
     */
    private int count;
    
    /**
     * 构造函数
     */
    public CC(Graph G){
	marked = new boolean[G.V()];
	id = new int[G.V()];
	size = new int[G.V()];
	for(int v=0;v<G.V();v++){
	    if(!marked[v]){
		dfs(G,v);
		count++;
	    }
	}
    }
    
    private void dfs(Graph G,int v){
	marked[v] = true;
	id[v] = count;
	size[count]++;
	for(int w:G.adj(v)){
	    if(!marked[w]){
		dfs(G, w);
	    }
	}
    }
    
 // id of connected component containing v
    public int id(int v) {
        return id[v];
    }

    // size of connected component containing v
    public int size(int v) {
        return size[id[v]];
    }

    // number of connected components
    public int count() {
        return count;
    }

    // are v and w in the same connected component?
    public boolean areConnected(int v, int w) {
        return id(v) == id(w);
    }

    
    //test client
    public static void main(String[] args){
	In in = new In(args[0]);
	Graph G = new Graph(in);
	CC cc = new CC(G);
	
	//number of connected compents
	int M = cc.count();
	StdOut.println(M+" compents");
	
	// compute list of vertices in each connected component
        Queue<Integer>[] components = (Queue<Integer>[]) new Queue[M];
        for (int i = 0; i < M; i++) {
            components[i] = new Queue<Integer>();
        }
        for (int v = 0; v < G.V(); v++) {
            components[cc.id(v)].enqueue(v);
        }

        // print results
        for (int i = 0; i < M; i++) {
            for (int v : components[i]) {
                StdOut.print(v + " ");
            }
            StdOut.println();
        }
    }
    
    
    
    
    
    
    
    
}
