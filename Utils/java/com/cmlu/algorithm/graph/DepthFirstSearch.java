package com.cmlu.algorithm.graph;

/**
 * 深度优先搜索算法
 * 基本思想：
 * 标志一个结点
 * 递归所有与该节点连接的未标志结点
 * @author Administrator
 *
 */
public class DepthFirstSearch {

    /**
     * marked[i]标志第i个结点是否已经被标志
     */
    private boolean[] marked;
    /**
     * 表示标志的结点数
     */
    private int count;
    
    public int S;
    
    /**
     * 构造函数
     * @param G
     * @param s 深度优先搜索的起点
     */
    public DepthFirstSearch(Graph G,int s){
	marked = new boolean[G.V()];
	S = s;
	dfs(G,s);
    }
    
    /**
     * 深度优先搜索
     * @param G
     * @param v
     */
    private void dfs(Graph G,int v){
	marked[v] = true;
	count++;
	for(int w:G.adj(v)){
	    if(!marked[w]){
		dfs(G, w);
	    }
	}
    }
    
    /**
     * 标志s和w是否连接
     * @param w
     * @return
     */
    public boolean marked(int w){
	return marked[w];
    }
    
    /**
     * 与s连接的定点数 如果等于G.V()-1则标志所有的顶点是连接的
     * @return
     */
    public int count(){
	return count;
    }
    
}
