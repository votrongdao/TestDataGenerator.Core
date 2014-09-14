﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using gk.DataGenerator.Exceptions;

namespace gk.DataGenerator
{
    public static class FileReader
    {
        public static NamedPatterns LoadNamedPatterns(string path)
        {
            NamedPatterns result;
            using (var reader = XmlReader.Create(path))
            {
                var ser = new XmlSerializer(typeof(NamedPatterns));
                result = ser.Deserialize(reader) as NamedPatterns;
            }
            return result;
        }

        public static string GetPatternFilePath(string filePath)
        {
            if (!Path.HasExtension(filePath)) filePath = Path.ChangeExtension(filePath, "tdg-patterns");

            var paths = new List<string>();
            paths.Add(filePath);

            var rooted = Path.IsPathRooted(filePath);
            if (!rooted)
            {
                // attempt to root relative path files within the current execution directory.
                paths.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath));

                // check if it is within the _PatternFolder_Name folder
                paths.Add(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tdg-patterns", filePath));
            }

            foreach (var path in paths)
            {
                if (File.Exists(path)) return path;
            }

            var msg = string.Format("Unable to find pattern file '{0}'. Searched in the following locations:\n{1}", filePath, string.Join("\n", paths));
            throw new GenerationException(msg);
        }

    }
}
