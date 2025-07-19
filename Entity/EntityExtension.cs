using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace DataEntitiesMapping.Entity
{
    /// <summary>Класс-расширение с методами сопоставления таблиц базы данных с классами</summary>
    public static class TypeExtension
    {
        /// <summary>Наполнение пустого объекта данными из строки таблицы</summary>
        /// <typeparam name="EntityType">Тип данных объекта</typeparam>
        /// <param name="entity">Наполняемый объект</param>
        /// <param name="mapping">Конфигурация сопоставления полей таблицы и свойств класса</param>
        /// <param name="data">Строка таблицы с исходными данными</param>
        /// <returns>Объект с данными</returns>
        public static EntityType Initialize<EntityType>(this EntityType entity, EntityTableMapping mapping, DataRow data)
            where EntityType : class
        {
            if (entity != null && mapping != null && data != null)
            {
                PropertyInfo[] properties = EntityTableMapping.GetProperties<EntityType>(mapping);

                foreach (PropertyInfo property in properties)
                {
                    if (property != null)
                    {
                        object value_to_set = null;
                        object source_value = EntityTableMapping.GetValue(data, mapping[property.Name]);

                        if (source_value != null && !Convert.IsDBNull(source_value))
                        {
                            Type property_type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                            value_to_set = Convert.ChangeType(source_value, property_type);
                        }

                        property.SetValue(entity, value_to_set, null);
                    }
                }
            }

            return entity;
        }

        /// <summary>Получение объекта класса на основе данных из строки таблицы</summary>
        /// <typeparam name="EntityType">Тип данных объекта</typeparam>
        /// <param name="data">Строка таблицы с исходными данными</param>
        /// <param name="mapping">Конфигурация сопоставления полей таблицы и свойств класса</param>
        /// <returns>Объект с данными</returns>
        public static EntityType Obtain<EntityType>(this DataRow data, EntityTableMapping mapping)
            where EntityType : class, new()
        {
            EntityType entity = new EntityType();
            return entity.Initialize(mapping, data);
        }

        /// <summary>Получение коллекции объектов класса на основе данных из таблицы</summary>
        /// <typeparam name="EntityType">Тип данных объекта</typeparam>
        /// <param name="data_source">Таблица с исходными данными</param>
        /// <param name="mapping">Конфигурация сопоставления полей таблицы и свойств класса</param>
        /// <returns>Коллекция объектов с данными</returns>
        public static ICollection<EntityType> Obtain<EntityType>(this DataTable data_source, EntityTableMapping mapping)
            where EntityType : class, new()
        {
            ICollection<EntityType> result_collection = null;

            if (data_source != null && mapping != null)
            {
                result_collection = new List<EntityType>();

                foreach (DataRow row in data_source.Rows)
                {
                    if (row != null)
                    {
                        result_collection.Add(row.Obtain<EntityType>(mapping));
                    }
                }
            }

            return result_collection;
        }
    }
}