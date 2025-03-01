﻿using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,32}", ErrorMessage = "Use more complex passwoard")]
    public string Password { get; set; }

    [Required]
    public string DisplayName { get; set; }

    public string UserName { get; set; }
}
