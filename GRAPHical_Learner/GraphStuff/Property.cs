using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    public class Property
    {
        private static Dictionary<string, int> propertyCode = new Dictionary<string, int>();
        private static List<string> propertyName = new List<string>();
        private static int idCounter = 0;

        public static int vertexColorId;
        public static int edgeWeighId;

        public Property(int propertyId, Object value)
        {
            id = propertyId;
            Value = value;
        }

        public static int GetPropertyId(string name)
        {
            int value;
            if (propertyCode.TryGetValue(name, out value)) return value;
            else
            {
                propertyCode.Add(name, idCounter);
                propertyName.Add(name);
                idCounter++;
                return idCounter - 1;
            }
        }

        public readonly int id;
        public string Name
        {
            get { return propertyName[id]; }
        }
        public Object Value;
    }
}
