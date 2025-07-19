using System;
using System.Reflection;

namespace DataEntitiesMapping
{
    public partial class EntityTableMapping
    {
        /// <summary>Получение тех публичных свойств класса, которые указаны в конфигурации сопоставления</summary>
        /// <typeparam name="DataType">Класс данных</typeparam>
        /// <param name="mapping">Конфигурация сопоставления свойств</param>
        /// <returns>Массив объектов класса PropertyInfo со сведениями о свойствах класса</returns>
        public static PropertyInfo[] GetProperties<DataType>(EntityTableMapping mapping)
            where DataType : class
        {
            Type data_type = typeof(DataType);
            PropertyInfo[] target_properties = new PropertyInfo[mapping.Properties.Length];

            for (int i = 0; i < mapping.Properties.Length; i++)
            {
                PropertyInfo property_to_check = data_type.GetProperty(mapping.Properties[i]);

                if (property_to_check != null && property_to_check.CanRead && property_to_check.CanWrite)
                {
                    target_properties[i] = property_to_check;
                }
            }

            return target_properties;
        }

        /// <summary>Получение имён полей таблицы базы данных</summary>
        /// <typeparam name="DataType">Класс данных</typeparam>
        /// <param name="data_type_properties">Свойства класса данных, к которым требуется получить соответствующие поля таблицы</param>
        /// <param name="mapping">Конфигурация сопоставления свойств с полями таблицы базы данных</param>
        /// <param name="item">Экземпляр класса данных</param>
        /// <returns>Массив строк с именами полей таблицы базы данных</returns>
        public static string[] GetFields<DataType>(PropertyInfo[] data_type_properties, EntityTableMapping mapping, DataType item)
            where DataType : class
        {
            string[] fields = null;

            if (data_type_properties != null && data_type_properties.Length > 0)
            {
                fields = new string[data_type_properties.Length];

                for (int i = 0; i < fields.Length; i++)
                {
                    fields[i] = mapping[data_type_properties[i].Name];
                }
            }

            return fields;
        }

        /// <summary>Получение форматированных под SQL-запрос значений свойств экземпляра класса данных</summary>
        /// <typeparam name="DataType">Класс данных</typeparam>
        /// <param name="data_type_properties">Свойства класса данных, значения которых требуется получить</param>
        /// <param name="mapping">Конфигурация сопоставления свойств</param>
        /// <param name="item">Экземпляр класса данных</param>
        /// <returns>Массив строк с форматированными под SQL-запрос значениями свойств</returns>
        public static string[] GetValues<DataType>(PropertyInfo[] data_type_properties, EntityTableMapping mapping, DataType item)
            where DataType : class
        {
            string[] values = null;

            if (data_type_properties != null && data_type_properties.Length > 0)
            {
                values = new string[data_type_properties.Length];

                for (int i = 0; i < values.Length; i++)
                {
                    PropertyInfo current_property = data_type_properties[i];
                    object property_value = current_property.GetValue(item, null);
                    values[i] = EntityTableMapping.GetSqlValue(property_value, current_property.PropertyType);
                }
            }

            return values;
        }
    }
}