using System;
using System.Data;

namespace DataEntitiesMapping
{
    public partial class EntityTableMapping
    {
        /// <summary>Получение значения по имени поля из указанной
        /// записи таблицы</summary>
        /// <param name="data_record">Запись таблицы данных</param>
        /// <param name="field_name">Имя поля</param>
        /// <returns>Значение ячейки как Object</returns>
        public static object GetValue(DataRow data_record,
                                      string field_name)
        {
            object target = null;

            if (data_record != null &&
                !string.IsNullOrEmpty(field_name.Trim()) &&
                data_record.Table.Columns.Contains(field_name))
            {
                target = data_record[field_name];
            }

            return target;
        }

        /// <summary>Получение строкового значения по имени поля из указанной
        /// записи таблицы</summary>
        /// <param name="data_record">Запись таблицы данных</param>
        /// <param name="field_name">Имя поля</param>
        /// <param name="default_value">Значение по умолчанию для случая,
        /// когда получить значение не удалось</param>
        /// <returns>Значение ячейки как строка</returns>
        public static string GetStringValue(DataRow data_record,
                                            string field_name,
                                            string default_value = "")
        {
            string converted_value = default_value;
            object obtained_value = GetValue(data_record,
                                             field_name);

            if (!Convert.IsDBNull(obtained_value))
            {
                converted_value = Convert.ToString(obtained_value);
            }

            return converted_value;
        }

        /// <summary>Получение целочисленного значения по имени поля
        /// из указанной записи таблицы</summary>
        /// <param name="data_record">Запись таблицы данных</param>
        /// <param name="field_name">Имя поля</param>
        /// <param name="default_value">Значение по умолчанию для случая,
        /// когда получить значение не удалось</param>
        /// <returns>Значение ячейки в виде целого числа</returns>
        public static int GetIntegerValue(DataRow data_record,
                                          string field_name,
                                          int default_value = 0)
        {
            int converted_value = default_value;
            int? obtained_value = GetIntegerNullableValue(data_record,
                                                          field_name);

            if (obtained_value.HasValue)
            {
                converted_value = obtained_value.Value;
            }

            return converted_value;
        }

        /// <summary>Получение целочисленного значения по имени поля
        /// из указанной записи таблицы</summary>
        /// <param name="data_record">Запись таблицы данных</param>
        /// <param name="field_name">Имя поля</param>
        /// <param name="default_value">Значение по умолчанию для случая,
        /// когда получить значение не удалось</param>
        /// <returns>Значение ячейки в виде целого числа</returns>
        public static int? GetIntegerNullableValue(DataRow data_record,
                                                   string field_name,
                                                   int? default_value = null)
        {
            int? converted_value = default_value;
            string obtained_string = GetStringValue(data_record,
                                                    field_name);

            if (int.TryParse(obtained_string,
                             out int try_parse_result))
            {
                converted_value = try_parse_result;
            }
            else
            {
                decimal? decimal_value = GetDecimalNullableValue(data_record,
                                                                 field_name);

                if (decimal_value.HasValue)
                {
                    converted_value = (int)Math.Round(decimal_value.Value);
                }
            }

            return converted_value;
        }

        /// <summary>Получение десятичного значения с плавающей точкой
        /// по имени поля из указанной записи таблицы</summary>
        /// <param name="data_record">Запись таблицы данных</param>
        /// <param name="field_name">Имя поля</param>
        /// <param name="default_value">Значение по умолчанию для случая,
        /// когда получить значение не удалось</param>
        /// <returns>Значение ячейки в виде десятичного числа
        /// с плавающей точкой</returns>
        public static decimal GetDecimalValue(DataRow data_record,
                                              string field_name,
                                              decimal default_value = 0)
        {
            decimal converted_value = default_value;
            decimal? obtained_value = GetDecimalNullableValue(data_record,
                                                              field_name);

            if (obtained_value.HasValue)
            {
                converted_value = obtained_value.Value;
            }

            return converted_value;
        }

        /// <summary>Получение десятичного значения с плавающей точкой
        /// по имени поля из указанной записи таблицы</summary>
        /// <param name="data_record">Запись таблицы данных</param>
        /// <param name="field_name">Имя поля</param>
        /// <param name="default_value">Значение по умолчанию для случая,
        /// когда получить значение не удалось</param>
        /// <returns>Значение ячейки в виде десятичного числа
        /// с плавающей точкой</returns>
        public static decimal? GetDecimalNullableValue(DataRow data_record,
                                                       string field_name,
                                                       decimal? default_value = null)
        {
            decimal? converted_value = default_value;
            string obtained_string = GetStringValue(data_record,
                                                    field_name);

            if (decimal.TryParse(obtained_string,
                                 out decimal try_parse_result))
            {
                converted_value = try_parse_result;
            }

            return converted_value;
        }

        /// <summary>Получение булевого значения по имени поля из указанной
        /// записи таблицы</summary>
        /// <param name="data_record">Запись таблицы данных</param>
        /// <param name="field_name">Имя поля</param>
        /// <returns>Значение ячейки как true или false</returns>
        public static bool GetBooleanValue(DataRow data_record,
                                           string field_name)
        {
            string obtained_string = GetStringValue(data_record,
                                                    field_name);
            bool.TryParse(obtained_string,
                          out bool converted_value);
            return converted_value;
        }

