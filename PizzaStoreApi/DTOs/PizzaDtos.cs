using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PizzaStoreApi.DTOs
{
    // Used for returning Pizza data, including nested Toppings
    public class PizzaDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ToppingDto> Toppings { get; set; } = new List<ToppingDto>();
    }

    // Used for creating or updating a Pizza using an array of Topping IDs
    public class PizzaCreateUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        public List<int> ToppingIds { get; set; } = new List<int>();
    }
}