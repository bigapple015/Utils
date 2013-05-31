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
            m_strSavedDir = Directory.GetCurrentDirectory();
            RetryDirectory.Default.SetCurrentDirectory(s);
        }

        void IDisposable.Dispose()
        {
            if (m_strSavedDir != null)
            {
                RetryDirectory.Default.SetCurrentDirectory(m_strSavedDir);
                m_strSavedDir = null;
            }
        }

        private string m_strSavedDir;
    }
}
