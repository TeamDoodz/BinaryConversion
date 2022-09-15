using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using BinaryConversion;
using Newtonsoft.Json;

namespace BinaryTester {
	public static class Program {

		public class TestClass : IBinarySerializable {
			public int myInt = 4;
			public string myString = "The quick brown fox jumps over the lazy dog.";
			public bool myBool = false;

			public void Deserialize(BinaryReader reader, BinarySerializer serializer) {
				myInt = reader.ReadInt32();
				myString = reader.ReadString();
				myBool = reader.ReadBoolean();
			}

			public void Serialize(BinaryWriter writer, BinarySerializer serializer) {
				writer.Write(myInt);
				writer.Write(myString);
				writer.Write(myBool);
			}
		}

		public static void Main(string[] args) {
			var value = new TestClass();
			value.myInt = 8;
			value.myString = "rtyewttgs";
			value.myBool = true;

			if(File.Exists("temp.bytes")) File.Delete("temp.bytes");

			using(BinaryWriter bw = new BinaryWriter(File.OpenWrite("temp.bytes"))) {
				BinaryConvert.ToBinary<TestClass>(value, bw);
			}

			using(BinaryReader bw = new BinaryReader(File.OpenRead("temp.bytes"))) {
				TestClass obj = BinaryConvert.FromBinary<TestClass>(bw);
				Console.WriteLine(obj.myInt);
				Console.WriteLine(obj.myString);
				Console.WriteLine(obj.myBool);
			}
		}

	}
}