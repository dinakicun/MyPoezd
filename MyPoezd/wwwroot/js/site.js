function myFunction() {
    document.getElementById("hidden").classList.toggle("show");
}
function myFunction2() {
    document.getElementById("myDropdown").classList.toggle("show1");
}

document.getElementById('vhod').addEventListener('click', myFunction2);
document.getElementById('vhod').addEventListener('click', myFunction6);

window.onclick = function (event) {
    if (!event.target.matches('.dropbtn')) {
        var dropdowns = document.getElementsByClassName("dropdown-content");
        var i;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show1')) {
                openDropdown.classList.remove('show1');
            }
        }
    }
}
//function myFunction3() {
//    document.getElementById("auto").classList.add("show3");
//}

//document.getElementById('myLink').addEventListener('click', myFunction4);
//document.getElementById('myLink').addEventListener('click', myFunction5);

//function myFunction4() {
//    document.getElementById("auto").classList.remove("show3");
//}
//function myFunction5() {
//    document.getElementById("regist").classList.add('show4');
//}

//function myFunction6() {
//    document.getElementById("regist").classList.remove("show4");
//}

function changeColor(element) {
    // Получение всех элементов с классом "seat"
    var seats = document.getElementsByClassName("seat");

    // Сброс цвета всех мест
    for (var i = 0; i < seats.length; i++) {
        seats[i].style.backgroundColor = "#97a4e5";
    }

    // Изменение цвета фона выбранного места
    element.style.backgroundColor = "#c8c8c8";
}