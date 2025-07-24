using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DataEntitiesMapping
{
    /// <summary>Контейнерный класс с набором конфигураций сопоставления
    /// с таблицами базы данных</summary>
    public class EntityTables
    {
        /// <summary>Имя сущности</summary>
        protected string _name;
        /// <summary>Имя таблицы по умолчанию</summary>
        protected string _defaultTable;
        /// <summary>Сведения о конфигурации сопоставления в формате XML
        /// </summary>
        protected XContainer _entityConfig;
        /// <summary>Набор конфигураций сопоставления</summary>
#if NETCOREAPP3_0_OR_GREATER
        protected SortedList<string, EntityTableMapping>? _tablesCollection;
#elif NET35_OR_GREATER
        protected SortedList<string, EntityTableMapping> _tablesCollection;
#endif

        /// <summary>Имя сущности</summary>
        public string Name { get { return _name; } }
        /// <summary>Имя таблицы по умолчанию</summary>
        public string DefaultTable { get { return _defaultTable; } }
        /// <summary>Конфигурация сопоставления для таблицы по умолчанию
        /// </summary>
#if NETCOREAPP3_0_OR_GREATER
        public EntityTableMapping? DefaultTableMapping
#elif NET35_OR_GREATER
        public EntityTableMapping DefaultTableMapping
#endif
        {
            get { return this[DefaultTable]; }
        }
        /// <summary>Конфигурация сопоставления по имени таблицы</summary>
        /// <param name="table">Имя таблицы</param>
        /// <returns>Объект со сведениями о конфигурации сопоставления
        /// </returns>
#if NETCOREAPP3_0_OR_GREATER
        public EntityTableMapping? this[string table]
#elif NET35_OR_GREATER
        public EntityTableMapping this[string table]
#endif
        {
            get
            {
                TryGetTableMapping(
                    table,
                    out var tableMapping);
                return tableMapping;
            }
        }

        /// <summary>Конструктор</summary>
        /// <param name="entityConfigs">Исходные сведения о наборе
        /// конфигураций сопоставления в формате XML</param>
        public EntityTables(XElement entityConfigs)
        {
            _name = entityConfigs.Name.ToString();
            _defaultTable = SetDefaultTable(entityConfigs);
            _entityConfig = entityConfigs;
            _tablesCollection = SetConfigsCollection(entityConfigs);
        }

        /// <summary>Получение конфигурации сопоставления по имени таблицы
        /// </summary>
        /// <param name="table">Имя таблицы</param>
        /// <param name="tableMapping">Объект со сведениями
        /// о конфигурации сопоставления</param>
        /// <returns></returns>
        public bool TryGetTableMapping(
            string table,
#if NETCOREAPP3_0_OR_GREATER
            out EntityTableMapping? tableMapping)
#elif NET35_OR_GREATER
            out EntityTableMapping tableMapping)
#endif
        {
            tableMapping = null;
            bool isSuccess = false;

            if (_tablesCollection != null)
            {
                isSuccess = _tablesCollection.TryGetValue(
                    table,
                    out tableMapping);
            }

            return isSuccess;
        }
        /// <summary>Получение конфигурации сопоставления для таблицы
        /// по умолчанию</summary>
        /// <param name="defaultMapping">Объект со сведениями
        /// о конфигурации сопоставления</param>
        /// <returns></returns>
        public bool TryGetDefaultTableMapping(
#if NETCOREAPP3_0_OR_GREATER
            out EntityTableMapping? defaultMapping)
#elif NET35_OR_GREATER
            out EntityTableMapping defaultMapping)
#endif
        {
            return TryGetTableMapping(
                DefaultTable,
                out defaultMapping);
        }

        #region Static

        /// <summary>Получение имени таблицы по умолчанию из исходных данных
        /// </summary>
        /// <param name="entityConfigs">Исходные сведения о наборе
        /// конфигураций сопоставления в формате XML</param>
        /// <returns>Имя таблицы по умолчанию</returns>
        public static string SetDefaultTable(XElement entityConfigs)
        {
            string defaultTableName = "";
            var defaultTableAttr
                = entityConfigs.Attribute("default");

            if (defaultTableAttr != null)
            {
                defaultTableName = defaultTableAttr.Value;
            }

            return defaultTableName;
        }

        /// <summary>Получение набора конфигураций сопоставления
        /// из исходных данных</summary>
        /// <param name="configsSource">Исходные сведения о наборе
        /// конфигураций сопоставления в формате XML</param>
        /// <returns>Набор конфигураций сопоставления</returns>
#if NETCOREAPP3_0_OR_GREATER
        public static SortedList<string, EntityTableMapping>? SetConfigsCollection(
            XElement? configsSource)
#elif NET35_OR_GREATER
        public static SortedList<string, EntityTableMapping> SetConfigsCollection(
            XElement configsSource)
#endif
        {
            if (configsSource == null ||
                !configsSource.HasElements)
            {
                return null;
            }

            var configsCollection
                = new SortedList<string, EntityTableMapping>();

            foreach (XElement configSource in configsSource.Elements())
            {
                var tableConfig
                    = new EntityTableMapping(configSource);
                configsCollection.Add(
                    tableConfig.Name,
                    tableConfig);
            }

            return configsCollection;
        }

        #endregion
    }
}