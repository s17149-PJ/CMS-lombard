using Lombard_00.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lombard_00.Controllers.Tranzit
{
    public class TagToken
    {
        public TagToken(TTag tag) {
            Id = tag.Id;
            Name = tag.Name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
