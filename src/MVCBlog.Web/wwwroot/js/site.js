/* Cookie consent */
(function () {
    var button = document.querySelector("#cookieConsent button[data-cookie-string]");
    if (button) {
        button.addEventListener("click", function (event) {
            document.cookie = button.dataset.cookieString;
        }, false);
    }
})();

/* Menu */
var localStorage = null;
if (typeof Storage !== "undefined") {
    localStorage = window.localStorage;
}

var collapsed = window.innerWidth < 768;

if (localStorage && localStorage.getItem("collapsed")) {
    collapsed = localStorage.getItem("collapsed") === 'true';
}

if (collapsed) {
    $('.flexmenu').addClass('collapsed');
}

$('.togglemenu').on('click', function (e) {
    e.preventDefault();

    collapsed = !collapsed;
    if (localStorage) {
        localStorage.setItem("collapsed", collapsed);
    }

    $('.flexmenu').toggleClass('collapsed');
});

$(document).ready(function () {
    // Date picker
    $.each($('.gijgodatepicker'), function (index, element) {
        element = $(element);

        element.datepicker({
            uiLibrary: 'bootstrap4',
            calendarWeeks: true,
            locale: 'de-de',
            format: 'dd.mm.yyyy',
            weekStartDay: 1
        });
    });

    $.each($('.gijgodatetimepicker'), function (index, element) {
        element = $(element);

        element.datetimepicker({
            datepicker: {
                calendarWeeks: true,
                weekStartDay: 1
            },
            uiLibrary: 'bootstrap4',
            locale: 'de-de',
            format: 'dd.mm.yyyy HH:MM'
        });
    });

    // Delete confirmation
    $('.confirmdelete').on('click', function () {
        return confirm($(this).data('confirmmessage'));
    });

    // EditBlogEntry
    $('.select2').select2({
        minimumInputLength: 2,
        theme: "bootstrap4",
        language: "@language",
        allowClear: true,
        tags: true
    });

    $('#imageModal').on('show.bs.modal', function () {
        $.ajax({
            url: '/Administration/ImagesSelection'
        }).done(function (result) {
            $('#imagesSelection').html(result);
        });
    });

    $('#imagesSelection').on('click', '.selectImage', function () {
        var target = $('#BlogEntry_Content');
        target.val(target.val() + '\n' + $(this).data('markdown'));

        return false;
    });
});