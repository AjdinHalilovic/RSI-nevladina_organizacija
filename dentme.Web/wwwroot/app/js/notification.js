function NotificationHandler(notification) {
    var jsonNotification;

    if (notification === null)
        jsonNotification = JSON.parse($("#m_js__notification").text());
    else
        jsonNotification = JSON.parse(notification);

    var Type = jsonNotification.Type;
    var Title = jsonNotification.Title;
    var Message = jsonNotification.Message;
    var Duration = jsonNotification.Duration;

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "1000",
        "hideDuration": "1000",
        "timeOut": Duration,
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    switch (Type) {
        case 1:
            toastr.success(Message, Title);
            break;
        case 2:
            toastr.warning(Message, Title);
            break;
        case 3:
            toastr.error(Message, Title);
            break;
        case 4:
            toastr.info(Message, Title);
            break;
    }
};