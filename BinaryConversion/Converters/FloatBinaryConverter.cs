using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BinaryConversion.Converters {
	/// <summary>
	/// The converter used for serializing <see cref="Single"/>s and <see cref="Double"/>s.
	/// </summary>
	public sealed class FloatBinaryConverter : BinaryConverter {
		static Type[] floatTypes = new Type[] {
			typeof(float),
			typeof(double),
		};

		public override bool CanRead(Type type, BinarySerializerSettings settings) {
			return floatTypes.Contains(type);
		}

		public override bool CanWrite(Type type, BinarySerializerSettings settings) {
			return floatTypes.Contains(type);
		}

		public override object Read(BinaryReader reader, Type returnType, BinarySerializer serializer) {
			if(returnType == typeof(float)) return reader.ReadSingle();
			if(returnType == typeof(double)) return reader.ReadDouble();
			throw new Exception($"Type {returnType} is not a float.");
		}

		public override void Write(BinaryWriter writer, Type returnType, object value, BinarySerializer serializer) {
			if(returnType == typeof(float)) writer.Write((float)(value ?? 0f));
			if(returnType == typeof(double)) writer.Write((double)(value ?? 0d));
		}
	}
}
