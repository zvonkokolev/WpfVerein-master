using System.ComponentModel.DataAnnotations;
using WpfVerein.Core.Entities;
using WpfVerein.Persistence;

namespace Wpf.Persistence.Validations
{
    public class EmailDuplicateValidation : ValidationAttribute
    {
        private readonly UnitOfWork _unitOfWork;

        public EmailDuplicateValidation(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Member userToValidate = (Member)validationContext.ObjectInstance;

            if(_unitOfWork.PersonRepository.CheckIfUserEmailExists(userToValidate.Email, userToValidate.Id).Result)
            {
                return new ValidationResult("The given email is allready in use!");
            }

            return ValidationResult.Success;
        }
    }
}
