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