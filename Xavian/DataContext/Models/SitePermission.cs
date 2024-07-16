using System;
using System.ComponentModel.DataAnnotations;
using Xavian.DataContext.Models.Template;

namespace Xavian.DataContext.Models
{
    public class SitePermission : IDefaultBase
    {
        [Key]
        public long Id { get; set; }

        public long SiteId { get; set; }

        public long TeamId { get; set; }

        public DateTime LastUpdatedDateTime { get; set; }
    
        public long? LastUpdatedUserId { get; set; }

        public long? OwnerUserId { get; set; }

        public bool Deleted { get; set; }
    }
}