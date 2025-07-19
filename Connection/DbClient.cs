using System;
using System.Collections.Generic;
using System.Data;
using DataEntitiesMapping.Entity;

namespace DataEntitiesMapping.Connection
{
    /// <summary>Класс-расширение с методами подключения к базе данных и получения данных из таблиц</summary>
    public static class DbClient
    {
        /// <summary>Получение коллекции объектов заданного типа по указанному тексту запроса</summary>
        /// <typeparam name="EntityType">Тип данных объектов</typeparam>
        /// <param name="connection">Подключение</param>
        /// <param name="mapping">Конфигурация сопоставления таблицы и класса</param>
        /// <param name="query">Текст запроса</param>
        /// <param name="result_collection">Целевая коллекция объектов</param>
        /// <param name="message">Сообщение</param>
        /// <returns>true - запрос данных прошёл без ошибок, false - произошла ошибка или передан недопустимый параметр</returns>
        public static bool Select<EntityType>(this IDbConnection connection,
                                              EntityTableMapping mapping,
                                              string query,
                                              out ICollection<EntityType> result_collection,
                                              out string message)
            where EntityType : class, new()
        {
            result_collection = null;
            message = "";
            bool is_correct = false;

            if (connection != null && mapping != null)
            {
                DataTable received_data = null;

                if (!string.IsNullOrEmpty(query))
                {
                    received_data = connection.SelectByQuery(query, out message);
                }
                else
                {
                    message = "Не заданы условия поиска!";
                }

                if (received_data != null)
                {
                    result_collection = received_data.Obtain<EntityType>(mapping);
                    is_correct = result_collection != null;

                    if (result_collection == null || result_collection.Count <= 0)
                    {
                        message = "Нет данных.";
                    }
                }
            }
            else
            {
                message = "Нет сведений о подключении к базе данных или о конфигурации соответствия сущности и класса!";
            }

            return is_correct;
        }

        /// <summary>Получение данных из указанной таблицы с указанным условием</summary>
        /// <param name="connection">Подключение</param>
        /// <param name="from">Имя таблицы или представления
        /// (или иного рода строка, являющаяся частью запроса SELECT
        /// и идущая после слова FROM)</param>
        /// <param name="condition">Условие запроса (или иного рода строка,
        /// являющаяся частью запроса SELECT и идущая после слова WHERE)</param>
        /// <param name="message">Сообщение</param>
        /// <returns>true - запрос данных прошёл без ошибок, false - произошла ошибка или передан недопустимый параметр</returns>
        public static DataTable SelectAll(this IDbConnection connection,
                                          string from,
                                          string condition,
                                          out string message)
        {
            return Select(connection, "*", from, condition, out message);
        }

        /// <summary>Получение данных из указанной таблицы с указанным условием</summary>
        /// <param name="connection">Подключение</param>
        /// <param name="param_set">Набор запрашиваемых полей таблицы</param>
        /// <param name="from">Имя таблицы или представления
        /// (или иного рода строка, являющаяся частью запроса SELECT
        /// и идущая после слова FROM)</param>
        /// <param name="condition">Условие запроса (или иного рода строка,
        /// являющаяся частью запроса SELECT и идущая после слова WHERE)</param>
        /// <param name="message">Сообщение</param>
        /// <returns>true - запрос данных прошёл без ошибок, false - произошла ошибка или передан недопустимый параметр</returns>
        public static DataTable Select(this IDbConnection connection,
                                       string param_set,
                                       string from,
                                       string condition,
                                       out string message)
        {
            DataTable result_data = null;
            string query = GetSelectQuery(from, param_set: param_set, condition: condition);

            if (!string.IsNullOrEmpty(query))
            {
                result_data = SelectByQuery(connection, query, out message);
            }
            else
            {
                message = "Не задан текст запроса!";
            }

            return result_data;
        }

        /// <summary>Получение данных по указанному запросу</summary>
        /// <param name="connection">Подключение</param>
        /// <param name="query">Текст запроса</param>
        /// <param name="message">Сообщение</param>
        /// <returns>true - запрос данных прошёл без ошибок, false - произошла ошибка или передан недопустимый параметр</returns>
        public static DataTable SelectByQuery(this IDbConnection connection,
                                              string query,
                                              out string message)
        {
            DataTable out_data = null;
            message = "";

            if (connection != null)
            {
                out_data = new DataTable();
                IDbCommand cmd = null;
                IDataReader reader = null;

                try
                {
                    cmd = connection.CreateCommand();

                    if (cmd != null)
                    {
                        cmd.CommandText = query;
                        cmd.Connection = connection;

                        connection.Open();
                        reader = cmd.ExecuteReader();

                        if (reader != null)
                        {
                            out_data.Load(reader);
                        }
                    }
                    else
                    {
                        message = "Не удалось создать команду!";
                    }
                }
                catch (Exception ex)
                {
                    out_data = null;
                    message = string.Format("Произошла ошибка!\r\n{0}", ex.Message);
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                        reader.Dispose();
                    }

                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }

                    connection.Close();
                }
            }
            else
            {
                message = "Нет сведений о подключениик базе данных!";
            }

            return out_data;
        }

        /// <summary>Получение запроса SELECT</summary>
        /// <param name="from">Имя таблицы или представления
        /// (или иного рода строка, являющаяся частью запроса SELECT
        /// и идущая после слова FROM)</param>
        /// <param name="param_set">Набор запрашиваемых полей таблицы (по умолчанию - *)</param>
        /// <param name="condition">Условие запроса (или иного рода строка,
        /// являющаяся частью запроса SELECT и идущая после слова WHERE)</param>
        /// <returns>Текст запроса</returns>
        public static string GetSelectQuery(string from, string param_set = "*", string condition = "")
        {
            string query = null;

            if (!string.IsNullOrEmpty(from.Trim()))
            {
                if (string.IsNullOrEmpty(param_set.Trim()))
                {
                    param_set = "*";
                }

                query = string.Format("SELECT {0} FROM {1}", param_set, from);

                if (!string.IsNullOrEmpty(condition.Trim()))
                {
                    query += string.Format(" WHERE {0}", condition);
                }
            }

            return query;
        }

        /// <summary>Получение запроса SELECT COUNT(*)</summary>
        /// <param name="from">Имя таблицы или представления
        /// (или иного рода строка, являющаяся частью запроса SELECT
        /// и идущая после слова FROM)</param>
        /// <param name="condition">Условие запроса (или иного рода строка,
        /// являющаяся частью запроса SELECT и идущая после слова WHERE)</param>
        /// <returns>Текст запроса</returns>
        public static string GetSelectCountQuery(string from, string condition = "")
        {
            return GetSelectQuery(from, "COUNT(*)", condition);
        }
    }
}