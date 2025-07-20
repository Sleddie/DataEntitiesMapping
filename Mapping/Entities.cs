using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using DataEntitiesMapping.Entity;

namespace DataEntitiesMapping
{
    /// <summary>Класс-контейнер конфигурации сопоставления классов и таблиц
    /// </summary>                                                                         
    public class Entities
    {
        /// <summary>XML-документ конфигурации</summary>
        protected XDocument _configsFile;
        /// <summary>Коллекция сущностей из конфгурации</summary>
        protected SortedList<string, EntityTables> _entitiesCollection;

        /// <summary>Получение сущности по его имени</summary>
        /// <param name="entity">Имя сущности</param>
        /// <returns>Объект конфигурации сущности</returns>
        public EntityTables this[string entity]
        {
            get
            {
                TryGetEntityTables(
                    entity,
                    out EntityTables entityConfigs);
                return entityConfigs;
            }
        }

        /// <summary>Конструктор по умолчанию</summary>
        protected Entities()
            : this(DefaultConfigFileName) { }

        /// <summary>Конструктор конфигурации по имени файла</summary>
        /// <param name="configFileName">Имя файла конфигурации</param>
        public Entities(string configFileName)
            : this(XDocument.Load(Path.GetFullPath(configFileName))) { }

        /// <summary>Конструктор конфигурации по XML-документу</summary>
        /// <param name="configsFile">XML-документ конфигурации</param>
        public Entities(XDocument configsFile)
        {
            _configsFile = configsFile;
            _entitiesCollection = SetEntitiesCollection(configsFile);
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
        /// <param name="entityConfigs">Объект конфигурации сущности</param>
        /// <returns>true, если объект конфигурации содержит конфигурацию
        /// указанного типа; в противном случае — false</returns>
        public bool TryGetEntityTables<EntityType>(
            out EntityTables entityConfigs)
        {
            return TryGetEntityTables(
                Entity<EntityType>.TypeName,
                out entityConfigs);
        }

        /// <summary>Получение сущности по типу данных</summary>
        /// <param name="entity">Имя сущности</param>
        /// <param name="entityConfigs">Объект конфигурации сущности</param>
        /// <returns>true, если объект конфигурации содержит конфигурацию
        /// с указанным именем; в противном случае — false</returns>
        public bool TryGetEntityTables(
            string entity,
            out EntityTables entityConfigs)
        {
            return _entitiesCollection.TryGetValue(
                entity,
                out entityConfigs);
        }

        #region Static

        /// <summary>Имя файла конфигурации по умолчанию</summary>
        public const string DefaultConfigFileName
            = "DataEntitiesMapping.config";

        /// <summary>Получение конфигурации по умолчанию</summary>
        public static Entities Default { get { return new Entities(); } }

        /// <summary>Получение коллекции сущностей
        /// из XML-документа конфигурации</summary>
        /// <param name="entitiesConfigsFile">XML-документ конфигурации
        /// </param>
        /// <returns>Коллекция сущностей в виде SortedList</returns>
        public static SortedList<string, EntityTables> SetEntitiesCollection(
            XDocument entitiesConfigsFile)
        {
            SortedList<string, EntityTables> configsCollection = null;
            XElement sourceRoot = entitiesConfigsFile?.Root;

            if (sourceRoot != null &&
                sourceRoot.HasElements)
            {
                configsCollection = new SortedList<string, EntityTables>();

                foreach (XElement configSource in sourceRoot.Elements())
                {
                    EntityTables tableConfig
                        = new EntityTables(configSource);
                    configsCollection.Add(
                        tableConfig.Name,
                        tableConfig);
                }
            }

            return configsCollection;
        }

        #endregion
    }
}