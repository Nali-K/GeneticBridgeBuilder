using System.ComponentModel.DataAnnotations;
namespace GenenticAlgorithmBlazor.Shared
{
    public class TestText
    {



        [Required]
        [StringLength(30, ErrorMessage = "Name is too long.")]
        public string? testText { get; set; }
    
    }
}