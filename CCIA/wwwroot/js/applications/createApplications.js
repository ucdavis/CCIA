//////////////////////
// Global variables //
//////////////////////

const spinner_div = `<div class="text-center"><div class="spinner-border text-center" role="status"><span class="sr-only">Loading...</span></div></div>`

////////////////////
// Event handlers //
////////////////////

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


// Set click listener for button to add field history records
$('#add-fieldhistory').on('click', (e) => {
    addNewFieldHistory(e, appTypeId);
});

// Datepicker
$(".datepicker").datepicker({
    format: 'LT',
    autoclose: "true"
});

// Set search variety click listener
// Located in seed.js because the callback of selecting a variety changes depending on App type
$('#varietySearch').on('click', () => {
    searchVarieties("variety-dropdown", "variety", "selectFirstVarietyFormRemainder", "Seed", "SeedAppPartial");
});

// Called before HTML form submitted to controller
$('#seedApplication').submit(function (e) {
    insertHiddenInput("growerId", growerId, "seedApplication");
    let fhIndicesStr = JSON.stringify(fhIndices);
    insertHiddenInput("AppViewModel.FieldHistoryIndices", fhIndicesStr, "seedApplication");
    return true;
});



///////////////////////////
// General-Use Functions //
///////////////////////////



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
     $("#ps1_PsEnteredVariety").val(varietyName);    
    $("#form-remainder").collapse('show');
}

// Takes a parent element as a parameter, and places a centered bootstrap spinner inside
function showSpinner(parentId) {
    $(`#${parentId}`)[0].innerHTML = spinner_div;
}


/////////////////////////////
// Field History Functions //
/////////////////////////////

function addNewFieldHistory(e, appTypeId) {
    e.preventDefault();
    // Populate div with an additional field history partial
    fhEntryId = findAvailableFhIndex();
    if (fhEntryId == -1) {
        return;
    }
    let fh_entry_div = `#fh-entry-${fhEntryId}`;
    // Show the section
    document.getElementById("fh-entry-" + fhEntryId).classList.remove("hidden");
    showSpinner("fh-entry-" + fhEntryId);
    $(fh_entry_div)
        .load("/Application/GetPartial?folder=Shared&partialName=_FieldHistoryEntry&orgId=" + orgId + "&appTypeId=" + appTypeId + "&fhEntryId=" + fhEntryId, (response, status, xhr) => {
            if (status == "error") {
                var msg = "Sorry, there was an error loading the remainder of this form: ";
                $("#error").html(msg + xhr.status + " " + xhr.statusText);
            }
            else {
                // Mark that id as used
                fhIndices[fhEntryId] = 1;

                // Click handler for removing additional field history entry
                document.getElementById(fhEntryId).onclick = (e) => {
                    e.preventDefault();
                    // Grabs ID from closest parent section -- the parent section containing the "X" button
                    let idToRemove = parseInt(e.target.closest("button").id);
                    // Hide the section
                    document.getElementById("fh-entry-" + idToRemove).classList.add("hidden");
                    // Mark this entry as unused
                    if (fhEntryCount === maxFieldHistories) {
                        // Re-enable button to add new entry
                        document.getElementById("add-fieldhistory").disabled = false;
                    }
                    idToRemove = parseInt(e.target.closest("button").id);
                    fhIndices[idToRemove] = 0;
                    fhEntryCount--;
                    removeFhEntryById(idToRemove);
                }
                fhEntryCount++;
            }
            if (fhEntryCount === maxFieldHistories) {
                document.getElementById("add-fieldhistory").disabled = true;
            }
        });
}

function findAvailableFhIndex() {
    for (let i = 0; i < fhIndices.length; i++) {
        // 0 = free, 1 = used
        if (fhIndices[i] === 0) {
            return i;
        }
    }
    return -1;
}

// Removes contents of a field history section, while keeping the section tags intact
function removeFhEntryById(id) {
    let fhEntrySection = `#fh-entry-${id}`;
    $(fhEntrySection).empty();
}



