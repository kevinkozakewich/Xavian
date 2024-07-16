using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using Xavian.DataContext.Models.Template;

namespace Xavian.DataContext.Models
{
    public class Log : IDefaultBase
    {
        [Key]
        public long Id { get; set; }

        public LogLevel Level { get; set; }

        public string Class { get; set; }

        public string Message { get; set; }

        public string FurtherDetails { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public long? CreatedUserId { get; set; }

        public DateTime LastUpdatedDateTime { get; set; }

        public long? LastUpdatedUserId { get; set; }

        public long? OwnerUserId { get; set; }

        public bool Deleted { get; set; }
    }
}