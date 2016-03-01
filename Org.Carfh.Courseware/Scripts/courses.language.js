
function loadLanguages() {
    var lang = $('#Language');
    $.getJSON('/Courses/Langs')
               .done(function (data) {

                   lang.removeProp('disabled');
                   $.each(data, function (i, litem) {
                       var option = '<option value="' + litem.Value + '">' + litem.Text + '</option>';
                       lang.append(option);
                   });
               })
               .fail(function (jqxhr, textStatus, error) {
                   var err = textStatus + ", " + error;
                   console.log("Language request Failed: " + err);
               });
}

$('document').ready(function () {

    loadLanguages();

});