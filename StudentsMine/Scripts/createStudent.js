$(document).ready(function () {
    Duplicator("dublicate-data-group", "data-field", "form0");
})

function orderStudentsByEmail(courseId) {
    var json = test();
    $.ajax({
        url: "/Student/OrderByEmailList?courseId=" + courseId,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ 'emails': json }),
        success: function (data) {
            console.log(data);
        }
    })
}

function createStudents(courseId , action) {
    prepareFieldsBeforeSend();
    setUIId()
    var json = test();
    $.ajax({
        url: "/Student/" + action + "?courseId=" + courseId,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(json),
        success: function (data) {-
            data.forEach(function (element) {
                var node = $(".dublicate-data-group[index=" + element.UIId + "]")
                if (element.Result) {
                    node.animate({ "background-color": "rgba(63, 255, 0, 0.22)" }, 500);                    
                } else {
                    node.animate({ "background-color": "rgba(255, 0, 0, 0.22);" }, 500);
                    $(".dublicate-data-group[index=" + element.UIId + "] .error-message")[0].innerHTML = element.ErrorMessage;
                    if (element.Status == 1) {
                        $(".dublicate-data-group[index=" + element.UIId + "] .error-message")[0].innerHTML += " <a href='#'>Send order by email</a>"
                    }
                }
            })
        }
    })
}

function setUIId() {
    var uiids = $(".UIId");
    uiids.each(function (index, node) {
        var indexHolder = node.parentElement;
        node.value = indexHolder.getAttribute("index");
    })
}