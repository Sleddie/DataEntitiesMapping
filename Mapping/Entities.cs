using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using DataEntitiesMapping.Entity;

namespace DataEntitiesMapping
{
    /// <summary>Класс-контейнер конфигурации сопоставления классов и таблиц</summary>
    public class Entities
    {
        /// <summary>XML-документ конфигурации</summary>
        protected XDocument _configs_file;
        /// <summary>Коллекция сущностей из конфгурации</summary>
        protected SortedList<string, EntityTables> _entities_collection;

        /// <summary>Получение сущности по его имени</summary>
        /// <param name="entity">Имя сущности</param>
        /// <returns>Объект конфигурации сущности</returns>
        public EntityTables this[string entity]
        {
            get
            {
                EntityTables entity_configs;
                TryGetEntityTables(entity, out entity_configs);
                return entity_configs;
            }
        }

        /// <summary>Конструктор по умолчанию</summary>
        protected Entities()
            : this(DEFAULT_CONFIG_FILE_NAME) { }

        /// <summary>Конструктор конфигурации по имени файла</summary>
        /// <param name="config_file_name">Имя файла конфигурации</param>
        public Entities(string config_file_name)
            : this(XDocument.Load(Path.GetFullPath(config_file_name))) { }

        /// <summary>Конструктор конфигурации по XML-документу</summary>
        /// <param name="configs_file">XML-документ конфигурации</param>
        public Entities(XDocument configs_file)
        {
            _configs_file = configs_file;
            _entities_collection = SetEntitiesCollection(configs_file);
        }

        /// <summary>Получение сущности по типу данных</summary>
        /// <typeparam name="EntityType">Тип данных сущности</typeparam>
        /// <returns>Объект конфигурации сущности</returns>
        public EntityTables GetEntityTables<EntityType>()
        {
            return this[Entity<EntityType>.TypeName];
        }

        /// <summary>Получение сущности по типу данных</summary>
        /// <typeparam name="EntityType">Тип данных сущности</typeparam>
        /// <param name="entity_configs">Объект конфигурации сущности</param>
        /// <returns>true, если объект конфигурации содержит конфигурацию
        /// указанного типа; в противном случае — false</returns>
        public bool TryGetEntityTables<EntityType>(out EntityTables entity_configs)
        {
            return TryGetEntityTables(Entity<EntityType>.TypeName, out entity_configs);
        }

        /// <summary>Получение сущности по типу данных</summary>
        /// <param name="entity">Имя сущности</param>
        /// <param name="entity_configs">Объект конфигурации сущности</param>
        /// <returns>true, если объект конфигурации содержит конфигурацию
        /// с указанным именем; в противном случае — false</returns>
        public bool TryGetEntityTables(string entity, out EntityTables entity_configs)
        {
            return _entities_collection.TryGetValue(entity, out entity_configs);
        }

        #region Static

        /// <summary>Имя файла конфигурации по умолчанию</summary>
        public const string DEFAULT_CONFIG_FILE_NAME = "DatabaseEntitiesMapping.config";

        /// <summary>Получение конфигурации по умолчанию</summary>
        public static Entities Default { get { return new Entities(); } }

        /// <summary>Получение коллекции сущностей из XML-документа конфигурации</summary>
        /// <param name="entities_configs_file">XML-документ конфигурации</param>
        /// <returns>Коллекция сущностей в виде SortedList</returns>
        public static SortedList<string, EntityTables> SetEntitiesCollection(XDocument entities_configs_file)
        {
            SortedList<string, EntityTables> configs_collection = null;
            XElement source_root = entities_configs_file != null ? entities_configs_file.Root : null;

            if (source_root != null && source_root.HasElements)
            {
                configs_collection = new SortedList<string, EntityTables>();

                foreach (XElement config_source in source_root.Elements())
                {
                    EntityTables table_config = new EntityTables(config_source);
                    configs_collection.Add(table_config.Name, table_config);
                }
            }

            return configs_collection;
        }

        #endregion
    }
}