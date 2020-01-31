using Ktru.infrastructure;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Ktru.ftp
{
    class FtpZakupkiSettings : IZakupkiSettings
    {
        public FtpZakupkiSettings()
        {}

        public string GetKtruDir()
        {
            return FtpZakupkiService.KTRU_FTP_URL;
        }

        public string GetLocalKtruDir()
        {
            FileInfo fi = new FileInfo(Assembly.GetExecutingAssembly().Location);
            Trace.Assert(fi.Exists);
            return fi.Directory.FullName + "\\ktru";
        }

        public string GetResultDir()
        {
            FileInfo fi = new FileInfo(Assembly.GetExecutingAssembly().Location);
            Trace.Assert(fi.Exists);
            return fi.Directory.FullName;
        }

        public string GetResultXlsx(bool onlyActual)
        {
            return GetResultDir() + (onlyActual ? "\\actual_ktru_result.xlsx" : "\\all_ktru_result.xlsx");
        }

        public string CreateLocalKtruDirIfNeed(out string error)
        {
            error = string.Empty;
            string result = GetLocalKtruDir();
            try
            {
                if (!Directory.Exists(result))
                {
                    Directory.CreateDirectory(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                error = e.Message;
            }
            return result;
        }

        public string PrepareLocalArchiveDir(out string error)
        {
            error = string.Empty;
            FileInfo fi = new FileInfo(Assembly.GetExecutingAssembly().Location);
            Trace.Assert(fi.Exists);
            string result = fi.Directory.FullName + "\\archive";
            try
            {
                if (Directory.Exists(result))
                {
                    Directory.Delete(result, true);
                }
                Directory.CreateDirectory(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                error = e.Message;
            }
            return result;
        }

        public void ClearLocalArchiveDir(out string error)
        {
            error = string.Empty;
            FileInfo fi = new FileInfo(Assembly.GetExecutingAssembly().Location);
            Trace.Assert(fi.Exists);
            string result = fi.Directory.FullName + "\\archive";
            try
            {
                if (Directory.Exists(result))
                {
                    Directory.Delete(result, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                error = e.Message;
            }
        }

    }
}
