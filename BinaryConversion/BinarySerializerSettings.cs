using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using BinaryConversion.Converters;

namespace BinaryConversion {
	/// <summary>
	/// The configuration that controls a <see cref="BinarySerializer"/> instance.
	/// </summary>
	public sealed class BinarySerializerSettings {
		/// <summary>
		/// The converters that the serializer will use, in order of priority.
		/// </summary>
		public List<BinaryConverter> Converters { get; set; } = new List<BinaryConverter>() { 
			new IntegerBinaryConverter(),
			new FloatBinaryConverter(),
			new StringBinaryConverter(),
			new BooleanBinaryConverter(),
			new ArrayBinaryConverter(),
			new DateTimeBinaryConverter(),
			new ICollectionGenericBinaryConverter(),
			new KeyValuePairBinaryConverter(),
			new AssemblyBinaryConverter(),
			new TypeBinaryConverter(),
			new NullableBinaryConverter(),
			new AutomaticBinaryConverter()
		};

		/// <summary>
		/// Whether the serializer will specify the name of the type that it is converting to if the type is not sealed.
		/// This has a bit of overhead (and can cause security holes if used incorrectly), so it should only be used when necessarry.
		/// </summary>
		public bool TypeNameHandling { get; set; } = false;

		/// <summary>
		/// Whether the <see cref="BinaryObjectAttribute"/> is required for a type to be converted with the <see cref="AutomaticBinaryConverter"/>.
		/// </summary>
		public bool AutoRequiresAttribute { get; set; } = false;

		/// <summary>
		/// Whether the <see cref="AutomaticBinaryConverter"/> can serialize private fields. This does not include compiler-generated property backing fields.
		/// </summary>
		public bool SerializePrivateFields { get; set; } = false;

		/// <summary>
		/// Finds the best-suited <see cref="BinaryConverter"/> to read from the given type.
		/// </summary>
		public bool GetBestReader(Type type, out BinaryConverter best) {
			best = null;
			foreach(BinaryConverter converter in Converters) {
				if(converter.CanRead(type, this)) {
					best = converter;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Finds the best-suited <see cref="BinaryConverter"/> to write to the given type.
		/// </summary>
		public bool GetBestWriter(Type type, out BinaryConverter best) {
			best = null;
			foreach(BinaryConverter converter in Converters) {
				if(converter.CanWrite(type, this)) {
					best = converter;
					return true;
				}
			}
			return false;
		}
	}
}
