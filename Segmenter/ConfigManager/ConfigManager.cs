using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Segmenter.ConfigManager
{
    public class ConfigManager
    {
        private static string _configFileDir = "Resources";
        private static string _dictFileName = "dict.txt";

        public static string ConfigFileBasePath
        {
            get
            {
                if(!Path.IsPathRooted(_configFileDir))
                {
                    var currentDomainPath = AppContext.BaseDirectory;
                    _configFileDir = Path.GetFullPath(Path.Combine(currentDomainPath, _configFileDir));
                }
                return _configFileDir;
            }
        }

        /// <summary>
        /// 字典文件路径
        /// </summary>
        public static string DictFileName
        {
            get { return Path.Combine(ConfigFileBasePath, _dictFileName); }
        }
    }
}
