using System.ComponentModel.DataAnnotations;

namespace WatermarkAzureSample.WebApp.ViewModels
{
    public class WatermarkModel
    {
        [StringLength(10, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string Text { get; set; }

        [Required]
        [FormFile]
        public IFormFile ImageFile { get; set; }
    }


    public class FormFileAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = validationContext.ObjectInstance as FormFile;

            if (file == null || !IsValidateImage(file.FileName))
            {
                return new ValidationResult("You can only upload JPG or PNG file.");
            }
            return ValidationResult.Success;
        }

        public static bool IsValidateImage(string fileName)
        {
            return fileName.EndsWith(".jpg") || fileName.EndsWith("png");
        }
    }
}
