$(function () {
//For Adding new regions - IDs: addRegion, regionName, countryID
    $("#addRegion").submit(function (e) {
        e.preventDefault();
        let regionName = $("#regionName").val();
        let countryID = $("#countryID").val();
        let data = {
            Name: regionName,
            CountryId: countryID
        }
        $.ajax(
            {
                url: 'Regions/Add',
                data: data,
                success: function (succData) {
                    let errorMessage = generateMessageDiv(succData.message, !succData.success);
                    $("#addRegion").prepend(errorMessage);
                    GetAllRegions();
                },
                error: function (data) {
                    console.log(data);
                }
            });
    });

    $("#btnAddRegionClose").click(function () {
        $("#addRegion .notification").remove();
        $("#addRegion")[0].reset();
    });

    //For editing regions - IDs: editRegion, regionNameEdit, countryIDEdit
    $("#editRegion").submit(function (e) {
        e.preventDefault();
        let regionIdEdit = $("#regionIdEdit").val();
        let regionName = $("#regionNameEdit").val();
        let countryID = $("#countryIDEdit").val();
        let data = {
            Id: regionIdEdit,
            Name: regionName,
            CountryId: countryID
        }
        $.ajax(
            {
                url: 'Regions/Edit',
                data: data,
                success: function (succData) {
                    let errorMessage = generateMessageDiv(succData.message, !succData.success);
                    $("#editRegion").prepend(errorMessage);
                    GetAllRegions();
                },
                error: function (data) {
                    console.log(data);
                }
            });
    });

    $("#btnEditRegionClose").click(function () {
        $("#editRegion .notification").remove();
        $("#editRegion")[0].reset();
    });


    resetEvLisForEditDelete();
});

//setting click event listeners for edit buttons on every row of Region table
function resetEvLisForEditDelete() {

    $(".btn_editRegion").each(function () {
        $(this).click(function () {
            let Id = $(this).parent().siblings(".Id")[0].innerHTML;
            let Name = $(this).parent().siblings(".Name")[0].innerHTML;
            let CountryName = $(this).parent().siblings(".CountryName")[0].innerHTML;

            //Set current region name
            $("#regionIdEdit").val(Id);
            $("#regionNameEdit").val(Name);
            //Mark current parent country as selected
            let countryOptions = $("#countryIDEdit option");
            countryOptions.each(function () { this.removeAttribute("selected"); });
            for (let i = 0; i < countryOptions.length; i++) {
                if (countryOptions[i].innerHTML === CountryName) {
                    countryOptions[i].setAttribute("selected", "selected");
                    break;
                }
            }
        });
    });

    $(".btn_deleteRegion").each(function () {

        $(this).click(function () {
            let RegionId = parseInt($(this).parent().siblings(".Id")[0].innerHTML);
            console.log(RegionId);

            $.ajax(
                {
                    url: 'Region/Delete',
                    data: { RegionId: RegionId},
                    success: function (succData) {
                        GetAllRegions();
                    },
                    error: function (data) {
                        console.log(data);
                    }
                });
        });
    });
}

function GetAllRegions() {
    $.ajax(
        {
            url: 'Region/GetAll',
            success: function (succData) {
                RefreshDataTable(succData.regions);
            },
            error: function (data) {
                console.log(data);
            }
        });
}

function RefreshDataTable(regions) {
    //clear the table
    document.getElementById("regionTableBody").innerHTML = "";

    for (let i = 0; i < regions.length; i++) {
        let row = document.createElement("tr");

        let tdId = document.createElement("td");
        let tdName = document.createElement("td");
        let tdCountryName = document.createElement("td");
        let tdActions = document.createElement("td");

        tdId.classList.add("Id");
        tdId.setAttribute("colspan", "3");
        tdId.innerHTML = regions[i].id;
        tdName.classList.add("Name");
        tdName.setAttribute("colspan", "3");
        tdName.innerHTML = regions[i].name;
        tdCountryName.classList.add("CountryName");
        tdCountryName.setAttribute("colspan", "3");
        tdCountryName.innerHTML = regions[i].country.name;

        tdActions.setAttribute("colspan", "3");
        tdActions.classList.add("actionBtns");

        let pre = document.createElement("pre");

        let editi = document.createElement("i");
        let editBtn = document.createElement("button");
        editi.classList.add("p-r-1", "la", "la-edit");
        editBtn.classList.add("btn", "btn-success", "btn_editRegion");
        editBtn.appendChild(editi);
        editBtn.setAttribute("data-toggle", "modal");
        editBtn.setAttribute("data-target", "#m_modal_2");

        let delete1i = document.createElement("i");
        let delete1Btn = document.createElement("button");
        delete1i.classList.add("flaticon-delete");
        delete1Btn.classList.add("btn", "btn-danger", "btn_deleteRegion");
        delete1Btn.appendChild(delete1i);

        tdActions.appendChild(editBtn);
        tdActions.appendChild(delete1Btn);

        row.appendChild(tdId);
        row.appendChild(tdName);
        row.appendChild(tdCountryName);
        row.appendChild(tdActions);

        document.getElementById("regionTableBody").appendChild(row);
    }
    resetEvLisForEditDelete();
}