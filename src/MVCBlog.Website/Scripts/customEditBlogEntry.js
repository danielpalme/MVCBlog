tinyMCE.init({
    mode : "none",
    theme: "advanced",
    convert_urls : false,
    forced_root_block : false,
    force_br_newlines : true,
    force_p_newlines : false,
    plugins: "table,advimage,advlink,inlinepopups,preview,searchreplace,paste,fullscreen,noneditable,nonbreaking,xhtmlxtras,wordcount,syntaxhighlighter",

    theme_advanced_buttons1: "bold,italic,underline,strikethrough,cite,|,formatselect,fontsizeselect,|,forecolor,backcolor,|,outdent,indent,|,undo,redo,|,search,replace,|,cleanup,preview,fullscreen,code",
    theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,bullist,numlist,|,link,unlink,image,syntaxhighlighter,|,tablecontrols,|,removeformat,|,sub,sup,|,charmap,|,nonbreaking",
    theme_advanced_buttons3: "",
    theme_advanced_buttons4: "",
    theme_advanced_toolbar_location: "top",
    theme_advanced_toolbar_align: "left",
    theme_advanced_statusbar_location: "bottom",
    theme_advanced_resizing: true
});
        
function toggleEdtitor(id) {
    if (!tinyMCE.get(id)) {
        tinyMCE.execCommand('mceAddControl', false, id);
    } else {
        tinyMCE.execCommand('mceRemoveControl', false, id);
    }
}
            
function selectImage(ev) {
    var url = $("#ImageDropDown").val();
    if (url != '') {
        var text = $("#ImageDropDown option:selected").text();
        var image = '<img src="' + url + '" alt="' + text + '" title="' + text + '" class="picture" />';
        var lightboximage = '<a class="lightbox" href="' + url + '" data-rel="group1" title="' + text + '"><img src="' + url + '" alt="' + text + '" title="' + text + '" /></a>';
        $("#Image").val(image);
        $("#ImageLightbox").val(lightboximage);
        $("#ImagePreview").attr('src', url);
        $("#ImagePreview").attr('alt', text);
    } else {
        $("#Image").val('');
        $("#ImageLightbox").val('');
        $("#ImagePreview").attr('src', '');
        $("#ImagePreview").attr('alt', '');
    }
}
        
function updateShortContentCounter() {
    $("#ShortContentCounter").html(1500 - $("#BlogEntry_ShortContent").val().length);
}
        
$(function() {
    $("#toggleShortContentEditor").click(function(ev) {
        ev.preventDefault();
        toggleEdtitor("BlogEntry_ShortContent");
    });
    $("#toggleContentEditor").click(function(ev) {
        ev.preventDefault();
        toggleEdtitor("BlogEntry_Content");
    });
            
    $("a.addImage").click(function() {
        $("#imageHelper").css("visibility", "visible");
    });
    $("#closeImageLink").click(function() {
        $("#imageHelper").css("visibility", "hidden");
    });
    $("#ImageDropDown").change(selectImage);
            
    $("#BlogEntry_ShortContent").keyup(updateShortContentCounter);
    updateShortContentCounter();
});