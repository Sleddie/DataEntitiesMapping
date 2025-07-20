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
        /// <summary>Сведения о конфигурации сопоставления в формате XML
        /// </summary>
        protected XContainer _tableConfig;

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
                TryGetField(
                    property,
                    out string field);
                return field;
            }
        }

        /// <summary>Конструктор</summary>
        /// <param name="configNode">Исходные сведения
        /// о конфигурации сопоставления в формате XML</param>
        public EntityTableMapping(XElement configNode)
        {
            _name = Convert.ToString(configNode.Name);
            _tableConfig = configNode;
            List<XElement> xmlProperties
                = new List<XElement>(configNode.Elements());
            _properties = new string[xmlProperties.Count];
            _fields = new string[xmlProperties.Count];

            int i = 0;
            foreach (XElement propertyConfig in xmlProperties)
            {
                if (propertyConfig != null)
                {
                    _properties[i]
                        = Convert.ToString(propertyConfig.Name);
                    _fields[i] = propertyConfig.Value;
                }

                i++;
            }

        }

        /// <summary>Получение имени поля таблицы по имени свойства класса
        /// </summary>
        /// <param name="property">Имя свойства класса</param>
        /// <param name="field">Имя поля таблицы</param>
        /// <returns>true - поля найдено, false - соответствующее поле
        /// не найдено или не существует, передан недопустимый аргумент
        /// или отсутствуют или не инициализированы исходные данные</returns>
        public bool TryGetField(string property,
                                out string field)
        {
            bool obtained = false;
            property = property.Trim();
            field = "";

            if (Properties != null &&
                Fields != null &&
                !string.IsNullOrEmpty(property))
            {
                int targetIndex = 0;

                for (; targetIndex < Properties.Length; targetIndex++)
                {
                    if (Properties[targetIndex] == property)
                    {
                        obtained = true;
                        break;
                    }
                }

                if (obtained)
                {
                    field = Fields[targetIndex];
                }
            }

            return obtained;
        }
    }
}