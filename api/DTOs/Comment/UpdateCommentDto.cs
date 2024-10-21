using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Comment;

public class UpdateCommentDto
{
    [Required]
    [MinLength(5, ErrorMessage = "Title must be at least 5 characters")]
    [MaxLength(280, ErrorMessage = "Title cannot be more than 280 characters")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MinLength(5, ErrorMessage = "Title must be at least 5 characters")]
    [MaxLength(280, ErrorMessage = "Title cannot be more than 280 characters")]
    public string Content { get; set; } = string.Empty;
}
