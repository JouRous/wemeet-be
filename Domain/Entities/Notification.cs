using System;
using Domain.Enums;

namespace Domain.Entities
{
    public class Notification : Bases
    {
        public virtual EntityEnum EntityType { get; set; }
        public virtual int EntityId { get; set; }
        public virtual string EndpointDetails { get; set; }
        public virtual string Message { get; set; }
        public virtual bool IsRead { get; set; } = false;
        public virtual ScopeType Scope { get; set; } = ScopeType.System;
        public virtual int? ScopeId { get; set; } = null;

    }
}