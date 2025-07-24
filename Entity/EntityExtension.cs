using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace DataEntitiesMapping.Entity
{
    /// <summary>Класс-расширение с методами сопоставления
    /// таблиц базы данных с классами</summary>
    public static class TypeExtension
    {
        /// <summary>Наполнение пустого объекта данными из строки таблицы
        /// </summary>
        /// <typeparam name="EntityType">Тип данных объекта</typeparam>
        /// <param name="entity">Наполняемый объект</param>
        /// <param name="mapping">Конфигурация сопоставления полей таблицы
        /// и свойств класса</param>
        /// <param name="data">Строка таблицы с исходными данными</param>
        /// <returns>Объект с данными</returns>
#if NETCOREAPP3_0_OR_GREATER
        public static EntityType? Initialize<EntityType>(
            this EntityType? entity,
            EntityTableMapping? mapping,
            DataRow? data)
#elif NET35_OR_GREATER
        public static EntityType Initialize<EntityType>(
            this EntityType entity,
            EntityTableMapping mapping,
            DataRow data)
#endif
            where EntityType : class
        {
            if (entity == null ||
                mapping == null ||
                data == null)
            {
                return entity;
            }

            PropertyInfo[] properties
                = EntityTableMapping.GetProperties<EntityType>(mapping);

            foreach (PropertyInfo property in properties)
            {
                if (property == null)
                {
                    continue;
                }

                try
                {
                    var sourceValue = EntityTableMapping.GetValue(
                        data,
                        mapping[property.Name]);

                    if (sourceValue == null ||
                        Convert.IsDBNull(sourceValue))
                    {
                        continue;
                    }

                    Type propertyType
                        = Nullable.GetUnderlyingType(property.PropertyType)
                        ?? property.PropertyType;
                    object valueToSet = Convert.ChangeType(
                        sourceValue,
                        propertyType);
                    property.SetValue(
                        entity,
                        valueToSet,
                        null);
                }
                catch { }
            }

            return entity;
        }

        /// <summary>Получение объекта класса на основе данных
        /// из строки таблицы</summary>
        /// <typeparam name="EntityType">Тип данных объекта</typeparam>
        /// <param name="data">Строка таблицы с исходными данными</param>
        /// <param name="mapping">Конфигурация сопоставления полей таблицы
        /// и свойств класса</param>
        /// <returns>Объект с данными</returns>
#if NETCOREAPP3_0_OR_GREATER
        public static EntityType? Obtain<EntityType>(
            this DataRow? data,
            EntityTableMapping? mapping)
#elif NET35_OR_GREATER
        public static EntityType Obtain<EntityType>(
            this DataRow data,
            EntityTableMapping mapping)
#endif
            where EntityType : class, new()
        {
            var entity = new EntityType();
            return entity.Initialize(
                mapping,
                data);
        }

        /// <summary>Получение коллекции объектов класса на основе данных
        /// из таблицы</summary>
        /// <typeparam name="EntityType">Тип данных объекта</typeparam>
        /// <param name="dataSource">Таблица с исходными данными</param>
        /// <param name="mapping">Конфигурация сопоставления полей таблицы
        /// и свойств класса</param>
        /// <returns>Коллекция объектов с данными</returns>
        public static ICollection<EntityType> Obtain<EntityType>(
            this DataTable dataSource,
            EntityTableMapping mapping)
            where EntityType : class, new()
        {
            var resultCollection = new List<EntityType>();

            if (dataSource != null &&
                dataSource.Rows != null &&
                dataSource.Rows.Count > 0 &&
                mapping != null)
            {
                foreach (DataRow row in dataSource.Rows)
                {
                    if (row != null)
                    {
                        var entity = row.Obtain<EntityType>(mapping);

                        if (entity != null)
                        {
                            resultCollection.Add(entity);
                        }
                    }
                }
            }

            return resultCollection;
        }
    }
}