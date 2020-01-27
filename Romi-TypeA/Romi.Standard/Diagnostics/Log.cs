using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Romi.Standard.Diagnostics
{
    public static class Log
    {
        public static void Debug(string content)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] [Debug] {content}");
        }

        public static void Error(string content)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] [Error] {content}");
        }

        public static void Exception(Exception ex)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] [Exception] {ex.ToString()}");
        }
    }
}
