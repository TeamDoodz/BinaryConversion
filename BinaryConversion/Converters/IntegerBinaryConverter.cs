using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BinaryConversion.Converters {
	/// <summary>
	/// A converter for serializing all C# integer types.
	/// </summary>
	public sealed class IntegerBinaryConverter : BinaryConverter {
		static Type[] intTypes = new Type[] { 
			typeof(byte),
			typeof(sbyte),
			typeof(short),
			typeof(ushort),
			typeof(int),
			typeof(uint),
			typeof(long),
			typeof(ulong),
		};

		public override bool CanRead(Type type, BinarySerializerSettings settings) {
			return type.IsEnum || intTypes.Contains(type);
		}

		public override bool CanWrite(Type type, BinarySerializerSettings settings) {
			return type.IsEnum || intTypes.Contains(type);
		}

		private static bool IsType<T>(Type type) {
			return type == typeof(T) || (type.IsEnum && type.GetEnumUnderlyingType() == typeof(T));
		}

		public override object Read(BinaryReader reader, Type returnType, BinarySerializer serializer) {
			if(IsType<byte>(returnType)) return reader.ReadByte();
			if(IsType<sbyte>(returnType)) return reader.ReadSByte();
			if(IsType<short>(returnType)) return reader.ReadInt16();
			if(IsType<ushort>(returnType)) return reader.ReadUInt16();
			if(IsType<int>(returnType)) return reader.ReadInt32();
			if(IsType<uint>(returnType)) return reader.ReadUInt32();
			if(IsType<long>(returnType)) return reader.ReadInt64();
			if(IsType<ulong>(returnType)) return reader.ReadUInt64();
			throw new Exception($"Type {returnType} is not an integer.");
		}

		public override void Write(BinaryWriter writer, Type returnType, object value, BinarySerializer serializer) {
			if(IsType<byte>(returnType)) writer.Write((byte)(value ?? 0));
			if(IsType<sbyte>(returnType)) writer.Write((sbyte)(value ?? 0));
			if(IsType<short>(returnType)) writer.Write((short)(value ?? 0));
			if(IsType<ushort>(returnType)) writer.Write((ushort)(value ?? 0));
			if(IsType<int>(returnType)) writer.Write((int)(value ?? 0));
			if(IsType<uint>(returnType)) writer.Write((uint)(value ?? 0));
			if(IsType<long>(returnType)) writer.Write((long)(value ?? 0));
			if(IsType<ulong>(returnType)) writer.Write((ulong)(value ?? 0));
		}
	}
}
