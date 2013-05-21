package com.cmlu.algorithm.graph;

import com.cmlu.commons.Queue;
import com.cmlu.commons.Stack;
import com.cmlu.lang.In;
import com.cmlu.lang.StdOut;

/**
 * 广度优先搜索算法
 */
public class BreadthFirstPaths {

    private static final int INFINITY = Integer.MAX_VALUE;
    
    /**
     * marked[v] = is there an s-v path
     */
    private boolean[] marked;
    
    /**
     * edgeTo[v] = previous edge on shortest s-v path
     */
    private int[] edgeTo;
    
    /**
     * distTo[v] = number of edges shortest s-v path
     */
    private int[] distTo;
    
    public BreadthFirstPaths(Graph G,int s) {
	// TODO Auto-generated constructor stub
	marked = new boolean[G.V()];
	distTo = new int[G.V()];
	edgeTo = new int[G.V()];
	bfs(G,s);
	assert check(G,s);
    }
    
    
    //multi sources
    public BreadthFirstPaths(Graph G,Iterable<Integer> sources){
	marked = new boolean[G.V()];
	distTo = new int[G.V()];
	edgeTo = new int[G.V()];
	for(int v=0;v<G.V();v++){
	    distTo[v] = INFINITY;
	}
	
	bfs(G,sources);
    }
    
    //bfs from single source
    private void bfs(Graph G,int s) {
	Queue<Integer> q = new Queue<Integer>();
	for(int v=0;v<G.V();v++){
	    distTo[v] = INFINITY;
	}
	distTo[s] = 0;
	marked[s] = true;
	q.enqueue(s);
	
	while(!q.isEmpty()){
	    int v = q.dequeue();
	    for(int w:G.adj(v)){
		if(!marked[w]){
		    edgeTo[w] = v;
		    distTo[w] = distTo[v] + 1;
		    marked[w] = true;
		    q.enqueue(w);
		}
	    }
	}
    }
    
    //bfs from multiple sources
    private void bfs(Graph G,Iterable<Integer> sources){
	Queue<Integer> q = new Queue<Integer>();
	for(int s:sources){
	    marked[s] = true;
	    distTo[s] = 0;
	    q.enqueue(s);
	}
	
	while(!q.isEmpty()){
	    int v = q.dequeue();
	    for(int w:G.adj(v)){
		if(!marked[w]){
		    edgeTo[w] = v;
		    distTo[w] = distTo[v] + 1;
		    marked[w] = true;
		    q.enqueue(w);
		}
	    }
	}
    }
    
    //is there a path between s and v
    public boolean hasPathTo(int v){
	return marked[v];
    }
    
    //length of shortest path between s and v
    public int distTo(int v){
	return distTo(v);
    }
    
    //shortest path between s and v; null if no such path
    public Iterable<Integer> pathTo(int v){
	if(!hasPathTo(v)) return null;
	Stack<Integer> path = new Stack<Integer>();
	int x;
	for(x =v;distTo[x] != 0;x=edgeTo[x]){
	    path.push(x);
	}
	path.push(x);
	return path;
    }
    
    
    //check optimality conditions for single source
    private boolean check(Graph G,int s){
	if(distTo[s] != 0){
	    StdOut.println("distance of source "+s+" to itself = "+distTo[s]);
	    return false;
	}
	
	//provided v is reachable from s
	for(int v=0;v<G.V();v++){
	    for(int w:G.adj(v)){
		if(hasPathTo(v)!=hasPathTo(w)){
		    StdOut.println("edge " + v + "-" + w);
                    StdOut.println("hasPathTo(" + v + ") = " + hasPathTo(v));
                    StdOut.println("hasPathTo(" + w + ") = " + hasPathTo(w));
                    return false;
		}
	    }
	}
	
	for (int w = 0; w < G.V(); w++) {
            if (!hasPathTo(w) || w == s) continue;
            int v = edgeTo[w];
            if (distTo[w] != distTo[v] + 1) {
                StdOut.println("shortest path edge " + v + "-" + w);
                StdOut.println("distTo[" + v + "] = " + distTo[v]);
                StdOut.println("distTo[" + w + "] = " + distTo[w]);
                return false;
            }
        }

        return true;
    }
    
    
    
    // test client
    public static void main(String[] args) {
        In in = new In(args[0]);
        Graph G = new Graph(in);
        // StdOut.println(G);

        int s = Integer.parseInt(args[1]);
        BreadthFirstPaths bfs = new BreadthFirstPaths(G, s);

        for (int v = 0; v < G.V(); v++) {
            if (bfs.hasPathTo(v)) {
                StdOut.printf("%d to %d (%d):  ", s, v, bfs.distTo(v));
                for (int x : bfs.pathTo(v)) {
                    if (x == s) StdOut.print(x);
                    else        StdOut.print("-" + x);
                }
                StdOut.println();
            }

            else {
                StdOut.printf("%d to %d (-):  not connected\n", s, v);
            }

        }
    }

}
