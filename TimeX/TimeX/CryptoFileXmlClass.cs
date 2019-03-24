using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Xml.Serialization;

namespace TsivanyukModulus
{
    /// <summary>
    /// Класс обертка для зашифрованного чтения и записи объекта
    /// </summary>
    /// <typeparam name="T">Тип для сериализации</typeparam>
    public class FileXml<T> where T : class
    {
        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string path = string.Empty;

        /// <summary>
        /// Объект
        /// </summary>
        public T obj = null;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="secretKey">Секретный ключ</param>
        public FileXml(string path)
        {
            this.path = path;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public FileXml() { }

        /// <summary>
        /// Функция для записи в файл
        /// </summary>
        /// <returns>Триггер успешности операции</returns>
        public bool Write() {return Write(obj); }

        /// <summary>
        /// Функция для записи в файл
        /// </summary>
        /// <param name="o">Объект для записи в файл</param>
        /// <returns>Триггер успешности операции</returns>
        public bool Write(T o)
        {            
            try
            {
                StreamWriter SW = new StreamWriter(this.path);
                XmlSerializer xmlList = new XmlSerializer(typeof(T));
                xmlList.Serialize(SW, o);
                SW.Close();
                obj = o;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Функция чтения из файла объекта
        /// </summary>
        /// <returns></returns>
        public T Read()
        {
            if (!File.Exists(this.path))
            {
                this.obj = null;
                return this.obj;
            }
            try
            {
                StreamReader SR = new StreamReader(this.path);
                XmlSerializer xml = new XmlSerializer(typeof(T));
                obj = (T)xml.Deserialize(SR);
                SR.Close();
            }
            catch (Exception)
            {
                this.obj = null;
            }
            return this.obj;
        }
    }
}
