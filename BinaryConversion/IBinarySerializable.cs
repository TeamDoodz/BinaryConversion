using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryConversion {
	/// <summary>
	/// Allows a class to implement its own methods for serialization.
	/// </summary>
	public interface IBinarySerializable {
		void Deserialize(BinaryReader reader, BinarySerializer serializer);
		void Serialize(BinaryWriter writer, BinarySerializer serializer);
	}
}
