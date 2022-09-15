using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryConversion.Utils {
	internal static class TypeExtensions {
		public static bool IsAssignableTo(this Type type1, Type type2) => type2.IsAssignableFrom(type1);
	}
}
