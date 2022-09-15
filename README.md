# BinaryConversion

A binary serialization library for .NET

## The Problem

Imagine you're trying to save some data to a file:

```cs
public class CharacterData {
	public string Name = "John Doe";
	public int Level = 0;
}
```

You've looked at all the different serialization methods and decide to go with binary. Here is some possible code that could serialize/deserialize the `CharacterData` class:

```cs
public void Serialize(BinaryWriter writer) {
	writer.Write(this.Name);
	writer.Write(this.Level);
}
public void Deserialize(BinaryReader reader) {
	this.Name = reader.ReadString();
	this.Level = reader.ReadInt32();
}
```

This code works fine; however, after working on your code more you've added a new field to the class:

```cs
public class CharacterData {
	public string Name = "John Doe";
	public int Level = 0;

	public List<ItemInfo> Inventory = new();
}
```

Although you could add support for this field in your `Serialize`/`Deserialize` methods, you would have to write serializers for the `ItemInfo` type, as well as any other types it may have as fields, any types those types may have, and so on.
As you add more and more fields to your `CharacterData` class, writing serializers for each and every one of them can get very time consuming.

## The Solution

BinaryConversion allows you to easily automate all of your binary serialization. For example, if you wanted to save/load an instance of `CharacterData` to/from disk, you would only have to do the following:

```cs
CharacterData steve = new CharacterData();
steve.Name = "Steve";
steve.Level = 2;

// Save
BinaryConvert.SaveToFile(steve, "Steve.character");

steve.Level = 3;

// Load
steve = BinaryConvert.LoadFromFile<CharacterData>("Steve.character");
Console.WriteLine(steve.Level); // 2
```

## The IBinarySerializable Interface

`IBinarySerializable` allows classes to define their own custom methods for serialization. For example, here is how the `CharacterData` class could implement this interface:

```cs
public class CharacterData : IBinarySerializable {
	public string Name = "John Doe";
	public int Level = 0;

	public List<ItemInfo> Inventory = new();

	public void Serialize(BinaryWriter writer, BinarySerializer serializer) {
		writer.Write(this.Name);
		writer.Write(this.Level);
		
		serializer.ToBinary(this.Inventory, writer); // Allow the library to automatically convert the Inventory collection.
	}
	public void Deserialize(BinaryReader reader, BinarySerializer serializer) {
		this.Name = reader.ReadString();
		this.Level = reader.ReadInt32();

		this.Inventory = serializer.FromBinary<List<ItemInfo>>(reader);
	}
}
```

## Custom Converters

Although `IBinarySerializable` is easy to use, it has some downsides:

1. You have no control over how an object is created before it is deserialized.
2. It can only be used with classes; structs and interfaces cannot implement it.
3. It cannot be used in external classes that you do not have control over.
4. Classes that implement the interface gain a dependency to the library, and won't compile without it.

If any of these downsides are a detriment, you can use a custom converter instead. Here is the previous example using `IBinarySerializable`, adapted into a converter:

```cs
public class CharacterDataBinaryConverter : BinaryConverter {
	public override bool CanRead(Type type, BinarySerializerSettings settings) => type == typeof(CharacterDataBinaryConverter);
	public override bool CanWrite(Type type, BinarySerializerSettings settings) => type == typeof(CharacterDataBinaryConverter);
	
	public override void Write(BinaryWriter writer, Type returnType, object? value, BinarySerializer serializer) {
		CharacterData character = (value as CharacterData) ?? throw new Exception("Value is not a CharacterData.");
		
		writer.Write(character.Name);
		writer.Write(character.Level);
		
		serializer.ToBinary(character.Inventory, writer); // Allow the library to automatically convert the Inventory collection.
	}
	public override object Reader(BinaryReader reader, Type returnType, BinarySerializer serializer) {
		CharacterData character = new CharacterData();

		character.Name = reader.ReadString();
		character.Level = reader.ReadInt32();

		character.Inventory = serializer.FromBinary<List<ItemInfo>>(reader);
	}
}
```