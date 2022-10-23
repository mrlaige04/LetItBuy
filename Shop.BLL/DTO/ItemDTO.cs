﻿using Shop.DAL.Data.Entities;

namespace Shop.BLL.DTO
{
    public class ItemDTO
    {
        public Guid ID { get; set; }
        public UserDTO OwnerUser { get; set; }
        public ICollection<ItemPhoto> Photos { get; set; } = null!;
        public string Name { get; set; }
        public string? Description { get; set; }
        
        public Guid? CategoryID { get; set; }
        public decimal Price { get; set; }
        public Currency Currency { get; set; }
        public string CategoryName { get; set; }

        public bool IsNew { get; set; }
    }
}