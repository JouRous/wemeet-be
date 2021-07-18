using System;
using Domain.Types;

namespace Domain.Models
{
    public class UserFilterModel
    {
        public string fullname { get; set; } = "";
        public string role { get; set; } = "";
        public Guid Team { get; set; } = Guid.Empty;
    }
}