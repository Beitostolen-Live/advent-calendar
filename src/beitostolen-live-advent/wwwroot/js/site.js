var clickItems = document.getElementsByClassName('click');

var onItemClick = function () {
    var id = this.getAttribute('id');
    window.location.replace("/day/" + id);
}

for (var i = 0; i < clickItems.length; i++) {
    clickItems[i].addEventListener('click', onItemClick, false);
}