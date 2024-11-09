$(document).ready(function () {
    $("#do_login").click(function () {
        closeLoginInfo();
        $(this).parent().find('span').css("display", "none");
        $(this).parent().find('span').removeClass("i-save");
        $(this).parent().find('span').removeClass("i-warning");
        $(this).parent().find('span').removeClass("i-close");

        var proceed = true;
        $("#login_form input").each(function () {

            if (!$.trim($(this).val())) {
                $(this).parent().find('span').addClass("i-warning");
                $(this).parent().find('span').css("display", "block");
                proceed = false;
            }
        });

        if (proceed) //everything looks good! proceed...
        {
            $(this).parent().find('span').addClass("i-save");
            $(this).parent().find('span').css("display", "block");
        }
    });

    //reset previously results and hide all message on .keyup()
    $("#login_form input").keyup(function () {
        $(this).parent().find('span').css("display", "none");
    });


    ///////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////
    $('select#serviceId option')[0].disabled = true;
    $('ul#loginLink').hide();

    $('#serviceId').change(function () {

        //select dropdown
        var service = $('#serviceId').val();

        var a = $('#servicePrice option[selected=selected]').text();
        $('select#servicePrice option[value="' + service + '"]').attr('selected', true).text() == a
        $('input#price').val('R'+$('select#servicePrice option[value="' + service + '"]').attr('selected', true).text());
    })


    //When documents ready wait 2 sec and desipate
    setTimeout(function () {
        $('div#feedback').hide();
    }, 2000);
});



