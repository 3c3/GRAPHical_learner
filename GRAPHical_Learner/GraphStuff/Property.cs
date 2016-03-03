using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    /// <summary>
    /// Свойство на връх или ребро
    /// </summary>
    public class Property
    {
        private static Dictionary<string, int> propertyCode = new Dictionary<string, int>(); // списък на регистрирани свойства
        private static List<string> propertyName = new List<string>(); // и техните имена
        private static int idCounter = 0;

        //специални свойства
        public static int vertexColorId; 
        public static int edgeWeighId;

        public Property(int propertyId, Object value)
        {
            id = propertyId;
            Value = value;
        }

        /// <summary>
        /// Връща id на свойство по име. Ако трябва - създава.
        /// </summary>
        /// <param name="name">Името на свойството</param>
        /// <returns>Id</returns>
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
