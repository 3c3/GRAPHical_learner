using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    /// <summary>
    /// Базов клас за носител на свойства
    /// </summary>
    public class PropertyHolder
    {
        public List<Property> properties = new List<Property>();

        /// <summary>
        /// Връща името на носителя, ползва се при UiPropertyPanel
        /// </summary>
        /// <returns></returns>
        public virtual string GetName()
        {
            return "Unknown";
        }

        /// <summary>
        /// Задава или добавя свойство
        /// </summary>
        /// <param name="propertyId">Id на свойството</param>
        /// <param name="value">Стойност</param>
        public virtual void SetProperty(int propertyId, Object value)
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

        /// <summary>
        /// Връща стойността на дадено свойство
        /// </summary>
        /// <param name="propertyId">Id на свойството</param>
        /// <returns></returns>
        public Object GetPropertyValue(int propertyId)
        {
            foreach (Property p in properties) if (p.id == propertyId) return p.Value;
            return null;
        }

        public Property GetProperty(int propertyId)
        {
            foreach(Property p in properties)
            {
                if (p.id == propertyId) return p;
            }

            properties.Add(new Property(Property.EdgeWeightId, ""));
            return properties.Last();
        }

        public bool HasProperty(int propertyId)
        {
            foreach (Property p in properties) if (p.id == propertyId) return true;
            return false;
        }
    }
}
