using System;
using System.Collections.Generic;
using System.IO;

namespace BinaryConversion.Converters {
	/// <summary>
	/// A converter used for serializing <see cref="DateTime"/> objects, using the <see cref="DateTime.Ticks"/> property.
	/// </summary>
	public sealed class DateTimeBinaryConverter : BinaryConverter {
		public override bool CanRead(Type type, BinarySerializerSettings settings) {
			return type == typeof(DateTime);
		}

		public override bool CanWrite(Type type, BinarySerializerSettings settings) {
			return type == typeof(DateTime);
		}

		public override object Read(BinaryReader reader, Type returnType, BinarySerializer serializer) {
			return new DateTime(reader.ReadInt64());
		}

		public override void Write(BinaryWriter writer, Type returnType, object value, BinarySerializer serializer) {
			writer.Write(((DateTime)value).Ticks);
		}
	}
}
