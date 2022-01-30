using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoAccess
{
    public class DbConifg
    {
        public const string Db = "Db";

        public string DbName { get; set; } = string.Empty;
        public string CollectionName { get; set; } = string.Empty;
    }
}
