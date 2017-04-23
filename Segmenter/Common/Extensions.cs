using System;
using System.Collections.Generic;
using System.Text;

namespace Segmenter.Common
{
    /// <summary>
    /// 扩展方法类
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// 获取以字符串首个字符开头，长度依次递增的子字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns>以字符串首个字符开头，长度依次递增的子字符串</returns>
        public static IEnumerable<string> Substrings(this string str)
        {
            if (string.IsNullOrEmpty(str))
                yield break;
            int index = 0;
            for(;index<str.Length;index++)
            {
                yield return str.Substring(0, index);
            }
        }

        /// <summary>
        /// 获取子字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="endIndex">结束索引</param>
        /// <returns></returns>
        public static string Substring(this string str,int startIndex,int endIndex)
        {
            if (endIndex < str.Length)
                throw new ArgumentException("索引超出字符串的长度");
            return str.Substring(startIndex, endIndex - startIndex);
        }
    }
}
