using System;

namespace Xavian.DataContext.Models.Template
{
    public interface IDefaultBase
    {
        long Id { get; set; }

        DateTime LastUpdatedDateTime { get; set; }

        long? LastUpdatedUserId { get; set; }

        long? OwnerUserId { get; set; }

        bool Deleted { get; set; }
    }
}
