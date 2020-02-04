namespace ZakupkiUtils.infrastructure
{
    public interface IZakupkiFactory
    {
        IZakupkiFileService CreteFileService();
        IZakupkiSettings CreateSettings();
    }
}
