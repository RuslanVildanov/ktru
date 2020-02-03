namespace Okpd2.infrastructure
{
    interface IZakupkiSettings
    {
        string GetOkpd2Dir();
        string GetLocalOkpd2Dir();
        string GetAppDir();
        string CreateLocalOkpd2DirIfNeed(out string error);
        /**
         * Это рабочая директория для временного извлечения содержимого архива
         * С помощью этого метода пересоздаётся директория для извлекаемых архивов
         */
        string PrepareLocalArchiveDir(out string error);
        void ClearLocalArchiveDir(out string error);
    }
}
