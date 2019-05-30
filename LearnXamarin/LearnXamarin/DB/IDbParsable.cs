using System;
using System.Collections.Generic;
using System.Text;

namespace LearnXamarin.DB
{
    public interface IDbParsable
    {
        string TableName { get; }
        string IdFieldName { get; }
        Dictionary<string, object> Fields { get; }
    }
}
