$(function () {
    $("body").on("click", ".modal-link", function (e) {
        e.preventDefault();
        $(this).attr('data-target', '#modal-container');
        $(this).attr('data-toggle', 'modal');
        var href = $(this).attr("href");

        $.ajax({
            url: href,
            type: "GET",
            success: function (data) {
                $("#update-modal").html(data);
            }
        })
    });
    $('#modal-container').on('hidden.bs.modal', function () {
        $(this).removeData('bs.modal');
        $(this).find('update-modal').html("");
    });

    $("body").on("click", ".modal-link-lg", function (e) {
        e.preventDefault();
        $(this).attr('data-target', '#modal-container-lg');
        $(this).attr('data-toggle', 'modal');
        var href = $(this).attr("href");

        $.ajax({
            url: href,
            type: "GET",
            success: function (data) {
                $("#update-modal-lg").html(data);
            }
        })
    });
    $('#modal-container-lg').on('hidden.bs.modal', function () {
        $(this).removeData('bs.modal');
        $(this).find('update-modal-lg').html("");
    });

    $(".date-picker").datepicker();


  

});

function closeModal() {
    $('#close-modal').click();
    $('#close-modal-lg').click();
}

/* Submit custom form */
function submitCustomForm(object) {

    if (!object) return false;
    if (!object.url) return false;
    if (!object.token) return false;
    if (!object.hiddenElements) return false;
    if (!object.method)
        object.method = "GET";

    var $token = $('<input />').attr("type", "hidden").attr("name", "__RequestVerificationToken")
        .val(object.token);
    var $form = $('<form />').attr("action", object.url).attr("method", object.method).append($token);

    if (object.ajax) {
        if (!object.ajaxOptions) return false;
        $form.removeAttr("method");
        $form.attr("data-ajax", object.ajax).attr("data-ajax-method", object.method);

        if (object.ajaxOptions.mode)
            $form.attr("data-ajax-mode", object.ajaxOptions.mode);
        if (object.ajaxOptions.update)
            $form.attr("data-ajax-update", object.ajaxOptions.update);

        if (object.ajaxOptions.success) {
            $form.attr("data-ajax-success", object.ajaxOptions.success);
        }

        if (object.ajaxOptions.failure) {
            $form.attr("data-ajax-failure", object.ajaxOptions.failure);
           
        }
    }

    object.hiddenElements.forEach(function (element) {
        var $input = $('<input />').attr("type", "hidden").attr("name", element.name).val(element.value);
        $form.append($input);
    });

    $("body").append($form);
    $form.submit();

    if (object.ajax) {
        $form.remove();
    }

    return true;
}




