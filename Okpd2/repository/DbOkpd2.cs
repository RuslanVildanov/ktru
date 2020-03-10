using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Okpd2.repository
{
    class DbOkpd2
    {
        [Required]
        public int Id { get; set; }
        public DbOkpd2 ParentOkpd2 { get; set; }
        [Required]
        [MaxLength(15)]
        public string Code { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Name { get; set; }
        [Required]
        public bool Actual { get; set; }
    }
}
