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


/* Date picker */
$(document).ready(function () {
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
});

$(document).ready(function () {
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
});
