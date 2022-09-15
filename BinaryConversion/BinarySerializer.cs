using System;
using System.IO;
using System.Reflection;
using BinaryConversion.Utils;

namespace BinaryConversion {
	/// <summary>
	/// A class that can write an objects data to binary, and vice-versa.
	/// </summary>
	public sealed class BinarySerializer {
		/// <summary>
		/// The configuration that this instance will use.
		/// </summary>
		public BinarySerializerSettings Settings { get; set; } = new BinarySerializerSettings();

		/// <summary>
		/// Reads from binary and converts it to an object.
		/// </summary>
		public object FromBinary(Type type, BinaryReader reader) {
			if(Settings.TypeNameHandling && !type.IsSealed && !type.IsAssignableTo(typeof(Type)) && !type.IsAssignableTo(typeof(Assembly))) {
				Type newType = FromBinary<Type>(reader);
				if(type.IsAssignableFrom(newType)) {
					type = newType;
				} else {
					throw new Exception($"Type {newType} is not assignable to type {type}.");
				}
			}
			if(type.GetInterface(nameof(IBinarySerializable)) != null) {
				object obj = Activator.CreateInstance(type);
				if(obj == null) throw new Exception($"Failed to create instance of type {type}");
				(obj as IBinarySerializable)?.Deserialize(reader, this);
				return obj;
			}
			if(Settings.GetBestReader(type, out BinaryConverter conv)) {
				return conv.Read(reader, type, this);
			} else {
				throw new ConverterNotFoundException(type, true);
			}
		}

		/// <summary>
		/// Writes an object's information to binary.
		/// </summary>
		public void ToBinary(Type type, object value, BinaryWriter writer) {
			if(Settings.TypeNameHandling && value != null) type = value.GetType();
			if(Settings.TypeNameHandling && !type.IsSealed && !type.IsAssignableTo(typeof(Type)) && !type.IsAssignableTo(typeof(Assembly))) {
				ToBinary(type, writer);
			}
			if(type.GetInterface(nameof(IBinarySerializable)) != null && value != null) {
				(value as IBinarySerializable)?.Serialize(writer, this);
				return;
			}
			if(Settings.GetBestWriter(type, out BinaryConverter conv)) {
				conv.Write(writer, type, value, this);
			} else {
				throw new ConverterNotFoundException(type, false);
			}
		}

		/// <summary>
		/// Reads from binary and converts it to an object.
		/// </summary>
		public T FromBinary<T>(BinaryReader reader) => (T)FromBinary(typeof(T), reader);
		/// <summary>
		/// Writes an object's information to binary.
		/// </summary>
		public void ToBinary<T>(T value, BinaryWriter writer) => ToBinary(typeof(T), value, writer);
	}
}