using System;
using System.Collections.Generic;
using System.IO;

namespace BinaryConversion.Converters {
	/// <summary>
	/// The converter used for serializing <see cref="Boolean"/>s.
	/// </summary>
	public sealed class BooleanBinaryConverter : BinaryConverter {
		public override bool CanRead(Type type, BinarySerializerSettings settings) {
			return type == typeof(bool);
		}

		public override bool CanWrite(Type type, BinarySerializerSettings settings) {
			return type == typeof(bool);
		}

		public override object Read(BinaryReader reader, Type returnType, BinarySerializer serializer) {
			return reader.ReadBoolean();
		}

		public override void Write(BinaryWriter writer, Type returnType, object value, BinarySerializer serializer) {
			writer.Write((bool)(value ?? false));
		}
	}
}
