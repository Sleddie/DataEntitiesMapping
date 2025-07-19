namespace DataEntitiesMapping.Entity
{
    /// <summary>Общий класс метаданных</summary>
    /// <typeparam name="Type">Класс данных</typeparam>
    public class Entity<Type>
    {
        /// <summary>Имя класса</summary>
        public static string TypeName { get { return typeof(Type).Name; } }
        /// <summary>Сведения о стандартной конфигурации соответствия
        /// </summary>
        public static EntityTables DefaultEntityConfigs
        {
            get { return Entities.Default[TypeName]; }
        }
        /// <summary>Конфигурация соответствия класса с таблицей
        /// по умолчанию</summary>
        public static EntityTableMapping DefaultMapping
        {
            get { return DefaultEntityConfigs.DefaultTableMapping; }
        }
    }
}