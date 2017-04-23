using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Segmenter
{
    public class Segmenter
    {
        private static WordDictinory WordDictionary { get { return WordDictinory.Instance; } }

        internal static Regex RegexChineseDefault { get { return new Regex(@"([\u4E00-\u9FD5a-zA-Z0-9+#&\._]+)", RegexOptions.Compiled); } }
        internal static Regex RegexSkipDefault { get { return new Regex(@"(\r\n|\s)", RegexOptions.Compiled); } }

        internal static Regex RegexChineseAll { get { return new Regex(@"([\u4E00-\u9FD5]+)", RegexOptions.Compiled); } }
        internal static Regex RegexSkipAll { get { return new Regex(@"[^a-zA-Z0-9+#\n]", RegexOptions.Compiled); } }

        internal static Regex RegexEnglishChars { get { return new Regex(@"[a-zA-Z0-9]", RegexOptions.Compiled); } }


        internal IDictionary<string,string> UserWordTagTab { get; set; }

        public Segmenter()
        {
            this.UserWordTagTab = new Dictionary<string, string>();
        }

        /// <summary>
        /// 分词
        /// </summary>
        /// <param name="text">原文本</param>
        /// <param name="cutAll">是否全切分</param>
        /// <param name="hmm"></param>
        /// <returns></returns>
        public IEnumerable<string> Split(string text,bool cutAll=false,bool hmm=true)
        {
            var regexChinese = RegexChineseDefault;
            var regexSkip = RegexSkipDefault;
            Func<string, IEnumerable<string>> SplitText = null;

            if(cutAll)
            {
                regexChinese = RegexChineseAll;
                regexSkip = RegexSkipAll;
            }

            if(cutAll)
            {
                SplitText = SplitAll;
            }
            else
            {
                throw new NotImplementedException();
            }

            return StartSplit(text, SplitText, regexChinese, regexSkip, cutAll);
        }

        /// <summary>
        /// 开始拆分
        /// </summary>
        /// <param name="text"></param>
        /// <param name="splitFunc"></param>
        /// <param name="regexChinese"></param>
        /// <param name="regexSkip"></param>
        /// <param name="cutAll"></param>
        /// <returns></returns>
        internal IEnumerable<string> StartSplit(string text,Func<string,IEnumerable<string>> splitFunc,Regex regexChinese,Regex regexSkip,bool cutAll)
        {
            var result = new List<string>();
            var blocks = regexChinese.Split(text);
            foreach(var item in blocks)
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;
                if(regexChinese.IsMatch(item))
                {
                    foreach(var word in splitFunc(item))
                    {
                        result.Add(word);
                    }
                }
                else
                {
                    var temp = regexSkip.Split(item);
                    foreach(var ch in temp)
                    {
                        if(regexSkip.IsMatch(ch))
                        {
                            result.Add(ch);
                        }
                        else if(!cutAll)
                        {
                            foreach(var c in ch)
                            {
                                result.Add(c.ToString());
                            }
                        }
                        else
                        {
                            result.Add(ch);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 全切分
        /// </summary>
        /// <param name="sentence">短句</param>
        /// <returns></returns>
        internal IEnumerable<string> SplitAll(string sentence)
        {
            var dag = GetDAG(sentence);

            var words = new List<string>();
            var lastPos = -1;
            foreach(var pair in dag)
            {
                var key = pair.Key;
                var nexts = pair.Value;
                if(nexts.Count==1&&key>lastPos)
                {
                    words.Add(sentence.Substring(key, nexts[0] + 1 - key));
                    lastPos = nexts[0];
                }
                else
                {
                    foreach(var j in nexts)
                    {
                        if(j>key)
                        {
                            words.Add(sentence.Substring(key, j + 1 - key));
                            lastPos = j;
                        }
                    }
                }
            }

            return words;
        }

        /// <summary>
        /// 生成有向无环图
        /// </summary>
        /// <param name="sentence">短句</param>
        /// <returns></returns>
        private Dictionary<int, List<int>> GetDAG(string sentence)
        {
            if (string.IsNullOrWhiteSpace(sentence))
                throw new ArgumentNullException("sentence");

            var dag = new Dictionary<int, List<int>>();
            var trie = WordDictionary.Trie;

            var length = sentence.Length;
            List<int> temp;
            for (var i = 0; i < length; i++)
            {
                temp = new List<int>();
                var current = sentence.Substring(i, 1);
                int j = i;
                //从当前汉字，向后遍历，知道短句终点
                while (j < length && trie.ContainsKey(current))
                {
                    //如果字典中存在当前汉字，并且概率大于0
                    if (trie[current] > 0)
                    {
                        temp.Add(j);
                    }

                    
                    j++;
                    if (j < length)
                    {
                        current = sentence.Substring(i, j + 1 - i);
                    }
                }
                if (temp.Count == 0)
                {
                    temp.Add(i);
                }
                dag[i] = temp;
            }
            return dag;
        }
    }
}
