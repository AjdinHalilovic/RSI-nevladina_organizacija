function generateMessageDiv(message,isError) {
    let wrapper = document.createElement("div");
    wrapper.classList.add("notification");

    let msgType = isError ? "alert-danger" : "alert-success";

    wrapper.classList.add("m-alert", "m-alert--outline", "alert", msgType, "alert-dismissible", "animated", "fadeIn");

    wrapper.setAttribute("role", "alert");

    let btnDismiss = document.createElement("button");
    btnDismiss.setAttribute("type", "button");
    btnDismiss.setAttribute("data-dismiss", "alert");
    btnDismiss.setAttribute("aria-label", "Close");
    btnDismiss.classList.add("close");

    let span = document.createElement("span");

    let innerDiv = document.createElement("div");
    innerDiv.classList.add("validation-summary-errors")
    innerDiv.setAttribute("data-valmsg-summary", "true");

    let ul = document.createElement("ul");
    let li = document.createElement("li");
    li.innerHTML = message;
    ul.appendChild(li);

    innerDiv.appendChild(ul);
    span.appendChild(innerDiv);
    wrapper.appendChild(span);
    wrapper.appendChild(btnDismiss);
    return wrapper;
}