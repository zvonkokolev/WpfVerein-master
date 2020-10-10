
using System.ComponentModel.DataAnnotations;
using WpfVerein.Core.Contracts;

namespace WpfVerein.Core
{
    public class EntityObject : IEntityObject
    {
        [Key]
        public int Id { get; set; }

        [Timestamp]
        public byte[] RowVersion
        {
            get;
            set;
        }
    }
}
