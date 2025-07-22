
using System.Reflection;

namespace CleanArchitecture.Domain.Shared
{
    //IEquatable  is used to compare two instances of Enumeration<TEnum> for equality.
    /*
     implementar un patrón llamado "Smart Enum" (también conocido como "Rich Enum" o "Enumeration Class"), que combina lo mejor de los enum con la flexibilidad de clases y diccionarios.
     */
    public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
        where TEnum : Enumeration<TEnum>
    {
        private static readonly Dictionary<int, TEnum> Enumerations = CreateEnumerations();


        public string Name { get; protected init; } = string.Empty;
        public int Id { get; protected init; }

        public Enumeration(int id, string name)
        {
            Name = name;
            Id = id;
        }

        public static TEnum? FromValue(int id)
        {
            return Enumerations.TryGetValue(id, out TEnum? enumeration) ? enumeration : default;
        }
        public static TEnum? FromName(string name)
        {
            return Enumerations.Values.FirstOrDefault(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        public static List<TEnum> GetValues()
        {
            return Enumerations.Values.ToList();
        }

        public bool Equals(Enumeration<TEnum> other)
        {
            if (other is null) return false;
            return GetType() == other.GetType() && Id.Equals(other.Id);
        }
        public override bool Equals(object obj)
        {
            return obj is Enumeration<TEnum> enumeration && Equals(enumeration);
            
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
               
        private static Dictionary<int, TEnum> CreateEnumerations()
        {
            var enumerationType = typeof(TEnum);
            // Forzar ejecución del constructor estático
            //System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(enumerationType.TypeHandle);
            //obtenemos las propiedades públicas estáticas del tipo de enumeración
            var fieldsForType = enumerationType.GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy)
                .Where(fieldInfo => enumerationType.IsAssignableFrom(fieldInfo.FieldType))
                .Select(FieldInfo => (TEnum)FieldInfo.GetValue(default)!);

            //creamos un diccionario de enumeraciones con Id como clave y la instancia de enumeración como valor
            // return fieldsForType.ToDictionary(e => e.Id);

            var dictionary = new Dictionary<int, TEnum>();

            foreach (var enumValue in fieldsForType)
            {
                if (dictionary.ContainsKey(enumValue.Id))
                {
                    throw new InvalidOperationException(
                        $"Duplicate ID {enumValue.Id} found in enumeration '{enumerationType.Name}' for value '{enumValue.Name}'.");
                }

                dictionary.Add(enumValue.Id, enumValue);
            }

            return dictionary;

        }

    }

}
