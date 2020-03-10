using System;
using System.Data.Entity;

namespace Okpd2.repository
{
    class Okpd2Repository
    {
        public Okpd2Repository()
        {
            // Удаление и создание БД, если изменилась структура
            Database.SetInitializer(
                new DropCreateDatabaseIfModelChanges<Okpd2Context>());
            _context = new Okpd2Context();
//            _context.DbOkpd2s;
            // попытка записать структуру БД
            int result = _context.SaveChanges();
            Console.WriteLine(result);
        }

        private Okpd2Context _context;
    }
}
