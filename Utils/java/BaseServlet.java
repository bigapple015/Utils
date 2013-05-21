package iflytek.common.base;


import java.io.UnsupportedEncodingException;

import javax.servlet.ServletContext;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpSession;

//****************************************************
//注意事项：
//1）在java中使用/表示网站的根路径，即WebRoot
//2）不要忘记在web.xml中配置session的有效期，配置如下
/*
<session-config>
<!-- 以分钟为单位 -->
<session-timeout>15 </session-timeout>
</session-config>
*/
/**
 * 此类作为编写vxml的Servlet的共同基类
 */
public class BaseServlet extends HttpServlet {

	/**
	 * 
	 */
	private static final long serialVersionUID = 3322483988152568915L;

	/**
	 * 获取文件或目录实际的路径
	 * @param mapUrl 指定虚拟路径的 String
	 * @return 文件或目录实际的路径，如果无法执行转换，则返回 null。
	 */
	public String getPath(String mapUrl){
		ServletContext app = this.getServletContext();
		return app.getRealPath(mapUrl);
	}
	
	/**
	 * 获取网站根路径的实际路径
	 * @return 网站的实际根路径
	 */
	public String getRootPath(){
		return getPath("/");
	}
	
	/**
	 * 使用指定名称将对象绑定到此会话。如果具有同样名称的对象已经绑定到该会话，则替换该对象。
	 * @param key 键
	 * @param value 值
	 * 
	 */
	public void putSessionValue(HttpServletRequest request,String key,Object value){
		//获取到session
		HttpSession session = request.getSession();
		session.setAttribute(key, value);
	}
	
	/**
	 * 获取指定名称的session
	 * @param key 键
	 * @return 返回与此会话中的指定名称绑定在一起的对象，如果没有对象绑定在该名称下，则返回 null。
	 */
	public Object getSessionValueOfObject(HttpServletRequest request,String key){
		//获取到session
		HttpSession session = request.getSession();
		return session.getAttribute(key);
	}
	
	/**
	 * 获取指定名称的session
	 * @param key 键
	 * @return 返回与此会话中的指定名称绑定在一起的字符串，如果没有字符串绑定在该名称下，则返回 null。
	 */
	public String getSessionValueOfString(HttpServletRequest request,String key){
		Object value = getSessionValueOfObject(request,key);
		if(value == null){
			return null;
		}
		return value.toString();
	}
	
	/**
	 * 从此会话中移除与指定名称绑定在一起的对象。如果会话没有与指定名称绑定在一起的对象，则此方法不执行任何操作。
	 * @param key 键
	 * 
	 */
	public void clearSession(HttpServletRequest request,String key){
		//获取到session
		HttpSession session = request.getSession();
	    session.removeAttribute(key);
	}
	
	/**
	 * 通常确定参数只有一个值时，才应该使用此方法。如果参数对应多个值，推荐重新设计你的程序，而不是调用getQueryStrings
	 * @param key 键
	 * @return 以 String 形式返回请求参数的值，如果该参数不存在，则返回 null。
	 */
	public String getQueryString(HttpServletRequest request,String key){
		return  request.getParameter(key);
	}
	
	/**
	 * 不推荐使用该方法，如果你使用了该方法，建议你重新设计程序。（虽然写在这里，但程序员不应该使用该方法）
	 * @param request
	 * @param key
	 * @return 返回包含给定请求参数拥有的所有值的 String 对象数组，如果该参数不存在，则返回 null。
	 */
	public String[] getQueryStrings(HttpServletRequest request,String key){
		return request.getParameterValues(key);
	}
	
	/**
	 * 解决get和post方式下中文查询字符串乱码的情况
	 * @param source
	 * @return
	 * @throws UnsupportedEncodingException 
	 */
	public String stringToGBK(String source) throws UnsupportedEncodingException{
		return new String(source.getBytes("ISO-8859-1"),"GBK");
	}
}
