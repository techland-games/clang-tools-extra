using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace LLVM.ClangTidy
{
    public class FilterInfo
    {
        [YamlAlias("Pattern")]
        public string Pattern { get; set; }

        [YamlAlias("Replacement")]
        public string Replacement { get; set; }

        [YamlAlias("Multiline")]
        public bool Multiline { get; set; } = false;
    }

    /// <summary>
    /// Reads the list of output window regex filters from Yaml
    /// There are basic filters defined in this C# solution and optional filters.
    /// Optional filters are searched for upwards in file system hierarchy starting
    /// from folder where currently validated source file is placed.
    /// </summary>
    public static class OutputFilterDatabase
    {
        static List<FilterInfo> BasicFilters = new List<FilterInfo>();
        static string FiltersFileName = ".clang-tidy-vsfilters";

        class FilterRoot
        {
            [YamlAlias("Filters")]
            public FilterInfo[] Filters { get; set; }
        }

        static OutputFilterDatabase()
        {
            string basicConfigPath = Path.Combine(Utility.GetVsixInstallPath(), "Resources", FiltersFileName);
            if (File.Exists(basicConfigPath))
                ReadConfigFile(basicConfigPath, ref BasicFilters);
        }

        /// <summary>
        /// Returns filters valid for source file currently validated by clang-tidy
        /// </summary>
        public static IEnumerable<FilterInfo> GetFilters(string validatedFilePath)
        {
            var customFilters = BasicFilters;

            ReadCustomConfigFiles(validatedFilePath, ref customFilters);

            return customFilters;
        }

        private static void ReadCustomConfigFiles(string validatedFilePath, ref List<FilterInfo> filters)
        {
            foreach (string P in Utility.SplitPath(validatedFilePath))
            {
                string configFile = Path.Combine(P, FiltersFileName);
                if (!File.Exists(configFile))
                    continue;

                ReadConfigFile(configFile, ref filters);
            }
        }

        private static void ReadConfigFile(string filePath, ref List<FilterInfo> filters)
        {
            using (StreamReader Reader = new StreamReader(filePath))
            {
                var D = new Deserializer(namingConvention: new PascalCaseNamingConvention());
                var Root = D.Deserialize<FilterRoot>(Reader);

                foreach (var filter in Root.Filters)
                {
                    if (filter.Replacement == null)
                        filter.Replacement = "";
                }

                filters.AddRange(Root.Filters);
            }
        }
    }
}
