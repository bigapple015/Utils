using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;

/*
 * @author   lcm
 * @date     2011年8月3日11:38:34
 * @version  1.0.0.0
 * @description: 文件操作工具类
 */

namespace iFlyTek.ECSS30.Tool.SMSFilter
{
    /// <summary>
    /// 用于操作文件的帮助类
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 读取文件的内容，如果文件不存在，将抛出<c>FileNotFoundException</c>
        /// </summary>
        /// <param name="fileName">要读取的文件</param>
        /// <returns>读取到的字符串</returns>
        public static string ReadFile(string fileName)
        {
            string result = string.Empty;
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("文件不存在");
            }
            StreamReader reader =null;
            try
            {
                reader = new StreamReader(fileName, Encoding.Default);
                result = reader.ReadToEnd();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                reader.Close();
            }
            return result;
        }

        /// <summary>
        /// 写内容到文件中，如果文件不存在，将创建该文件。如果文件存在，则覆盖它。
        /// </summary>
        /// <param name="content"></param>
        /// <param name="fileName"></param>
        public static void WriteFile(string content, string fileName)
        {
            FileStream stream = null;
            StreamWriter writer = null;
            try
            {
                stream = new FileStream(fileName, FileMode.Create);
                writer = new StreamWriter(stream, Encoding.Default);
                writer.Write(content);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                writer.Close();
                stream.Close();
            }
        }

        /// <summary>
        /// 附加内容到文件的最后，如果文件不存在，将创建该文件
        /// </summary>
        /// <param name="content"></param>
        /// <param name="fileName"></param>
        public static void AppendFile(string content, string fileName)
        {
            FileStream stream = null;
            StreamWriter writer = null;
            try
            {
                stream = new FileStream(fileName, FileMode.Append);
                writer = new StreamWriter(stream, Encoding.Default);
                writer.Write(content);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                writer.Close();
                stream.Close();
            }
        }

        /// <summary>
        /// 使用GZip压缩算法写内容到文件中，如果文件不存在，将创建该文件。如果文件存在，则覆盖它。
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="data">要写的数据</param>
        public static void SaveGZipFile(string filename, string data)
        {
            FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write);
            GZipStream compressionStream = new GZipStream(fileStream, CompressionMode.Compress);
            StreamWriter writer = new StreamWriter(compressionStream);
            writer.Write(data);
            writer.Close();
        }


        /// <summary>
        /// 获取使用GZip算法压缩后的文件的内容。如果文件不存在，则抛出<c>FileNotFoundException</c>异常。
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string LoadGZipFile(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("文件不存在");
            }
            FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            GZipStream compressionStream = new GZipStream(fileStream, CompressionMode.Decompress);
            StreamReader reader = new StreamReader(compressionStream);
            string data = reader.ReadToEnd();
            reader.Close();
            return data;
        }

        /// <summary>
        /// 使用Deflate压缩算法写内容到文件中，如果文件不存在，将创建该文件。如果文件存在，则覆盖它。
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="data"></param>
        public static void SaveDeflateFile(string filename,string data)
        {
            FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write);
            DeflateStream compressionStream = new DeflateStream(fileStream, CompressionMode.Compress);
            StreamWriter writer = new StreamWriter(compressionStream);
            writer.Write(data);
            writer.Close();
        }

        /// <summary>
        /// 获取使用Deflate算法压缩后的文件的内容。如果文件不存在，则抛出<c>FileNotFoundException</c>异常。
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string LoadDeflateFile(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("文件不存在");
            }
            FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            DeflateStream compressionStream = new DeflateStream(fileStream, CompressionMode.Decompress);
            StreamReader reader = new StreamReader(compressionStream);
            string data = reader.ReadToEnd();
            reader.Close();
            return data;
        }
    }
}
