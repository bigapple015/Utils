package com.cmlu.algorithm.graph;

import com.cmlu.commons.Bag;
import com.cmlu.lang.In;
import com.cmlu.lang.StdOut;

/**
 * 图的实现
 * 实现了无向图，图的各个节点标志从0到v-1
 * @author Administrator
 *
 */
public class Graph {

    /*
     * 顶点数
     */
    private final int V;
    /**
     * 边数
     */
    private int E;
    
    /**
     * 第i顶点集合连接的其它结点数
     */
    private Bag<Integer>[] adj;
    
    /**
     * 构造函数，创建一个带v个顶点空的图
     * @param V
     */
    public Graph(int V){
	if(V < 0){
	    throw new RuntimeException("Number of vertices must be nonnegative");
	}
	
	this.V = V;
	this.E = 0;
	adj =  (Bag<Integer> [])new Bag[V];
	
	for(int i=0;i<V;i++){
	    adj[i] = new Bag<Integer>();
	}
    }
    
    /**
     * 创建一个随机的图，带有v个顶点和E个边
     * @param V
     * @param E
     */
    public Graph(int V,int E){
	this(V);
	if(E < 0){
	    throw new RuntimeException("Number of edges must be nonnegative");
	}
	
	for(int i=0;i<E;i++){
	    //定义两个顶点，构成一条边
	    int v = (int) (Math.random()*V);
	    int w = (int) (Math.random()*V);
	    addEdge(v,w);
	}
    }
    
    /**
     * 从输入流中创建一个图
     * @param in
     */
    public Graph(In in){
	this(in.readInt());
	int E = in.readInt();
	for(int i=0;i<E;i++){
	    int v = in.readInt();
	    int w = in.readInt();
	    addEdge(v,w);
	}
    }
    
    /**
     * 返回图中节点的数目
     * @return
     */
    public int V(){
	return V;
    }
    
    /**
     * 返回图中边的个数
     * @return
     */
    public int E(){
	return E;
    }
    
    /**
     * 增加一个无向边 w-v
     */
    public void addEdge(int v,int w){
	E++;
	adj[v].add(w);
	adj[w].add(v);
    }
    
    /**
     * 获取第v个顶点的邻居(直接连接的边)
     * @param v
     * @return
     */
    public Iterable<Integer> adj(int v){
	return adj[v];
    }
    
    @Override
    public String toString(){
	StringBuilder s = new StringBuilder();
	String NEWLINE = System.getProperty("line.separator");
	s.append(V+" vertices, "+E+" edges "+NEWLINE);
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
        Graph G = new Graph(in);
        StdOut.println(G);
    }
}
