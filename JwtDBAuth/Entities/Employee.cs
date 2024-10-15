using System.ComponentModel.DataAnnotations;

namespace JwtAuthDB.Entities
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Position { get; set; }
        public required string Company { get; set; }
    }
}
