package iflytek.common.base;

import java.io.*;


/**
 * 文件访问类，用于读取vxml文件
 * @author Administrator
 */
public class FileAccess {

	/**
	 * 读取vxml文件，并将其内容作为字符串返回
	 * @param path 文件的路径
	 * @return 读取到的vxml内容
	 * @throws IOException
	 */
	public static String readVxmlFile(String path) throws IOException{
		BufferedInputStream bufferInput = null;
		StringBuffer buffer = new StringBuffer();
		try
		{
			File vxmlFile = new File(path);
			bufferInput = new BufferedInputStream(new FileInputStream(vxmlFile));
			int readBytes = 0;
			while( (readBytes=bufferInput.read()) != -1)
			{
				buffer.append((char)readBytes);
			}
			
			return buffer.toString();
		} catch (Exception ex) {
			return "";
		}
		finally
		{
			if(bufferInput!=null) {
				bufferInput.close();
			}
		}
	}
	
	/**
	 * 译码 （将.gt.;等替换为>等）
	 * @param input 输入字符串
	 * @return 译码后的字符串
	 */
	public static String Decode(String input){
		String output = input;
		output = output.replace("&amp;", "&");
		output = output.replace("&lt;", "<");
		output = output.replace("&gt;", ">");
		output = output.replace("&quot;", "\"");
		return output;
	}
	
	/**
	 * 编码  （将>等替换为 .gt.;等）
	 * @param input
	 * @return
	 */
	public static String Encode(String input){
		if(input==null || input.isEmpty()){
			return "";
		}
		
		String output = input;
		output = output.replace("&","&amp;");
		output = output.replace("<","&lt;");
		output = output.replace(">","&gt;");
		output = output.replace("\"","&quot;");
		return output;
	}
}
