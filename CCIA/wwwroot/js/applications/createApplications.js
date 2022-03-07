//////////////////////
// Global variables //
//////////////////////

const spinner_div = `<div class="text-center"><div class="spinner-border text-center" role="status"><span class="sr-only">Loading...</span></div></div>`

////////////////////
// Event handlers //
////////////////////

$(function() {
    if($("#Application.EnteredVariety").val() != "")
    {
        $("#form-remainder").collapse('show');
    }
    if($("#PlantingStock1.PsEnteredVariety").val() != "" || $("#PlantingStock1.PsCertNum").val() != "" )
    {
        $("#second-ps").collapse('show');
        $("#add-second-ps").collapse('hide');
    }
});


$.validator.unobtrusive.adapters.addBool("mandatory", "required");


$("#add-second-ps").click(function() {
    $("#second-ps").collapse('show');
    $("#add-second-ps").collapse('hide');
});

// Click handler for removing the second planting stocks record
function removeSecondPSEntry() {
    $("#second-ps").collapse('hide');
    $("#add-second-ps").collapse('show');
    return false;
};


// Datepicker
$(".datepicker").datepicker({
    format: 'LT',
    autoclose: "true"
});

function ClassChange()
{
    var val = $('input[name="Application.ClassProducedId"]:checked').val();
    if(val == 14)
    {
        $("#AccessionSelector").collapse('show');
    } else
    {
        $("#AccessionSelector").collapse('hide');
        $("#Application_ClassProducedAccession").val("");
    }
}

$("#ps1_PsClass").change(function() {
    var val = $("#PlantingStock1.PsClass").val();
    if(val == 14)
    {
        $("#PSAccessionSelector").collapse('show');
    } else
    {
        $("#PSAccessionSelector").collapse('hide');
        $("#ps1_PsAccession").val("");
    }
});


// Set search variety click listener
// Located in seed.js because the callback of selecting a variety changes depending on App type
$('#varietySearch').on('click', () => {
    searchVarieties("variety-dropdown", "variety", "selectFirstVarietyFormRemainder", "Seed", "SeedAppPartial");
});



/* Load the rest of the form after selecting (or entering) a variety*/
function loadFormRemainder(varietyId, varietyName, remainderFolder, remainderName) {
    showSpinner("form-remainder");
    $("#form-remainder")
        .load("/Application/GetPartial?folder="+remainderFolder+"&partialName="+remainderName+"&orgId="+orgId+"&appTypeId="+appTypeId, 
        (response, status, xhr) => {
            if (status == "error") {
                var msg = "Sorry, the following error occurred while loading the remainder of the form: ";
                $("#error").html(msg + xhr.status + " " + xhr.statusText);
            }
            else {
                // Datepicker
                $("#datepicker").datepicker({
                    format: 'LT',
                    autoclose: "true"
                });

                // Set click listener for button to add second plantingstock
                $('#add-second-ps').on('click', addSecondPs);

                // Set click listener for button to add field history records
                $('#add-fieldhistory').on('click', (e) => {
                    addNewFieldHistory(e, appTypeId);
                });               

                showSpinner("variety-dropdown");
            }
        });
}

function searchVarieties(dropdownId, varietyInputId, selectVarietyCallback, remainderFolder, remainderName) {      
    // Display error if user tries to search for variety before selecting crop
    let cropId =$("#Application_CropId").val();   
    if (cropId === "0") {
        $("#cropAlert").modal('show');
        return;
    }

    let varietyName = $("#Application_EnteredVariety").val();
    if (varietyName === "") {
        $("#emptyVarAlert").modal('show');
        return;
    }

    let data = {
        name: varietyName,
        cropId: cropId
    };
    let vs = $("#DropdownId");    
    // Take text in input box and autofill input with the variety name that most closely matches it    
    $.ajax({
        type: "GET",
        url: "/Client/Application/FindVariety",
        data: data,
        success: function (res) {            
            // No varieties were found
            if (res.length === 0) {
                $("#varAlert").modal('show');
                // If this is the first variety, then load the rest of the form.
                window[selectVarietyCallback](-1, varietyName);
            }
            // Populate dropdown with list of varieties
            vs.html("");
            res.forEach((el) => {
                vs.html(vs.html() + `<a class="dropdown-item" id="variety-${el.id}" onClick="selectFirstVarietyFormRemainder(${el.id}, '${el.name}');">${el.name}</a>`);
            })
        },
        error: function (res) {
            alert("There was an error processing the request");
        }
    });
}



function selectFirstVarietyFormRemainder(varietyId, varietyName) {
    // Original variety
    // Set hidden input of variety id from selected variety
    $("#Application_SelectedVarietyId").val(varietyId);
    // Set variety input text to be the selected variety from dropdown
    $("#Application_EnteredVariety").val(varietyName); 
    $("#PlantingStock1_PsEnteredVariety").val(varietyName);    
    $("#form-remainder").collapse('show');
}

function ddlVarietySelected(){
    var varietyId = $("#Application_EnteredVariety").val();
    var varietyName = $("#Application_EnteredVariety :selected").text();
    $("#Application_SelectedVarietyId").val(varietyId);   
    $("#ps1_PsEnteredVariety").val(varietyName);    
    $("#form-remainder").collapse('show');
}

// Takes a parent element as a parameter, and places a centered bootstrap spinner inside
function showSpinner(parentId) {
    $(`#${parentId}`)[0].innerHTML = spinner_div;
}


