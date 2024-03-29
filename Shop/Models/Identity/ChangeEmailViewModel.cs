﻿using System.ComponentModel.DataAnnotations;

namespace Shop.UI.Models.Identity
{
    public class ChangeEmailViewModel
    {
        [Required]
        [EmailAddress]
        public string OldEmail { get; set; }

        [Required]
        [EmailAddress]
        public string NewEmail { get; set; }


    }
}
