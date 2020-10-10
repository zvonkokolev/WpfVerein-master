using System;
using System.ComponentModel.DataAnnotations;

namespace WpfVerein.Core.Entities
{
    public class Member : EntityObject
    {
        [MaxLength(50)]
        public string Firstname { get; set; }
        [MaxLength(50)]
        public string Lastname { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(30)]
        public string Phone { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ActualDateTime { get; set; }

        public string Name => Firstname + " " + Lastname;
    }
}
