package com.cmlu.algorithm;

import com.cmlu.lang.StdIn;
import com.cmlu.lang.StdOut;
import com.cmlu.lang.Stopwatch;

public class QuickFindUF {

    // UnionFind(int n);
    // void union(int p,int q);
    // int find(int p)
    // boolean connected(int p,int q)
    // int count()

    public int count() {
	return count;
    }

    public void union(int p, int q) {
	if (connected(p, q)) {
	    return;
	}
	count--;
	int c1 = sz[p] + sz[q];
	for (int i = 0; i < id.length; i++) {
	    if (id[i] == id[p]) {
		id[i] = id[q];
		sz[i] = c1;
	    }

	    if (id[i] == id[q]) {
		sz[q] = c1;
	    }
	}
    }

    // 测试两个对象是否连接
    public boolean connected(int p, int q) {
	return id[p] == id[q];
    }

    public int find(int p) {
	/*
	 * while(p != id[p]){ p = id[p]; } return p;
	 */
	return id[p];
    }

    // 等价类的数目
    private int count;
    // 第i（索引i）个对象所在的等价类的id
    private int[] id;
    // 第i（索引i）个等价类包含的对象数
    private int[] sz;

    // count表示总共多少个对象
    public QuickFindUF(int count) {
	this.count = count;
	id = new int[count];
	sz = new int[count];
	// 初始化id,sz
	for (int i = 0; i < id.length; i++) {
	    id[i] = i;
	    sz[i] = 1;
	}
    }

    public static void main(String[] args) {

	int N = StdIn.readInt();
	Stopwatch watch = new Stopwatch();
	QuickFindUF uf = new QuickFindUF(N);

	// read in a sequence of pairs of integers (each in the range 0 to N-1),
	// calling find() for each pair: If the members of the pair are not
	// already
	// call union() and print the pair.
	while (!StdIn.isEmpty()) {
	    int p = StdIn.readInt();
	    int q = StdIn.readInt();
	    if (uf.connected(p, q))
		continue;
	    uf.union(p, q);
	    StdOut.println(p + " " + q);
	}
	StdOut.println("# components: " + uf.count());
	StdOut.println("time used:" + watch.elapsedTime() + "s");

    }

    /*
     * 10 4 3 3 8 6 5 9 4 2 1 8 9 5 0 7 2 6 1 1 0 6 7
     */
}
