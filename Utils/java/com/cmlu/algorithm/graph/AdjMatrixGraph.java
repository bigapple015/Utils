package com.cmlu.algorithm.graph;

import java.util.Iterator;
import java.util.NoSuchElementException;

import com.cmlu.lang.StdOut;

/**
 * 图的邻接矩阵实现，V*V的矩阵
 * @author Administrator
 *
 */
public class AdjMatrixGraph {
    /**
     * 结点数
     */
    private int V;
    
    /**
     * 边数
     */
    private int E;
    
    /**
     * 如果为true，则表示i和j是连接的
     */
    private boolean[][] adj;
    
    /**
     * 构造函数
     */
    public AdjMatrixGraph(int V){
	if(V<0){
	    throw new RuntimeException("Number of vertices must be nonnegative");
	}
	this.V = V;
	E = 0;
	adj = new boolean[V][V];
    }
    
    /**
     * 构造函数
     * @param V
     * @param E
     */
    public AdjMatrixGraph(int V,int E){
	this(V);
	if(E<0){
	    throw new RuntimeException("Number of edges must be nonnegative");
	}
	
	if(E>V*V) {
	    throw new RuntimeException("Too many edges");
	}
	
	if(this.E != E){
	    int v = (int)(V*Math.random());
	    int w = (int)(V*Math.random());
	    addEdge(v,w);
	}
	
    }
    
    /**
     * 返回结点数
     * @return
     */
    public int V(){
	return V;
    }
    
    /**
     * 返回边
     * @return
     */
    public int E(){
	return E;
    }
    
    /**
     * 增加一条边
     */
    public void addEdge(int v,int w){
	if(!adj[v][w]) E++;
	
	adj[w][v] = true;
	adj[v][w] = true;
    }
    
    /**
     * 是否包含v-w边
     */
    public boolean contains(int v,int w){
	return adj[v][w];
    }
    
    
    // return list of neighbors of v
    public Iterable<Integer> adj(int v) {
        return new AdjIterator(v);
    }

    // support iteration over graph vertices
    private class AdjIterator implements Iterator<Integer>, Iterable<Integer> {
        int v, w = 0;
        AdjIterator(int v) { this.v = v; }

        public Iterator<Integer> iterator() { return this; }

        public boolean hasNext() {
            while (w < V) {
                if (adj[v][w]) return true;
                w++;
            }
            return false;
        }

        public Integer next() {
            if (hasNext()) { return w++;                         }
            else           { throw new NoSuchElementException(); }
        }

        public void remove()  { throw new UnsupportedOperationException();  }
    }


    // string representation of Graph - takes quadratic time
    public String toString() {
        String NEWLINE = System.getProperty("line.separator");
        StringBuilder s = new StringBuilder();
        s.append(V + " " + E + NEWLINE);
        for (int v = 0; v < V; v++) {
            s.append(v + ": ");
            for (int w : adj(v)) {
                s.append(w + " ");
            }
            s.append(NEWLINE);
        }
        return s.toString();
    }


    // test client
    public static void main(String[] args) {
        int V = Integer.parseInt(args[0]);
        int E = Integer.parseInt(args[1]);
        AdjMatrixGraph G = new AdjMatrixGraph(V, E);
        StdOut.println(G);
    }
    
    
    
}
