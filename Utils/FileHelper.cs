using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;



namespace Com.Utility.Commons
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
                if (reader != null)
                {
                    reader.Close();
                }
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
        /// 读取所有的行
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<String> ReadAllLines(string fileName)
        {
            try
            {
                List<string> results = new List<string>();
                String[] lines = File.ReadAllLines(fileName, Encoding.GetEncoding("gb2312"));
                foreach (string line in lines)
                {
                    if(!string.IsNullOrEmpty(line) && !results.Contains(line))
                    {
                        results.Add(line);
                    }
                }
                return results;
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex.ToString());
                return null;
            }
        } 

        /// <summary>
        /// 获取文件的二进制表示
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Byte[] GetByteFromFile(string fileName)
        {
            byte[] bytes = null;
            using(FileStream stream = new FileStream(fileName,FileMode.Open,FileAccess.Read))
            {
                int count = (int) (stream.Length);
                bytes = new byte[count];
                stream.Read(bytes, 0, count);
            }
            return bytes;
        }

        /// <summary>
        /// 将一个文件复制到目标文件
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destFile"></param>
        public static void CopyFile(string sourceFile,string destFile)
        {
            File.Copy(sourceFile,destFile);
        }

        /// <summary>
        /// 递归复制目录，如果目标目录不存在，将创建，存在则删除
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="destDirectory"></param>
        public static void CopyDirectoryDeleteIfExist(string sourceDirectory,string destDirectory)
        {
            DirectoryInfo source = new DirectoryInfo(sourceDirectory);
            DirectoryInfo dest = new DirectoryInfo(destDirectory);
            if(!source.Exists)
            {
                throw new Exception("source directory does exists");
            }

            if(dest.Exists)
            {
                dest.Delete(true);
            }
            dest.Create();

            foreach(FileInfo fileInfo in source.GetFiles())
            {
                fileInfo.CopyTo(Path.Combine(dest.FullName, fileInfo.Name));
            }

            foreach (DirectoryInfo directoryInfo in source.GetDirectories())
            {
                //递归复制
                CopyDirectory(directoryInfo.FullName,destDirectory);
            }
        }

        /// <summary>
        /// 递归复制目录，如果目标目录不存在，将创建，存在则使用存在的
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="destDirectory"></param>
        public static void CopyDirectory(string sourceDirectory, string destDirectory)
        {
            DirectoryInfo source = new DirectoryInfo(sourceDirectory);
            DirectoryInfo dest = new DirectoryInfo(destDirectory);
            if (!source.Exists)
            {
                throw new Exception("source directory does exists");
            }

            if (!dest.Exists)
            {
                dest.Create();
            }
            

            foreach (FileInfo fileInfo in source.GetFiles())
            {
                if(!File.Exists(Path.Combine(dest.FullName, fileInfo.Name)))
                {
                    fileInfo.CopyTo(Path.Combine(dest.FullName, fileInfo.Name));
                }
            }

            foreach (DirectoryInfo directoryInfo in source.GetDirectories())
            {
                //递归复制
                CopyDirectory(directoryInfo.FullName, destDirectory);
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
