﻿using System.ComponentModel.DataAnnotations;

namespace Shop.Models.UserModels
{
    public class EditItemViewModel
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        [Range(0, long.MaxValue)]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
        [Required]
        public Guid ItemId { get; set; }
        public Catalog? ItemCatalog { get; set; }
    }
}
