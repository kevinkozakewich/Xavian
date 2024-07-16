using System;
using System.ComponentModel.DataAnnotations;
using Xavian.DataContext.Models.Template;

namespace Xavian.DataContext.Models
{
    public class TeamUser : IDefaultBase
    {
        [Key]
        public long Id { get; set; }

        public long TeamId { get; set; }

        public long UserId { get; set; }

        public bool Accepted { get; set; }

        public bool Manager { get; set; }

        public DateTime LastUpdatedDateTime { get; set; }

        public long? LastUpdatedUserId { get; set; }

        public long? OwnerUserId { get; set; }

        public bool Deleted { get; set; }

    }
}