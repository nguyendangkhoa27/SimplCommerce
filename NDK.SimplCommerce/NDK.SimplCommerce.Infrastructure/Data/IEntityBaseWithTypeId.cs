using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NDK.SimplCommerce.Infrastructure.Data;

public abstract class IEntityBaseWithTypeId<TId>
{
    TId Id {get;}
}
