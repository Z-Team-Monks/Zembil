using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Zembil.Views;

namespace Zembil.ValidationAttributes
{
    public class ValidProdcutCategory : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var product = (ProductCreateDto)validationContext.ObjectInstance;

            //to be fetched from db
            string[] categories = { "Electronics", "Cosmotics" };

            string[] conditions = { "Used", "New" };

            if (!categories.Any(category => category == product.Category)) return new ValidationResult("Invalid Category");

            if (!conditions.Any(condition => condition == product.Condition)) return new ValidationResult("Condition must be either Used or New");

            return ValidationResult.Success;
        }
    }
}
