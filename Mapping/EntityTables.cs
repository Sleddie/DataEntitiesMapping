using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DataEntitiesMapping
{
    /// <summary>Контейнерный класс с набором конфигураций сопоставления с таблицами базы данных</summary>
    public class EntityTables
    {
        /// <summary>Имя сущности</summary>
        protected string _name;
        /// <summary>Имя таблицы по умолчанию</summary>
        protected string _default_table;
        /// <summary>Сведения о конфигурации сопоставления в формате XML</summary>
        protected XContainer _entity_config;
        /// <summary>Набор конфигураций сопоставления</summary>
        protected SortedList<string, EntityTableMapping> _tables_collection;

        /// <summary>Имя сущности</summary>
        public string Name { get { return _name; } }
        /// <summary>Имя таблицы по умолчанию</summary>
        public string DefaultTable { get { return _default_table; } }
        /// <summary>Конфигурация сопоставления для таблицы по умолчанию</summary>
        public EntityTableMapping DefaultTableMapping { get { return this[DefaultTable]; } }
        /// <summary>Конфигурация сопоставления по имени таблицы</summary>
        /// <param name="table">Имя таблицы</param>
        /// <returns>Объект со сведениями о конфигурации сопоставления</returns>
        public EntityTableMapping this[string table]
        {
            get
            {
                EntityTableMapping table_mapping;
                TryGetTableMapping(table, out table_mapping);
                return table_mapping;
            }
        }

        /// <summary>Конструктор</summary>
        /// <param name="entity_configs">Исходные сведения о наборе конфигураций сопоставления в формате XML</param>
        public EntityTables(XElement entity_configs)
        {
            _name = Convert.ToString(entity_configs.Name);
            _default_table = SetDefaultTable(entity_configs);
            _entity_config = entity_configs;
            _tables_collection = SetConfigsCollection(entity_configs);
        }

        /// <summary>Получение конфигурации сопоставления по имени таблицы</summary>
        /// <param name="table">Имя таблицы</param>
        /// <param name="table_mapping">Объект со сведениями о конфигурации сопоставления</param>
        /// <returns></returns>
        public bool TryGetTableMapping(string table, out EntityTableMapping table_mapping) { return _tables_collection.TryGetValue(table, out table_mapping); }
        /// <summary>Получение конфигурации сопоставления для таблицы по умолчанию</summary>
        /// <param name="default_mapping">Объект со сведениями о конфигурации сопоставления</param>
        /// <returns></returns>
        public bool TryGetDefaultTableMapping(out EntityTableMapping default_mapping) { return TryGetTableMapping(DefaultTable, out default_mapping); }

        #region Static

        /// <summary>Получение имени таблицы по умолчанию из исходных данных</summary>
        /// <param name="entity_configs">Исходные сведения о наборе конфигураций сопоставления в формате XML</param>
        /// <returns>Имя таблицы по умолчанию</returns>
        public static string SetDefaultTable(XElement entity_configs)
        {
            string default_table_name = "";
            XAttribute default_table_attr = entity_configs.Attribute("default");

            if (default_table_attr != null)
            {
                default_table_name = default_table_attr.Value;
            }

            return default_table_name;
        }

        /// <summary>Получение набора конфигураций сопоставления из исходных данных</summary>
        /// <param name="configs_source">Исходные сведения о наборе конфигураций сопоставления в формате XML</param>
        /// <returns>Набор конфигураций сопоставления</returns>
        public static SortedList<string, EntityTableMapping> SetConfigsCollection(XElement configs_source)
        {
            SortedList<string, EntityTableMapping> configs_collection = null;

            if (configs_source != null && configs_source.HasElements)
            {
                configs_collection = new SortedList<string, EntityTableMapping>();

                foreach (XElement config_source in configs_source.Elements())
                {
                    EntityTableMapping table_config = new EntityTableMapping(config_source);
                    configs_collection.Add(table_config.Name, table_config);
                }
            }

            return configs_collection;
        }

        #endregion
    }
}