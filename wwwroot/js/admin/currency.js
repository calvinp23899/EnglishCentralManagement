$(document).ready(function () {

    var $displayInput = $("#HourlyRateDisplay");

    if ($displayInput.length && $displayInput.val()) {
        formatNumber($displayInput[0]); // truyền DOM element vào
    }

});

function formatNumber(input) {
    //See _TeacherFormLayout.cshtml => hourlyrate => remember data-target 
    let raw = input.value.replace(/,/g, '').replace(/\D/g, '');

    if (!raw) {
        input.value = '';
        if (input.dataset.target) {
            document.getElementById(input.dataset.target).value = '';
        }
        return;
    }

    // format có dấu ,
    input.value = raw.replace(/\B(?=(\d{3})+(?!\d))/g, ",");

    // set giá trị thật
    if (input.dataset.target) {
        document.getElementById(input.dataset.target).value = raw;
    }
}
