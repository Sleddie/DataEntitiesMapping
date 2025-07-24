using System;
using System.Collections.Generic;
using System.Data;
using DataEntitiesMapping.Entity;

namespace DataEntitiesMapping.Connection
{
    /// <summary>Класс-расширение с методами подключения к базе данных
    /// и получения данных из таблиц</summary>
    public static class DbClient
    {
        /// <summary>Получение коллекции объектов заданного типа
        /// по указанному тексту запроса</summary>
        /// <typeparam name="EntityType">Тип данных объектов</typeparam>
        /// <param name="connection">Подключение</param>
        /// <param name="mapping">Конфигурация сопоставления таблицы и
        /// класса</param>
        /// <param name="query">Текст запроса</param>
        /// <param name="resultCollection">Целевая коллекция объектов</param>
        /// <param name="message">Сообщение</param>
        /// <returns>true - запрос данных прошёл без ошибок, false -
        /// произошла ошибка или передан недопустимый параметр</returns>
        public static bool Select<EntityType>(
            this IDbConnection connection,
            EntityTableMapping mapping,
            string query,
#if NETCOREAPP3_0_OR_GREATER
            out ICollection<EntityType>? resultCollection,
#elif NET35_OR_GREATER
            out ICollection<EntityType> resultCollection,
#endif
            out string message)
            where EntityType : class, new()
        {
            resultCollection = null;

            if (connection == null ||
                mapping == null)
            {
                message = "Нет сведений о подключении к базе данных или " +
                          "о конфигурации соответствия сущности и класса!";
                return false;
            }

            if (string.IsNullOrEmpty(query))
            {
                message = "Не заданы условия поиска!";
                return false;
            }

            var receivedData = connection.SelectByQuery(
                query,
                out message);

            resultCollection
                = receivedData.Obtain<EntityType>(mapping);

            if (resultCollection == null ||
                resultCollection.Count <= 0)
            {
                message = "Нет данных.";
            }

            return resultCollection != null;
        }

        /// <summary>Получение данных из указанной таблицы с указанным
        /// условием</summary>
        /// <param name="connection">Подключение</param>
        /// <param name="from">Имя таблицы или представления
        /// (или иного рода строка, являющаяся частью запроса SELECT
        /// и идущая после слова FROM)</param>
        /// <param name="condition">Условие запроса (или иного рода строка,
        /// являющаяся частью запроса SELECT и идущая после слова WHERE)
        /// </param>
        /// <param name="message">Сообщение</param>
        /// <returns>true - запрос данных прошёл без ошибок, false -
        /// произошла ошибка или передан недопустимый параметр</returns>
        public static DataTable SelectAll(
            this IDbConnection connection,
            string from,
            string condition,
            out string message)
        {
            return Select(
                connection,
                "*",
                from,
                condition,
                out message);
        }

        /// <summary>Получение данных из указанной таблицы с указанным
        /// условием</summary>
        /// <param name="connection">Подключение</param>
        /// <param name="paramSet">Набор запрашиваемых полей таблицы</param>
        /// <param name="from">Имя таблицы или представления
        /// (или иного рода строка, являющаяся частью запроса SELECT
        /// и идущая после слова FROM)</param>
        /// <param name="condition">Условие запроса (или иного рода строка,
        /// являющаяся частью запроса SELECT и идущая после слова WHERE)
        /// </param>
        /// <param name="message">Сообщение</param>
        /// <returns>true - запрос данных прошёл без ошибок, false -
        /// произошла ошибка или передан недопустимый параметр</returns>
        public static DataTable Select(
            this IDbConnection connection,
            string paramSet,
            string from,
            string condition,
            out string message)
        {
            var receivedData = new DataTable();
            string query = GetSelectQuery(
                from,
                paramSet: paramSet,
                condition: condition);

            if (!string.IsNullOrEmpty(query))
            {
                receivedData = SelectByQuery(
                    connection,
                    query,
                    out message);
            }
            else
            {
                message = "Не задан текст запроса!";
            }

            return receivedData;
        }

        /// <summary>Получение данных по указанному запросу</summary>
        /// <param name="connection">Подключение</param>
        /// <param name="query">Текст запроса</param>
        /// <param name="message">Сообщение</param>
        /// <returns>true - запрос данных прошёл без ошибок, false -
        /// произошла ошибка или передан недопустимый параметр</returns>
        public static DataTable SelectByQuery(
            this IDbConnection connection,
            string query,
            out string message)
        {
            var receivedData = new DataTable();
            message = "";

            if (connection == null)
            {
                message = "Нет сведений о подключениик базе данных!";
                return receivedData;
            }

            //receivedData = new DataTable();
            IDbCommand cmd = connection.CreateCommand();
#if NETCOREAPP3_0_OR_GREATER
            IDataReader? reader = null;
#elif NET35_OR_GREATER
            IDataReader reader = null;
#endif

            try
            {
                if (cmd == null)
                {
                    message = "Не удалось создать команду!";
                    return receivedData;
                }

                cmd.CommandText = query;
                cmd.Connection = connection;

                connection.Open();
                reader = cmd.ExecuteReader();

                if (reader != null)
                {
                    receivedData.Load(reader);
                }
            }
            catch (Exception ex)
            {
                receivedData.Clear();
                message = $"Произошла ошибка!\r\n{ex.Message}";
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }

                cmd?.Dispose();
                connection.Close();
            }

            return receivedData;
        }

        /// <summary>Получение запроса SELECT</summary>
        /// <param name="from">Имя таблицы или представления
        /// (или иного рода строка, являющаяся частью запроса SELECT
        /// и идущая после слова FROM)</param>
        /// <param name="paramSet">Набор запрашиваемых полей таблицы
        /// (по умолчанию - *)</param>
        /// <param name="condition">Условие запроса (или иного рода строка,
        /// являющаяся частью запроса SELECT и идущая после слова WHERE)
        /// </param>
        /// <returns>Текст запроса</returns>
        public static string GetSelectQuery(
            string from,
            string paramSet = "*",
            string condition = "")
        {
            string query = "";

            if (string.IsNullOrEmpty(from.Trim()))
            {
                return query;
            }

            if (string.IsNullOrEmpty(paramSet.Trim()))
            {
                paramSet = "*";
            }

            query = $"SELECT {paramSet} FROM {from}";

            if (!string.IsNullOrEmpty(condition.Trim()))
            {
                query += $" WHERE {condition}";
            }

            return query;
        }

        /// <summary>Получение запроса SELECT COUNT(*)</summary>
        /// <param name="from">Имя таблицы или представления
        /// (или иного рода строка, являющаяся частью запроса SELECT
        /// и идущая после слова FROM)</param>
        /// <param name="condition">Условие запроса (или иного рода строка,
        /// являющаяся частью запроса SELECT и идущая после слова WHERE)
        /// </param>
        /// <returns>Текст запроса</returns>
        public static string GetSelectCountQuery(
            string from,
            string condition = "")
        {
            return GetSelectQuery(
                from,
                "COUNT(*)",
                condition);
        }
    }
}