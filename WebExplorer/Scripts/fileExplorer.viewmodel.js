/* 
    менеджер файлов: viewmodel 
*/
$(function () {
    //создадим модель и забиндим её на html
    ko.virtualElements.allowedBindings.foreach = true;
    var explorer = new FileExplorer("/api", location.hash.replace(/^#/, ''));
    ko.applyBindings(explorer, $("#explorer")[0]);

    //-------------------------------------------------------------------------------------
    //код для загрузки файлов с помощью html5
    var focusInside = false;    //флаг обозначающий нахождение фокуса дропа внутри грида
    var focusMove = false;      //флаг обозначающий перемещение фокуса дропа с элемента на элемент внутри грида
    $("#file-grid.dropable").on('dragenter', function (e) {
        e.stopPropagation();
        e.preventDefault();
        
        if (!focusInside)
            focusInside = true;
        else
            focusMove = true;

        $(this).addClass("over");
    });
    $("#file-grid.dropable").on('dragleave', function (e) {
        if (!focusMove) {
            $("#file-grid").removeClass("over");
            focusInside = false;
        }
        focusMove = false;
    });
    $("#file-grid.dropable").on('drop', function (e) {
        e.stopPropagation();
        e.preventDefault();
        $("#file-grid").removeClass("over");
        explorer.UploadFiles(e.originalEvent.dataTransfer.files);
    });
    

    //-------------------------------------------------------------------------------------
    //код для загрузки файлов с помощью модального окна
    $("#upload-file").on('shown', function (e) {
        $("#upload-file input[type=file]").val("").eq(0).focus();
        $("#upload-file .alert").hide();
    });
    $("#upload-file-save").click(function (e) { $("#upload-file-form").submit(); });
    $("#upload-file-iframe").load(function (e) {
        if ('true' == $("#upload-file-iframe").contents().text().toLowerCase()) {
            explorer.Reload();
            $("#upload-file").modal('hide');
        } else {
            $("#upload-file .alert").show();
        }
            
    });
    
    //-------------------------------------------------------------------------------------
    //код для создания папки с помощью модального окна
    $("#new-folder").on('shown', function (e) { $("#new-folder-name").focus(); });
    $("#new-folder input[type=text]").keypress(function (e) {
        if (e.which == 13) {
            $("#new-folder-create").click();
        }
    });
    $("#new-folder-create").click(function (e) {
        //подготовим окно
        $("#new-folder-name").val();
        $("#new-folder .alert").hide();
        //выставим статус кнопки
        var btn = $(this);
        btn.button('loading');
        explorer.CreateFolder($("#new-folder-name").val())
            .done(function (data) {
                $("#new-folder").modal('hide');
            })
            .fail(function (err) {
                $("#new-folder .alert").show();
            })
            .always(function (data) {
                btn.button('reset');
            });
    });

    //-------------------------------------------------------------------------------------
    //создание папки из строки
    $("#new-folder-row-create").click(function(e) {
        var btn = $(this);
        btn.button('loading');
        explorer.CreateFolder($("#new-folder-row-name").val())
            .done(function (data) {
                $("#new-folder-row input[type=text]").blur();
                $("body").click();
                $("#new-folder-row .alert").hide();
            })
            .fail(function (err) {
                $("#new-folder-row .alert").show();
            })
            .always(function (data) {
                btn.button('reset');
            });
    });
    $("#new-folder-row-name")
        .keypress(function(e) {
            if (e.which == 13) {
                $("#new-folder-row-create").click();
            }
        })
        .focusin(function(e) {
            $("#new-folder-row").addClass("active");
        });
    $("html")
        .click(function (e) {
            if (!$(e.target).closest("#new-folder-row").length) {
                $("#new-folder-row .alert").hide();
                $("#new-folder-row-name").val("");
                $("#new-folder-row").removeClass("active");
            }
        });
    

    //-------------------------------------------------------------------------------------
    //первичная информация
    if (!$.cookie("explorer-help-shown")) {
        //установим куку
        $.cookie("explorer-help-shown", true, { expires: 365 * 2 });

        var openInfo = {
            animation: true,
            placement: 'top',
            delay: { show: 1000, hide: 1000 },
            content: "Файлы для загрузки можно перетаскивать прямо сюда и они загрузятся."
        };

        $("#file-grid").popover(openInfo).popover('show');

        setTimeout(function () {
            $("#file-grid").popover('destroy');

            openInfo.content = "Если вы спользуете древний браузер или IE, то для вас есть специальная кнопка.";
            openInfo.placement = "bottom";

            $("#upload-file-open-modal").popover(openInfo).popover('show');

            setTimeout(function () {
                $("#upload-file-open-modal").popover('destroy');
                
                openInfo.content = "Тут можно создать новую папку.";

                $("#new-folder-open-modal").popover(openInfo).popover('show');

                setTimeout(function () {
                    $("#new-folder-open-modal").popover('destroy');

                    openInfo.content = "Но удобнее тут.";
                    openInfo.placement = "top";

                    $("#new-folder-row-name").popover(openInfo).popover('show');

                    setTimeout(function () {
                        $("#new-folder-row-name").popover('destroy');
                    }, 3000);

                }, 5000);

            }, 5000);

        }, 5000);
    }
});
