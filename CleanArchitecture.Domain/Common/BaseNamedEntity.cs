using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Domain.Common
{
    public abstract class BaseNamedEntity : BaseEntity
    {
        public string Name { get; init; }
    }
}
