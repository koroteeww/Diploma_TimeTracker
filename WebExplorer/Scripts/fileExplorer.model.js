/* 
    менеджер файлов: модель 
*/
function FileExplorer(apiUrl,explorerPath) {
    //сохраняем ссылку на контекст
    var explorer = this;
    
    //---------------------------------------------------------------------------
    // PRIVATE METHODS
    //разбор строкового пути
    var parsePath = function(path) {
        if(typeof(path) == "undefined")
            return [];

        //разберем строку если это строка
        var result = path instanceof Array
            ? path
            : path.replace(/\/\/+/g, "/").replace(/^\//, "").replace(/\/$/, "").split("/");
        
        //если первый элемент пустой, выкинем его
        if(result[0] == "")
            result.shift();

        return result;
    };
    
    //---------------------------------------------------------------------------
    //PUBLIC PROPERTY
    //путь к апи
    explorer.ApiUrl = apiUrl == undefined ? "/api" : apiUrl.replace(/\/$/, "");
    //текущий путь
    explorer.CurrentPath = ko.observableArray();
    //список файлов
    explorer.Files = ko.observableArray();
    //ошибки загрузки
    explorer.Errors = ko.observableArray();
    //идет загрузка файлов
    explorer.UploadingFiles = ko.observableArray();
    
    //---------------------------------------------------------------------------
    //PUBLIC METHOD
    //загрузка списка
    explorer.Open = function (path) {
        //deffered объект
        var deffered = $.Deferred();
        //обработаем путь
        if (path instanceof Array)
            for (var i = 0; i < path.length; i++)
                path[i] = unescape(path[i]);
        else
        path = unescape(path);
        
        path = parsePath(path);
        var stringPath = "/" + path.join("/");

        //загрузим список
        $.ajax(explorer.ApiUrl+"/files", { data: { path: stringPath } })
            .done(function (data) {
                explorer.CurrentPath(path);
                explorer.Files(data);
                location.hash = stringPath;
                deffered.resolve();
            })
            .fail(function () {
                explorer.Errors.push("При подключении к серверу возникла ошибка. Попробуйте позже.");
                deffered.reject();
            });

        //вернем объект для коллбэка
        return deffered.promise();
    };
    
    //создание папки
    explorer.CreateFolder = function (name) {
        var deffered = $.Deferred();
        var stringPath = explorer.CurrentPath().join("/");

        //пошлем запрос
        $.ajax(explorer.ApiUrl + "/directory", { type: "POST", data: { path: stringPath, name: name } })
            .done(function (data) {
                explorer.Reload()
                    .done(function () { deffered.resolve(); })
                    .fail(function () { deffered.reject(); });
            })
            .fail(function () {
                deffered.reject();
            });

        //вернем deffered объект
        return deffered.promise();
    };
    
    //загрузка файлов
    explorer.UploadFiles = function(files) {
        //создадим deffered объект для результата
        var deffered = $.Deferred();

        //только если есть что загружать
        if (files.length <= 0) {
            deffered.resolve();
            return deffered.promise();
        }

        //начнем загрузку файлов
        var fileInfo = { Progress: ko.observable(0), xhr: null };
        fileInfo.Cancel = function () { if (fileInfo.xhr != null) fileInfo.xhr.abort(); };
        explorer.UploadingFiles.push(fileInfo);

        //создаем объект для отправки и добавляем в него параметры
        var postData = new window.FormData();
        for (var i = 0; i < files.length; i++)
            postData.append("file-" + i, files[i]);
        //добавим путь
        postData.append("path", explorer.CurrentPath().join("/"));

        //и отправим это все на сервер
        $.ajax(explorer.ApiUrl + "/files", {
            type: 'POST',
            xhr: function () { //добавим обработку процесса загрузки
                // создаем дефолтный объект
                fileInfo.xhr = $.ajaxSettings.xhr();
                
                //если есть событие, подписываемся на него
                if (fileInfo.xhr.upload)
                    fileInfo.xhr.upload.addEventListener('progress',
                        function (e) {
                            if (e.lengthComputable)
                                fileInfo.Progress(Math.round(e.loaded / e.total * 100));
                        }, false);
                return fileInfo.xhr;
            },
            data: postData,
            cache: false,
            contentType: false,
            processData: false
        })
            .done(function(data) {
                //файлы загружены
                fileInfo.Progress(100);

                //обновим список
                explorer.Reload()
                    .done(function() { deffered.resolve(); })
                    .fail(function() { deffered.reject(); });
            })
            .fail(function(data) {
                explorer.Errors.push("При отправке файлов возникла ошибка. Возможно файлы превышают допустимый размер или загрузка отменена. Попробуйте позже.");
                deffered.reject();
            })
            .always(function(data) {
                explorer.UploadingFiles.remove(fileInfo);
            });

        //вернем объект для реализации коллбэка
        return deffered.promise();
    };
    
    //обновление списка
    explorer.Reload = function() {
        return explorer.Open(explorer.CurrentPath());
    };
    
    //получение пути
    explorer.GetFullPath = function (index) {
        //заготовка результата
        var result = [];
        //получим массив с частями пути
        var path = explorer.CurrentPath();
        //если индекс не задан, получим весь путь
        if (!arguments.length)
            index = path.length - 1;
        //собираем путь
        for (var i = 0; i <= index; i++)
            result.push(path[i]);
        //возвращаем
        return result.join("/");
    };
    
    //красивое отображение размера
    explorer.GetPrintSize = function(size) {
        if (size < 1024)
            return size + ' Б';
        else if (size < 1024 * 1024)
            return Math.round(size / 1024, 1) + ' КБ';
        else if (size < 1024 * 1024 * 1024)
            return Math.round(size / 1024 / 1024, 1) + ' МБ';
        else
            return Math.round(size / 1024 / 1024 / 1024, 1) + ' ГБ';
    };
    
    //загружаем заданный путь
    explorer.Open(explorerPath);
}
