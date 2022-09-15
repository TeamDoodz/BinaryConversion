using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BinaryConversion.Converters {
	/// <summary>
	/// A converter for serializing references to assemblies.
	/// </summary>
	public sealed class AssemblyBinaryConverter : BinaryConverter {
		public override bool CanRead(Type type, BinarySerializerSettings settings) {
			return typeof(Assembly).IsAssignableFrom(type);
		}

		public override bool CanWrite(Type type, BinarySerializerSettings settings) {
			return typeof(Assembly).IsAssignableFrom(type);
		}

		public override object Read(BinaryReader reader, Type returnType, BinarySerializer serializer) {
			string assemblyName = reader.ReadString();
			Assembly outp = Assembly.Load(assemblyName);
			return outp;
		}

		public override void Write(BinaryWriter writer, Type returnType, object value, BinarySerializer serializer) {
			writer.Write((value as Assembly).FullName);
		}
	}
}
