using ZakupkiUtils.ftp;

namespace ZakupkiUtils.infrastructure
{
    public class ZakupkiFactory : IZakupkiFactory
    {
        public IZakupkiSettings CreateSettings()
        {
            return new FtpZakupkiSettings();
        }

        public IZakupkiFileService CreteFileService()
        {
            return new FtpZakupkiService();
        }

        public IZakupkiLocalFileService CreateLocalFileService(IZakupkiSettings settings)
        {
            return new ZakupkiLocalFileService(settings);
        }
    }
}
