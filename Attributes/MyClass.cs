using System.Reflection;

namespace ParamsAttribute;

public class MyClass
{
    [IniConfig("Config1.ini")]
    public string? Property1 { get; set; }

    [IniConfig("Config2.ini")]
    public int Property2 { get; set; }

    [IniConfig("Config3.ini")]
    public int Property3 { get; set; }

    public MyClass()
    {
        LoadPropertiesFromIni();
    }

    private void LoadPropertiesFromIni()
    {
        PropertyInfo[] properties = GetType().GetProperties();

        foreach (PropertyInfo property in properties)
        {
            var iniAttribute = Attribute.GetCustomAttribute(property, typeof(IniConfigAttribute)) as IniConfigAttribute;

            if (iniAttribute != null)
            {
                string fileName = iniAttribute.FileName;
                string propertyName = property.Name;
                string value = ReadValueFromIni(fileName, propertyName);

                // Проверка, найдено ли значение в ini-файле
                if (!string.IsNullOrEmpty(value))
                {
                    // Преобразование значения из строки в соответствующий тип свойства
                    object typedValue = Convert.ChangeType(value, property.PropertyType);

                    // Определение значения свойства
                    property.SetValue(this, typedValue);
                }
            }
        }
    }
    private string ReadValueFromIni(string fileName, string propertyName)
    {
        string value = string.Empty;

        try
        {
            // Полный путь к ini-файлу
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            // Проверяем существование файла
            if (File.Exists(filePath))
            {
                // Читаем все строки из ini-файла
                string[] lines = File.ReadAllLines(filePath);

                // Ищем строку с соответствующим ключом (именем свойства)
                foreach (string line in lines)
                {
                    // Предполагаем, что ключ и значение разделены знаком равно
                    string[] parts = line.Split('=');

                    if (parts.Length == 2 && parts[0].Trim() == propertyName)
                    {
                        // Нашли нужный ключ, присваиваем значение
                        value = parts[1].Trim();
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine($"Ini-файл {fileName} не найден.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении из ini-файла: {ex.Message}");
        }

        return value;
    }
}