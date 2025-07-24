using System;
using System.Reflection;

namespace DataEntitiesMapping
{
    public partial class EntityTableMapping
    {
        /// <summary>Получение тех публичных свойств класса, которые указаны
        /// в конфигурации сопоставления</summary>
        /// <typeparam name="DataType">Класс данных</typeparam>
        /// <param name="mapping">Конфигурация сопоставления свойств</param>
        /// <returns>Массив объектов класса PropertyInfo со сведениями
        /// о свойствах класса</returns>
        public static PropertyInfo[] GetProperties<DataType>(
            EntityTableMapping mapping)
            where DataType : class
        {
            Type dataType = typeof(DataType);
            PropertyInfo[] targetProps
                = new PropertyInfo[mapping.Properties.Length];

            for (int i = 0; i < mapping.Properties.Length; i++)
            {
                var propName = mapping.Properties[i];

                if (string.IsNullOrEmpty(propName))
                {
                    continue;
                }

                var propToCheck
                    = dataType.GetProperty(propName);

                if (propToCheck != null &&
                    propToCheck.CanRead &&
                    propToCheck.CanWrite)
                {
                    targetProps[i] = propToCheck;
                }
            }

            return targetProps;
        }

        /// <summary>Получение имён полей таблицы базы данных</summary>
        /// <param name="dataTypeProps">Свойства класса данных,
        /// к которым требуется получить соответствующие поля таблицы</param>
        /// <param name="mapping">Конфигурация сопоставления свойств
        /// с полями таблицы базы данных</param>
        /// <returns>Массив строк с именами полей таблицы базы данных</returns>
        public static string[] GetFields(
            PropertyInfo[] dataTypeProps,
            EntityTableMapping mapping)
        {
            string[] fields = new string[dataTypeProps.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = mapping[dataTypeProps[i].Name];
            }

            return fields;
        }

        /// <summary>Получение имён полей таблицы базы данных</summary>
        /// <typeparam name="DataType">Класс данных</typeparam>
        /// <param name="dataTypeProps">Свойства класса данных,
        /// к которым требуется получить соответствующие поля таблицы</param>
        /// <param name="mapping">Конфигурация сопоставления свойств
        /// с полями таблицы базы данных</param>
        /// <param name="item">Экземпляр класса данных</param>
        /// <returns>Массив строк с именами полей таблицы базы данных</returns>
        [Obsolete("Рекомендуется вызывать метод GetFields(" +
            "PropertyInfo[] dataTypeProps, EntityTableMapping mapping)")]
#if NETCOREAPP3_0_OR_GREATER
        public static string[]? GetFields<DataType>(
            PropertyInfo[]? dataTypeProps,
            EntityTableMapping mapping,
            DataType? item = null)
#elif NET35_OR_GREATER
        public static string[] GetFields<DataType>(
            PropertyInfo[] dataTypeProps,
            EntityTableMapping mapping,
            DataType item)
#endif
            where DataType : class
        {
            if (dataTypeProps == null)
            {
                return null;
            }

            return GetFields(
                dataTypeProps,
                mapping);
        }

        /// <summary>Получение форматированных под SQL-запрос значений
        /// свойств экземпляра класса данных</summary>
        /// <typeparam name="DataType">Класс данных</typeparam>
        /// <param name="dataTypeProps">Свойства класса данных,
        /// значения которых требуется получить</param>
        /// <param name="item">Экземпляр класса данных</param>
        /// <returns>Массив строк с форматированными под SQL-запрос
        /// значениями свойств</returns>
        public static string[] GetValues<DataType>(
            PropertyInfo[] dataTypeProps,
            DataType item)
            where DataType : class
        {
            var values = new string[dataTypeProps.Length];

            for (int i = 0; i < values.Length; i++)
            {
                PropertyInfo currentProp = dataTypeProps[i];
                var propValue = currentProp.GetValue(
                    item,
                    null);
                values[i] = GetSqlValue(
                    propValue,
                    currentProp.PropertyType);
            }

            return values;
        }

        /// <summary>Получение форматированных под SQL-запрос значений
        /// свойств экземпляра класса данных</summary>
        /// <typeparam name="DataType">Класс данных</typeparam>
        /// <param name="dataTypeProps">Свойства класса данных,
        /// значения которых требуется получить</param>
        /// <param name="mapping">Конфигурация сопоставления свойств</param>
        /// <param name="item">Экземпляр класса данных</param>
        /// <returns>Массив строк с форматированными под SQL-запрос
        /// значениями свойств</returns>
        [Obsolete("Рекомендуется вызывать метод GetValues<DataType>(" +
            "PropertyInfo[] dataTypeProps, DataType item)")]
#if NETCOREAPP3_0_OR_GREATER
        public static string[]? GetValues<DataType>(
            PropertyInfo[]? dataTypeProps,
            EntityTableMapping? mapping,
            DataType item)
#elif NET35_OR_GREATER
        public static string[] GetValues<DataType>(
        PropertyInfo[] dataTypeProps,
        EntityTableMapping mapping,
        DataType item)
#endif
        where DataType : class
        {
            if (dataTypeProps == null)
            {
                return null;
            }

            return GetValues(
                dataTypeProps,
                item);
        }
    }
}