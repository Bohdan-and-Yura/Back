﻿ using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectUs.Domain.IRepositories
{
    public interface IDependencyResolver
    {
        T ResolveOrDefault<T>() where T : class;

    }
}
