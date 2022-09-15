using System;
using System.Collections.Generic;
using System.IO;

namespace BinaryConversion.Converters {
	public sealed class StringBinaryConverter : BinaryConverter {
		public override bool CanRead(Type type, BinarySerializerSettings settings) {
			return type == typeof(string);
		}

		public override bool CanWrite(Type type, BinarySerializerSettings settings) {
			return type == typeof(string);
		}

		public override object Read(BinaryReader reader, Type returnType, BinarySerializer serializer) {
			return reader.ReadString();
		}

		public override void Write(BinaryWriter writer, Type returnType, object value, BinarySerializer serializer) {
			writer.Write((string)(value ?? string.Empty));
		}
	}
}
