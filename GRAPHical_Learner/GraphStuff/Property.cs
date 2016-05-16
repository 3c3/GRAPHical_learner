using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    public enum SpecialProperty
    {
        Used = 0, EdgeWeight, Color, Visible
    };

    /// <summary>
    /// Свойство на връх или ребро
    /// </summary>
    public class Property
    {
        private static Dictionary<string, int> propertyCode = new Dictionary<string, int>(); // списък на регистрирани свойства
        private static List<string> propertyName = new List<string>(); // и техните имена
        private static int idCounter = 0;

        //специални свойства
        private static int usedId = -1; 
        private static int edgeWeightId = -1;
        private static int colorId = -1;
        private static int visibleId = -1;

        public static int UsedId
        {
            get { return usedId; }
        }

        public static int EdgeWeightId
        {
            get { return edgeWeightId; }
        }

        public static int ColorId
        {
            get { return colorId; }
        }

        public static int VisibleId
        {
            get { return visibleId; }
        }

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

        public static void SetSpecialProperty(int propId, SpecialProperty type)
        {
            switch(type)
            {
                case SpecialProperty.Used:
                    usedId = propId;
                    //Console.WriteLine("usedId: {0}", usedId);
                    break;
                case SpecialProperty.EdgeWeight:
                    edgeWeightId = propId;
                    break;
                case SpecialProperty.Color:
                    colorId = propId;
                    break;
                case SpecialProperty.Visible:
                    visibleId = propId;
                    break;
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
