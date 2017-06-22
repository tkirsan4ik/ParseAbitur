using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace ParseAbitur
{
    class Parser
    {
        /// <summary>
        /// Путь до файлов
        /// </summary>
        public string pathToFile;

        /// <summary>
        /// Каталог, в который будут сохраняться новые файлы
        /// </summary>
        string pathToFinal = "final";
        
        /// <summary>
        /// Список файлов с полным путем
        /// </summary>
        public string[] files;

        /// <summary>
        /// Новые имена файлов
        /// </summary>
        public string[] newNameFiles;
        
        /// <summary>
        /// Варианты формы обучения. Использовать только в том порядке, который дан. Для регулярок.
        /// </summary>
        string[] forma = {
            "очн?заоч",
            "оч?заоч",
            "очн",
            "очка",
            "oчн",
            "заочн",
            "заочка"
        };

        /// <summary>
        /// Шаблон формы обучения в имени нового файла
        /// </summary>
        string[] formaName =
        {
            "och",
            "och-zao",
            "zao"
        };

        /// <summary>
        /// Соответствие регулярки для формы обучения (forma) и шаблона для имени файла(formaName)
        /// </summary>
        int[] sootvetstvieForma =
        {
            2,
            2,
            1,
            1,
            1,
            3,
            3
        };
        
        /// <summary>
        /// Коды направлений подготовки
        /// </summary>
        string[] code = {
            "09.03.03",
            "38.03.01",
            "40.03.01"
        };
        
        /// <summary>
        /// Регулярка для проверки кодов направлений подготовки
        /// </summary>
        string regExpCode = @"*\d{2}[-.]?\d{2}[-.]?\d{2}*";

        /// <summary>
        /// Название направления подготовки
        /// </summary>
        string[] codeName = {
            "Прикладная информатика",
            "Экономика",
            "Юриспруденция"
        };
        
        /// <summary>
        /// Запускаем обработку с передачей только лишь пути
        /// </summary>
        /// <param name="path"></param>
        public void run(string path)
        {
            this.setPathToFiles(path);
            //Ищем среди файлов необходимы нам по структуре имени
            this.findActualFiles();
        }

        ///////////////////////Files/////////////////////////////////////

        /// <summary>
        /// Получить файлы
        /// </summary>
        /// <returns>Массив с файлами</returns>
        public string[] getFiles()
        {
            //Получаем список всех файлов
            return this.files;
        }
        

        /// <summary>
        /// Поиск файлов, подходящих под расширение
        /// </summary>
        private void findFiles()
        {
            //сразу ищем файлы, которые подходят под определенную структуру
            //"код направления"-название
            this.files = Directory.GetFiles(this.pathToFile, "*.htm?");
        }

        /// <summary>
        /// Установить каталог до файлов
        /// </summary>
        /// <param name="path"></param>
        public void setPathToFiles(string path)
        {
            var pathToF = path;
            if (Directory.Exists(path))
            {
                this.pathToFile = path;
                //Сразу загружаем все файлы
                this.findFiles();
            }
            else
            {
                throw new Exception(@"Путь указан не верно или не сущетсвует");
            }
        }
        
        /// <summary>
        /// Данная фукнция ищет все файлы, которые подходят под шаблон и пересохраняет массив 
        /// </summary>
        private void findActualFiles()
        {
            if (this.files.Length <= 0)
            {
                findFiles();
                if (this.files.Length > 0)
                {
                    //var forma = new Dictionary<string, Array>();
                    List<string> files = new List<string> { };
                    //Отыскиваем все файлы, которые нам необходимы
                    foreach (string i in this.files)
                    {
                        if (this.regCreate(i) == true)
                        {
                            files.Add(i);
                        }
                    }
                    this.files = files.ToArray();
                }
                else
                {
                    throw new Exception("В каталоге отсутствуют файлы для анализа.");
                }
            }
        }
        
        /// <summary>
        /// Проверка файла на то, является ли его название типовым по структуре. Если файл типовой, то возвращается true.
        /// </summary>
        /// <param name="fileName">полный путь до файла.</param>
        /// <returns>Значение true, если название файла fileName является типовым по структуре; значение false, если
        //     название файла отлично от типового.</returns>
        private bool regCreate(string fileName)
        {
            bool result = false;
            string code;
            foreach (string type in this.forma)
            {
                result = Regex.IsMatch(fileName, this.regExpCode + "*" + type + "*");
                code = Regex.Replace(fileName, this.regExpCode,".");
                if (result == true)
                {
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Получаем имя файла для сохранения на сайт
        /// </summary>
        /// <param name="fileName">Имя исходного файла</param>
        /// <returns>Имя нового файла</returns>
        private string eNewName(string fileName)
        {
            var result = "";
            //Получаем код направления
            var code = Regex.Replace(fileName, this.regExpCode, ".");
            int i = 0;
            var fName = "";
            //Получаем форму обучения
            foreach (string f in this.forma)
            {
                if (Regex.IsMatch(fileName, f))
                {
                    fName = this.formaName[this.sootvetstvieForma[i]];
                    return code + "-" + fName+".html";
                }
                i++;
            }

            return result;
        }
        ////////////////////////Files/////////////////////////////


        ///////////////////////Parser///////////////////////////////

        public void runParser()
        {
            //Получаем файлы, необходимые для парсинга и обработки
            foreach (string file in getFiles())
            { 
                //1. Формируем новое название файла (для вывески на сайт)
                var newName = this.eNewName(file);
                if (newName != "")
                {
                    //2. Получаем содержимое файла
                    var html = File.OpenRead(file).ToString();
                    //3. Парсим HTML
                    var domHtml = "";
                    //4. Убираем из HTML две строки (конкурсная группа и количество мест)
                }

            }
        }
    }
}
