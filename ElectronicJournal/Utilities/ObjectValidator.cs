using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElectronicJournal.Utilities
{
	public static class ObjectValidator
	{
		public static bool Validate<T>(T instance, out List<ValidationResult> message)
		{
			message = new List<ValidationResult>();
			return Validator.TryValidateObject(
				validationContext: new ValidationContext(instance: instance),
				instance: instance, validationResults: message, validateAllProperties: true
			);
		}
	}
}
