namespace Ktru.infrastructure
{
    interface IZakupkiSettings
    {
        string GetKtruDir();
        string GetLocalKtruDir();
        string GetResultDir();
        string GetResultXlsx(bool onlyActual);
        string CreateLocalKtruDirIfNeed(out string error);
        /**
         * Это рабочая директория для временного извлечения содержимого архива
         */
        string PrepareLocalArchiveDir(out string error);
        void ClearLocalArchiveDir(out string error);
    }
}
