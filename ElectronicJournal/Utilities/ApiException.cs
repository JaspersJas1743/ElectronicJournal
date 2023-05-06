﻿using System;
using System.Runtime.Serialization;

namespace ElectronicJournal.Utilities
{
	[Serializable]
	public class ApiException : Exception
	{
		public ApiException() { }
		
		public ApiException(string message) : base(message) { }
		
		public ApiException(string message, Exception inner) : base(message, inner) { }
		
		protected ApiException(SerializationInfo info, StreamingContext context) : base(info, context) { }

		public string PropertyName { get; set; }
	}
}