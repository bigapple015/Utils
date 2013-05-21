package com.cmlu.algorithm;

import com.cmlu.lang.StdIn;
import com.cmlu.lang.StdOut;
import com.cmlu.lang.Stopwatch;

/**
 * Union-find 数据模型
 * 
 * @author Administrator
 * 
 */
/**
 * 这个算法复杂度是各个节点深度的均值（至多是lgN）*N
 * @author Administrator
 *
 */
public class QuickUnionUF {
    // id[i] = parent of i
    // 第i个对象的等价类中的另外一个对象
    // 这样就构成了一个连接，id[i]->id[id[i]]直到id[i] == i,此时的i是等价类的根
    private int[] id;
    // sz[i] = number of objects in subtree rooted at i
    // i的取值只能是root
    private int[] sz;
    // number of compents
    private int count;

    /**
     * 这里的N是指把连接性问题中的每个对象（总共N个对象）转换为0至N-1的整数
     * 
     * @param N
     */
    public QuickUnionUF(int N) {
	count = N;
	id = new int[N];
	sz = new int[N];
	for (int i = 0; i < N; i++) {
	    id[i] = i;
	    sz[i] = 1;
	}
    }

    /**
     * return the id of component corresponding of object p
     * 这里可以进一步优化效率，在查找时，记录下p!=id[p]的p，待找到根后，将id[p]直接置为根的值，这样每个节点的深度为1
     * 这叫做path compression
     * @param p
     * @return
     */
    public int find(int p) {
	while (p != id[p]) {
	    p = id[p];
	}
	return p;
    }

    /**
     * return the number of disjoint sets
     */
    public int count() {
	return count;
    }

    /**
     * are objects p and q in the same set?
     */
    public boolean connected(int p, int q) {
	return find(p) == find(q);
    }

    /**
     * replace the set containing p and q with their union
     */
    public void union(int p, int q) {
	int i = find(p);
	int j = find(q);
	if (i == j)
	    return;
	// make smaller root point to larger one
	// 这里的选择是可选的
	/*if (sz[i] < sz[j]) {
	    id[i] = j;
	    sz[j] += sz[i];
	} else {
	    id[j] = i;
	    sz[i] += sz[j];
	}*/
	//永远将一端连接到另一端，这样产生的树趋于不平衡（随机）
	id[i] = j;
	sz[j] = sz[i] + sz[j];
	count--;
    }
    
    public void weightedUnion(int p,int q){
	int i = find(p);
	int j = find(q);
	if (i == j)
	    return;
	// make smaller root point to larger one
	// 这里的选择是可选的
	if (sz[i] < sz[j]) {
	    //将小的一端连接到root，从而使整个树更趋于平衡
	    //因此算法的效率更高
	    id[i] = j;
	    sz[j] += sz[i];
	} else {
	    id[j] = i;
	    sz[i] += sz[j];
	}
    }

    public static void main(String[] args) {
	try {
	    int N = StdIn.readInt();
	    Stopwatch watch = new Stopwatch();
	    QuickUnionUF uf = new QuickUnionUF(N);

	    // read in a sequence of pairs of integers (each in the range 0 to
	    // N-1),
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

	} catch (Exception ex) {
	    StdOut.println(ex.toString());
	}
    }
}
