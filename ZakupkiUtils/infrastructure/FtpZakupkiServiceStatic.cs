using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZakupkiUtils.infrastructure
{
    public class FtpZakupkiServiceStatic
    {
        public const string OKPD_FTP_URL = "ftp://ftp.zakupki.gov.ru/fcs_nsi/nsiOKPD";
        public const string OKPD2_FTP_URL = "ftp://ftp.zakupki.gov.ru/fcs_nsi/nsiOKPD2";
        public const string KTRU_FTP_URL = "ftp://ftp.zakupki.gov.ru/fcs_nsi/nsiKTRU";
        public const string ZAKUPKI_LOGIN = "free";
        public const string ZAKUPKI_PASS = "free";
        /**
         * Match Groups:
            1. object type:
                d : directory
                - : file
            2. Array[3] of permissions (rwx-)
            3. File Size
            4. Last Modified Date
            5. Last Modified Time
            6. File/Directory Name
        */
        public const string FTP_DIR_DETAILS_REGEX = @"^([d-])([rwxt-]{3}){3}\s+\d{1,}\s+.*?(\d{1,})\s+(\w+\s+\d{1,2}\s+(?:\d{4})?)(\d{1,2}:\d{2})?\s+(.+?)\s?$";

        public static IEnumerable<ZakupkiFile> GetFiles(string zakupki_dir)
        {
            IList<ZakupkiFile> result = new List<ZakupkiFile>();
            string ftpResult = string.Empty;

            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(zakupki_dir);
                request.Credentials = new NetworkCredential(ZAKUPKI_LOGIN, ZAKUPKI_PASS);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    ftpResult = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            Regex ftpDirDetailsRegex = new Regex(
                FTP_DIR_DETAILS_REGEX,
                RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            foreach (Match match in ftpDirDetailsRegex.Matches(ftpResult))
            {
                string size_str = match.Groups[3].Value;
                long size = 0;
                if (!long.TryParse(size_str, out size))
                {
                    Console.WriteLine("Can not parse file size: " + size_str);
                }
                DateTime modified = new DateTime();
                try
                {
                    DateTime date = DateTime.Parse(match.Groups[4].Value);
                    DateTime time = new DateTime();
                    string hours = match.Groups[5].Value;
                    hours.Trim();
                    if (hours != string.Empty)
                    {
                        time = DateTime.ParseExact(
                            match.Groups[5].Value,
                            "HH:mm",
                            CultureInfo.InvariantCulture);
                    }
                    modified = date;
                    modified = modified.AddHours(time.Hour);
                    modified = modified.AddMinutes(time.Minute);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                result.Add(new ZakupkiFile(
                    zakupki_dir,
                    match.Groups[6].Value,
                    modified,
                    size,
                    match.Groups[1].Value != "d"));
            }
            return result;
        }

        public static async Task DownloadFile(
            ZakupkiFile file,
            string targetLocalFile,
            Action<long> progress,
            Action<string> error)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(file.FullPath("/"));
                request.KeepAlive = true;
                request.UsePassive = true;
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(ZAKUPKI_LOGIN, ZAKUPKI_PASS);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (FileStream fileStream = File.Open(targetLocalFile, FileMode.Create))
                {
                    await CopyStream(responseStream, fileStream, progress);
                }
                File.SetLastWriteTime(targetLocalFile, file.Modified);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                error(e.Message);
                return;
            }
            error(string.Empty);
        }

        public static async Task CopyStream(Stream from, Stream to, Action<long> progress)
        {
            int buffer_size = 10240;
            byte[] buffer = new byte[buffer_size];
            long total_read = 0;
            int read;
            do
            {
                read = await from.ReadAsync(buffer, 0, buffer_size);
                await to.WriteAsync(buffer, 0, read);
                total_read += read;
                progress(total_read);
            }
            while (read > 0);
        }
    }
}
