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
                PropertyInfo propToCheck
                    = dataType.GetProperty(mapping.Properties[i]);

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
        /// <typeparam name="DataType">Класс данных</typeparam>
        /// <param name="dataTypeProps">Свойства класса данных,
        /// к которым требуется получить соответствующие поля таблицы</param>
        /// <param name="mapping">Конфигурация сопоставления свойств
        /// с полями таблицы базы данных</param>
        /// <param name="item">Экземпляр класса данных</param>
        /// <returns>Массив строк с именами полей таблицы базы данных</returns>
        public static string[] GetFields<DataType>(
            PropertyInfo[] dataTypeProps,
            EntityTableMapping mapping,
            DataType item)
            where DataType : class
        {
            string[] fields = null;

            if (dataTypeProps == null ||
                dataTypeProps.Length == 0)
            {
                return fields;
            }

            fields = new string[dataTypeProps.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = mapping[dataTypeProps[i].Name];
            }

            return fields;
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
        public static string[] GetValues<DataType>(
            PropertyInfo[] dataTypeProps,
            EntityTableMapping mapping,
            DataType item)
            where DataType : class
        {
            string[] values = null;

            if (dataTypeProps == null ||
                dataTypeProps.Length == 0)
            {
                return values;
            }

            values = new string[dataTypeProps.Length];

            for (int i = 0; i < values.Length; i++)
            {
                PropertyInfo currentProp = dataTypeProps[i];
                object propValue = currentProp.GetValue(
                    item,
                    null);
                values[i] = GetSqlValue(
                    propValue,
                    currentProp.PropertyType);
            }

            return values;
        }
    }
}