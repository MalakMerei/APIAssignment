using System.ComponentModel.DataAnnotations;

namespace WebAssignment.Models
{
    public class Source
    {
        [Key]
        public string Name { get; set; }
        public string targetedValue { get; set; }
        public string apiURL { get; set; }
    }
}
