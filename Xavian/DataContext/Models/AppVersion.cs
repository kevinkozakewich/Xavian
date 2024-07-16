using System.ComponentModel.DataAnnotations;

namespace Xavian.DataContext.Models
{
    public class AppVersion
    {
        [Key]
        public long Id { get; set; }

        public int Version { get; set; }
    }
}