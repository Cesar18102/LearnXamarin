using System;
using System.Collections.Generic;
using System.Text;

namespace LearnXamarin.DB
{
    public interface IDbParsable
    {
        string TableName { get; }
        List<string> FieldNames { get; }
        Dictionary<string, string> Fields { get; }

    }
}
