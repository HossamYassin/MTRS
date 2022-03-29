using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTRS.Web.Models
{
    public class ProjectNode
    {
        public string Text { get; set; }
        public string Icon { get; set; }
        public List<ActivityNode> Nodes { get; set; }
    }

    public class ActivityNode
    {
        public string Text { get; set; }
        public string Icon { get; set; }
    }
}
