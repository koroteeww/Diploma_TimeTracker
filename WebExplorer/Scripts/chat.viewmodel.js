/* 
    чатик: viewmodel 
*/
$(function () {
    //создадим чатик
    var chat = new Chat("/api");
    //забиндим его на хтмл
    ko.applyBindings(chat, $("#chat")[0]);

    //сделаем скролинг области и вызов последнего сообщения
    var $chatMessages = $("#chat-messages");
    var $lastMessage = $("header .last-message");
    chat.OnMessage = function (data) {
        //последнее сообщение
        ko.cleanNode($lastMessage[0]);
        ko.applyBindings(data[data.length - 1], $lastMessage[0]);
        
        //скроллинг
        scrollToBottom();
    };

    //скроллим вних
    var scrollToBottom = function() {
        $chatMessages.animate({
            scrollTop: $chatMessages[0].scrollHeight
        }, 1000);
    };

    //сделаем отправку
    var $chatMessage = $("#chat-message");
    $chatMessage
        .keypress(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                $("#chat-send").click();
            }
        });
    $("#chat-send").click(function () {
        var text = $chatMessage.val();
        if (text == "") return;

        var chatControls = $('#chat-send, #chat-message').attr('disabled', true);
        chat.Post($chatMessage.val())
            .success(function () {
                chatControls.attr('disabled', false);
                $chatMessage.val("").focus();
            });
    });

    //публичные функции
    var chatShown = false;
    window.ChatHide = function () {
        $("#chat").switchClass("shown", "hidden", 500, 'easeInCubic', function() {
            $("#chat").css("visibility", "hidden");
            chatShown = false;
        });
    };
    window.ChatShow = function () {
        $("#chat").css("visibility", "visible").switchClass("hidden", "shown", 500, 'easeOutCubic', function () {
            scrollToBottom();
            $chatMessage.focus();
            chatShown = true;
        });
    };
    window.ChatToggle = function () {
        if (!chatShown)
            window.ChatShow();
        else
            window.ChatHide();
    };

    //мапимся на хотекеи
    $(document.body).keydown(function (e) {
        if (e.which == 192 && e.ctrlKey)
            window.ChatToggle();
    });
    
    //закрытие чатика при клике вне чата
    var chatClick = false;
    $("body").click(function () {
        if (!chatClick && chatShown) ChatHide();
        chatClick = false;
    });
    $("#chat").click(function() { chatClick = true; });
});