        /// <summary>Получение значения в виде даты и времени по имени поля
        /// из указанной записи таблицы</summary>
        /// <param name="data_record">Запись таблицы данных</param>
        /// <param name="field_name">Имя поля</param>
        /// <returns>Значение ячейки как DateTime</returns>
        public static DateTime GetDateTimeValue(DataRow data_record,
                                                string field_name)
        {
            string obtained_string = GetStringValue(data_record,
                                                    field_name);
            DateTime.TryParse(obtained_string,
                              out DateTime converted_value);
            return converted_value;
        }

        /// <summary>Получение значения в виде даты и времени по имени поля
        /// из указанной записи таблицы</summary>
        /// <param name="data_record">Запись таблицы данных</param>
        /// <param name="field_name">Имя поля</param>
        /// <param name="default_value">Значение по умолчанию для случая,
        /// когда получить значение не удалось</param>
        /// <returns>Значение ячейки как DateTime</returns>
        public static DateTime GetDateTimeValue(DataRow data_record,
                                                string field_name,
                                                DateTime default_value)
        {
            DateTime converted_value = default_value;
            DateTime? try_parse_result = GetDateTimeNullableValue(data_record,
                                                                  field_name);

            if (try_parse_result.HasValue)
            {
                converted_value = try_parse_result.Value;
            }

            return converted_value;
        }

        /// <summary>Получение значения в виде даты и времени по имени поля
        /// из указанной записи таблицы</summary>
        /// <param name="data_record">Запись таблицы данных</param>
        /// <param name="field_name">Имя поля</param>
        /// <param name="default_value">Значение по умолчанию для случая,
        /// когда получить значение не удалось</param>
        /// <returns>Значение ячейки как DateTime</returns>
        public static DateTime? GetDateTimeNullableValue(DataRow data_record,
                                                         string field_name,
                                                         DateTime? default_value = null)
        {
            DateTime? converted_value = default_value;
            string obtained_string = GetStringValue(data_record,
                                                    field_name);

            if (DateTime.TryParse(obtained_string,
                                  out DateTime try_parse_result))
            {
                converted_value = try_parse_result;
            }

            return converted_value;
        }

        /// <summary>Приведение указанного значения указанного типа к строке,
        /// форматированной под SQL-запрос</summary>
        /// <param name="value">Исходное значение</param>
        /// <param name="type">Тип значения</param>
        /// <returns>Строковое представление значения, форматированное
        /// под SQL-запрос,</returns>
        public static string GetSqlValue(object value,
                                         Type type)
        {
            string converted_value = "null";

            if (value != null)
            {
                if (type.Equals(typeof(string)))
                {
                    converted_value = GetSqlValue(Convert.ToString(value));
                }
                else if (type.Equals(typeof(int)) ||
                         type.Equals(typeof(int?)) ||
                         type.Equals(typeof(uint)) ||
                         type.Equals(typeof(uint?)) ||
                         type.Equals(typeof(byte)) ||
                         type.Equals(typeof(byte?)) ||
                         type.Equals(typeof(sbyte)) ||
                         type.Equals(typeof(sbyte?)) ||
                         type.Equals(typeof(short)) ||
                         type.Equals(typeof(short?)) ||
                         type.Equals(typeof(ushort)) ||
                         type.Equals(typeof(ushort?)) ||
                         type.Equals(typeof(long)) ||
                         type.Equals(typeof(long?)) ||
                         type.Equals(typeof(ulong)) ||
                         type.Equals(typeof(ulong?)))
                {
                    converted_value = GetSqlValue((int)Convert
                                                  .ChangeType(value,
                                                              typeof(int)));
                }
                else if (type.Equals(typeof(decimal)) ||
                         type.Equals(typeof(decimal?)) ||
                         type.Equals(typeof(float)) ||
                         type.Equals(typeof(float?)) ||
                         type.Equals(typeof(double)) ||
                         type.Equals(typeof(double?)))
                {
                    converted_value = GetSqlValue((decimal)Convert
                                                  .ChangeType(value,
                                                              typeof(decimal)));
                }
                else if (type.Equals(typeof(DateTime)) ||
                         type.Equals(typeof(DateTime?)))
                {
                    converted_value = GetSqlValue((DateTime)Convert
                                                  .ChangeType(value,
                                                              typeof(DateTime)));
                }
            }

            return converted_value;
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
                return string.Format("'{0}'", value);
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
            return string.Format("{0}", value);
        }

        /// <summary>Приведение десятичного значения к строке,
        /// форматированной под SQL-запрос</summary>
        /// <param name="value">Исходное десятичное значение</param>
        /// <returns>Строковое представление значения, форматированное
        /// под SQL-запрос,</returns>
        public static string GetSqlValue(decimal? value)
        {
            return string.Format("{0}", value);
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
                return string.Format("'{0:yyyy-MM-dd HH:mm:ss}'",
                                     value.Value);
            }
            else
            {
                return "null";
            }
        }
    }
}