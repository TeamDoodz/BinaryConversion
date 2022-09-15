using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace BinaryConversion.Converters {
	/// <summary>
	/// A converter for serializing classes that implement <see cref="ICollection{T}"/>.
	/// </summary>
	public sealed class ICollectionGenericBinaryConverter : BinaryConverter {
		public override bool CanRead(Type type, BinarySerializerSettings settings) {
			return type.GetInterface("ICollection`1") != null;
		}

		public override bool CanWrite(Type type, BinarySerializerSettings settings) {
			return type.GetInterface("ICollection`1") != null;
		}

		public override object Read(BinaryReader reader, Type returnType, BinarySerializer serializer) {
			Type elementType = (returnType.GetInterface("ICollection`1") ?? throw new Exception("Type does not implement ICollection<T>.")).GenericTypeArguments[0];
			int length = reader.ReadInt32();

			object outp = Activator.CreateInstance(returnType) ?? throw new Exception("Activator.CreateInstance returned null.");
			MethodInfo add = typeof(ICollection<>).MakeGenericType(elementType).GetMethod("Add", BindingFlags.Public | BindingFlags.Instance) ?? throw new Exception("Could not find Add method on type ICollection<T>");
			for(int i = 0; i < length; i++) {
				add.Invoke(outp, new object[] { serializer.FromBinary(elementType, reader) });
			}

			return outp;
		}

		public override void Write(BinaryWriter writer, Type returnType, object value, BinarySerializer serializer) {
			Type elementType = (returnType.GetInterface("ICollection`1") ?? throw new Exception("Type does not implement ICollection<T>.")).GenericTypeArguments[0];

			MethodInfo count = typeof(ICollection<>).MakeGenericType(elementType).GetMethod("get_Count", BindingFlags.Public | BindingFlags.Instance) ?? throw new Exception("Could not find get_Count method on type ICollection<T>");
			int length = (int)count.Invoke(value, new object[] { });
			writer.Write(length);

			foreach(object item in (IEnumerable)value) {
				serializer.ToBinary(elementType, item, writer);
			}
		}
	}
}
