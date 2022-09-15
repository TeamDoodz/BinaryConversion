using System;
using System.Collections.Generic;
using System.IO;

namespace BinaryConversion {
	public abstract class BinaryConverter {
		public abstract bool CanRead(Type type, BinarySerializerSettings settings);
		public abstract bool CanWrite(Type type, BinarySerializerSettings settings);

		public abstract object Read(BinaryReader reader, Type returnType, BinarySerializer serializer);
		public abstract void Write(BinaryWriter writer, Type returnType, object value, BinarySerializer serializer);
	}
}
