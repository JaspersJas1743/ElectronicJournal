using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace ElectronicJournal.Utilities
{
	public static class ObjectValidator
	{
		public static bool Validate<T>(T instance, out List<ValidationResult> messages)
		{
			messages = new List<ValidationResult>();
			return Validator.TryValidateObject(
				validationContext: new ValidationContext(instance: instance),
				instance: instance, validationResults: messages, validateAllProperties: true
			);
		}

		public static bool Validate<T>(T instance)
			=> Validate(instance: instance, messages: out _);

		public static bool ValidateProperty<T1, T2>(T1 instance, T2 property, string propName, out List<ValidationResult> messages)
		{
			messages = new List<ValidationResult>();
			return Validator.TryValidateProperty(
				validationContext: new ValidationContext(instance: instance) { MemberName = propName },
				value: property, validationResults: messages
			);
		}

		public static bool ValidateProperty<T1, T2>(T1 instance, T2 property, string propName)
			=> ValidateProperty(instance: instance, property: property, propName: propName, messages: out _);
	}
}
