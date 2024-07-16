using System;
using System.ComponentModel.DataAnnotations;
using Xavian.DataContext.Models.Template;

namespace Xavian.DataContext.Models
{ 
    public class Site : IDefaultBase
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public long? TerritoryId { get; set; }

        public DateTime LastUpdatedDateTime { get; set; }

        public long? LastUpdatedUserId { get; set; }

        public long? OwnerUserId { get; set; }

        public bool Deleted { get; set; }
    }
}