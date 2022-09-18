using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BinaryConversion.Converters {
	/// <summary>
	/// The converter used for serializing <see cref="Nullable{T}"/> objects.
	/// </summary>
	public class NullableBinaryConverter : BinaryConverter {
		public override bool CanRead(Type type, BinarySerializerSettings settings) {
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		public override bool CanWrite(Type type, BinarySerializerSettings settings) {
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		public override object Read(BinaryReader reader, Type returnType, BinarySerializer serializer) {
			bool isNull = !reader.ReadBoolean();
			if(isNull) return null;
			Type t = returnType.GetGenericArguments()[0];
			return serializer.FromBinary(t, reader);
		}

		public override void Write(BinaryWriter writer, Type returnType, object value, BinarySerializer serializer) {
			if(value == null) {
				writer.Write(false);
				return;
			}
			writer.Write(true);
			Type t = returnType.GetGenericArguments()[0];
			serializer.ToBinary(t, value, writer);
		}
	}
}
