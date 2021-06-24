# photo-warehouse

# Базовые требования для запуска проекта
* [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
* Для открытия и запуска проекта - Visual Studio 2019 (или любой текстовый редактор, но тогда вся работа с проектом ведется через [dotnet CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/))
* MySQL8.0

# Структура проекта
* PhotoWarehouse.Domain - содержит классы, описывающие предметную область. Используются ORM *(Entity Framework Core 5)* для создания соответствующих таблиц в БД
* PhotoWarehouse.Data - содержит классы, обеспечивающие работу ORM по связыванию сущностей предметной области с БД, а также классы-репозитории для выполнения запросов к БД
* PhotoWarehouseApp - проект веб-приложения

# Конфигурация
1. В PhotoWarehouseApp в файле appsettings.json проверить строку подключения к локальной БД (вероятно, необходимо поменять имя пользователя)
2. Добавить секретные данные в конфигурацию приложения:
  * При работе в VS2019 нажать правой кнопкой по проекту PhotoWarehouseApp и выбрать *"Manage User Secrets"*
 При этом откроется json-файл. В него необходимо добавить следующее содержимое:
 ```json
{
  "DbPassword": "<пароль_к_серверу_баз_данных>",
  "AdministratorEmail": "admin@pw.com",
  "AdministratorPassword": "<пароль_для_администратора_сайта>"
}
```
Почту администратора (AdministratorEmail) менять не обязательно, она выдуманная
  * Если нет VS2019, то каждую из 3-х настроек можно задать командой `dotnet user-secrets set "<ключ>" "<значение>"`
 Например,  `dotnet user-secrets set "DbPassword" "pass123"`

3. Создать базу данных путем применения миграций из проекта PhotoWarehouse.Data
Предварительно, необходимо убедиться, что установлен инструмент для работы с Entity Framework: dotnet-ef.
Для его установки, необходимо выполнить команду `dotnet tool install --global dotnet-ef` <br/>
После установки dotnet-ef, перейти в консоли в папку с проектом PhotoWarehouse.Data (В VS2019 в контекстном меню для проекта PhotoWarehouse.Data можно выбрать опцию открытия в терминале - *Open in Terminal*).  
Выполнить команду `dotnet ef database update -s ..\PhotoWarehouseApp\PhotoWarehouseApp.csproj`  
База данных должна создаться, и её можно увидеть в списке БД, например, в MySQL Workbench. Если БД не удается создать, возможно проблема в правах доступа, и БД необходимо создать вручную.

4. Запуск
* В VS2019 можно выбрать запуск либо на IIS Express, либо Kestrel (PhotoWarehouseApp в выпадающем списке рядом с зеленой кнопкой запуска)
* Из консоли можно выполнить команду `dotnet run`
* После запуска можно войти как администратор, используя имя administrator и пароль, заданный в конфигурации AdministratorPassword, а также создать новую учетную запись пользователя-клиента

# Что сделано
* Регистрация и вход пользователей (автоматически создается учетная запись администратора (владельца) ресурса
* Создание категорий (.../admin/categories)
* Вывод списка фото (.../admin/photos)
* Добавление одного фото (...admin/photos/create)
* Добавление серии фото (...admin/photos/createMultiple)
* Просмотр описания фотографии и всех связанных вариантов фото (.../admin/photos/details/{id}), где {id} - идентификатор фото (типа int)
### **UPD: 22/06/21**
* Добавлена возможность редактирования фото, а также редактирования списка связанных вариантов фото
* Добавлена возможность поиска фотографий по названию
### **UPD: 23/06/21**
* Добавлена возможность вывода списка фотографий в категории
* Переход к подробному описанию фото с выводом статистики для администратора и выпадающими списками с возможными размерами и форматами фотографии
* Добавление пользователем-клиентом отдельной фотографии в корзину (без отображения самой корзины)
### **UPD: 24/06/21**
* Показ содержимого корзины
* Редактирование предметов корзины (изменение выбор размера и формата из доступных для данной фотографии, а также удаление предмета)
* Оформление заказа
* Отображение содержимого заказа

# Над чем ведется работа
* Корзина и заказы пользователя
* Скачивание фотографий
* Интерфейс пока самый базовый, по завершению работы с серверной частью внешний вид и формат вывода некоторых типов данных (например, дат) будет улучшен


Stock photo project implemented as part of coursework at VLSU
