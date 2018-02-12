var isOpen = false;

function openMultyPage(heigth, width, x, y , node) {
    var page = node;
    page.style.width = width + 'px';
    page.style.height = heigth + 'px';
    page.style.top = y + 'px';
    page.style.left = (x - width) + 'px';
}

function messageClick() {
    console.log()
    var page = $('#multypage')[0];
    if (isOpen) {
        page.style.display = "none";
        isOpen = false;
    } else {
        var icon = $("#messagesIndic")[0]
        openMultyPage(400, 350, icon.x, icon.height , page)
        page.style.display = 'block';
        isOpen = true;
    }
}
