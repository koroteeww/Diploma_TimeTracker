/*
    Параметры редактор BB кода
*/
$(function () {
    $.fn.insertImage = function(imgurl, thumburl) {
        var editor = this.data("wbb");
        var code = (thumburl)
            ? editor.getCodeByCommand('lightbox', { full: imgurl, src: thumburl })
            : editor.getCodeByCommand('img', { src: imgurl });
        this.insertAtCursor(code);
        return editor;
    };

    var customOptions = function() {
    };

    customOptions.prototype.img_uploadurl = "/api/UploadImage";
    customOptions.prototype.buttons = "cut,|,bold,italic,underline,strike,sup,sub,|,img,youtube,link,|,bullist,numlist,|,fontcolor,fontsize,|,quote,lightbox";
    customOptions.prototype.allButtons = {
        cut: {
            title: "Остальное в \"читать далее\"",
            buttonHTML: '<span class="ve-tlb-cut"></span>',
            hotkey: 'ctrl+shift+q',
            transform: {
                '<hr />': "[newscut][/newscut]"
            }
        },
        quote: {
            title: CURLANG.quote,
            buttonHTML: '<span class="ve-tlb-quote"></span>',
            hotkey: 'ctrl+shift+3',
            //subInsert: true,
            transform: {
                '<blockquote>{SELTEXT}</blockquote>': "[quote]{SELTEXT}[/quote]"
            }
        },
        img: {
            title: CURLANG.img,
            buttonHTML: '<span class="ve-tlb-img"></span>',
            hotkey: 'ctrl+shift+1',
            modal: {
                title: CURLANG.modal_img_title,
                width: "600px",
                tabs: [
                    {
                        title: CURLANG.modal_img_tab2,
                        html: '<div id="imguploader"> <form id="fupform" class="upload" action="{img_uploadurl}" method="post" enctype="multipart/form-data" target="fupload"><input type="hidden" name="iframe" value="1"/><input type="hidden" name="idarea" value="news-body" /><div class="fileupload"><input id="fileupl" class="file" type="file" name="img" /><button id="nicebtn" class="wbb-button">' + CURLANG.modal_img_btn + '</button> </div> </form> </div><iframe id="fupload" name="fupload" src="about:blank" frameborder="0" style="width:0px;height:0px;display:none"></iframe></div>'
                    },
                    {
                        title: CURLANG.modal_img_tab1,
                        html: null,
                        input: [
                            { param: "SRC", title: CURLANG.modal_imgsrc_text, validation: '^http(s)?://.*?\.(jpg|png|gif|jpeg)$' }
                        ]
                    }
                ],
                onLoad: this.imgLoadModal
            },
            transform: {
                '<img src="{SRC}" />': "[img]{SRC}[/img]"
            }
        },
        lightbox: {
            title: 'Lightbox',
            buttonHTML: '<span></span>',
            transform: {
                '<span class="lightbox" href="{FULL}"><img src="{SRC}" /></span>': "[lightbox={FULL}]{SRC}[/lightbox]"
            }
        },
        numlist: {
            title: CURLANG.numlist,
            buttonHTML: '<span class="ve-tlb-numlist"></span>',
            excmd: 'insertOrderedList',
            transform: {
                '<ol>{SELTEXT}</ol>': "[nlist]{SELTEXT}[/nlist]",
                '<li>{SELTEXT}</li>': "[*]{SELTEXT}[/*]"
            }
        },
        youtube: {
            title: CURLANG.video,
            buttonHTML: '<span class="ve-tlb-video"></span>',
            modal: {
                title: "Вставить видео с youtube",
                width: "600px",
                tabs: [
                    {
                        title: "Вставить видео с youtube",
                        input: [
                            { param: "SRC", title: "Адрес странички с видео" }
                        ]
                    }
                ],
                onSubmit: function(cmd, opt, queryState) {
                    var url = this.$modal.find('input[name="SRC"]').val();
                    var a = url.match(/^http:\/\/www\.youtube\.com\/watch\?v=([a-z0-9_]+)/i);
                    if (a && a.length == 2) {
                        var code = a[1];
                        this.insertAtCursor(this.getCodeByCommand(cmd, { src: code }));
                    }
                    this.closeModal();
                    return false;
                }
            },
            transform: {
                '<iframe src="http://www.youtube.com/embed/{SRC}" width="640" height="480" frameborder="0"></iframe>': '[youtube]{SRC}[/youtube]'
            }
        },

        //smiles
        smileList: [],

        //select options
        fs_verysmall: {
            title: CURLANG.fs_verysmall,
            buttonText: "fs1",
            excmd: 'fontSize',
            exvalue: "1",
            transform: {
                '<font size="1">{SELTEXT}</font>': '[size=0.6]{SELTEXT}[/size]'
            }
        },
        fs_small: {
            title: CURLANG.fs_small,
            buttonText: "fs2",
            excmd: 'fontSize',
            exvalue: "2",
            transform: {
                '<font size="2">{SELTEXT}</font>': '[size=0.8]{SELTEXT}[/size]'
            }
        },
        fs_normal: {
            title: CURLANG.fs_normal,
            buttonText: "fs3",
            excmd: 'fontSize',
            exvalue: "3",
            transform: {
                '<font size="3">{SELTEXT}</font>': '[size=1]{SELTEXT}[/size]'
            }
        },
        fs_big: {
            title: CURLANG.fs_big,
            buttonText: "fs4",
            excmd: 'fontSize',
            exvalue: "4",
            transform: {
                '<font size="4">{SELTEXT}</font>': '[size=1.2]{SELTEXT}[/size]'
            }
        },
        fs_verybig: {
            title: CURLANG.fs_verybig,
            buttonText: "fs5",
            excmd: 'fontSize',
            exvalue: "6",
            transform: {
                '<font size="6">{SELTEXT}</font>': '[size=1.4]{SELTEXT}[/size]'
            }
        }
    };

    window.wbbOptComment = new customOptions();
    window.wbbOptComment.buttons = "bold,italic,underline,strike,sup,sub,|,img,youtube,link,|,bullist,numlist,|,fontcolor,fontsize,|,quote,lightbox";

    

    window.wbbOptNews = new customOptions();
});