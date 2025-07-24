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
#if NETCOREAPP3_0_OR_GREATER
        protected SortedList<string, EntityTables>? _entitiesCollection;
#elif NET35_OR_GREATER
        protected SortedList<string, EntityTables> _entitiesCollection;
#endif

        /// <summary>Получение сущности по его имени</summary>
        /// <param name="entity">Имя сущности</param>
        /// <returns>Объект конфигурации сущности</returns>
#if NETCOREAPP3_0_OR_GREATER
        public EntityTables? this[string entity]
#elif NET35_OR_GREATER
        public EntityTables this[string entity]
#endif
        {
            get
            {
                TryGetEntityTables(
                    entity,
                    out var entityConfigs);
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
#if NETCOREAPP3_0_OR_GREATER
        public EntityTables? GetEntityTables<EntityType>()
#elif NET35_OR_GREATER
        public EntityTables GetEntityTables<EntityType>()
#endif
        {
            return this[Entity<EntityType>.TypeName];
        }

        /// <summary>Получение сущности по типу данных</summary>
        /// <typeparam name="EntityType">Тип данных сущности</typeparam>
        /// <param name="entityConfigs">Объект конфигурации сущности</param>
        /// <returns>true, если объект конфигурации содержит конфигурацию
        /// указанного типа; в противном случае — false</returns>
        public bool TryGetEntityTables<EntityType>(
#if NETCOREAPP3_0_OR_GREATER
            out EntityTables? entityConfigs)
#elif NET35_OR_GREATER
            out EntityTables entityConfigs)
#endif
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
#if NETCOREAPP3_0_OR_GREATER
            out EntityTables? entityConfigs)
#elif NET35_OR_GREATER
            out EntityTables entityConfigs)
#endif
        {
            entityConfigs = null;
            bool isSuccess = false;

            if (_entitiesCollection != null)
            {
                isSuccess = _entitiesCollection.TryGetValue(
                    entity,
                    out entityConfigs);
            }

            return isSuccess;
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
#if NETCOREAPP3_0_OR_GREATER
        public static SortedList<string, EntityTables>? SetEntitiesCollection(
            XDocument? entitiesConfigsFile)
#elif NET35_OR_GREATER
        public static SortedList<string, EntityTables> SetEntitiesCollection(
            XDocument entitiesConfigsFile)
#endif
        {
            var sourceRoot = entitiesConfigsFile?.Root;

            if (sourceRoot == null ||
                !sourceRoot.HasElements)
            {
                return null;
            }

            var configsCollection = new SortedList<string, EntityTables>();

            foreach (XElement configSource in sourceRoot.Elements())
            {
                var tableConfig
                    = new EntityTables(configSource);
                configsCollection.Add(
                    tableConfig.Name,
                    tableConfig);
            }

            return configsCollection;
        }

        #endregion
    }
}