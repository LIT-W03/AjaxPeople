$(function () {
    var peopleCount;
    clearTableAndPopulate();

    setInterval(function() {
        $.get('/home/getcount', function(result) {
            if (peopleCount && result.count !== peopleCount) {
                clearTableAndPopulate();
            }
        });
    }, 1000);


    $("#show-add").on('click', function () {
        $(".modal-title").text('Add Person');
        $("#update").hide();
        $("#add").show();
        $(".modal").modal();
    });

    $("#add").on('click', function () {
        var person = {
            firstName: $("#firstName").val(),
            lastName: $("#lastName").val(),
            age: $("#age").val()
        };
        $.post('/home/add', person, function (person) {
            console.log(person.Id);
            clearTableAndPopulate(function () {
                $(".modal").modal('hide');
                clearModal();
            });
        });
    });

    $("#update").on('click', function() {
        var id = $(this).data('person-id');
        var person = {
            firstName: $("#firstName").val(),
            lastName: $("#lastName").val(),
            age: $("#age").val(),
            id: id
        };
        $.post('/home/update', person, function () {
            clearTableAndPopulate(function () {
                $(".modal").modal('hide');
                clearModal();
            });
        });
    });

    function clearModal() {
        $("#firstName").val('');
        $("#lastName").val('');
        $("#age").val('');
    }


    $(".table").on('click','.edit', function() {
        $(".modal-title").text('Edit Person');
        $("#update").show();
        $("#add").hide();
        $(".modal").modal();

        var button = $(this);
        var row = button.closest('tr');
        var firstName = row.find('td:eq(0)').text();
        var lastName = row.find('td:eq(1)').text();
        var age = row.find('td:eq(2)').text();
        $("#firstName").val(firstName);
        $("#lastName").val(lastName);
        $("#age").val(age);
        $("#update").data('person-id', button.data('person-id'));
    });

    function clearTableAndPopulate(cb) {
        $(".table tr:gt(1)").remove();
        $("#spinner-row").show();
        $.get('/home/getpeople', function (result) {
            peopleCount = result.length;
            $("#spinner-row").hide();
            result.forEach(function (person) {
                $(".table").append(`<tr><td>${person.FirstName}</td><td>${person.LastName}</td>` +
                    `<td>${person.Age}</td><td><button class='btn btn-warning edit'` +
                    `data-person-id='${person.Id}'>Edit</button><button class='btn btn-danger'` +
                    `data-person-id='${person.Id}'>Delete</button>` +
                    `</td></tr>`);
            });
            if (cb) {
                cb();
            }
        });

    }
});