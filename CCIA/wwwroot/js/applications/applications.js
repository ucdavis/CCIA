//////////////////////
// Global variables //
//////////////////////

const spinner_div = `<div class="text-center"><div class="spinner-border text-center" role="status"><span class="sr-only">Loading...</span></div></div>`

////////////////////
// Event handlers //
////////////////////

/* Adds red outline to invalid text boxes on loading after failed form submission */
$(document).ready(() => {
    $('.field-validation-error').parents('.form-group').addClass('has-error');
});

// Set click listener for button to add second plantingstock
$('#add-second-ps').on('click', addSecondPs);

// Set click listener for button to add field history records
$('#add-fieldhistory').on('click', (e) => {
    addNewFieldHistory(e, appTypeId);
});

// Datepicker
$("#datepicker").datepicker({
    format: 'LT',
    autoclose: "true"
});


///////////////////////////
// General-Use Functions //
///////////////////////////

function fillVarietyNameAndId(varietyNameInput, varietyIdInput, varietyId, varietyName) {
    // Set hidden input of variety id from selected variety
    document.getElementById(varietyIdInput).value = varietyId;
    // Set variety input text to be the selected variety from dropdown
    document.getElementById(varietyNameInput).value = varietyName;
}

/* Load the rest of the form after selecting (or entering) a variety*/
function loadFormRemainder(varietyId, varietyName) {
    showSpinner("form-remainder");
    /* If something is inside form-remainder div, clear it out */
    $("#form-remainder")
        .load("/Application/GetPartial?folder=Seed&partialName=SeedAppPartial&orgId=" + orgId + "&appTypeId=" + appTypeId, (response, status, xhr) => {
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

                // Planting Stock 1 Variety
                fillVarietyNameAndId("ps1-variety", "ps1-variety-id", varietyId, varietyName);
            }
        });
}

function searchVarieties(dropdownId, varietyInputId, selectVarietyCallback) {
    // Display error if user tries to search for variety before selecting crop
    let crop = document.getElementsByName("CropId")[0];
    let cropText = crop.options[crop.selectedIndex].text;
    let cropId = crop.options[crop.selectedIndex].value;
    if (cropText === "") {
        $("#cropAlert").modal('show');
        return;
    }

    let varietyName = document.getElementById(varietyInputId).value;
    if (varietyName === "") {
        $("#emptyVarAlert").modal('show');
        return;
    }

    let data = {
        name: varietyName,
        cropId: cropId
    };
    let vs = document.getElementById(dropdownId);
    // Take text in input box and autofill input with the variety name that most closely matches it
    $.ajax({
        type: "GET",
        url: "/Application/FindVariety",
        data: data,
        success: function (res) {
            vs.innerHTML = "";
            // No varieties were found
            if (res.length === 0) {
                $("#varAlert").modal('show');
                // If this is the first variety, then load the rest of the form.
                window[selectVarietyCallback](-1, varietyName);
            }
            // Populate dropdown with list of varieties
            res.forEach((el) => {
                vs.innerHTML += `<a class="dropdown-item" id=variety-${el.varOffId} value=${el.varOffId}>${el.varOffName}</a>`;
                document.getElementById(`variety-${el.varOffId}`).addEventListener('click', (e) => {
                    window[selectVarietyCallback](el.varOffId, el.varOffName);
                });
            })
        },
        error: function (res) {
            alert("There was an error processing the request");
        }
    });
}

function selectFirstVarietyFormRemainder(varietyId, varietyName) {
    // Original variety
    fillVarietyNameAndId("variety", "variety-id", varietyId, varietyName);
    loadFormRemainder(varietyId, varietyName);
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


//////////////////////////////
// Planting Stock Functions //
//////////////////////////////

function addSecondPs(e) {
    e.preventDefault();
    showSpinner("second-ps");
    let appTypeId = 1;
    if (secondPsRendered) {
        return;
    }
    // Populate div with second planting stocks partial
    $("#second-ps")
        .load("/Application/GetPartial?folder=Seed&partialName=_SecondPlantingStock&orgId=" + orgId + "&appTypeId=" + appTypeId, (response, status, xhr) => {
            if (status == "error") {
                var msg = "Sorry, there was an error loading the remainder of this form: ";
                $("#error").html(msg + xhr.status + " " + xhr.statusText);
            }
            else {
                // Click listener for ps2 variety search button
                $('#ps2-variety-search').on('click', function (e) {
                    e.preventDefault();
                    searchVarieties("ps2-variety-dropdown", "ps2-variety", "selectPs2Variety");
                });

                secondPsRendered = true;
                let addSecondPsBtn = document.getElementById("add-second-ps");
                addSecondPsBtn.disabled = true;

                // Click handler for removing the second planting stocks record
                document.getElementById("remove-ps2").onclick = (e) => {
                    secondPsRendered = false;
                    e.preventDefault();
                    removeSecondPSEntry();
                    addSecondPsBtn.disabled = false;
                }
            }
        });
}

// Click handler for removing the second planting stocks record
function removeSecondPSEntry() {
    secondPsRendered = false;
    document.getElementById("add-second-ps").disabled = false;
    $("#second-ps").empty();
    return false;
};