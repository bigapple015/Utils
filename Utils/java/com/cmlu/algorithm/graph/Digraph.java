package com.cmlu.algorithm.graph;

import com.cmlu.commons.Bag;
import com.cmlu.commons.Stack;
import com.cmlu.lang.In;
import com.cmlu.lang.StdOut;

/**
 * 有向图
 */
public class Digraph {

    /**
     * 顶点的数
     */
    private final int V;
    
    /**
     * 边的个数
     */
    private int E;
    
    /**
     * adj[i]表示第i个结点为起点的边
     */
    private Bag<Integer>[] adj;
    
    /**
     * 创建一个空的带有V个顶点的有向图
     */
    public Digraph(int V){
	if(V < 0){
	    throw new RuntimeException("Number of vertices must be nonnegative");
	}
	this.V = V;
	this.E = 0;
	adj = (Bag<Integer>[]) new Bag[V];
	
	for(int v=0;v<V;v++){
	    adj[v] = new Bag<Integer>();
	}
    }
    
    /**
     * 创建一个有向图从输入流
     */
    public Digraph(In in){
	this(in.readInt());
	int E = in.readInt();
	for(int i=0;i<E;i++){
	    int v = in.readInt();
	    int w = in.readInt();
	    addEdge(v,w);
	}
    }
    
    /**
     * 复制构造函数, 构建一个与由原来完全相反的图
     */
    public Digraph(Digraph G){
	this(G.V());
	this.E = G.V();
	for(int v=0;v<G.V();v++){
	    Stack<Integer> reverse = new Stack<Integer>();
	    for(int w:G.adj[v]){
		reverse.push(w);
	    }
	    
	    for(int w:reverse){
		adj[w].add(w);
	    }
	}
    }
    
    /**
     * 返回结点的数目
     */
    public int V(){
	return V;
    }
    
    /**
     * 返回边的数目
     */
    public int E(){
	return E;
    }
    
    /**
     * 增加一条有向边v->w
     */
    public void addEdge(int v,int w){
	adj[v].add(w);
	E++;
    }
    
    /**
     * Return the list of vertices pointed to from vertex v as an Iterable.
     */
    public Iterable<Integer> adj(int v) {
        return adj[v];
    }
    
    /**
     * 返回有向图的反转版本
     */
    public Digraph reverse(){
	Digraph R = new Digraph(V);
	for(int v=0;v<V;v++){
	    for(int w:adj(v)){
		R.addEdge(w, v);
	    }
	}
	return R;
    }
    
    /**
     * Return a string representation of the digraph.
     */
    public String toString() {
        StringBuilder s = new StringBuilder();
        String NEWLINE = System.getProperty("line.separator");
        s.append(V + " " + E + NEWLINE);
        for (int v = 0; v < V; v++) {
            s.append(v + ": ");
            for (int w : adj[v]) {
                s.append(w + " ");
            }
            s.append(NEWLINE);
        }
        return s.toString();
    }

    /**
     * Test client.
     */
    public static void main(String[] args) {
        In in = new In(args[0]);
        Digraph G = new Digraph(in);
        StdOut.println(G);

        StdOut.println();
        for (int v = 0; v < G.V(); v++)
            for (int w : G.adj(v))
                StdOut.println(v + "->" + w);
    }

    
    
    
    
    
    
    
}
