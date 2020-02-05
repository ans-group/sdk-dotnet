using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace UKFast.API.Client.Json
{
	public class UKFastJsonConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue(value.ToString());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			serializer.NullValueHandling = NullValueHandling.Ignore;

			bool isNullable = IsNullableType(objectType);

			Type enumType = isNullable ? Nullable.GetUnderlyingType(objectType) : objectType;

			string[] names = Enum.GetNames(enumType ?? throw new InvalidOperationException());

			if (reader.TokenType == JsonToken.String)
			{
				string enumText = reader.Value.ToString();

				if (!string.IsNullOrEmpty(enumText))
				{
					string match = names.FirstOrDefault(n => string.Equals(n, enumText, StringComparison.OrdinalIgnoreCase));

					if (match != null)
					{
						return Enum.Parse(enumType, match);
					}
				}
			}
			else if (reader.TokenType == JsonToken.Integer)
			{
				int enumVal = Convert.ToInt32(reader.Value);
				int[] values = (int[]) Enum.GetValues(enumType);
				if (values.Contains(enumVal))
				{
					return Enum.Parse(enumType, enumVal.ToString());
				}
			}

			if (!isNullable)
			{
				string fallbackValue = names.FirstOrDefault(n => string.Equals(n, "Unknown", StringComparison.OrdinalIgnoreCase));

				string defaultName = fallbackValue ?? throw new JsonSerializationException("Unable to parse JSON value");

				return Enum.Parse(enumType, defaultName);
			}

			return null;
		}

		public override bool CanConvert(Type objectType)
		{

			Type type = IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;

			return type.IsEnum;
		}

		private static bool IsNullableType(Type t)
		{
			return (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
		}
	}
}
