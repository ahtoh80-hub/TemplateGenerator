# Основные команды Git для проекта TemplateGenerator

## 1. Инициализация репозитория

```bash
# Перейти в папку проекта
cd C:\_Work\VSCode\TemplateGenerator\TemplateGenerator

# Инициализировать Git репозиторий
git init

# Проверить статус
git status
```

## 2. Настройка Git (если не настроен)

```bash
# Установить имя пользователя
git config user.name "Антон Решетов"

# Установить email
git config user.email "your.email@example.com"

# Проверить настройки
git config --list
```

## 3. Работа с файлами

```bash
# Проверить статус изменений
git status

# Посмотреть изменения в файлах
git diff

# Посмотреть изменения в конкретном файле
git diff Form1.cs

# Добавить все файлы в индекс
git add .

# Добавить конкретные файлы
git add Form1.cs Form1.Designer.cs Program.cs TemplateGenerator.csproj

# Добавить только C# файлы
git add *.cs

# Удалить файл из индекса (но оставить в папке)
git rm --cached имя_файла
```

## 4. Создание коммитов

```bash
# Сделать коммит с сообщением
git commit -m "Initial commit: Генератор экземпляров по шаблону"

# Коммит с подробным описанием
git commit -m "v2.3: Добавлена поддержка XML и визуальное выделение тегов" -m "- Поддержка XML с обновлением атрибута NAME
- Визуальное выделение несовпадающих тегов красным
- Адаптивная панель кнопок
- Исправлены ошибки инициализации"

# Добавить все и сделать коммит одной командой
git commit -am "Обновление: исправлены ошибки"

# Изменить последний коммит (добавить файлы или изменить сообщение)
git commit --amend -m "Новое сообщение для последнего коммита"
```

## 5. Работа с удаленным репозиторием (GitHub)

```bash
# Добавить удаленный репозиторий (создать на GitHub сначала!)
git remote add origin https://github.com/ваш_username/TemplateGenerator.git

# Или через SSH
git remote add origin git@github.com:ваш_username/TemplateGenerator.git

# Проверить удаленные репозитории
git remote -v

# Отправить изменения в GitHub (первый раз)
git push -u origin main

# Или если ветка master
git push -u origin master

# Отправить изменения (последующие разы)
git push

# Отправить изменения с указанием ветки
git push origin main
```

## 6. Клонирование репозитория

```bash
# Клонировать репозиторий
git clone https://github.com/ваш_username/TemplateGenerator.git

# Клонировать в конкретную папку
git clone https://github.com/ваш_username/TemplateGenerator.git MyTemplateGenerator
```

## 7. Работа с ветками

```bash
# Создать новую ветку
git branch feature/xml-support

# Переключиться на ветку
git checkout feature/xml-support

# Создать и переключиться одной командой
git checkout -b feature/xml-support

# Посмотреть все ветки
git branch

# Посмотреть все ветки (включая удаленные)
git branch -a

# Переключиться на главную ветку
git checkout main

# Объединить ветку с текущей
git merge feature/xml-support

# Удалить ветку
git branch -d feature/xml-support

# Удалить ветку на удаленном репозитории
git push origin --delete feature/xml-support
```

## 8. Обновление и синхронизация

```bash
# Получить изменения с удаленного репозитория (без объединения)
git fetch

# Получить изменения и объединить с текущей веткой
git pull

# Получить изменения из конкретной ветки
git pull origin main

# Получить изменения и перезаписать локальные (опасно!)
git pull --force
```

## 9. Отмена изменений

```bash
# Отменить изменения в файле (вернуть к последнему коммиту)
git checkout -- имя_файла

# Отменить все изменения
git checkout -- .

# Отменить последний коммит (оставить изменения)
git reset --soft HEAD~1

# Отменить последний коммит (удалить изменения)
git reset --hard HEAD~1

# Отменить коммит на удаленном репозитории
git revert HEAD
git push

# Удалить неотслеживаемые файлы
git clean -fd
```

## 10. Просмотр истории

```bash
# Посмотреть историю коммитов
git log

# Посмотреть историю в одну строку
git log --oneline

# Посмотреть историю с графиком
git log --oneline --graph --all

# Посмотреть изменения в конкретном коммите
git show hash_коммита

# Посмотреть последние N коммитов
git log -n 5
```

## 11. Для вашего конкретного проекта

