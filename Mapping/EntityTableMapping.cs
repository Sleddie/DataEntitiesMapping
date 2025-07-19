using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DataEntitiesMapping
{
    /// <summary>Класс с конфигурацией сопоставления полей таблицы базы данных
    /// с публичными get-/set- свойствами класса</summary>
    public partial class EntityTableMapping
    {
        /// <summary>Имя таблицы</summary>
        protected readonly string _name;
        /// <summary>Массив имён свойств класса</summary>
        protected readonly string[] _properties;
        /// <summary>Массив имён полей таблицы</summary>
        protected readonly string[] _fields;
        /// <summary>Сведения о конфигурации сопоставления в формате XML</summary>
        protected XContainer _table_config;

        /// <summary>Имя таблицы</summary>
        public string Name { get { return _name; } }
        /// <summary>Массив имён свойств класса</summary>
        public string[] Properties { get { return _properties; } }
        /// <summary>Массив имён полей таблицы</summary>
        public string[] Fields { get { return _fields; } }
        /// <summary>Имя поля таблицы по имени свойства класса</summary>
        /// <param name="property">Имя свойства класса</param>
        /// <returns>Имя поля таблицы</returns>
        public string this[string property]
        {
            get
            {
                string field;
                TryGetField(property, out field);
                return field;
            }
        }

        /// <summary>Конструктор</summary>
        /// <param name="config_node">Исходные сведения о конфигурации сопоставления в формате XML</param>
        public EntityTableMapping(XElement config_node)
        {
            _name = Convert.ToString(config_node.Name);
            _table_config = config_node;

            {
                List<XElement> properties_config = new List<XElement>(config_node.Elements());
                _properties = new string[properties_config.Count];
                _fields = new string[properties_config.Count];

                int i = 0;
                foreach (XElement property_config in properties_config)
                {
                    if (property_config != null)
                    {
                        _properties[i] = Convert.ToString(property_config.Name);
                        _fields[i] = property_config.Value;
                    }

                    i++;
                }
            }
        }

        /// <summary>Получение имени поля таблицы по имени свойства класса</summary>
        /// <param name="property">Имя свойства класса</param>
        /// <param name="field">Имя поля таблицы</param>
        /// <returns>true - поля найдено, false - соответствующее поле не найдено или не существует,
        /// передан недопустимый аргумент или отсутствуют или не инициализированы исходные данные</returns>
        public bool TryGetField(string property, out string field)
        {
            bool obtained = false;
            property = property.Trim();
            field = "";

            if (Properties != null && Fields != null && !string.IsNullOrEmpty(property))
            {
                int target_index = 0;

                for (; target_index < Properties.Length; target_index++)
                {
                    if (Properties[target_index] == property)
                    {
                        obtained = true;
                        break;
                    }
                }

                if (obtained)
                {
                    field = Fields[target_index];
                }
            }

            return obtained;
        }
    }
}