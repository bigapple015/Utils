package com.cmlu.algorithm.graph;

/**
 * 一个关节结点是一个节点删除了将会增加了组件的数目
 * @author Administrator
 *
 */
public class Biconnected {
    private int[] low;
    private int[] pre;
    private int cnt;
    private int bridges;
    private boolean[] articulation;

    public Biconnected(Graph G) {
        low = new int[G.V()];
        pre = new int[G.V()];
        articulation = new boolean[G.V()];
        for (int v = 0; v < G.V(); v++) low[v] = -1;
        for (int v = 0; v < G.V(); v++) pre[v] = -1;
        
        for (int v = 0; v < G.V(); v++)
            if (pre[v] == -1)
                dfs(G, v, v);
    }

    private void dfs(Graph G, int u, int v) {
        int children = 0;
        pre[v] = cnt++;
        low[v] = pre[v];
        for (int w : G.adj(v)) {
            if (pre[w] == -1) {
                children++;
                dfs(G, v, w);

                // update low number
                low[v] = Math.min(low[v], low[w]);

                // non-root of DFS is an articulation point if low[w] >= pre[v]
                if (low[w] >= pre[v] && u != v) 
                    articulation[v] = true;
            }

            // update low number - ignore reverse of edge leading to v
            else if (w != u)
                low[v] = Math.min(low[v], pre[w]);
        }

        // root of DFS is an articulation point if it has more than 1 child
        if (u == v && children > 1)
            articulation[v] = true;
    }

    // is vertex v an articulation point?
    public boolean isArticulation(int v) { return articulation[v]; }

    // test client
    public static void main(String[] args) {
        int V = Integer.parseInt(args[0]);
        int E = Integer.parseInt(args[1]);
        Graph G = new Graph(V, E);
        System.out.println(G);

        Biconnected bic = new Biconnected(G);

        // print out articulation points
        System.out.println();
        System.out.println("Articulation points");
        System.out.println("-------------------");
        for (int v = 0; v < G.V(); v++)
            if (bic.isArticulation(v)) System.out.println(v);
    }

}
