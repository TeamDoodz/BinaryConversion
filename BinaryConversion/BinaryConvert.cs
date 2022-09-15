using System;
using System.Collections.Generic;
using System.IO;

namespace BinaryConversion {
	/// <summary>
	/// Helper class that simplifies the <see cref="BinarySerializer"/> class.
	/// </summary>
	public static class BinaryConvert {
		private static readonly BinarySerializer serializer = new BinarySerializer();
		/// <summary>
		/// The <see cref="BinarySerializer"/> instance that this class uses.
		/// </summary>
		public static BinarySerializer Serializer => serializer;

		/// <summary>
		/// Reads from binary and converts it to an object.
		/// </summary>
		public static T FromBinary<T>(BinaryReader reader) => Serializer.FromBinary<T>(reader);
		/// <summary>
		/// Writes an object's information to binary.
		/// </summary>
		public static void ToBinary<T>(T value, BinaryWriter writer) => Serializer.ToBinary(value, writer);

		/// <summary>
		/// Reads from binary and converts it to an object.
		/// </summary>
		public static object FromBinary(Type type, BinaryReader reader) => Serializer.FromBinary(type, reader);
		/// <summary>
		/// Writes an object's information to binary.
		/// </summary>
		public static void ToBinary(Type type, object value, BinaryWriter writer) => Serializer.ToBinary(type, value, writer);

		/// <summary>
		/// Writes an object's information to a file.
		/// </summary>
		public static void SaveToFile<T>(T value, string path) {
			using(BinaryWriter bw = new BinaryWriter(File.OpenWrite(path))) {
				ToBinary<T>(value, bw);
			}
		}

		/// <summary>
		/// Reads from a file and converts it's data to an object.
		/// </summary>
		public static T LoadFromFile<T>(string path) {
			using(BinaryReader bw = new BinaryReader(File.OpenRead(path))) {
				return FromBinary<T>(bw);
			}
		}
	}
}
