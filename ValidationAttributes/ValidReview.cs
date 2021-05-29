using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Zembil.Views;

namespace Zembil.ValidationAttributes
{
    public class ValidReview : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var reviewDto =  (ReviewDto)validationContext.ObjectInstance;
            if(0 >= reviewDto.Rating || reviewDto.Rating > 5)
            {
                return new ValidationResult("Rating should be between 1 and 5");
            }
            return ValidationResult.Success;
        }

    }
}
