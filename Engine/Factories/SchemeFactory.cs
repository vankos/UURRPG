using Engine.Models.Items;
using System.Collections.Generic;

namespace Engine.Factories
{
    public static class SchemeFactory
    {
        private static readonly List<Scheme> _schemes = new List<Scheme>();

        static SchemeFactory()
        {
            Scheme SmallRedSyringe = new Scheme(1, "Small red syringe");
            SmallRedSyringe.AddIngredient(3, 1);
            SmallRedSyringe.AddIngredient(4, 1);
            SmallRedSyringe.AddOutputItem(5, 1);

            _schemes.Add(SmallRedSyringe);
        }

        public static Scheme GetSchemeById(int id) => _schemes.Find(x => x.ID == id);
    }
}
