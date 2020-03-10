using System.Data.Entity;

namespace Okpd2.repository
{
    class Okpd2Context : DbContext
    {
        public Okpd2Context() : base("Okpd2")
        {}

        public DbSet<DbOkpd2> DbOkpd2s { get; set; }

    }
}
