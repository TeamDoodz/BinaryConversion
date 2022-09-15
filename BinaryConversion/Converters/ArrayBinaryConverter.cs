using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryConversion.Converters {
	//TODO: Support non-1D arrays
	/// <summary>
	/// A converter for one-dimensional array objects.
	/// </summary>
	public class ArrayBinaryConverter : BinaryConverter {
		public override bool CanRead(Type type, BinarySerializerSettings settings) {
			return type.IsArray && type.GetArrayRank() == 1;
		}

		public override bool CanWrite(Type type, BinarySerializerSettings settings) {
			return type.IsArray && type.GetArrayRank() == 1;
		}

		public override object Read(BinaryReader reader, Type returnType, BinarySerializer serializer) {
			int length = reader.ReadInt32();
			Type elementType = returnType.GetElementType() ?? throw new Exception("GetElementType returned null. Are you sure you are deserializing an array?");

			Array outp = Array.CreateInstance(elementType, length);
			for(int i = 0; i < length; i++) {
				outp.SetValue(serializer.FromBinary(elementType, reader), i);
			}

			return outp;
		}

		public override void Write(BinaryWriter writer, Type returnType, object value, BinarySerializer serializer) {
			Array arr = (Array)(value ?? throw new Exception("Input array cannot be null."));
			Type elementType = returnType.GetElementType() ?? throw new Exception("GetElementType returned null. Are you sure you are serializing an array?");

			writer.Write(arr.Length);
			foreach(object o in arr) {
				serializer.ToBinary(elementType, o, writer);
			}
		}
	}
}
