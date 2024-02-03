using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispoDataAssistant.Data.Models.ServiceNow
{
    public class LifecycleMembers
    {
        public LifecycleMembers() { }
        public LifecycleMembers(List<LifecycleItem> stages, List<LifecycleItem> statuses)
        {
            Stages = stages;
            Statuses = statuses;
        }
        public List<LifecycleItem>? Statuses { get; set; }

        public List<LifecycleItem> Stages { get; set; }
    }
}