```bash
# 1. Перейти в папку проекта
cd C:\_Work\VSCode\TemplateGenerator\TemplateGenerator

# 2. Проверить статус
git status

# 3. Добавить все файлы
git add .

# 4. Сделать коммит
git commit -m "v2.3: Генератор экземпляров по шаблону

Новые функции:
- Поддержка XML с обновлением атрибута NAME
- Визуальное выделение несовпадающих тегов красным
- Адаптивная панель кнопок (FlowLayoutPanel)
- Белый цвет заголовка 'Детали замен по экземплярам'

Исправления:
- Ошибка инициализации таблицы замен (Object reference)
- Исправлен порядок создания колонок DataGridView
- Добавлена обработка исключений в конструкторе

Технические изменения:
- Создание колонок DataGridView вручную
- Добавлен FlowLayoutPanel для адаптивности кнопок
- Улучшена обработка ошибок в Program.cs"

# 5. Отправить на GitHub
git push origin main

# Или если первый раз:
git push -u origin main
```

## 12. Шпаргалка по .gitignore для вашего проекта

Создайте файл `.gitignore` в корневой папке проекта:

```bash
# Создать .gitignore
echo "# Visual Studio" > .gitignore
echo ".vs/" >> .gitignore
echo "bin/" >> .gitignore
echo "obj/" >> .gitignore
echo "*.user" >> .gitignore
echo "*.suo" >> .gitignore
echo "" >> .gitignore
echo "# Build results" >> .gitignore
echo "[Dd]ebug/" >> .gitignore
echo "[Rr]elease/" >> .gitignore
echo "x64/" >> .gitignore
echo "x86/" >> .gitignore
echo "" >> .gitignore
echo "# User-specific files" >> .gitignore
echo "*.rsuser" >> .gitignore
echo "*.suo" >> .gitignore
echo "*.user" >> .gitignore
```

Или скачайте готовый .gitignore для Visual Studio:
```bash
# Скачать .gitignore для Visual Studio
curl -o .gitignore https://raw.githubusercontent.com/github/gitignore/main/VisualStudio.gitignore
```

## 13. Полезные комбинации команд

```bash
# Показать статус и добавить все файлы одной командой
git status && git add .

# Сделать коммит и отправить одной командой
git commit -m "Сообщение" && git push

# Посмотреть статус, добавить файлы, сделать коммит и отправить
git status && git add . && git commit -m "Сообщение" && git push
```

## 14. Решение проблем

```bash
# Если забыли добавить файл в коммит
git add забытый_файл.cs
git commit --amend --no-edit

# Если ошиблись в сообщении коммита
git commit --amend -m "Исправленное сообщение"

# Если нужно отменить последний коммит на удаленном
git reset --hard HEAD~1
git push --force

# Если конфликт при pull
git pull
# Исправить конфликты в файлах
git add .
git commit -m "Разрешение конфликтов"
git push
```

## 15. Создание нового репозитория на GitHub

```bash
# 1. Зайдите на GitHub.com и создайте новый репозиторий

# 2. В локальной папке выполните:
git init
git add .
git commit -m "Initial commit: Генератор экземпляров по шаблону"

# 3. Добавьте удаленный репозиторий (замените URL на ваш)
git remote add origin https://github.com/ваш_username/TemplateGenerator.git

# 4. Отправьте код
git push -u origin main
```

## 16. Полный цикл для вашего проекта

```bash
# ============================================
# ЕСЛИ РЕПОЗИТОРИЙ ЕЩЕ НЕ СОЗДАН
# ============================================

# 1. Перейти в папку проекта
cd C:\_Work\VSCode\TemplateGenerator\TemplateGenerator

# 2. Инициализировать Git
git init

# 3. Создать .gitignore
echo "bin/" >> .gitignore
echo "obj/" >> .gitignore
echo ".vs/" >> .gitignore
echo "*.user" >> .gitignore
echo "*.suo" >> .gitignore

# 4. Добавить все файлы
git add .

# 5. Сделать первый коммит
git commit -m "Initial commit: Генератор экземпляров по шаблону v2.3"

# 6. Добавить удаленный репозиторий (ЗАМЕНИТЕ URL!)
git remote add origin https://github.com/ваш_username/TemplateGenerator.git

# 7. Отправить на GitHub
git push -u origin main

# ============================================
# ЕСЛИ РЕПОЗИТОРИЙ УЖЕ ЕСТЬ (ОБНОВЛЕНИЕ)
# ============================================

# 1. Перейти в папку проекта
cd C:\_Work\VSCode\TemplateGenerator\TemplateGenerator

# 2. Проверить статус
git status

# 3. Добавить изменения
git add .

# 4. Сделать коммит с описанием
git commit -m "v2.3: Добавлена поддержка XML и визуальное выделение тегов

- Добавлена поддержка XML с обновлением атрибута NAME
- Визуальное выделение несовпадающих тегов красным
- Адаптивная панель кнопок (FlowLayoutPanel)
- Исправлена ошибка инициализации таблицы замен"

# 5. Отправить на GitHub
git push
```