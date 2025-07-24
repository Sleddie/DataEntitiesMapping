namespace DataEntitiesMapping.Entity
{
    /// <summary>Общий класс метаданных</summary>
    /// <typeparam name="Type">Класс данных</typeparam>
    public class Entity<Type>
    {
        /// <summary>Имя класса</summary>
        public static string TypeName
        {
            get { return typeof(Type).Name; }
        }
        /// <summary>Сведения о стандартной конфигурации соответствия
        /// </summary>
#if NETCOREAPP3_0_OR_GREATER
        public static EntityTables? DefaultEntityConfigs
#elif NET35_OR_GREATER
        public static EntityTables DefaultEntityConfigs
#endif
        {
            get { return Entities.Default[TypeName]; }
        }
        /// <summary>Конфигурация соответствия класса с таблицей
        /// по умолчанию</summary>
#if NETCOREAPP3_0_OR_GREATER
        public static EntityTableMapping? DefaultMapping
#elif NET35_OR_GREATER
        public static EntityTableMapping DefaultMapping
#endif
        {
            get { return DefaultEntityConfigs?.DefaultTableMapping; }
        }
    }
}