using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NDK.SimplCommerce.Infrastructure.Data;

public class EntityBaseWithTypeId<TId> : IEntityBaseWithTypeId<TId>
{
    public virtual TId Id {get; protected set;}
}
