using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Data;

namespace 表达式计算测试
{
    class Program
    {
        static void Main(string[] args)
        {
            Object obj = Eval("1+2+3*4");
            Console.WriteLine(obj);
        }

        public static object Eval(string cCharpCode)
        {
            object obj = new DataTable().Compute(cCharpCode, null);
            return obj;
        }

    }
}
