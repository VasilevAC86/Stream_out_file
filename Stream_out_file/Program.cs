using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Stream_out_file
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // С-реализация - это процесс преобразования объекта в последовательность байтов или любую др.структуру
            // для передачи по сети. Несколько способов: бинарная, джей-сон, xml.
            // Формат xml нужен для передачи данных по сети текстом

            // Создаём xml-документ
            /*XmlDocument doc = new XmlDocument(); // Инициализируем новый объект-документ
            // В каждом xml-документе должен быть корневой элемент, внутри которого храняться остальные элементы
            XmlElement root = doc.CreateElement("BookStore"); // Объект root имеет название BookStore
            // root мы должны закрепить в документе
            doc.AppendChild(root); // root - дочерний элемент корневого doc

            XmlElement book = doc.CreateElement("Book");
            book.SetAttribute("ISBN", "123456"); // Книге делаем атрибут элемента (атрибут - это текст внутри тэга)
            root.AppendChild(book); // Закрепляем (добавляем) book в root

            // Создаём элемент "Название" внутри книги
            XmlElement title = doc.CreateElement("Title");
            title.InnerText = "C# Programming"; // иннертекст - это текст между тэгами
            book.AppendChild(title);

            doc.Save("book.xml"); // Сохраняем документ в папку с программой (\Stream_out_file\bin\Debug\net8.0)*/

            // Открытие файла для считывания и изменения
            /*XmlDocument doc = new XmlDocument(); // Для открытия файла создаём объект
            doc.Load("book.xml"); // Открываем ранее созданный файл*/

            // Считывания информации в открытом файле
            /*XmlNode root = doc.DocumentElement; // Вытаскиваем BookStore (1-ый элемент)
            foreach (XmlNode bookNode in root.ChildNodes ) // Цикл для перебирания книг (которые внитри корневого root)
            {
                Console.WriteLine($"Book ISBN: {bookNode.Attributes["ISBN"].Value}"); // Значения ISBN книги
                Console.WriteLine($"Title: {bookNode["Title"].InnerText}"); // Название книги
            }*/

            // Изменение информации в открытом файле
            /*XmlNode bookNode = doc.SelectSingleNode("/BookStore/Book[@ISBN='123456']/Title"); // Меняем текст в Title в книге 123456 
            // Проверяем
            if (bookNode != null )
            {
                bookNode.InnerText = "Update";
            }
            // Сохраняем документ для сохранения изменений
            doc.Save("book.xml");*/

            // Запись в файл с помощью fstream (работает с двоичными байтами)
            //string filePath = "output.txt"; // Путь к файлу
            // Создаём новый файловый поток (путь, создать поток, для записи)
            /*using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                // Мы должны создать буфер байтов (fstream работает с бинарными файлами)
                string text = "Hello";
                byte[] buffer = System.Text.Encoding.ASCII.GetBytes(text); // Разбиваем текст text на байты -> В массив байтов
                fileStream.Write(buffer, 0, buffer.Length); // Записываем в поток массив байтов
            }*/

            // Чтение из файла
            /*using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                // Мы должны создать буфер байтов (fstream работает с бинарными файлами)                
                byte[] buffer = new byte[1024]; // Разбиваем текст text на байты -> В массив байтов
                int bytesRead; // Размер прочитанного файла
                while((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string text = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine(text);
                }
            }*/

            // string filePath = "output.txt";
            // stream writer
            /*using (StreamWriter writer = new StreamWriter((filePath), true)) // true, чтобы писать в конце файла
            {
                writer.WriteLine(" agdsffgd");
            }*/
            // stream reader
            /*using (StreamReader reader = new StreamReader(filePath)) // true, чтобы писать в конце файла
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
                Console.WriteLine(File.ReadAllText(filePath));
            }*/

            // Нагит пакидж (менеджер пакетов)
            /*var person = new { Name = "Вася", Age = 30, IsMarried = false }; // Объект анонимного класса
            string json = JsonConvert.SerializeObject(person);
            Console.WriteLine(json);
            var deserialized = JsonConvert.DeserializeObject<Person>(json); // Перевводим строку Json в объект класса Person
            Console.WriteLine($"Name: {deserialized.Name}, Age: {deserialized.Age}, Is married: {deserialized.IsMarried}");*/

            // Задача - Приложение для считывания инф-ии о книгах из 3-ёх разных источников
            // XML
            List<Book> books = new List<Book>(); // Список книг
            XDocument xmlDoc = XDocument.Load("Books.xml"); // Згрузили из файла Books.xml
            foreach (var el in xmlDoc.Element("books").Elements("book")) // XDocument позволяет работоть LINQ
            {
                Book book = new Book
                {
                    Title = el.Element("title").Value,
                    Author = el.Element("author").Value,
                    Year = int.Parse(el.Element("year").Value) // Преобразуем число в строку
                };
                books.Add(book);
            }
            // Json
            using (StreamReader r = new StreamReader("books.json"))
            {
                string json = r.ReadToEnd();
                dynamic array = JsonConvert.DeserializeObject(json);
                foreach (var item in array.books)
                {
                    Book book = new Book
                    {
                        Title = item.title,
                        Author = item.author,
                        Year = item.year,
                    };
                    books.Add(book);
                }
            }
            // txt
            using (StreamReader r = new StreamReader("books.txt"))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 3)
                    {
                        Book book = new Book
                        {
                            Title = parts[0].Trim(), // Trim() убирает пробелы в начале и в конце
                            Author = parts[1].Trim(),
                            Year = int.Parse(parts[2].Trim()),
                        };
                        books.Add(book);
                    }
                }
            }
            foreach (var book in books)
            {
                Console.WriteLine($"{book.Title}, {book.Author}, {book.Year}");
            }
        }
    }
}

public class Person
{
    public string Name { get; set; }
    public string Age { get; set; }
    public string IsMarried { get; set; }
}
