﻿using Shop.DAL.Data.Entities;

namespace Shop.BLL.Models
{
    public class UserRegisterResultModel : ServicesResultModel
    {
        public ApplicationUser User { get; set; }
    }
}
