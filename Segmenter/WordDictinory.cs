using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Segmenter.Common;

namespace Segmenter
{
    public class WordDictinory
    {
        private static readonly Lazy<WordDictinory> lazyObj = new Lazy<WordDictinory>();

        /// <summary>
        /// 字典路径
        /// </summary>
        private static string DictPath { get { return ConfigManager.ConfigManager.DictFileName; } }

        private IDictionary<string, int> _trie = new Dictionary<string, int>();
        /// <summary>
        /// Trie树
        /// </summary>
        internal IDictionary<string, int> Trie { get { return _trie; } set { _trie = value; } }

        /// <summary>
        /// 所有词的概率
        /// </summary>
        public double Total { get; set; }

        public WordDictinory()
        {
            LoadDict();
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        public static WordDictinory Instance
        {
            get { return lazyObj.Value; }
            //get { return new WordDictinory(); }
        }

        /// <summary>
        /// 加载字典，并初始化Trie树
        /// </summary>
        private void LoadDict()
        {
            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                using (var fs = File.Open(DictPath, FileMode.Open))
                {
                    using (var reader = new StreamReader(fs))
                    {
                        var currentLine = string.Empty;
                        string[] tokens;
                        //词
                        string word = string.Empty;
                        //频率
                        int freq = 0;

                        while (!reader.EndOfStream)
                        {
                            currentLine = reader.ReadLine();
                            tokens = currentLine.Split(' ');
                            if (tokens.Length < 2)
                            {
                                Debug.Fail($"无效的行:{currentLine}");
                                continue;
                            }
                            word = tokens[0];
                            freq = int.Parse(tokens[1]);

                            this.Trie[word] = freq;
                            this.Total += freq;

                            foreach (var substring in word.Substrings())
                            {
                                if (!this.Trie.ContainsKey(substring))
                                {
                                    this.Trie[substring] = 0;
                                }
                            }
                        }
                    }
                }

                stopWatch.Stop();
                Debug.WriteLine($"字典加载用时:{stopWatch.ElapsedMilliseconds}毫秒");
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
