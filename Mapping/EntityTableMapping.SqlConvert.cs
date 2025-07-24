using System;
using System.Data;

namespace DataEntitiesMapping
{
    public partial class EntityTableMapping
    {
        /// <summary>Получение значения по имени поля из указанной
        /// записи таблицы</summary>
        /// <param name="dataRecord">Запись таблицы данных</param>
        /// <param name="fieldName">Имя поля</param>
        /// <returns>Значение ячейки как Object</returns>
#if NETCOREAPP3_0_OR_GREATER
        public static object? GetValue(DataRow dataRecord,
#elif NET35_OR_GREATER
        public static object GetValue(DataRow dataRecord,
#endif
                                      string fieldName)
        {
            if (dataRecord != null &&
                !string.IsNullOrEmpty(fieldName.Trim()) &&
                dataRecord.Table.Columns.Contains(fieldName))
            {
                return dataRecord[fieldName];
            }

            return null;
        }

#if NETCOREAPP3_0_OR_GREATER
        /// <summary>Получение строкового значения по имени поля из указанной
        /// записи таблицы</summary>
        /// <param name="dataRecord">Запись таблицы данных</param>
        /// <param name="fieldName">Имя поля</param>
        /// <param name="defaultValue">Значение по умолчанию для случая,
        /// когда получить значение не удалось</param>
        /// <returns>Значение ячейки как строка, допускающая null</returns>
        public static string? GetStringNullableValue(
            DataRow dataRecord,
            string fieldName,
            string? defaultValue = null)
        {
            string? convertedValue = defaultValue;
            object? obtainedValue = GetValue(
                dataRecord,
                fieldName);

            if (obtainedValue != null &&
                !Convert.IsDBNull(obtainedValue))
            {
                convertedValue = Convert.ToString(obtainedValue);
            }

            return convertedValue;
        }
#endif

        /// <summary>Получение строкового значения по имени поля из указанной
        /// записи таблицы</summary>
        /// <param name="dataRecord">Запись таблицы данных</param>
        /// <param name="fieldName">Имя поля</param>
        /// <param name="defaultValue">Значение по умолчанию для случая,
        /// когда получить значение не удалось</param>
        /// <returns>Значение ячейки как строка</returns>
        public static string GetStringValue(DataRow dataRecord,
                                            string fieldName,
                                            string defaultValue = "")
        {
            string convertedValue = defaultValue;
            var obtainedValue
#if NETCOREAPP3_0_OR_GREATER
                = GetStringNullableValue(
                    dataRecord,
                    fieldName,
                    defaultValue);

            if (obtainedValue != null)
            {
                convertedValue = obtainedValue;
            }
#elif NET35_OR_GREATER
                = GetValue(
                    dataRecord,
                    fieldName);

            if (obtainedValue != null &&
                !Convert.IsDBNull(obtainedValue))
            {
                convertedValue = Convert.ToString(obtainedValue);
            }
#endif

            return convertedValue;
        }

        /// <summary>Получение целочисленного значения по имени поля
        /// из указанной записи таблицы</summary>
        /// <param name="dataRecord">Запись таблицы данных</param>
        /// <param name="fieldName">Имя поля</param>
        /// <param name="defaultValue">Значение по умолчанию для случая,
        /// когда получить значение не удалось</param>
        /// <returns>Значение ячейки в виде целого числа</returns>
        public static int GetIntegerValue(DataRow dataRecord,
                                          string fieldName,
                                          int defaultValue = 0)
        {
            int convertedValue = defaultValue;
            int? obtainedValue = GetIntegerNullableValue(
                dataRecord,
                fieldName);

            if (obtainedValue.HasValue)
            {
                convertedValue = obtainedValue.Value;
            }

            return convertedValue;
        }

        /// <summary>Получение целочисленного значения по имени поля
        /// из указанной записи таблицы</summary>
        /// <param name="dataRecord">Запись таблицы данных</param>
        /// <param name="fieldName">Имя поля</param>
        /// <param name="defaultValue">Значение по умолчанию для случая,
        /// когда получить значение не удалось</param>
        /// <returns>Значение ячейки в виде целого числа, допускающего null
        /// </returns>
        public static int? GetIntegerNullableValue(DataRow dataRecord,
                                                   string fieldName,
                                                   int? defaultValue = null)
        {
            int? convertedValue = defaultValue;
            string obtainedString = GetStringValue(
                dataRecord,
                fieldName);

            if (int.TryParse(
                obtainedString,
                out int tryParseResult))
            {
                convertedValue = tryParseResult;
            }
            else
            {
                decimal? decimalValue = GetDecimalNullableValue(
                    dataRecord,
                    fieldName);

                if (decimalValue.HasValue)
                {
                    convertedValue = (int)Math.Round(decimalValue.Value);
                }
            }

            return convertedValue;
        }

        /// <summary>Получение десятичного значения с плавающей точкой
        /// по имени поля из указанной записи таблицы</summary>
        /// <param name="dataRecord">Запись таблицы данных</param>
        /// <param name="fieldName">Имя поля</param>
        /// <param name="defaultValue">Значение по умолчанию для случая,
        /// когда получить значение не удалось</param>
        /// <returns>Значение ячейки в виде десятичного числа
        /// с плавающей точкой</returns>
        public static decimal GetDecimalValue(DataRow dataRecord,
                                              string fieldName,
                                              decimal defaultValue = 0)
        {
            decimal convertedValue = defaultValue;
            decimal? obtainedValue = GetDecimalNullableValue(
                dataRecord,
                fieldName);

            if (obtainedValue.HasValue)
            {
                convertedValue = obtainedValue.Value;
            }

            return convertedValue;
        }

        /// <summary>Получение десятичного значения с плавающей точкой
        /// по имени поля из указанной записи таблицы</summary>
        /// <param name="dataRecord">Запись таблицы данных</param>
        /// <param name="fieldName">Имя поля</param>
        /// <param name="defaultValue">Значение по умолчанию для случая,
        /// когда получить значение не удалось</param>
        /// <returns>Значение ячейки в виде десятичного числа
        /// с плавающей точкой, допускающего null</returns>
        public static decimal? GetDecimalNullableValue(
            DataRow dataRecord,
            string fieldName,
            decimal? defaultValue = null)
        {
            decimal? convertedValue = defaultValue;
            string obtainedString = GetStringValue(
                dataRecord,
                fieldName);

            if (decimal.TryParse(
                obtainedString,
                out decimal tryParseResult))
            {
                convertedValue = tryParseResult;
            }

            return convertedValue;
        }

        /// <summary>Получение булевого значения по имени поля из указанной
        /// записи таблицы</summary>
        /// <param name="dataRecord">Запись таблицы данных</param>
        /// <param name="fieldName">Имя поля</param>
        /// <returns>Значение ячейки как true или false</returns>
        public static bool GetBooleanValue(DataRow dataRecord,
                                           string fieldName)
        {
            string obtainedString = GetStringValue(
                dataRecord,
                fieldName);
            bool.TryParse(
                obtainedString,
                out bool convertedValue);
            return convertedValue;
        }

        /// <summary>Получение значения в виде даты и времени по имени поля
        /// из указанной записи таблицы</summary>
        /// <param name="dataRecord">Запись таблицы данных</param>
        /// <param name="fieldName">Имя поля</param>
        /// <returns>Значение ячейки как DateTime</returns>
        public static DateTime GetDateTimeValue(DataRow dataRecord,
                                                string fieldName)
        {
            string obtainedString = GetStringValue(
                dataRecord,
                fieldName);
            DateTime.TryParse(
                obtainedString,
                out DateTime convertedValue);
            return convertedValue;
        }

        /// <summary>Получение значения в виде даты и времени по имени поля
        /// из указанной записи таблицы</summary>
        /// <param name="dataRecord">Запись таблицы данных</param>
        /// <param name="fieldName">Имя поля</param>
        /// <param name="defaultValue">Значение по умолчанию для случая,
        /// когда получить значение не удалось</param>
        /// <returns>Значение ячейки как DateTime</returns>
        public static DateTime GetDateTimeValue(DataRow dataRecord,
                                                string fieldName,
                                                DateTime defaultValue)
        {
            DateTime convertedValue = defaultValue;
            DateTime? tryParseResult = GetDateTimeNullableValue(
                dataRecord,
                fieldName);

            if (tryParseResult.HasValue)
            {
                convertedValue = tryParseResult.Value;
            }

            return convertedValue;
        }

        /// <summary>Получение значения в виде даты и времени по имени поля
        /// из указанной записи таблицы</summary>
        /// <param name="dataRecord">Запись таблицы данных</param>
        /// <param name="fieldName">Имя поля</param>
        /// <param name="defaultValue">Значение по умолчанию для случая,
        /// когда получить значение не удалось</param>
        /// <returns>Значение ячейки как DateTime, допускающий null</returns>
        public static DateTime? GetDateTimeNullableValue(
            DataRow dataRecord,
            string fieldName,
            DateTime? defaultValue = null)
        {
            DateTime? convertedValue = defaultValue;
            string obtainedString = GetStringValue(
                dataRecord,
                fieldName);

            if (DateTime.TryParse(
                obtainedString,
                out DateTime tryParseResult))
            {
                convertedValue = tryParseResult;
            }

            return convertedValue;
        }

        /// <summary>Приведение указанного значения указанного типа к строке,
        /// форматированной под SQL-запрос</summary>
        /// <param name="value">Исходное значение</param>
        /// <param name="type">Тип значения</param>
        /// <returns>Строковое представление значения, форматированное
        /// под SQL-запрос,</returns>
#if NETCOREAPP3_0_OR_GREATER
        public static string GetSqlValue(object? value,
                                         Type type)
#elif NET35_OR_GREATER
        public static string GetSqlValue(object value,
                                         Type type)
#endif
        {
            string convertedValue = "null";

            if (value == null)
            {
                return convertedValue;
            }

            try
            {
                if (type.Equals(typeof(string)))
                {
                    convertedValue = GetSqlValue((string)value);
                }
                else if (type.Equals(typeof(byte)) ||
                         type.Equals(typeof(byte?)) ||
                         type.Equals(typeof(sbyte)) ||
                         type.Equals(typeof(sbyte?)) ||
                         type.Equals(typeof(short)) ||
                         type.Equals(typeof(short?)) ||
                         type.Equals(typeof(int)) ||
                         type.Equals(typeof(int?)))
                {
                    convertedValue
                        = GetSqlValue((int)Convert.ChangeType(
                            value,
                            typeof(int)));
                }
                else if (type.Equals(typeof(float)) ||
                         type.Equals(typeof(float?)) ||
                         type.Equals(typeof(double)) ||
                         type.Equals(typeof(double?)) ||
                         type.Equals(typeof(decimal)) ||
                         type.Equals(typeof(decimal?)))
                {
                    convertedValue
                        = GetSqlValue((decimal)Convert.ChangeType(
                            value,
                            typeof(decimal)));
                }
                else if (type.Equals(typeof(DateTime)) ||
                         type.Equals(typeof(DateTime?)))
                {
                    convertedValue
                        = GetSqlValue((DateTime)Convert.ChangeType(
                            value,
                            typeof(DateTime)));
                }
                else if (type.Equals(typeof(long)) ||
                         type.Equals(typeof(long?)))
                {
                    convertedValue
                        = GetSqlValue((long)Convert.ChangeType(
                            value,
                            typeof(long)));
                }
                else if (type.Equals(typeof(ushort)) ||
                         type.Equals(typeof(ushort?)) ||
                         type.Equals(typeof(uint)) ||
                         type.Equals(typeof(uint?)) ||
                         type.Equals(typeof(ulong)) ||
                         type.Equals(typeof(ulong?)))
                {
                    convertedValue
                        = GetSqlValue((ulong)Convert.ChangeType(
                            value,
                            typeof(ulong)));
                }
            }
            catch (Exception)
            {
                convertedValue = "null";
            }

            return convertedValue;
        }

        /// <summary>Приведение строкового значения к строке,
        /// форматированной под SQL-запрос</summary>
        /// <param name="value">Исходное строковое значение</param>
        /// <returns>Строковое представление значения, форматированное
        /// под SQL-запрос,</returns>
        public static string GetSqlValue(string value)
        {
            if (value != null)
            {
                return $"'{value}'";
            }
            else
            {
                return "null";
            }
        }

        /// <summary>Приведение целочисленного значения к строке,
        /// форматированной под SQL-запрос</summary>
        /// <param name="value">Исходное целочисленное значение</param>
        /// <returns>Строковое представление значения, форматированное
        /// под SQL-запрос,</returns>
        public static string GetSqlValue(int? value)
        {
            return $"{value}";
        }

        /// <summary>Приведение целочисленного значения к строке,
        /// форматированной под SQL-запрос</summary>
        /// <param name="value">Исходное целочисленное значение</param>
        /// <returns>Строковое представление значения, форматированное
        /// под SQL-запрос,</returns>
        public static string GetSqlValue(long? value)
        {
            return $"{value}";
        }

        /// <summary>Приведение целочисленного значения к строке,
        /// форматированной под SQL-запрос</summary>
        /// <param name="value">Исходное целочисленное значение</param>
        /// <returns>Строковое представление значения, форматированное
        /// под SQL-запрос,</returns>
        public static string GetSqlValue(ulong? value)
        {
            return $"{value}";
        }

        /// <summary>Приведение десятичного значения к строке,
        /// форматированной под SQL-запрос</summary>
        /// <param name="value">Исходное десятичное значение</param>
        /// <returns>Строковое представление значения, форматированное
        /// под SQL-запрос,</returns>
        public static string GetSqlValue(decimal? value)
        {
            return $"{value}";
        }

        /// <summary>Приведение значения даты и времени к строке,
        /// форматированной под SQL-запрос</summary>
        /// <param name="value">Исходное значение типа DateTime</param>
        /// <returns>Строковое представление значения, форматированное
        /// под SQL-запрос,</returns>
        public static string GetSqlValue(DateTime? value)
        {
            if (value.HasValue)
            {
                return $"'{value.Value:yyyy-MM-dd HH:mm:ss}'";
            }
            else
            {
                return "null";
            }
        }
    }
}