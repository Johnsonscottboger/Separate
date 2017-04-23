using System;
using System.Collections.Generic;
using System.Text;

namespace Separate
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            

            var segmenter = new Segmenter.Segmenter();

            //var textList = new List<string>()
            //{
            //    "那些年那些时光",
            //    "要如何来珍藏",
            //    "那些快乐 那些伤 记得体谅",
            //    "爱未曾被遗忘 用浅浅情殇"
            //};

            var textList = new List<string>()
            {
                "繁华声 遁入空门 折煞了世人",
                "梦偏冷 辗转一生 情债又几本",
                "如你默认 生死苦等",
                "枯等一圈又一圈的年轮",
                "浮屠塔 断了几层 断了谁的魂"
            };

            foreach (var text in textList)
            {
                var segments = segmenter.Split(text, true);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(text);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[切分:] {string.Join("/", segments)}");
                Console.WriteLine();
            }



            Console.Read();
        }
    }
}