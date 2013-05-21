package com.cmlu.algorithm.graph;

import com.cmlu.commons.Stack;

public class DepthFirstPaths {

    private boolean[] marked;
    
    //last vertex on known path to this vertex
    private int[] edgeTo;
    
    private final int s;
    
    public DepthFirstPaths(Graph G,int s){
	marked = new boolean[G.V()];
	edgeTo = new int[G.V()];
	this.s = s;
	dfs(G,s);
    }
    
    
    private void dfs(Graph G,int v){
	marked[v] = true;
	for(int w:G.adj(v)){
	    if(!marked[w]){
		edgeTo[w] = v;
		dfs(G, w);
	    }
	}
    }
    
    
    public boolean hasPathTo(int v){
	return marked[v];
    }
    
    
    //return a path between s to v,null if no such path
    public Iterable<Integer> pathTo(int v){
	if(!hasPathTo(v)){
	    return null;
	}
	
	Stack<Integer> pathStack = new Stack<Integer>();
	for(int x=v;x!=s;x=edgeTo[x]){
	    pathStack.push(x);
	}
	pathStack.push(s);
	return pathStack;
    }
    
    
    
    
}
