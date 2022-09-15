using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BinaryConversion.Utils;

namespace BinaryConversion.Converters {
	/// <summary>
	/// A converter for serializing references to types.
	/// </summary>
	public sealed class TypeBinaryConverter : BinaryConverter {
		public override bool CanRead(Type type, BinarySerializerSettings settings) {
			return type.IsAssignableTo(typeof(Type));
		}

		public override bool CanWrite(Type type, BinarySerializerSettings settings) {
			return type.IsAssignableTo(typeof(Type));
		}

		public override object Read(BinaryReader reader, Type returnType, BinarySerializer serializer) {
			string typeName = reader.ReadString();
			Assembly ass = serializer.FromBinary<Assembly>(reader);
			Type outp = ass.GetType(typeName);
			if(outp != null) return outp;
			throw new Exception($"Type {typeName} in assembly {ass.FullName} not found.");
		}

		public override void Write(BinaryWriter writer, Type returnType, object value, BinarySerializer serializer) {
			Type t = value as Type ?? throw new Exception("Value is not a type.");
			writer.Write(t.FullName ?? throw new Exception("This type cannot be serialized."));
			serializer.ToBinary(t.Assembly, writer);
		}
	}
}
