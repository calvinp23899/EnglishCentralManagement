function toggleTeacherDropdown() {
    document.getElementById("teacherDropdown").style.display = "block";
}

function filterTeacher() {
    let filter = document.getElementById("teacherSearch").value.toLowerCase();
    let items = document.querySelectorAll("#teacherDropdown li");

    items.forEach(i => {
        i.style.display = i.innerText.toLowerCase().includes(filter)
            ? "block"
            : "none";
    });
}

function selectTeacher(name, id) {
    document.getElementById("teacherInput").value = name;
    document.getElementById("TeacherId").value = id;
    document.getElementById("teacherDropdown").style.display = "none";
}

document.addEventListener("click", function (e) {
    if (!e.target.closest(".teacher-select")) {
        document.getElementById("teacherDropdown").style.display = "none";
    }
});



//========Course Dropdown
function toggleCourseDropdown() {
    document.getElementById("courseDropdown").style.display = "block";
}

function filterCourse() {
    let filter = document.getElementById("courseSearch").value.toLowerCase();
    let items = document.querySelectorAll("#courseDropdown li");

    items.forEach(i => {
        i.style.display = i.innerText.toLowerCase().includes(filter)
            ? "block"
            : "none";
    });
}

function selectCourse(name, id) {
    document.getElementById("courseInput").value = name;
    document.getElementById("CourseId").value = id;
    document.getElementById("courseDropdown").style.display = "none";
}

document.addEventListener("click", function (e) {
    if (!e.target.closest(".course-select")) {
        document.getElementById("courseDropdown").style.display = "none";
    }
});