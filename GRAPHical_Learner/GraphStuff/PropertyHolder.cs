using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    public class PropertyHolder
    {
        public List<Property> properties = new List<Property>();

        public virtual string GetName()
        {
            return "Unknown";
        }

        public void SetProperty(int propertyId, Object value)
        {
            foreach (Property p in properties)
            {
                if (p.id == propertyId)
                {
                    p.Value = value;
                    return;
                }
            }

            properties.Add(new Property(propertyId, value));
        }

        public Object GetProperty(int propertyId)
        {
            foreach (Property p in properties) if (p.id == propertyId) return p.Value;
            return null;
        }
    }
}
