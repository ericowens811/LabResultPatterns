using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using QTB3.Model.Abstractions;

namespace QTB3.Model.LabResultPatterns.UnitsOfMeasure
{
    public class Uom : IEntity
    {
        private int _id;
        private string _name;
        private string _description;

        public Uom() : this(0, string.Empty, string.Empty)
        {
        }

        [JsonConstructor]
        public Uom
        (
            int id,
            string name,
            string description
        )
        {
            _id = id;
            _name = name;
            _description = description;
        }

        public int Id => _id;

        [Required]
        [StringLength(30, MinimumLength = 1)]
        public string Name => _name;

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Description => _description;

        // good introduction to immutability and the'with' methods:
        // https://blogs.msdn.microsoft.com/dotnet/2013/09/25/immutable-collections-ready-for-prime-time/
        public Uom WithId(int id)
        {
            return id == _id
                ? this
                : new Uom(id, _name, _description);
        }

        public Uom WithName(string name)
        {
            return name == _name
                ? this
                : new Uom(_id, name, _description);
        }

        public Uom WithDescription(string description)
        {
            return description == _description
                ? this
                : new Uom(_id, _name, description);
        }

        // raison d'etre: https://msdn.microsoft.com/en-us/magazine/dn166926.aspx
        // the generic write repository will call this method on every entity before adding it to the context
        // Since, Uom has no EF navigation properties, the implementation here is empty
        public void NullifyNavProps()
        {
        }
    }
}
