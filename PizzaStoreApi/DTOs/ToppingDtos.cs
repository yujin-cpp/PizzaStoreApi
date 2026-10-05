using System.ComponentModel.DataAnnotations;

namespace PizzaStoreApi.DTOs
{
    // Used for returning data to the client
    public class ToppingDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    // Used for receiving data from the client (Create/Update)
    public class ToppingCreateUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}