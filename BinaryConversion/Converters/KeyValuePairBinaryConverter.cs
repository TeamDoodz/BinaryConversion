using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BinaryConversion.Converters {
	/// <summary>
	/// A converter used for serializing <see cref="KeyValuePair{TKey,TValue}"/> objects.
	/// </summary>
	public sealed class KeyValuePairBinaryConverter : BinaryConverter {
		public override bool CanRead(Type type, BinarySerializerSettings settings) {
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>);
		}

		public override bool CanWrite(Type type, BinarySerializerSettings settings) {
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>);
		}

		public override object Read(BinaryReader reader, Type returnType, BinarySerializer serializer) {
			Type keyType = returnType.GenericTypeArguments[0];
			Type valueType = returnType.GenericTypeArguments[1];

			object key = serializer.FromBinary(keyType, reader);
			object value = serializer.FromBinary(valueType, reader);

			return CreateKeyValuePair(keyType, valueType, key, value);
		}

		public override void Write(BinaryWriter writer, Type returnType, object value, BinarySerializer serializer) {
			Type keyType = returnType.GenericTypeArguments[0];
			Type valueType = returnType.GenericTypeArguments[1];

			MethodInfo getKey = returnType.GetMethod("get_Key");//.MakeGenericMethod(keyType);
			MethodInfo getValue = returnType.GetMethod("get_Value");//.MakeGenericMethod(valueType);

			object key = getKey.Invoke(value, new object[] {  });
			object val = getValue.Invoke(value, new object[] {  });

			serializer.ToBinary(keyType, key, writer);
			serializer.ToBinary(valueType, val, writer);
		}

		private static object CreateKeyValuePair(Type TKey, Type TValue, object key, object value) {
			return typeof(KeyValuePair<,>)
				.MakeGenericType(TKey, TValue)
				.GetConstructor(new Type[] { TKey, TValue })
				.Invoke(new object[] { key, value });
		}
	}
}
