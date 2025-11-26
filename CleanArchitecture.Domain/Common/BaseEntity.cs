using CleanArchitecture.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Common
{
    public abstract class BaseEntity : IAuditable
    {
        public bool IsDeleted { get; set; }

        public int Id { get; init; }

        public DateTime CreatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public DateTime ModifiedAt { get; set; }


        public string CreatedBy { get; set; }
        public string DeletedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
