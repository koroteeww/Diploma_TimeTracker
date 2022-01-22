/* 
    чатик: модель 
*/
function Chat(apiUrl) {
    //---------------------------------------------------------------------------
    //PRIVATE PROPERTY
    //сохраним ссылку на скоуп
    var chat = this;
    //последнее загруженное сообщение
    var lastId = 0;
    
    //---------------------------------------------------------------------------
    //PUBLIC PROPERTY
    //путь к апи
    chat.ApiUrl = apiUrl == undefined ? "/api" : apiUrl.replace(/\/$/, "");
    //список сообщений
    chat.Messages = ko.observableArray();
    //коллбэк по загрузке
    chat.OnMessage = null;

    
    //---------------------------------------------------------------------------
    // PRIVATE METHODS
    //получение сообщений
    var loadMessages = function() {
        //deffered объект
        var deffered = $.Deferred();

        //загружаем сообщения
        $.ajax(chat.ApiUrl + "/chat/" + lastId)
            .done(function (data) {
                //сохраним сообщения
                for (var i = 0; i < data.length; i++)
                    chat.Messages.push(data[i]);
                
                //сохраним последний айдишник
                if (data.length > 0)
                    lastId = data[data.length - 1].id;
                
                //вызовем коллбэк
                if (data.length > 0)
                    chat.OnMessage && chat.OnMessage(data);

                //разрешим ожидающий объект
                deffered.resolve();
            })
            .fail(function () {
                deffered.reject();
            });

        return deffered.promise();
    };

    //---------------------------------------------------------------------------
    //PUBLIC METHOD
    //отправка сообщений
    chat.Post = function (message) {
        return $.ajax(chat.ApiUrl + "/chat/", { type: "POST", data: { body: message } });
    };
    
    //---------------------------------------------------------------------------
    //INIT
    //запускаем загрузку сообщений в infinitive loop
    var loopLoad = function() {
        loadMessages().done(function() {
            setTimeout(function() {
                loopLoad();
            }, 1);
        });
    };
    loopLoad();
}