using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Core;

namespace AppMetadataAPIServer.Query
{
    public class QueryContext
    {
        public IList<QueryTerm> ANDClause { get; set; } = new List<QueryTerm>();

        public override string ToString()
        {
            IEnumerable<string> andClause = this.ANDClause.Select(_ => _.ToString());
            return $"AND({String.Join(", ", andClause)})";
        }
    }
   
}