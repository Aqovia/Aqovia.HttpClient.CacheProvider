using System;
using System.Collections.Generic;
using System.Text;

namespace Aqovia.HttpClient.CacheProvider
{
    public interface IModelQuery<in TModel, out TResult>
    {
        TResult Execute(TModel model);
    }
}
