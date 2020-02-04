namespace ZakupkiUtils.infrastructure
{
    public interface IZakupkiSettings
    {
        string GetKtruDir();
        string GetLocalKtruDir();
        string GetAppDir();
        string GetResultXlsx(bool onlyActual);
        string CreateLocalKtruDirIfNeed(out string error);
        /**
         * Это рабочая директория для временного извлечения содержимого архива ktru
         * С помощью этого метода пересоздаётся директория для извлекаемых архивов ktru
         */
        string PrepareLocalKtruArchiveDir(out string error);
        void ClearLocalKtruArchiveDir(out string error);

        string GetOkpd2Dir();
        string GetLocalOkpd2Dir();
        string CreateLocalOkpd2DirIfNeed(out string error);
        /**
         * Это рабочая директория для временного извлечения содержимого архива okpd2
         * С помощью этого метода пересоздаётся директория для извлекаемых архивов okpd2
         */
        string PrepareLocalOkpd2ArchiveDir(out string error);
        void ClearLocalOkpd2ArchiveDir(out string error);
    }
}
