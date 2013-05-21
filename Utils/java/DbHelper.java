package iflytek.common.base;

import java.sql.*;;

/**
 * 该类作为使用java操作数据库的基类
 * @author Administrator
 *
 */
public class DbHelper {

	/**
	 * 单件实例
	 */
	private static DbHelper dbHelper;
	/**
	 * 与特定数据库的连接（会话）。
	 */
	private Connection conn;
	
	/**
	 * 数据库驱动类名，如com.mysql.jdbc.Driver
	 */
	private String driver;
	
	/**
	 * jdbc:subprotocol:subname 形式的数据库 url
	 */
	private String url;
	
	/**
	 * 连接数据库的用户名
	 */
	private String userName;
	
	/**
	 * 连接数据库的密码
	 */
	private String password;
	

	
	/**
	 * 私有的构造函数
	 * @throws Exception
	 */
	private DbHelper(String driver,String url,String userName,String password) throws Exception{
		this.driver = driver;
		this.url = url;
		this.userName = userName;
		this.password = password;
		//注册数据库驱动
		Class.forName(driver);
		this.conn = DriverManager.getConnection(url,userName,password);
	}


	/**
	 * 获取单件实例
	 * @return
	 */
	public static DbHelper getInstance(String driver,String url,String userName,String password) throws Exception{
		if(dbHelper == null){
			dbHelper = new DbHelper(driver,url,userName,password);
		}
		
		return dbHelper;
	}


	/**
	 * 获取与数据库的连接（会话）。
	 * @return
	 * @throws Exception 
	 */
	public Connection getConn() throws Exception {
		if(conn == null){
			Class.forName(this.driver);
			conn = DriverManager.getConnection(url,userName,password);
		}
		return conn;
	}

	/**
	 * 设置与数据库的连接
	 * @param conn
	 */
	public void setConn(Connection conn) {
		this.conn = conn;
	}

	/**
	 * 获取数据库驱动字符串
	 * @return
	 */
	public String getDriver() {
		return driver;
	}

	/**
	 * 设置数据库驱动字符串
	 * @param driver
	 */
	public void setDriver(String driver) {
		this.driver = driver;
	}

	/**
	 * 获取数据库url
	 * @return
	 */
	public String getUrl() {
		return url;
	}

	/**
	 * 设置数据库url
	 * @param url
	 */
	public void setUrl(String url) {
		this.url = url;
	}

	/**
	 * 获取数据库用户名
	 * @return
	 */
	public String getUserName() {
		return userName;
	}

	/**
	 * 设置数据库用户名
	 * @param userName
	 */
	public void setUserName(String userName) {
		this.userName = userName;
	}

	/**
	 * 获取数据库密码
	 * @return
	 */
	public String getPassword() {
		return password;
	}

	/**
	 * 设置数据库密码
	 * @param password
	 */
	public void setPassword(String password) {
		this.password = password;
	}
	
	/**
	 * 执行插入操作
	 * @param sql
	 * @return 是否插入成功
	 * @throws Exception
	 */
	public boolean insert(String sql) throws Exception{
		Statement stmt = this.conn.createStatement();
		if(stmt.executeUpdate(sql) != 1){
			return false;
		}
		return true;
	}
	
	/**
	 * 执行查询操作
	 * @return
	 * @throws Exception
	 */
	public ResultSet query(String sql) throws Exception{
		Statement stmt = conn.createStatement();
		return stmt.executeQuery(sql);
	}
	
	/**
	 * 返回受影响的行数
	 * @return
	 * @throws Exception
	 */
	public int delete(String sql) throws Exception{
		Statement stmt = conn.createStatement();
		return stmt.executeUpdate(sql);
	}
	
	/**
	 * 返回受影响的行数
	 * @return
	 * @throws Exception
	 */
	public int update(String sql) throws Exception{
		Statement stmt = conn.createStatement();
		return stmt.executeUpdate(sql);
	}
}
