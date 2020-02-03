using System;
using System.Diagnostics;

namespace ZakupkiUtils
{
    public class ZakupkiFile
    {
        public ZakupkiFile(string parentDir, string name, DateTime modified, long size, bool isFile)
        {
            ParentDir = parentDir;
            Name = name;
            Modified = modified;
            Size = size;
            IsFile = isFile;
        }
        public string ParentDir { get; private set; }
        public string Name { get; private set; }
        public DateTime Modified { get; private set; }
        public long Size { get; private set; }
        public bool IsFile { get; private set; }

        public string FullPath(string separator = "\\")
        {
            return ParentDir + separator + Name;
        }

        public bool Equals(ZakupkiFile f)
        {
            Trace.Assert(f != null);
            return Name == f.Name
                        && IsFile == f.IsFile
                        && Size == f.Size
                        && Modified == f.Modified;
        }
    }
}
