using Xavian.DataContext.Models.Template;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System;

namespace Xavian.DataContext.Models
{
    public class User : IDefaultBase
    {
        [Key]
        public long Id { get; set; }

        [JsonIgnore]
        public string GoogleAuthenticationId { get; set; }

        [JsonIgnore]
        public string MicrosoftAuthenticationId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        [JsonIgnore]
        public bool WebAccess { get; set; }

        [JsonIgnore]
        public bool DbAdmin { get; set; }

        public DateTime? LastLoggedInDateTime { get; set; }

        public DateTime LastUpdatedDateTime { get; set; }

        public long? LastUpdatedUserId { get; set; }

        public long? OwnerUserId { get; set; }

        public bool Deleted { get; set; }
    }
}