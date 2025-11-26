using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Interfaces
{
    public interface IAuditable
    {
        bool IsDeleted { get; set; }

        int Id { get; init; }

        string CreatedBy { get; set; }
        string DeletedBy { get; set; }
        string ModifiedBy { get; set; }

        DateTime CreatedAt { get; set; }
        DateTime DeletedAt { get; set; }
        DateTime ModifiedAt { get; set; }
    }
}
