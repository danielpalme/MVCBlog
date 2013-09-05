function submitCommentForm(ev) {
    ev.preventDefault();
    $("#submitCommentForm").attr('disabled', 'disabled');
    var dataString = $('#commentForm').serialize();
    $.ajax(
            {
                type: "POST",
                data: dataString,
                url: $("#commentForm").attr("action"),
                success: function (result) {
                    $("#commentscontainer").html(result);
                    $("#commentForm").submit(submitCommentForm);
                },
                error: function (result) {
                    $("#submitCommentForm").removeAttr('disabled');
                }
            });
}
function printPage(ev) {
    ev.preventDefault();
    window.print();
}
$(function () {
    $(".lightbox").lightbox({
        fitToScreen: true,
        imageClickClose: false
    });

    $("#commentForm").submit(submitCommentForm);

    $(".printicon").click(printPage);
    
    SyntaxHighlighter.autoloader(
      'c-sharp csharp Scripts/SyntaxHighlighter/shBrushCSharp.js',
      'xml xhtml xslt html xhtml Scripts/SyntaxHighlighter/shBrushXml.js',
      'php Scripts/SyntaxHighlighter/shBrushPhp.js',
      'js jscript javascript Scripts/SyntaxHighlighter/shBrushJScript.js',
      'java Scripts/SyntaxHighlighter/shBrushJava.js'
    );

    SyntaxHighlighter.defaults['toolbar'] = false;
    SyntaxHighlighter.all();
});