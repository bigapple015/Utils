using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Common.IO
{
    public class DirectoryHold : IDisposable
    {
        public DirectoryHold(string s)
        {
            m_savedDir = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(s);
        }

        void IDisposable.Dispose()
        {
            if (!string.IsNullOrWhiteSpace(m_savedDir))
            {
                Directory.SetCurrentDirectory(m_savedDir);
                m_savedDir = null;
            }
        }

        private string m_savedDir;
    }
}
