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
        protected SortedList<string, EntityTableMapping> _tablesCollection;

        /// <summary>Имя сущности</summary>
        public string Name { get { return _name; } }
        /// <summary>Имя таблицы по умолчанию</summary>
        public string DefaultTable { get { return _defaultTable; } }
        /// <summary>Конфигурация сопоставления для таблицы по умолчанию
        /// </summary>
        public EntityTableMapping DefaultTableMapping
        {
            get { return this[DefaultTable]; }
        }
        /// <summary>Конфигурация сопоставления по имени таблицы</summary>
        /// <param name="table">Имя таблицы</param>
        /// <returns>Объект со сведениями о конфигурации сопоставления
        /// </returns>
        public EntityTableMapping this[string table]
        {
            get
            {
                TryGetTableMapping(
                    table,
                    out EntityTableMapping tableMapping);
                return tableMapping;
            }
        }

        /// <summary>Конструктор</summary>
        /// <param name="entityConfigs">Исходные сведения о наборе
        /// конфигураций сопоставления в формате XML</param>
        public EntityTables(XElement entityConfigs)
        {
            _name = Convert.ToString(entityConfigs.Name);
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
        public bool TryGetTableMapping(string table,
                                       out EntityTableMapping tableMapping)
        {
            return _tablesCollection.TryGetValue(
                table,
                out tableMapping);
        }
        /// <summary>Получение конфигурации сопоставления для таблицы
        /// по умолчанию</summary>
        /// <param name="defaultMapping">Объект со сведениями
        /// о конфигурации сопоставления</param>
        /// <returns></returns>
        public bool TryGetDefaultTableMapping(
            out EntityTableMapping defaultMapping)
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
            XAttribute defaultTableAttr
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
        public static SortedList<string, EntityTableMapping> SetConfigsCollection(
            XElement configsSource)
        {
            SortedList<string, EntityTableMapping> configsCollection = null;

            if (configsSource == null ||
                !configsSource.HasElements)
            {
                return configsCollection;
            }

            configsCollection
                = new SortedList<string, EntityTableMapping>();

            foreach (XElement configSource in configsSource.Elements())
            {
                EntityTableMapping tableConfig
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