using System.ComponentModel.DataAnnotations;

namespace UserService.Application.Models;

public record LoginRequestModel([Required] string Username, [Required] string Password);