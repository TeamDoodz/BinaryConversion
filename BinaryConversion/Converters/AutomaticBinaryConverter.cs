using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BinaryConversion.Converters {
	/// <summary>
	/// The fallback converter that should be used when no other converters are available for a given type. This class isn't super efficent, and if speed is an issue you should write your own converters for larger types.
	/// </summary>
	public sealed class AutomaticBinaryConverter : BinaryConverter {
		public override bool CanRead(Type type, BinarySerializerSettings settings) {
			return !type.IsGenericType && (!settings.AutoRequiresAttribute || type.GetCustomAttribute<BinaryObjectAttribute>() != null);
		}

		public override bool CanWrite(Type type, BinarySerializerSettings settings) {
			return !type.IsGenericType && (!settings.AutoRequiresAttribute || type.GetCustomAttribute<BinaryObjectAttribute>() != null);
		}

		public override object Read(BinaryReader reader, Type returnType, BinarySerializer serializer) {
			object outp = Activator.CreateInstance(returnType);

			string[] keys = new string[reader.ReadInt32()];
			for(int i = 0; i < keys.Length; i++) {
				keys[i] = reader.ReadString();
			}
			object[] values = new object[keys.Length];
			for(int i = 0; i < values.Length; i++) {
				if(keys[i].StartsWith("<") && keys[i].Contains("__BackingField")) throw new Exception("Cannot deserialize compiler-generated property backing fields.");
				Type fieldType = serializer.Settings.SerializePrivateFields ?
					returnType.GetField(keys[i], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.FieldType :
					returnType.GetField(keys[i], BindingFlags.Public | BindingFlags.Instance)?.FieldType;
				if(fieldType == null) throw new Exception($"Field \"{keys[i]}\" does not exist{(serializer.Settings.SerializePrivateFields ? "" : " or is private")}.");
				values[i] = serializer.FromBinary(fieldType, reader);
			}

			for(int i = 0; i < keys.Length; i++) {
				string key = keys[i];
				object value = values[i];

				FieldInfo field = returnType.GetField(key, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				field.SetValue(outp, value);
			}

			return outp;
		}

		public override void Write(BinaryWriter writer, Type returnType, object value, BinarySerializer serializer) {
			FieldInfo[] fields = serializer.Settings.SerializePrivateFields ? 
				returnType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) : 
				returnType.GetFields(BindingFlags.Public | BindingFlags.Instance);

			fields = fields.Where((x) => x.GetCustomAttribute<CompilerGeneratedAttribute>() == null).ToArray();

			writer.Write(fields.Length);
			for(int i = 0; i < fields.Length; i++) {
				writer.Write(fields[i].Name);
			}
			for(int i = 0; i < fields.Length; i++) {
				serializer.ToBinary(fields[i].FieldType, fields[i].GetValue(value), writer);
			}
		}
	}
}
