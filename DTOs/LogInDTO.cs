﻿using System.ComponentModel.DataAnnotations;

namespace HotelsListing.DTOs
{
    public class LogInDTO
    {

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        
        public string Password { get; set; }
    }
}
