﻿using System.ComponentModel.DataAnnotations;

namespace WatermarkAzureSample.WebApp.ViewModels
{
    public class WatermarkAddViewModel
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
            var viewModel = validationContext.ObjectInstance as WatermarkAddViewModel;

            if (viewModel == null || viewModel.ImageFile == null || !IsValidateImage(viewModel.ImageFile.FileName))
            {
                return new ValidationResult("You can only upload JPG or PNG file.");
            }
            return ValidationResult.Success;
        }

        public static bool IsValidateImage(string fileName)
        {
            return fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase);
        }
    }
}