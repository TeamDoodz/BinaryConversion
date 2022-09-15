using System;

namespace BinaryConversion {
	public sealed class ConverterNotFoundException : Exception {
		public ConverterNotFoundException(Type convertingType, bool reading) : base($"Could not find {(reading?"reader":"writer")} for type \"{convertingType.FullName}\"") { }
	}
}
